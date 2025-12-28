var gl, program;

var exampleCube = {
    vertices: [
        // Cara frontal (normal 0, 0, 1)
        -0.5, -0.5,  0.5,   0.0, 0.0, 1.0,
        0.5, -0.5,  0.5,   0.0, 0.0, 1.0,
        0.5,  0.5,  0.5,   0.0, 0.0, 1.0,
        -0.5,  0.5,  0.5,   0.0, 0.0, 1.0,

        // Cara derecha (normal 1, 0, 0)
        0.5, -0.5,  0.5,   1.0, 0.0, 0.0,
        0.5, -0.5, -0.5,   1.0, 0.0, 0.0,
        0.5,  0.5, -0.5,   1.0, 0.0, 0.0,
        0.5,  0.5,  0.5,   1.0, 0.0, 0.0,

        // Cara trasera (normal 0, 0, -1)
        0.5, -0.5, -0.5,   0.0, 0.0, -1.0,
        -0.5, -0.5, -0.5,   0.0, 0.0, -1.0,
        -0.5,  0.5, -0.5,   0.0, 0.0, -1.0,
        0.5,  0.5, -0.5,   0.0, 0.0, -1.0,

        // Cara izquierda (normal -1, 0, 0)
        -0.5, -0.5, -0.5,  -1.0, 0.0, 0.0,
        -0.5, -0.5,  0.5,  -1.0, 0.0, 0.0,
        -0.5,  0.5,  0.5,  -1.0, 0.0, 0.0,
        -0.5,  0.5, -0.5,  -1.0, 0.0, 0.0,

        // Cara superior (normal 0, 1, 0)
        -0.5,  0.5,  0.5,   0.0, 1.0, 0.0,
        0.5,  0.5,  0.5,   0.0, 1.0, 0.0,
        0.5,  0.5, -0.5,   0.0, 1.0, 0.0,
        -0.5,  0.5, -0.5,   0.0, 1.0, 0.0,

        // Cara inferior (normal 0, -1, 0)
        -0.5, -0.5, -0.5,   0.0, -1.0, 0.0,
        0.5, -0.5, -0.5,   0.0, -1.0, 0.0,
        0.5, -0.5,  0.5,   0.0, -1.0, 0.0,
        -0.5, -0.5,  0.5,   0.0, -1.0, 0.0
    ],

    indices: [
        0,  1,  2,   0,  2,  3,
        4,  5,  6,   4,  6,  7,
        8,  9, 10,   8, 10, 11,
        12, 13, 14,  12, 14, 15,
        16, 17, 18,  16, 18, 19,
        20, 21, 22,  20, 22, 23
    ]
};

// Material
var Silver = {
    mat_ambient:  [0.19225, 0.19225, 0.19225],
    mat_diffuse:  [0.50754, 0.50754, 0.50754],
    mat_specular: [0.50827, 0.50827, 0.50827],
    alpha: 51.2
};

function getWebGLContext() {
    var canvas = document.getElementById("myCanvas");
    try { return canvas.getContext("webgl2"); } catch (e) { }
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

    program.vertexNormalAttribute = gl.getAttribLocation(program, "VertexNormal");
    program.normalMatrixIndex = gl.getUniformLocation(program, "normalMatrix");
    gl.enableVertexAttribArray(program.vertexNormalAttribute);

    program.KaIndex    = gl.getUniformLocation(program, "Material.Ka");
    program.KdIndex    = gl.getUniformLocation(program, "Material.Kd");
    program.KsIndex    = gl.getUniformLocation(program, "Material.Ks");
    program.alphaIndex = gl.getUniformLocation(program, "Material.alpha");

    program.LaIndex       = gl.getUniformLocation(program, "Light.La");
    program.LdIndex       = gl.getUniformLocation(program, "Light.Ld");
    program.LsIndex       = gl.getUniformLocation(program, "Light.Ls");
    program.PositionIndex = gl.getUniformLocation(program, "Light.Position");
}

function getNormalMatrix(modelViewMatrix) {
    var normalMatrix = mat3.create();
    mat3.normalFromMat4(normalMatrix, modelViewMatrix);
    return normalMatrix;
}

function setShaderNormalMatrix(normalMatrix) {
    gl.uniformMatrix3fv(program.normalMatrixIndex, false, normalMatrix);
}

function setShaderMaterial(material) {
    gl.uniform3fv(program.KaIndex, material.mat_ambient);
    gl.uniform3fv(program.KdIndex, material.mat_diffuse);
    gl.uniform3fv(program.KsIndex, material.mat_specular);
    gl.uniform1f(program.alphaIndex, material.alpha);
}

function setShaderLight() {
    gl.uniform3f(program.LaIndex, 1.0, 1.0, 1.0);
    gl.uniform3f(program.LdIndex, 1.0, 1.0, 1.0);
    gl.uniform3f(program.LsIndex, 1.0, 1.0, 1.0);
    // En coordenadas del ojo (luz "pegada" a la cámara)
    gl.uniform3f(program.PositionIndex, 10.0, 10.0, 0.0);
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
    gl.enable(gl.DEPTH_TEST);
    gl.clearColor(0.15, 0.15, 0.15, 1.0);
}

function drawSolid(model) {
    gl.bindBuffer(gl.ARRAY_BUFFER, model.idBufferVertices);

    gl.vertexAttribPointer(
        program.vertexPositionAttribute,
        3, gl.FLOAT, false,
        2 * 3 * 4,
        0
    );

    gl.vertexAttribPointer(
        program.vertexNormalAttribute,
        3, gl.FLOAT, false,
        2 * 3 * 4,
        3 * 4
    );

    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, model.idBufferIndices);
    gl.drawElements(gl.TRIANGLES, model.indices.length, gl.UNSIGNED_SHORT, 0);
}

// ===================== CÁMARA ORBITAL =====================
const camera = {
    target: [0, 0, 0],
    distance: 3.0,
    yaw: 0.0,
    pitch: 0.3,
    fov: Math.PI / 4,
    near: 0.1,
    far: 100.0,
    minDistance: 0.5,
    maxDistance: 20.0
};
let isDragging = false;
let lastX = 0, lastY = 0;
let dragMode = 'rotate';

function clamp(val, min, max) { return Math.max(min, Math.min(max, val)); }

function getEyePosition() {
    const x = camera.target[0] + camera.distance * Math.cos(camera.pitch) * Math.sin(camera.yaw);
    const y = camera.target[1] + camera.distance * Math.sin(camera.pitch);
    const z = camera.target[2] + camera.distance * Math.cos(camera.pitch) * Math.cos(camera.yaw);
    return [x, y, z];
}

function updateCameraUniforms(modelMatrix) {
    const eye = getEyePosition();
    const view = mat4.create();
    mat4.lookAt(view, eye, camera.target, [0, 1, 0]);

    const proj = mat4.create();
    const aspect = gl.canvas.width / gl.canvas.height;
    mat4.perspective(proj, camera.fov, aspect, camera.near, camera.far);

    const modelView = mat4.create();
    mat4.multiply(modelView, view, modelMatrix);

    gl.uniformMatrix4fv(program.modelViewMatrixIndex, false, modelView);
    gl.uniformMatrix4fv(program.projectionMatrixIndex, false, proj);

    const normalMatrix = getNormalMatrix(modelView);
    setShaderNormalMatrix(normalMatrix);
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
        for (let i=0; i<3; ++i) {
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
    canvas.width = canvas.clientWidth;
    canvas.height = canvas.clientHeight;
    gl.viewport(0, 0, canvas.width, canvas.height);
    requestAnimationFrame(drawScene);
}
// ===================== FIN CÁMARA ORBITAL =====================

function drawScene() {
    gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

    var modelMatrix = mat4.create();
    mat4.fromScaling(modelMatrix, [0.5, 0.5, 0.5]);

    updateCameraUniforms(modelMatrix);
    setShaderMaterial(Silver);
    setShaderLight();
    drawSolid(exampleCube);
}

function initWebGL() {
    gl = getWebGLContext();
    if (!gl) {
        alert("WebGL 2.0 no está disponible");
        return;
    }
    initShaders();
    initBuffers(exampleCube);
    initRendering();

    const canvas = gl.canvas;
    canvas.addEventListener('pointerdown', onPointerDown);
    canvas.addEventListener('pointermove', onPointerMove);
    canvas.addEventListener('pointerup', onPointerUp);
    canvas.addEventListener('wheel', onWheel, {passive:false});
    window.addEventListener('resize', onResize);
    onResize();
}

initWebGL();
