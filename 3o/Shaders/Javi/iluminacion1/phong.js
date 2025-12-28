var gl, program;

// ===================== GEOMETRÍA: CUBO SÓLIDO CON NORMALES =====================
// Vértices duplicados por cara (24) para normales “planas” por cara.
// Layout intercalado: position.xyz, normal.xyz
var cubeSolid = {
    vertices: [
        // Cara frontal (0,0,1)
        -0.5, -0.5,  0.5,   0.0, 0.0, 1.0,
        0.5, -0.5,  0.5,   0.0, 0.0, 1.0,
        0.5,  0.5,  0.5,   0.0, 0.0, 1.0,
        -0.5,  0.5,  0.5,   0.0, 0.0, 1.0,

        // Cara derecha (1,0,0)
        0.5, -0.5,  0.5,   1.0, 0.0, 0.0,
        0.5, -0.5, -0.5,   1.0, 0.0, 0.0,
        0.5,  0.5, -0.5,   1.0, 0.0, 0.0,
        0.5,  0.5,  0.5,   1.0, 0.0, 0.0,

        // Cara trasera (0,0,-1)
        0.5, -0.5, -0.5,   0.0, 0.0, -1.0,
        -0.5, -0.5, -0.5,   0.0, 0.0, -1.0,
        -0.5,  0.5, -0.5,   0.0, 0.0, -1.0,
        0.5,  0.5, -0.5,   0.0, 0.0, -1.0,

        // Cara izquierda (-1,0,0)
        -0.5, -0.5, -0.5,  -1.0, 0.0, 0.0,
        -0.5, -0.5,  0.5,  -1.0, 0.0, 0.0,
        -0.5,  0.5,  0.5,  -1.0, 0.0, 0.0,
        -0.5,  0.5, -0.5,  -1.0, 0.0, 0.0,

        // Cara superior (0,1,0)
        -0.5,  0.5,  0.5,   0.0, 1.0, 0.0,
        0.5,  0.5,  0.5,   0.0, 1.0, 0.0,
        0.5,  0.5, -0.5,   0.0, 1.0, 0.0,
        -0.5,  0.5, -0.5,   0.0, 1.0, 0.0,

        // Cara inferior (0,-1,0)
        -0.5, -0.5, -0.5,   0.0, -1.0, 0.0,
        0.5, -0.5, -0.5,   0.0, -1.0, 0.0,
        0.5, -0.5,  0.5,   0.0, -1.0, 0.0,
        -0.5, -0.5,  0.5,   0.0, -1.0, 0.0
    ],

    // Triángulos (2 por cara). Reordenados para que el “provoking vertex” sea el mismo
    // si luego quieres comparar con flat sin diagonales raras.
    indices: [
        // Frontal (0,1,2) (0,3,2)
        0,  1,  2,   0,  3,  2,
        // Derecha (4,5,6) (4,7,6)
        4,  5,  6,   4,  7,  6,
        // Trasera (8,9,10) (8,11,10)
        8,  9, 10,   8, 11, 10,
        // Izquierda (12,13,14) (12,15,14)
        12, 13, 14,  12, 15, 14,
        // Superior (16,17,18) (16,19,18)
        16, 17, 18,  16, 19, 18,
        // Inferior (20,21,22) (20,23,22)
        20, 21, 22,  20, 23, 22
    ]
};

// ===================== MATERIAL =====================
var Silver = {
    mat_ambient:  [0.19225, 0.19225, 0.19225],
    mat_diffuse:  [0.50754, 0.50754, 0.50754],
    mat_specular: [0.50827, 0.50827, 0.50827],
    alpha: 51.2
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

    program.vertexNormalAttribute = gl.getAttribLocation(program, "VertexNormal");
    gl.enableVertexAttribArray(program.vertexNormalAttribute);

    program.modelViewMatrixIndex   = gl.getUniformLocation(program, "modelViewMatrix");
    program.projectionMatrixIndex  = gl.getUniformLocation(program, "projectionMatrix");
    program.normalMatrixIndex      = gl.getUniformLocation(program, "normalMatrix");

    program.KaIndex    = gl.getUniformLocation(program, "Material.Ka");
    program.KdIndex    = gl.getUniformLocation(program, "Material.Kd");
    program.KsIndex    = gl.getUniformLocation(program, "Material.Ks");
    program.alphaIndex = gl.getUniformLocation(program, "Material.alpha");

    program.LaIndex       = gl.getUniformLocation(program, "Light.La");
    program.LdIndex       = gl.getUniformLocation(program, "Light.Ld");
    program.LsIndex       = gl.getUniformLocation(program, "Light.Ls");
    program.PositionIndex = gl.getUniformLocation(program, "Light.Position");
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
    gl.depthFunc(gl.LEQUAL);
}

// ===================== MATRIZ NORMAL =====================
function getNormalMatrix(modelViewMatrix) {
    var normalMatrix = mat3.create();
    mat3.normalFromMat4(normalMatrix, modelViewMatrix);
    return normalMatrix;
}

function setShaderNormalMatrix(normalMatrix) {
    gl.uniformMatrix3fv(program.normalMatrixIndex, false, normalMatrix);
}

// ===================== MATERIAL / LUZ =====================
function setShaderMaterial(material) {
    gl.uniform3fv(program.KaIndex, material.mat_ambient);
    gl.uniform3fv(program.KdIndex, material.mat_diffuse);
    gl.uniform3fv(program.KsIndex, material.mat_specular);
    gl.uniform1f(program.alphaIndex, material.alpha);
}

// Luz en coordenadas del ojo (fija respecto a cámara)
function setShaderLight() {
    gl.uniform3f(program.LaIndex, 1.0, 1.0, 1.0);
    gl.uniform3f(program.LdIndex, 1.0, 1.0, 1.0);
    gl.uniform3f(program.LsIndex, 1.0, 1.0, 1.0);
    gl.uniform3f(program.PositionIndex, 10.0, 10.0, 0.0);
}

// ===================== DIBUJO SÓLIDO (pos+normal) =====================
function drawSolid(model) {
    gl.bindBuffer(gl.ARRAY_BUFFER, model.idBufferVertices);

    // Posición: 3 floats
    gl.vertexAttribPointer(
        program.vertexPositionAttribute,
        3, gl.FLOAT, false,
        6 * 4,
        0
    );

    // Normal: 3 floats
    gl.vertexAttribPointer(
        program.vertexNormalAttribute,
        3, gl.FLOAT, false,
        6 * 4,
        3 * 4
    );

    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, model.idBufferIndices);
    gl.drawElements(gl.TRIANGLES, model.indices.length, gl.UNSIGNED_SHORT, 0);
}

// ===================== CÁMARA ORBITAL =====================
const camera = {
    target: [0, 1.5, 0],
    distance: 15.0,
    yaw: 0.7,
    pitch: 0.25,
    fov: Math.PI / 4,
    near: 0.1,
    far: 200.0,
    minDistance: 2.0,
    maxDistance: 60.0
};

let isDragging = false;
let lastX = 0, lastY = 0;
let dragMode = 'rotate'; // 'rotate' o 'pan'

function clamp(val, min, max) { return Math.max(min, Math.min(max, val)); }

function getEyePosition() {
    const x = camera.target[0] + camera.distance * Math.cos(camera.pitch) * Math.sin(camera.yaw);
    const y = camera.target[1] + camera.distance * Math.sin(camera.pitch);
    const z = camera.target[2] + camera.distance * Math.cos(camera.pitch) * Math.cos(camera.yaw);
    return [x, y, z];
}

function onPointerDown(e) {
    isDragging = true;
    lastX = e.clientX;
    lastY = e.clientY;
    dragMode = (e.shiftKey || e.button === 1) ? 'pan' : 'rotate';
    e.target.setPointerCapture(e.pointerId);
}

function onPointerMove(e) {
    if (!isDragging) return;
    const dx = e.clientX - lastX;
    const dy = e.clientY - lastY;
    lastX = e.clientX;
    lastY = e.clientY;

    if (dragMode === 'rotate') {
        camera.yaw += dx * 0.01;
        camera.pitch += dy * 0.01;
        camera.pitch = clamp(camera.pitch, -Math.PI/2 + 0.05, Math.PI/2 - 0.05);
    } else {
        const eye = getEyePosition();
        const viewDir = [
            camera.target[0] - eye[0],
            camera.target[1] - eye[1],
            camera.target[2] - eye[2]
        ];
        const right = vec3.create();
        vec3.cross(right, viewDir, [0,1,0]);
        vec3.normalize(right, right);
        const up = vec3.create();
        vec3.cross(up, right, viewDir);
        vec3.normalize(up, up);

        const panSpeed = camera.distance * 0.002;
        for (let i = 0; i < 3; ++i) {
            camera.target[i] -= right[i] * dx * panSpeed;
            camera.target[i] += up[i] * dy * panSpeed;
        }
    }

    requestAnimationFrame(drawScene);
}

function onPointerUp(e) {
    isDragging = false;
    e.target.releasePointerCapture(e.pointerId);
}

function onWheel(e) {
    e.preventDefault();
    camera.distance *= 1 + e.deltaY * 0.001;
    camera.distance = clamp(camera.distance, camera.minDistance, camera.maxDistance);
    requestAnimationFrame(drawScene);
}

function onResize() {
    const canvas = gl.canvas;
    canvas.width  = canvas.clientWidth  || canvas.width;
    canvas.height = canvas.clientHeight || canvas.height;
    gl.viewport(0, 0, canvas.width, canvas.height);
    requestAnimationFrame(drawScene);
}

// ===================== TRANSFORM Y MODELO: CASCOS =====================
function drawCubePart(view, pos, rotDeg, scale) {
    rotDeg = rotDeg || [0,0,0];

    const S = mat4.create();
    mat4.fromScaling(S, scale);

    const R = mat4.create();
    mat4.rotateX(R, R, rotDeg[0] * Math.PI / 180);
    mat4.rotateY(R, R, rotDeg[1] * Math.PI / 180);
    mat4.rotateZ(R, R, rotDeg[2] * Math.PI / 180);

    const T = mat4.create();
    mat4.fromTranslation(T, pos);

    const TR = mat4.create();
    mat4.multiply(TR, T, R);

    const M = mat4.create();
    mat4.multiply(M, TR, S); // Model = T*R*S

    const MV = mat4.create();
    mat4.multiply(MV, view, M);

    gl.uniformMatrix4fv(program.modelViewMatrixIndex, false, MV);
    setShaderNormalMatrix(getNormalMatrix(MV));

    drawSolid(cubeSolid);
}

function drawCascos(view) {
    // Bases (2) (2x3x3)
    drawCubePart(view, [-3, 0.0, 0.0], [0,0,0], [2, 3, 3]); // izquierda
    drawCubePart(view, [ 3, 0.0, 0.0], [0,0,0], [2, 3, 3]); // derecha

    // Columnas / brazos (2) (1x2x1)
    drawCubePart(view, [ 2.5, 2.5, 0.0], [0,0,0], [1, 2, 1]); // derecha
    drawCubePart(view, [-2.5, 2.5, 0.0], [0,0,0], [1, 2, 1]); // izquierda

    // Diadema (1) (6x1x1)
    drawCubePart(view, [0.0, 4.0, 0.0], [0,0,0], [6, 1, 1]);
}

// ===================== ESCENA =====================
function drawScene() {
    gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

    const eye = getEyePosition();

    const view = mat4.create();
    mat4.lookAt(view, eye, camera.target, [0, 1, 0]);

    const proj = mat4.create();
    const aspect = gl.canvas.width / gl.canvas.height;
    mat4.perspective(proj, camera.fov, aspect, camera.near, camera.far);

    gl.uniformMatrix4fv(program.projectionMatrixIndex, false, proj);

    setShaderMaterial(Silver);
    setShaderLight();

    drawCascos(view);
}

function initWebGL() {
    gl = getWebGLContext();
    if (!gl) {
        alert("WebGL 2.0 no está disponible");
        return;
    }

    initShaders();
    initBuffers(cubeSolid);
    initRendering();

    const canvas = gl.canvas;
    canvas.addEventListener('pointerdown', onPointerDown);
    canvas.addEventListener('pointermove', onPointerMove);
    canvas.addEventListener('pointerup', onPointerUp);
    canvas.addEventListener('wheel', onWheel, { passive:false });
    window.addEventListener('resize', onResize);

    onResize();
}

initWebGL();
