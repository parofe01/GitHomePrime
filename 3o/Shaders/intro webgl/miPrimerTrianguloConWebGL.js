var gl = null;
var program;

var exampleTriangle = {

  "vertices" : [0.5, -0.5, 0.0,
                 0.65, -0.55, 0.0, 
                 0.6,  -0.4, 0.0],

  "indices" : [ 0, 1, 2]

};

function getWebGLContext() {

  var canvas = document.getElementById("myCanvas");

  var names = ["webgl", "experimental-webgl", "webkit-3d", "moz-webgl"];

  for (var i = 0; i < names.length; ++i) {
    try {
      return canvas.getContext(names[i]);
    }
    catch(e) {
    }
  }

  return null;

}

function initShaders() {

  // paso 1
  var vertexShader = gl.createShader(gl.VERTEX_SHADER);
  gl.shaderSource(vertexShader, document.getElementById('myVertexShader').text);
  gl.compileShader(vertexShader);

  var fragmentShader = gl.createShader(gl.FRAGMENT_SHADER);
  gl.shaderSource(fragmentShader, document.getElementById('myFragmentShader').text);
  gl.compileShader(fragmentShader);
  
  // paso 2
  program = gl.createProgram();
  gl.attachShader(program, vertexShader);
  gl.attachShader(program, fragmentShader);
  
  // paso 3
  gl.linkProgram(program);

  gl.useProgram(program);

  program.vertexPositionAttribute = gl.getAttribLocation( program, "VertexPosition");
  gl.enableVertexAttribArray(program.vertexPositionAttribute);

}

function initRendering() {

  gl.clearColor(0.35,0.0,0.35,1.0);

}
  
function initBuffers(model) {

  model.idBufferVertices = gl.createBuffer ();
  gl.bindBuffer (gl.ARRAY_BUFFER, model.idBufferVertices);
  gl.bufferData (gl.ARRAY_BUFFER, new Float32Array(model.vertices), gl.STATIC_DRAW);

  model.idBufferIndices = gl.createBuffer ();
  gl.bindBuffer (gl.ELEMENT_ARRAY_BUFFER, model.idBufferIndices);
  gl.bufferData (gl.ELEMENT_ARRAY_BUFFER, new Uint16Array(model.indices), gl.STATIC_DRAW);

}

function draw(model) {

  gl.bindBuffer(gl.ARRAY_BUFFER, model.idBufferVertices);
  gl.vertexAttribPointer(program.vertexPositionAttribute, 3, gl.FLOAT, false, 0, 0);

  gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, model.idBufferIndices);
  gl.drawElements(gl.TRIANGLES, 3, gl.UNSIGNED_SHORT, 0);
  
}

function drawScene() {

  gl.clear(gl.COLOR_BUFFER_BIT);

  draw(exampleTriangle);

}

function initWebGL() {
      
  gl = getWebGLContext();

  if (!gl) {
    alert("WebGL no está disponible");
    return;
  }

  initShaders();
  initRendering();
  initBuffers(exampleTriangle);
  
  requestAnimationFrame(drawScene);

}

initWebGL();     
