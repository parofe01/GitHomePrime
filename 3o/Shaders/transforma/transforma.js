"use strict";

let gl, program;

// Cubo (triangulado) centrado en el origen, lado 1
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

    program.MIndex = gl.getUniformLocation(program, "M");
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
    gl.enable(gl.DEPTH_TEST);
    gl.viewport(0, 0, gl.canvas.width, gl.canvas.height);
}

function draw(model) {
    gl.bindBuffer(gl.ARRAY_BUFFER, model.idBufferVertices);
    gl.vertexAttribPointer(program.vertexPositionAttribute, 3, gl.FLOAT, false, 0, 0);

    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, model.idBufferIndices);

    // Alambre por triángulos (LINE_LOOP de 3 en 3)
    for (let i = 0; i < model.indices.length; i += 3) {
        gl.drawElements(gl.LINE_LOOP, 3, gl.UNSIGNED_SHORT, i * 2);
    }
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

function drawScene() {
    gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

    // Vista “lateral” plana (ortográfica) mirando al plano XY (a lo largo del eje Z)
    const view = mat4.create();
    mat4.lookAt(view, [0, 6, 20], [0, 6, 0], [0, 1, 0]); // ojo en +Z, mira al centro

    const proj = mat4.create();
    const aspect = gl.canvas.width / gl.canvas.height;

    // “zoom” ortográfico: súbelo para ver más, bájalo para acercar
    const orthoSize = 6.0;
    const halfH = orthoSize;
    const halfW = orthoSize * aspect;

    mat4.ortho(proj, -halfW, halfW, -halfH, halfH, 0.1, 100.0);

    // Ajustes globales de la grúa (más pequeña y centrada)
    const craneScale = 0.5;
    const craneOffset = [-4.4, 5, 0.0];

    const world = mat4.create();
    mat4.scale(world, world, [craneScale, craneScale, craneScale]);
    mat4.translate(world, world, craneOffset);

    function setM(modelMatrix) {
        const mv = mat4.create();
        mat4.multiply(mv, view, modelMatrix);    // mv = V * M
        const mvp = mat4.create();
        mat4.multiply(mvp, proj, mv);            // mvp = P * mv
        gl.uniformMatrix4fv(program.MIndex, false, mvp);
    }

    function drawPart(localModel) {
        const model = mat4.create();
        mat4.multiply(model, world, localModel); // model = world * local
        setM(model);
        draw(cubeModel);
    }

    // ===== Grúa estática (solo “que exista”) =====

    // Base
    drawPart(buildModelMatrix([0, 0.25, 0], [0, 0, 0], [2.5, 0.5, 2.5]));

    // Pie (vertical, longitud 10) -> centro y=5 para ir de 0 a 10
    drawPart(buildModelMatrix([0, 5.0, 0], [0, 0, 0], [1.0, 10.0, 1.0]));

    // Brazo (horizontal, longitud 10) -> centro x=5 para ir de 0 a 10
    drawPart(buildModelMatrix([5.0, 10.5, 0], [0, 0, 0], [10.0, 1.0, 1.0]));

    // Contrapeso (lado 1.4)
    drawPart(buildModelMatrix([-1.0, 10.5, 0], [0, 0, 0], [2.0, 2.0, 20.]));

    // Carga (lado 1)
    drawPart(buildModelMatrix([10.0, 7.0, 0], [0, 0, 0], [1.0, 1.0, 1.0]));

    // Cable (fino)
    drawPart(buildModelMatrix([10.0, 9.0, 0], [0, 0, 0], [0.12, 3.0, 0.12]));
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

    // Dibujo único (sin animación ni interacción)
    drawScene();
}

initWebGL();
