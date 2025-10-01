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

function drawScene() {

    gl.clear(gl.COLOR_BUFFER_BIT);

    //Calcula la matriz de transformacion
    var modelMatrix = mat4.create();
    mat4.fromScaling(modelMatrix, [0.5, 0.5, 0.5]);

    //Establece la matriz modelMatrix en el shader de vertices
    gl.uniformMatrix4fv(program.modelMatrixIndex, false, modelMatrix);

    //para la matriz de la normal
    // var normalMatrix = mat3.create()
    //mat3.normalFromMat4(normalMatrix, modelMatrix);
    //gl.uniformMatrix3fv(program.normalMatrixIndez, false, normalMatrix)
    //Dibuja la primitiva
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
    requestAnimationFrame(drawScene);
}

initWebGL();