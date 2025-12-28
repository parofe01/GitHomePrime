"use strict";

let gl, program;

// Cubo triangulado (lado 1, centrado en el origen)
const cubeModel = {
    vertices: [
        // Front (z = -0.5)
        -0.5, -0.5, -0.5,
        0.5, -0.5, -0.5,
        -0.5,  0.5, -0.5,
        0.5,  0.5, -0.5,

        // Back (z = +0.5)
        -0.5, -0.5,  0.5,
        0.5, -0.5,  0.5,
        -0.5,  0.5,  0.5,
        0.5,  0.5,  0.5,
    ],
    // 12 triángulos (2 por cara)
    indices: [
        // Front
        0, 1, 2,  1, 2, 3,
        // Back
        4, 6, 5,  5, 6, 7,
        // Left
        0, 2, 4,  2, 6, 4,
        // Right
        1, 5, 3,  3, 5, 7,
        // Bottom
        0, 4, 1,  1, 4, 5,
        // Top
        2, 3, 6,  3, 7, 6
    ]
};

function getWebGLContext() {
    const canvas = document.getElementById("myCanvas");
    try { return canvas.getContext("webgl2"); } catch (e) {}
    return null;
}

function initShaders() {
    const vs = gl.createShader(gl.VERTEX_SHADER);
    gl.shaderSource(vs, document.getElementById("myVertexShader").textContent);
    gl.compileShader(vs);
    if (!gl.getShaderParameter(vs, gl.COMPILE_STATUS)) {
        console.error("Vertex shader error:", gl.getShaderInfoLog(vs));
        return false;
    }

    const fs = gl.createShader(gl.FRAGMENT_SHADER);
    gl.shaderSource(fs, document.getElementById("myFragmentShader").textContent);
    gl.compileShader(fs);
    if (!gl.getShaderParameter(fs, gl.COMPILE_STATUS)) {
        console.error("Fragment shader error:", gl.getShaderInfoLog(fs));
        return false;
    }

    program = gl.createProgram();
    gl.attachShader(program, vs);
    gl.attachShader(program, fs);
    gl.linkProgram(program);

    if (!gl.getProgramParameter(program, gl.LINK_STATUS)) {
        console.error("Program link error:", gl.getProgramInfoLog(program));
        return false;
    }

    gl.useProgram(program);

    program.vertexPositionAttribute = gl.getAttribLocation(program, "VertexPosition");
    gl.enableVertexAttribArray(program.vertexPositionAttribute);

    program.modelViewMatrixIndex = gl.getUniformLocation(program, "modelViewMatrix");
    program.projectionMatrixIndex = gl.getUniformLocation(program, "projectionMatrix");
    program.uColorIndex = gl.getUniformLocation(program, "uColor");

    return true;
}

function initBuffers(model) {
    model.idBufferVertices = gl.createBuffer();
    gl.bindBuffer(gl.ARRAY_BUFFER, model.idBufferVertices);
    gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(model.vertices), gl.STATIC_DRAW);

    model.idBufferIndices = gl.createBuffer();
    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, model.idBufferIndices);
    gl.bufferData(gl.ELEMENT_ARRAY_BUFFER, new Uint16Array(model.indices), gl.STATIC_DRAW);
}

function initRendering() {
    gl.clearColor(0.15, 0.15, 0.15, 1.0);

    // Actividad 4: z-buffer (test de profundidad)
    gl.enable(gl.DEPTH_TEST);
    gl.clearDepth(1.0);
}

// --- Color por primitiva (triángulo) ---
function hsvToRgb(h, s, v) {
    const i = Math.floor(h * 6);
    const f = h * 6 - i;
    const p = v * (1 - s);
    const q = v * (1 - f * s);
    const t = v * (1 - (1 - f) * s);

    let r, g, b;
    switch (i % 6) {
        case 0: r = v; g = t; b = p; break;
        case 1: r = q; g = v; b = p; break;
        case 2: r = p; g = v; b = t; break;
        case 3: r = p; g = q; b = v; break;
        case 4: r = t; g = p; b = v; break;
        case 5: r = v; g = p; b = q; break;
    }
    return [r, g, b, 1.0];
}

function setColorForCube(cubeIndex) {
    // Hue distribuido para que cada cubo salga distinto
    const h = (cubeIndex * 0.61803398875) % 1.0;
    const rgba = hsvToRgb(h, 0.65, 0.95);
    gl.uniform4fv(program.uColorIndex, rgba);
}


// Dibuja el modelo como TRIÁNGULOS, cambiando el color por cada triángulo
function drawTriangles(model) {
    gl.bindBuffer(gl.ARRAY_BUFFER, model.idBufferVertices);
    gl.vertexAttribPointer(program.vertexPositionAttribute, 3, gl.FLOAT, false, 0, 0);

    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, model.idBufferIndices);

    // Dibuja todos los triángulos del cubo de golpe
    gl.drawElements(gl.TRIANGLES, model.indices.length, gl.UNSIGNED_SHORT, 0);
}


function buildModelMatrix(pos, rotXYZ, scl) {
    const m = mat4.create();
    mat4.translate(m, m, pos);
    mat4.rotateX(m, m, rotXYZ[0]);
    mat4.rotateY(m, m, rotXYZ[1]);
    mat4.rotateZ(m, m, rotXYZ[2]);
    mat4.scale(m, m, scl);
    return m;
}

// Dibuja la grúa (estática) con una view+proj dadas
function drawCrane(view, proj) {
    gl.uniformMatrix4fv(program.projectionMatrixIndex, false, proj);

    // Transform global para que quede centrada y no sea gigante
    const worldScale = 0.25;
    const worldTranslate = [-1.1, -1.4, 0.0];

    const world = mat4.create();
    mat4.translate(world, world, worldTranslate);
    mat4.scale(world, world, [worldScale, worldScale, worldScale]);

    let cubeCounter = 0;

    function drawPart(localModel) {
        const model = mat4.create();
        mat4.multiply(model, world, localModel);

        const modelView = mat4.create();
        mat4.multiply(modelView, view, model);
        gl.uniformMatrix4fv(program.modelViewMatrixIndex, false, modelView);

        setColorForCube(cubeCounter++); // <-- color único por cubo/pieza
        drawTriangles(cubeModel);
    }


    // ===== Grúa estática =====

    // Base
    drawPart(buildModelMatrix([0, 0.25, 0], [0, 0, 0], [2.5, 0.5, 2.5]));

    // Pie (vertical, longitud 10)
    drawPart(buildModelMatrix([0, 5.0, 0], [0, 0, 0], [1.0, 10.0, 1.0]));

    // Brazo (horizontal, longitud 10)
    drawPart(buildModelMatrix([5.0, 10.5, 0], [0, 0, 0], [10.0, 1.0, 1.0]));

    // Contrapeso (lado 1.4)
    drawPart(buildModelMatrix([-1.0, 10.5, 0], [0, 0, 0], [2.0, 2.0, 2.0]));

    // Carga (lado 1)
    drawPart(buildModelMatrix([10.0, 7.0, 0], [0, 0, 0], [1.0, 1.0, 1.0]));

    // Cable (fino)
    drawPart(buildModelMatrix([10.0, 9.0, 0], [0, 0, 0], [0.12, 3.0, 0.12]));
}

// ===================== ACTIVIDAD 4 =====================
// 4 viewports + ortográfica + 4 direcciones de vista + z-buffer + triángulos + color por triángulo
function drawScene() {
    // Actividad 4: limpiar también DEPTH
    gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

    const canvas = gl.canvas;
    const W = canvas.width;
    const H = canvas.height;

    const halfW = Math.floor(W / 2);
    const halfH = Math.floor(H / 2);

    // Proyección ortográfica ajustada al aspect del viewport
    const aspect = halfW / halfH;
    const orthoHalfH = 3.5; // “zoom” ortográfico
    const orthoHalfW = orthoHalfH * aspect;

    const proj = mat4.create();
    mat4.ortho(proj, -orthoHalfW, orthoHalfW, -orthoHalfH, orthoHalfH, 0.1, 100.0);

    const d = 20.0;

    // View 1: dirección eje Z (frontal XY)
    {
        gl.viewport(0, halfH, halfW, halfH); // arriba-izquierda
        const view = mat4.create();
        mat4.lookAt(view, [0, 0, d], [0, 0, 0], [0, 1, 0]);
        drawCrane(view, proj);
    }

    // View 2: dirección eje X (lateral YZ)
    {
        gl.viewport(halfW, halfH, halfW, halfH); // arriba-derecha
        const view = mat4.create();
        mat4.lookAt(view, [d, 0, 0], [0, 0, 0], [0, 1, 0]);
        drawCrane(view, proj);
    }

    // View 3: dirección eje Y (planta XZ)
    {
        gl.viewport(0, 0, halfW, halfH); // abajo-izquierda
        const view = mat4.create();
        // UP no puede ser paralelo a la dirección (por eso usamos Z como up)
        mat4.lookAt(view, [0, d, 0], [0, 0, 0], [0, 0, -1]);
        drawCrane(view, proj);
    }

    // View 4: dirección (1,1,1)
    {
        gl.viewport(halfW, 0, halfW, halfH); // abajo-derecha
        const view = mat4.create();
        const invLen = 1.0 / Math.sqrt(3.0);
        const eye = [d * invLen, d * invLen, d * invLen];
        mat4.lookAt(view, eye, [0, 0, 0], [0, 1, 0]);
        drawCrane(view, proj);
    }
}

function onResize() {
    const canvas = gl.canvas;
    canvas.width = canvas.clientWidth;
    canvas.height = canvas.clientHeight;
    drawScene();
}

function initWebGL() {
    gl = getWebGLContext();
    if (!gl) {
        alert("WebGL 2.0 no está disponible");
        return;
    }

    if (!initShaders()) return;
    initBuffers(cubeModel);
    initRendering();

    window.addEventListener("resize", onResize);
    onResize(); // primer dibujo
}

initWebGL();
