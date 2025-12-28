var gl, program;

// ===================== GEOMETRÍA: CUBO =====================
// Cubo centrado en el origen, lado 1 ([-0.5, 0.5])
// En el ejercicio 4: TRIÁNGULOS (relleno) + DEPTH TEST + color por cubo.
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
    // 12 triángulos = 36 índices
    triIndices: [
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

// ===================== WEBGL BASE =====================
function getWebGLContext() {
    var canvas = document.getElementById("myCanvas");
    try { return canvas.getContext("webgl2"); } catch (e) {}
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

    program.modelViewMatrixIndex = gl.getUniformLocation(program, "modelViewMatrix");
    program.projectionMatrixIndex = gl.getUniformLocation(program, "projectionMatrix");
    program.colorIndex = gl.getUniformLocation(program, "uColor");
}

function initBuffers(model) {
    model.idBufferVertices = gl.createBuffer();
    gl.bindBuffer(gl.ARRAY_BUFFER, model.idBufferVertices);
    gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(model.vertices), gl.STATIC_DRAW);

    model.idBufferTris = gl.createBuffer();
    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, model.idBufferTris);
    gl.bufferData(gl.ELEMENT_ARRAY_BUFFER, new Uint16Array(model.triIndices), gl.STATIC_DRAW);
}

function initRendering() {
    gl.clearColor(0.15, 0.15, 0.15, 1.0);

    // Ejercicio 4: z-buffer (DEPTH TEST) + limpiar también el depth buffer al dibujar
    gl.enable(gl.DEPTH_TEST);
    gl.depthFunc(gl.LEQUAL);
}

function drawTriangles(model) {
    gl.bindBuffer(gl.ARRAY_BUFFER, model.idBufferVertices);
    gl.vertexAttribPointer(program.vertexPositionAttribute, 3, gl.FLOAT, false, 0, 0);

    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, model.idBufferTris);
    gl.drawElements(gl.TRIANGLES, model.triIndices.length, gl.UNSIGNED_SHORT, 0);
}

// ===================== TRANSFORMACIONES POR CUBO =====================
/**
 * Dibuja un cubo aplicando: modelView = View * (T * R * S)
 * pos   = [x,y,z]
 * rot   = [rx,ry,rz] en grados (Euler)
 * scale = [sx,sy,sz]
 * color = [r,g,b,a]
 */
function drawCubePart(view, pos, rot, scale, color) {
    rot = rot || [0, 0, 0];

    var S = mat4.create();
    mat4.fromScaling(S, scale);

    var R = mat4.create();
    mat4.rotateX(R, R, rot[0] * Math.PI / 180);
    mat4.rotateY(R, R, rot[1] * Math.PI / 180);
    mat4.rotateZ(R, R, rot[2] * Math.PI / 180);

    var T = mat4.create();
    mat4.fromTranslation(T, pos);

    var TR = mat4.create();
    mat4.multiply(TR, T, R);

    var M = mat4.create();
    mat4.multiply(M, TR, S);        // M = T * R * S

    var MV = mat4.create();
    mat4.multiply(MV, view, M);     // MV = View * Model

    gl.uniformMatrix4fv(program.modelViewMatrixIndex, false, MV);

    // Color por “primitiva” (por cubo)
    gl.uniform4fv(program.colorIndex, color);

    // Un draw call por cubo (esto cumple “primitivas = cubos”)
    drawTriangles(cube);
}

// ===================== MODELO: CASCOS (5 CUBOS) =====================
function drawCascos(view) {
    // Colores distintos para apreciar la ocultación
    var cBaseL = [1.0, 0.2, 0.2, 1.0];
    var cBaseR = [0.2, 1.0, 0.2, 1.0];
    var cBrazoL = [0.2, 0.2, 1.0, 1.0];
    var cBrazoR = [1.0, 1.0, 0.2, 1.0];
    var cDiadema = [1.0, 0.2, 1.0, 1.0];

    // Tu modelo corregido “de pie”
    drawCubePart(view, [-3, 0.0, 0.0], [0, 0, 0], [2, 3, 3], cBaseL);  // base izq
    drawCubePart(view, [ 3, 0.0, 0.0], [0, 0, 0], [2, 3, 3], cBaseR);  // base der

    drawCubePart(view, [ 2.5, 2.5, 0.0], [0, 0, 0], [1, 2, 1], cBrazoL); // “brazo” (según tu script)
    drawCubePart(view, [-2.5, 2.5, 0.0], [0, 0, 0], [1, 2, 1], cBrazoR); // “brazo”

    drawCubePart(view, [0.0, 4.0, 0.0], [0, 0, 0], [6, 1, 1], cDiadema); // diadema
}

// ===================== EJERCICIO 4: 4 VIEWPORTS + Z-BUFFER =====================

function normalize3(v) {
    var len = Math.hypot(v[0], v[1], v[2]);
    if (len < 1e-8) return [0, 0, 0];
    return [v[0]/len, v[1]/len, v[2]/len];
}

function setViewportAndOrtho(x, y, w, h) {
    gl.viewport(x, y, w, h);

    var aspect = w / h;
    var halfH = 6.0;
    var halfW = halfH * aspect;

    var proj = mat4.create();
    mat4.ortho(proj, -halfW, halfW, -halfH, halfH, 0.1, 100.0);
    gl.uniformMatrix4fv(program.projectionMatrixIndex, false, proj);
}

function makeViewFromDirection(target, dir, distance, up) {
    dir = normalize3(dir);
    var eye = [
        target[0] - dir[0] * distance,
        target[1] - dir[1] * distance,
        target[2] - dir[2] * distance
    ];
    var view = mat4.create();
    mat4.lookAt(view, eye, target, up);
    return view;
}

function drawScene() {
    // Importante para el z-buffer: limpiar color Y profundidad
    gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

    var canvas = gl.canvas;
    var W = canvas.width;
    var H = canvas.height;
    var halfW = Math.floor(W / 2);
    var halfH = Math.floor(H / 2);

    // Centro aproximado del modelo “de pie”
    var target = [0, 2.0, 0.0];
    var dist = 18.0;

    // 1) Abajo-Izq: dirección (1,0,0)
    setViewportAndOrtho(0, 0, halfW, halfH);
    var viewX = makeViewFromDirection(target, [1, 0, 0], dist, [0, 1, 0]);
    drawCascos(viewX);

    // 2) Abajo-Der: dirección (0,1,0)
    setViewportAndOrtho(halfW, 0, W - halfW, halfH);
    var viewY = makeViewFromDirection(target, [0, -1, 0], dist, [0, 0, 1]);
    drawCascos(viewY);

    // 3) Arriba-Izq: dirección (0,0,1)
    setViewportAndOrtho(0, halfH, halfW, H - halfH);
    var viewZ = makeViewFromDirection(target, [0, 0, 1], dist, [0, 1, 0]);
    drawCascos(viewZ);

    // 4) Arriba-Der: dirección (1,1,1)
    setViewportAndOrtho(halfW, halfH, W - halfW, H - halfH);
    var view111 = makeViewFromDirection(target, [0.8, -0.8, 1], dist, [0, 1, 0]);
    drawCascos(view111);
}

function onResize() {
    const canvas = gl.canvas;
    canvas.width = canvas.clientWidth || canvas.width;
    canvas.height = canvas.clientHeight || canvas.height;
    requestAnimationFrame(drawScene);
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

    window.addEventListener('resize', onResize);
    onResize();
}

initWebGL();
