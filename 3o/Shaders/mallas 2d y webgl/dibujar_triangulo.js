var gl, program;

var estrella = {
    "vertices": [
        //Exteriores
        0.0, 0.9, 0.0,
        -0.95, 0.2, 0.0,
        -0.6, -0.9, 0.0,
        0.6, -0.9, -0.0,
        0.95, 0.2, 0.0,
        //Interiores
        -0.23, 0.2, 0.0,
        -0.37, -0.22, 0.0,
        0.0, -0.48, 0.0,
        0.37, -0.22, 0.0,
        0.23, 0.2, 0.0,
    ],
    "indicesRellenos": [
        0, 5, 9,
        1, 5, 6,
        6, 2, 7,
        7, 3, 8,
        8, 9, 4,
        5, 9, 6,
        6, 7, 8,
        6, 8, 9
    ],
    "indicesHuecos": [
        0, 9
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

    idMyColor = gl.getUniformLocation(program, "myColor");
}


function initBuffers(model) {
    // Buffer de vértices
    model.idBufferVertices = gl.createBuffer();
    gl.bindBuffer(gl.ARRAY_BUFFER, model.idBufferVertices);
    gl.bufferData(gl.ARRAY_BUFFER,
        new Float32Array(model.vertices), gl.STATIC_DRAW);

    // Buffer de índices Huecos
    model.idBufferIndices = gl.createBuffer();
    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, model.idBufferIndices);
    gl.bufferData(gl.ELEMENT_ARRAY_BUFFER,
        new Uint16Array(model.indicesHuecos), gl.STATIC_DRAW);

    // Buffer de índices Rellenos
    model.idBufferIndices = gl.createBuffer();
    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, model.idBufferIndices);
    gl.bufferData(gl.ELEMENT_ARRAY_BUFFER,
        new Uint16Array(model.indicesRellenos), gl.STATIC_DRAW);
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
    /*gl.drawElements(gl.
        TRIANGLES, model.indices.length, gl.UNSIGNED_SHORT, 0); // Pentagono Relleno 1 color  */
    gl.drawElements(gl.
        TRIANGLES, model.indicesRellenos.length, gl.UNSIGNED_SHORT, 0); // Pentagono y Estrella Rallada Hueco 
}

function drawOutline(model) {
    gl.bindBuffer(gl.ARRAY_BUFFER, model.idBufferVertices);
    gl.vertexAttribPointer(
        program.vertexPositionAttribute,
        3, gl.FLOAT, false, 0, 0
    );
    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, model.idBufferIndices);
    gl.drawElements(gl.
        LINE_STRIP, model.indicesHuecos.length, gl.UNSIGNED_SHORT, 0); // Pentagono y Estrella Rallada Hueco 
}

function SetIdColor(newColor) {
    gl.uniform4f(idMyColor, newColor[0], newColor[1], newColor[2], newColor[3]);
}

function drawScene() {
  if (!program) {
    console.warn("El programa WebGL no está inicializado.");
    return;
  }
  gl.clear(gl.COLOR_BUFFER_BIT);
  SetIdColor([0.2, 0.8, 0.2, 1.0]); // rojo
  draw(estrella);
  SetIdColor([0.8, 0.2, 0.2, 1.0]); // rojo
  drawOutline(estrella);
}


function initWebGL() {
    gl = getWebGLContext();
    if (!gl) {
        alert("WebGL 2.0 no está disponible");
        return;
    }
    initShaders();
    initBuffers(estrella);
    initRendering();
    requestAnimationFrame(drawScene);
}

initWebGL();