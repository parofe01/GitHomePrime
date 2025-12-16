var gl, program;
var cube, wheel, turret, cannon;

function createCube() {
    return {
        vertices: [
            // Frente
            -0.5, -0.5, 0.5,
            0.5, -0.5, 0.5,
            0.5, 0.5, 0.5,
            -0.5, 0.5, 0.5,
            // Atrás
            -0.5, -0.5, -0.5,
            0.5, -0.5, -0.5,
            0.5, 0.5, -0.5,
            -0.5, 0.5, -0.5,
        ],
        indices: [
            0, 1, 2, 0, 2, 3,    // Frente
            4, 5, 6, 4, 6, 7,    // Atrás
            0, 4, 7, 0, 7, 3,    // Izquierda
            1, 5, 6, 1, 6, 2,    // Derecha
            3, 2, 6, 3, 6, 7,    // Arriba
            0, 1, 5, 0, 5, 4     // Abajo
        ]
    };
}

// --- Crear cilindro con tapas y radios ---
function createWheelWithSpokes(segments = 20, radius = 0.1, height = 0.8) {
    let vertices = [];
    let indices = [];

    // Laterales
    for (let i = 0; i <= segments; i++) {
        let theta = (i / segments) * 2 * Math.PI;
        let x = radius * Math.cos(theta);
        let z = radius * Math.sin(theta);
        vertices.push(x, -height / 2, z);
        vertices.push(x, height / 2, z);
    }

    for (let i = 0; i < segments; i++) {
        let p0 = i * 2;
        let p1 = p0 + 1;
        let p2 = ((i + 1) % (segments + 1)) * 2;
        let p3 = p2 + 1;
        indices.push(p0, p1, p3, p0, p3, p2);
    }

    // --- Agregar los radios (bases) ---
    let baseCenter = vertices.length / 3;
    vertices.push(0, -height / 2, 0); // centro base inferior
    vertices.push(0, height / 2, 0); // centro base superior

    for (let i = 0; i < segments; i++) {
        let p1 = i * 2;
        let p2 = ((i + 1) % (segments + 1)) * 2;
        indices.push(baseCenter, p1, p2); // base inferior
        let p3 = p1 + 1;
        let p4 = p2 + 1;
        indices.push(baseCenter + 1, p3, p4); // base superior
    }

    return { vertices, indices };
}

function createCylinder(segments = 20, radius = 0.03, height = 0.8) {
    let vertices = [];
    let indices = [];

    for (let i = 0; i <= segments; i++) {
        let theta = (i / segments) * 2 * Math.PI;
        let x = radius * Math.cos(theta);
        let z = radius * Math.sin(theta);
        vertices.push(x, -height / 2, z);
        vertices.push(x, height / 2, z);
    }

    for (let i = 0; i < segments; i++) {
        let p0 = i * 2;
        let p1 = p0 + 1;
        let p2 = ((i + 1) % (segments + 1)) * 2;
        let p3 = p2 + 1;
        indices.push(p0, p1, p3, p0, p3, p2);
    }

    return { vertices, indices };
}

function getWebGLContext() {
    var canvas = document.getElementById("myCanvas");
    return canvas.getContext("webgl2");
}

function initShaders() {
    const vs = gl.createShader(gl.VERTEX_SHADER);
    gl.shaderSource(vs, document.getElementById("myVertexShader").textContent);
    gl.compileShader(vs);
    if (!gl.getShaderParameter(vs, gl.COMPILE_STATUS)) console.error(gl.getShaderInfoLog(vs));

    const fs = gl.createShader(gl.FRAGMENT_SHADER);
    gl.shaderSource(fs, document.getElementById("myFragmentShader").textContent);
    gl.compileShader(fs);
    if (!gl.getShaderParameter(fs, gl.COMPILE_STATUS)) console.error(gl.getShaderInfoLog(fs));

    program = gl.createProgram();
    gl.attachShader(program, vs);
    gl.attachShader(program, fs);
    gl.linkProgram(program);
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

function draw(model, modelMatrix) {
    gl.bindBuffer(gl.ARRAY_BUFFER, model.idBufferVertices);
    gl.vertexAttribPointer(program.vertexPositionAttribute, 3, gl.FLOAT, false, 0, 0);
    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, model.idBufferIndices);
    gl.uniformMatrix4fv(program.modelMatrixIndex, false, modelMatrix);
    gl.drawElements(gl.LINE_LOOP, model.indices.length, gl.UNSIGNED_SHORT, 0);
}

function drawScene() {
    gl.clear(gl.COLOR_BUFFER_BIT);

    // --- Cuerpo del tanque (sin tocar) ---
    let cubeMatrix = mat4.create();
    mat4.translate(cubeMatrix, cubeMatrix, [0, -0.35, 0]); 
    mat4.scale(cubeMatrix, cubeMatrix, [1.3, 0.25, 0.4]);
    draw(cube, cubeMatrix);

    // --- Ruedas (sin tocar) ---
    for (let i = -2; i <= 2; i++) { //3
        let wheelMatrix = mat4.create();
        mat4.translate(wheelMatrix, wheelMatrix, [i * 0.24, -0.6, 0]); 
        mat4.rotateX(wheelMatrix, wheelMatrix, Math.PI / 2); 
        mat4.scale(wheelMatrix, wheelMatrix, [1.2, 1.2, 1.2]);
        draw(wheel, wheelMatrix);
    }

    // --- Torreta (rueda superior central) ---
    let turretMatrix = mat4.create();
    mat4.translate(turretMatrix, turretMatrix, [0.4, -0.2, 0]);  
    mat4.rotateX(turretMatrix, turretMatrix, Math.PI / 2);
    mat4.scale(turretMatrix, turretMatrix, [1.35, 1.35, 1.35]);     
    draw(turret, turretMatrix);

    // --- Cañón  ---
    let cannonMatrix = mat4.create();
    mat4.translate(cannonMatrix, cannonMatrix, [-0.01, 0.05, 0]); 
    mat4.rotateZ(cannonMatrix, cannonMatrix, -Math.PI / -3);      
    mat4.rotateY(cannonMatrix, cannonMatrix, Math.PI / 2);
    mat4.scale(cannonMatrix, cannonMatrix, [1.3, 1.3, 1.3]);     
    draw(cannon, cannonMatrix);
}

function initRendering() {
    gl.clearColor(0.1, 0.1, 0.1, 1.0);
}

function initWebGL() {
    gl = getWebGLContext();
    if (!gl) return alert("WebGL 2.0 no disponible");

    initShaders();
    initRendering();

    cube = createCube();
    wheel = createWheelWithSpokes(15, 0.1, 0.8);  
    turret = createWheelWithSpokes(15, 0.2, 0.8); 
    cannon = createCylinder(20, 0.03, 0.8);     

    initBuffers(cube);
    initBuffers(wheel);
    initBuffers(turret);
    initBuffers(cannon);

    drawScene();
}

initWebGL();
