var gl, program;

// Cubo centrado en el origen, lado 1 ([-0.5, 0.5])
var cube = {
    vertices: [
        -0.5, -0.5, -0.5, // 0
        0.5, -0.5, -0.5, // 1
        0.5,  0.5, -0.5, // 2
        -0.5,  0.5, -0.5, // 3
        -0.5, -0.5,  0.5, // 4
        0.5, -0.5,  0.5, // 5
        0.5,  0.5,  0.5, // 6
        -0.5,  0.5,  0.5  // 7
    ],
    indices: [
        // Cara -Z
        0, 1, 2,   0, 2, 3,
        // Cara +Z
        4, 6, 5,   4, 7, 6,
        // Cara -Y
        4, 5, 1,   4, 1, 0,
        // Cara +Y
        7, 3, 2,   7, 2, 6,
        // Cara +X
        5, 6, 2,   5, 2, 1,
        // Cara -X
        4, 0, 3,   4, 3, 7
    ]
};

function getWebGLContext() {
    var canvas = document.getElementById("myCanvas");
    try {
        return canvas.getContext("webgl2");
    } catch (e) {}
    return null;
}

function initShaders() {
    const vs = gl.createShader(gl.VERTEX_SHADER);
    gl.shaderSource(vs, document.getElementById("myVertexShader").textContent);
    gl.compileShader(vs);
    if (!gl.getShaderParameter(vs, gl.COMPILE_STATUS)) {
        console.error("Vertex shader error:", gl.getShaderInfoLog(vs));
        return;
    }

    const fs = gl.createShader(gl.FRAGMENT_SHADER);
    gl.shaderSource(fs, document.getElementById("myFragmentShader").textContent);
    gl.compileShader(fs);
    if (!gl.getShaderParameter(fs, gl.COMPILE_STATUS)) {
        console.error("Fragment shader error:", gl.getShaderInfoLog(fs));
        return;
    }

    program = gl.createProgram();
    gl.attachShader(program, vs);
    gl.attachShader(program, fs);
    gl.linkProgram(program);
    if (!gl.getProgramParameter(program, gl.LINK_STATUS)) {
        console.error("Program link error:", gl.getProgramInfoLog(program));
        program = undefined;
        return;
    }
    gl.useProgram(program);

    program.vertexPositionAttribute = gl.getAttribLocation(program, "VertexPosition");
    gl.enableVertexAttribArray(program.vertexPositionAttribute);

    program.modelMatrixIndex = gl.getUniformLocation(program, "M");
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
    gl.viewport(0, 0, gl.canvas.width, gl.canvas.height);
    gl.clearColor(0.15, 0.15, 0.15, 1.0);

    gl.enable(gl.DEPTH_TEST);
    gl.depthFunc(gl.LEQUAL);
}

function draw(model) {
    gl.bindBuffer(gl.ARRAY_BUFFER, model.idBufferVertices);
    gl.vertexAttribPointer(program.vertexPositionAttribute, 3, gl.FLOAT, false, 0, 0);

    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, model.idBufferIndices);

    // Wireframe simple: cada triángulo como LINE_LOOP (dibujará diagonales)
    for (var i = 0; i < model.indices.length; i += 3) {
        gl.drawElements(gl.LINE_LOOP, 3, gl.UNSIGNED_SHORT, i * 2);
    }
}

/**
 * Dibuja un cubo aplicando: M = G * (T * R * S)
 * pos   = [x,y,z]
 * rot   = [rx,ry,rz] en radianes (Euler)
 * scale = [sx,sy,sz]
 */
function drawCubePart(G, pos, rot, scale) {
    // Defaults seguros si no pasas rot
    rot = rot || [0, 0, 0];

    var S = mat4.create();
    mat4.fromScaling(S, scale);

    var R = mat4.create();
    // Orden Euler (puedes cambiarlo si tu profe exige otro):
    // X -> Y -> Z
    mat4.rotateX(R, R, rot[0]*Math.PI/180);
    mat4.rotateY(R, R, rot[1]*Math.PI/180);
    mat4.rotateZ(R, R, rot[2]*Math.PI/180);

    var T = mat4.create();
    mat4.fromTranslation(T, pos);

    // TR = T * R
    var TR = mat4.create();
    mat4.multiply(TR, T, R);

    // TRS = (T * R) * S
    var TRS = mat4.create();
    mat4.multiply(TRS, TR, S);

    // M = G * TRS
    var M = mat4.create();
    mat4.multiply(M, G, TRS);

    gl.uniformMatrix4fv(program.modelMatrixIndex, false, M);
    draw(cube);
}

function drawScene() {
    gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

    // Matriz global: "vista" plana como tu captura
    var G = mat4.create();

    // Z -> Y (vista frontal tipo captura)
    mat4.rotateX(G, G, -90*Math.PI/180);
    mat4.rotateY(G, G, 0*Math.PI/180);
    mat4.rotateZ(G, G, 0*Math.PI/180);

    // Escala global para que quepa en clip-space
    mat4.scale(G, G, [0.15, 0.15, 0.15]);

    // === Auriculares (5 piezas, sin extras) ===
    // Construidos en plano X-Z (Y=0), luego el giro anterior los deja “como en la foto”.

    // Bases (2)  (3x1x3)
    drawCubePart(G, [-3, 0.0, -0.5], [0, 0, 0], [2, 3, 3]); // izquierda
    drawCubePart(G, [ 3, 0.0, -0.5], [0, 0, 0], [2, 3, 3]); // derecha

    // Columnas / brazos (2) (1x1x2)
    drawCubePart(G, [-2.5, 0.0,  2.0], [0, 0, 0], [1, 1, 2]); // izquierda
    drawCubePart(G, [ 2.5, 0.0,  2.0], [0, 0, 0], [1, 1, 2]); // derecha

    // Viga superior / diadema (1) (6x1x1)
    drawCubePart(G, [0.0, 0.0, 3.5], [0, 0, 0], [6, 1, 1]);
}

function initWebGL() {
    gl = getWebGLContext();
    if (!gl) {
        alert("WebGL 2.0 no está disponible");
        return;
    }

    initShaders();
    initBuffers(cube);
    initRendering();

    // Un solo frame (sin animación)
    requestAnimationFrame(drawScene);
}

initWebGL();
