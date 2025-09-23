var gl, program;

var exampleTriangle = {
    "vertices": [
        -0.7, -0.7, 0.0,  1.0 , 0.0 , 0.0 , 1.0 , // rojo
        0.7, -0.7, 0.0, 0.0 , 1.0 , 0.0 , 1.0 , // verde
        0.0, 0.7, 0.0 , 0.0 , 0.0 , 1.0 , 1.0   // azul
    ],
    "indices": [0, 1, 2]
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

  // Obtener y habilitar los atributos
  program.vertexPositionAttribute =
    gl.getAttribLocation (program , "VertexPosition") ;
    gl.enableVertexAttribArray (program.vertexPositionAttribute);
 program.vertexColorAttribute =
    gl.getAttribLocation (program , "VertexColor") ;
    gl.enableVertexAttribArray (program.vertexColorAttribute);
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
        program.vertexPositionAttribute, 3, gl.FLOAT, false, 7*4, 0);
    gl.vertexAttribPointer(
        program.vertexColorAttribute, 4, gl.FLOAT, false, 7*4, 3*4);
    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, model.idBufferIndices);
    gl.drawElements(gl.TRIANGLES, 3, gl.UNSIGNED_SHORT, 0);
}

function drawScene() {
  if (!program) {
    console.warn("El programa WebGL no está inicializado.");
    return;
  }
  gl.clear(gl.COLOR_BUFFER_BIT);
  draw(exampleTriangle);
}


function initWebGL() {
    gl = getWebGLContext();
    if (!gl) {
        alert("WebGL 2.0 no está disponible");
        return;
    }
    initShaders();
    initBuffers(exampleTriangle);
    initRendering();
    requestAnimationFrame(drawScene);
}

initWebGL();