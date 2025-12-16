var gl, program;

var estrellaBorde = {
    "vertices": [
        0.0, 0.9, 0.0,
        -0.97, 0.2, 0.0,
        -0.6, -0.9, 0.0,
        0.6, -0.9, 0.0,
        0.95, 0.2, 0.0,
        -0.23, 0.2, 0.0,
        -0.37, -0.22, 0.0,
        0.0, -0.48, 0.0,
        0.37, -0.22, 0.0,
        0.23, 0.2, 0.0
    ],
    "indices": [0, 5, 1, 6, 2, 7, 3, 8, 4, 9]
};

var estrellaFill = {
    "vertices": [
        0.0, 0.9, 0.0,
        -0.97, 0.2, 0.0,
        -0.6, -0.9, 0.0,
        0.6, -0.9, 0.0,
        0.95, 0.2, 0.0,
        -0.23, 0.2, 0.0,
        -0.37, -0.22, 0.0,
        0.0, -0.48, 0.0,
        0.37, -0.22, 0.0,
        0.23, 0.2, 0.0
    ],
    "indices": [0, 2, 8, 1, 9, 3, 5, 2, 4]
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
    gl.clearColor(0.8, 0.0, 0.0, 1.0);
}

function SetIdColor(newColor) {
    
    var idMyColor = gl.getUniformLocation(program, "myColor");
    gl.uniform4f(idMyColor, newColor[0], newColor[1], newColor[2], newColor[3]);
    
}

function drawBorde(model) {
    gl.bindBuffer(gl.ARRAY_BUFFER, model.idBufferVertices);
    gl.vertexAttribPointer(
        program.vertexPositionAttribute,
        3, gl.FLOAT, false, 0, 0
    );
    
    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, model.idBufferIndices);
    gl.drawElements(gl.LINE_LOOP, model.indices.length, gl.UNSIGNED_SHORT, 0);
}

function drawFill(model) {
    gl.bindBuffer(gl.ARRAY_BUFFER, model.idBufferVertices);
    gl.vertexAttribPointer(
        program.vertexPositionAttribute,
        3, gl.FLOAT, false, 0, 0
    );
    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, model.idBufferIndices);
    gl.drawElements(gl.TRIANGLES, model.indices.length, gl.UNSIGNED_SHORT, 0);
}

function drawScene() {
  if (!program) {
    console.warn("El programa WebGL no está inicializado.");
    return;
  }
  gl.clear(gl.COLOR_BUFFER_BIT);
  SetIdColor([1.0, 0.0, 0.0, 1.0]);
  drawFill(estrellaFill);
  SetIdColor([1.0, 1.0, 0.0, 1.0]);
  drawBorde(estrellaBorde);
  
}


function initWebGL() {
    gl = getWebGLContext();
    if (!gl) {
        alert("WebGL 2.0 no está disponible");
        return;
    }
    initShaders();
    initBuffers(estrellaFill);
    initBuffers(estrellaBorde);
    initRendering();
    requestAnimationFrame(drawScene);
}

initWebGL();