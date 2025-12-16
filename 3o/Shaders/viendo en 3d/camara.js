var gl, program;

// Prisma triangular
var triangularPrism = {
    vertices: [
        // Base triangle (bottom)
        -0.5, -0.5, -0.5,
        0.5, -0.5, -0.5,
        0.0, -0.5, 0.5,
        // Top triangle (above base)
        -0.5, 0.5, -0.5,
        0.5, 0.5, -0.5,
        0.0, 0.5, 0.5
    ],
    indices: [
        0, 1, 2,    // Base
        3, 5, 4,    // Top
        0, 3, 1, 1, 3, 4,    // Side 1
        1, 4, 2, 2, 4, 5,    // Side 2
        2, 5, 0, 0, 5, 3     // Side 3
    ]
};

function getWebGLContext() {
    var canvas = document.getElementById("myCanvas");
    try {
        return canvas.getContext("webgl2");
    } catch (e) { }
    return null;
}

function initShaders() {
  const vs = gl.createShader(gl.VERTEX_SHADER);
  gl.shaderSource(vs, document.getElementById("myVertexShader").textContent);
  gl.compileShader(vs);
  if (!gl.getShaderParameter(vs, gl.COMPILE_STATUS)) {
    console.error("Vertex shader error:", gl.getShaderInfoLog(vs));
    return;  // ojo: si retornas aquí, 'program' nunca se asigna
  }

  const fs = gl.createShader(gl.FRAGMENT_SHADER);
  gl.shaderSource(fs, document.getElementById("myFragmentShader").textContent);
  gl.compileShader(fs);
  if (!gl.getShaderParameter(fs, gl.COMPILE_STATUS)) {
    console.error("Fragment shader error:", gl.getShaderInfoLog(fs));
    return;
  }

  // Solo si llegamos aquí creamos y enlazamos el programa
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

  // Obtener y habilitar el atributo
  program.vertexPositionAttribute = gl.getAttribLocation(program, "VertexPosition");
  gl.enableVertexAttribArray(program.vertexPositionAttribute);

  // Obtener la ubicación de los uniforms correctos según el shader
  program.modelViewMatrixIndex = gl.getUniformLocation(program, "modelViewMatrix");
  program.projectionMatrixIndex = gl.getUniformLocation(program, "projectionMatrix");

}


function initBuffers(model) {
    // Buffer de vértices
    model.idBufferVertices = gl.createBuffer();
    gl.bindBuffer(gl.ARRAY_BUFFER, model.idBufferVertices);
    gl.bufferData(gl.ARRAY_BUFFER,
        new Float32Array(model.vertices), gl.STATIC_DRAW);

    // Buffer de índices
    model.idBufferIndices = gl.createBuffer();
    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, model.idBufferIndices);
    gl.bufferData(gl.ELEMENT_ARRAY_BUFFER,
        new Uint16Array(model.indices), gl.STATIC_DRAW);
}

function initRendering() {
    gl.clearColor(0.15, 0.15, 0.15, 1.0);
}

function draw(model) {
    gl.bindBuffer(gl.ARRAY_BUFFER, model.idBufferVertices);
    gl.vertexAttribPointer(
        program.vertexPositionAttribute,
        3, gl.FLOAT, false, 0, 0
    );
    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, model.idBufferIndices);
    for (var i = 0; i < model.indices.length; i+= 3)
        gl.drawElements(gl.LINE_LOOP, 3, gl.UNSIGNED_SHORT, i*2);
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
let dragMode = 'rotate'; // 'rotate' o 'pan'

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
    } else if (dragMode === 'pan') {
        // Pan en el plano de la cámara
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
    var modelMatrix = mat4.create();
    mat4.fromScaling(modelMatrix, [0.5, 0.5, 0.5]);
    updateCameraUniforms(modelMatrix);
    draw(triangularPrism);
}

function initWebGL() {
    gl = getWebGLContext();
    if (!gl) {
        alert("WebGL 2.0 no está disponible");
        return;
    }
    initShaders();
    initBuffers(triangularPrism);
    initRendering();
    // Eventos de cámara interactiva
    const canvas = gl.canvas;
    canvas.addEventListener('pointerdown', onPointerDown);
    canvas.addEventListener('pointermove', onPointerMove);
    canvas.addEventListener('pointerup', onPointerUp);
    canvas.addEventListener('wheel', onWheel, {passive:false});
    window.addEventListener('resize', onResize);
    // Ajuste inicial de viewport
    onResize();
}

initWebGL();