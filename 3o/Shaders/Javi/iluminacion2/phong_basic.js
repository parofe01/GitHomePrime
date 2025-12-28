var gl, program;

// =======================================================
// GEOMETRÍA: CUBO (pos+normal intercalados, 24 verts)
// =======================================================
var cubeMesh = {
  vertices: [
    // +Z
    -0.5, -0.5,  0.5,   0.0, 0.0, 1.0,
    0.5, -0.5,  0.5,   0.0, 0.0, 1.0,
    0.5,  0.5,  0.5,   0.0, 0.0, 1.0,
    -0.5,  0.5,  0.5,   0.0, 0.0, 1.0,

    // -Z
    0.5, -0.5, -0.5,   0.0, 0.0,-1.0,
    -0.5, -0.5, -0.5,   0.0, 0.0,-1.0,
    -0.5,  0.5, -0.5,   0.0, 0.0,-1.0,
    0.5,  0.5, -0.5,   0.0, 0.0,-1.0,

    // +Y
    -0.5,  0.5,  0.5,   0.0, 1.0, 0.0,
    0.5,  0.5,  0.5,   0.0, 1.0, 0.0,
    0.5,  0.5, -0.5,   0.0, 1.0, 0.0,
    -0.5,  0.5, -0.5,   0.0, 1.0, 0.0,

    // -Y
    -0.5, -0.5, -0.5,   0.0,-1.0, 0.0,
    0.5, -0.5, -0.5,   0.0,-1.0, 0.0,
    0.5, -0.5,  0.5,   0.0,-1.0, 0.0,
    -0.5, -0.5,  0.5,   0.0,-1.0, 0.0,

    // +X
    0.5, -0.5,  0.5,   1.0, 0.0, 0.0,
    0.5, -0.5, -0.5,   1.0, 0.0, 0.0,
    0.5,  0.5, -0.5,   1.0, 0.0, 0.0,
    0.5,  0.5,  0.5,   1.0, 0.0, 0.0,

    // -X
    -0.5, -0.5, -0.5,  -1.0, 0.0, 0.0,
    -0.5, -0.5,  0.5,  -1.0, 0.0, 0.0,
    -0.5,  0.5,  0.5,  -1.0, 0.0, 0.0,
    -0.5,  0.5, -0.5,  -1.0, 0.0, 0.0
  ],
  indices: [
    0,1,2, 0,2,3,
    4,5,6, 4,6,7,
    8,9,10, 8,10,11,
    12,13,14, 12,14,15,
    16,17,18, 16,18,19,
    20,21,22, 20,22,23
  ]
};

// =======================================================
// GEOMETRÍA: PLANO (suelo) (pos+normal intercalados)
// =======================================================
var planeMesh = {
  // Un quad grande en Y = 0, normal hacia +Y
  // Tamaño 40x40 (de -20 a 20)
  vertices: [
    -20.0, 0.0, -20.0,   0.0, 1.0, 0.0,
    20.0, 0.0, -20.0,   0.0, 1.0, 0.0,
    20.0, 0.0,  20.0,   0.0, 1.0, 0.0,
    -20.0, 0.0,  20.0,   0.0, 1.0, 0.0
  ],
  indices: [
    0,1,2,
    0,2,3
  ]
};

// =======================================================
// MATERIALES
// =======================================================
var MatHeadband = {
  Ka: [0.08, 0.08, 0.08],
  Kd: [0.10, 0.20, 0.75],
  Ks: [0.90, 0.90, 0.90],
  alpha: 48.0
};

var MatEarcup = {
  Ka: [0.08, 0.08, 0.08],
  Kd: [0.08, 0.12, 0.55],
  Ks: [0.70, 0.70, 0.70],
  alpha: 24.0
};

var MatPlane = {
  Ka: [0.05, 0.05, 0.05],
  Kd: [0.25, 0.25, 0.25],
  Ks: [0.10, 0.10, 0.10],
  alpha: 8.0
};

// Two-sided (que se note MUCHO)
var MatFrontTwo = {
  Ka: [0.08, 0.08, 0.08],
  Kd: [0.05, 0.35, 0.95], // azul vivo
  Ks: [0.90, 0.90, 0.90],
  alpha: 32.0
};

var MatBackTwo = {
  Ka: [0.08, 0.08, 0.08],
  Kd: [0.95, 0.25, 0.05], // naranja/rojo vivo
  Ks: [0.40, 0.40, 0.40],
  alpha: 8.0
};

// =======================================================
// ESTADO UI
// =======================================================
var state = {
  shaderMode: 0,          // 0..4
  dir: [1, 1, 1],         // dirección del foco (eye-space)
  exponent: 0,
  cutoff: 100
};

// =======================================================
// WEBGL BASE
// =======================================================
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

  // Attributes
  program.aPos = gl.getAttribLocation(program, "VertexPosition");
  program.aNor = gl.getAttribLocation(program, "VertexNormal");
  gl.enableVertexAttribArray(program.aPos);
  gl.enableVertexAttribArray(program.aNor);

  // Matrices
  program.uMV  = gl.getUniformLocation(program, "modelViewMatrix");
  program.uP   = gl.getUniformLocation(program, "projectionMatrix");
  program.uNrm = gl.getUniformLocation(program, "normalMatrix");

  // Light
  program.uLightPos = gl.getUniformLocation(program, "Light.Position");
  program.uLa       = gl.getUniformLocation(program, "Light.La");
  program.uLd       = gl.getUniformLocation(program, "Light.Ld");
  program.uLs       = gl.getUniformLocation(program, "Light.Ls");
  program.uDir      = gl.getUniformLocation(program, "Light.Direction");
  program.uExp      = gl.getUniformLocation(program, "Light.Exponent");
  program.uCut      = gl.getUniformLocation(program, "Light.Cutoff");

  // Material front
  program.uKa    = gl.getUniformLocation(program, "Material.Ka");
  program.uKd    = gl.getUniformLocation(program, "Material.Kd");
  program.uKs    = gl.getUniformLocation(program, "Material.Ks");
  program.uAlpha = gl.getUniformLocation(program, "Material.alpha");

  // Material back
  program.uKaB    = gl.getUniformLocation(program, "MaterialBack.Ka");
  program.uKdB    = gl.getUniformLocation(program, "MaterialBack.Kd");
  program.uKsB    = gl.getUniformLocation(program, "MaterialBack.Ks");
  program.uAlphaB = gl.getUniformLocation(program, "MaterialBack.alpha");

  // Fog
  program.uFogMin = gl.getUniformLocation(program, "Fog.minDist");
  program.uFogMax = gl.getUniformLocation(program, "Fog.maxDist");
  program.uFogCol = gl.getUniformLocation(program, "Fog.color");

  // Mode
  program.uMode = gl.getUniformLocation(program, "uShaderMode");
}

function initMeshBuffers(mesh) {
  mesh.vbo = gl.createBuffer();
  gl.bindBuffer(gl.ARRAY_BUFFER, mesh.vbo);
  gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(mesh.vertices), gl.STATIC_DRAW);

  mesh.ibo = gl.createBuffer();
  gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, mesh.ibo);
  gl.bufferData(gl.ELEMENT_ARRAY_BUFFER, new Uint16Array(mesh.indices), gl.STATIC_DRAW);

  gl.bindBuffer(gl.ARRAY_BUFFER, null);
  gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, null);
}

function initRendering() {
  gl.clearColor(0.15, 0.15, 0.15, 1.0);

  gl.enable(gl.DEPTH_TEST);
  gl.depthFunc(gl.LEQUAL);

  // MUY IMPORTANTE para que se note Two-sided: no hacemos culling
  gl.disable(gl.CULL_FACE);
}

// =======================================================
// CÁMARA ORBITAL (como la original)
// =======================================================
const camera = {
  target: [0, 2.0, 0],
  distance: 18.0,
  yaw: 0.0,
  pitch: 0.30,
  fov: Math.PI / 4,
  near: 0.1,
  far: 200.0,
  minDistance: 2.0,
  maxDistance: 80.0
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

var viewMatrix = mat4.create();
var projMatrix = mat4.create();

function updateViewProj() {
  const eye = getEyePosition();
  mat4.lookAt(viewMatrix, eye, camera.target, [0, 1, 0]);

  const aspect = gl.canvas.width / gl.canvas.height;
  mat4.perspective(projMatrix, camera.fov, aspect, camera.near, camera.far);

  gl.uniformMatrix4fv(program.uP, false, projMatrix);
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
  try { e.target.releasePointerCapture(e.pointerId); } catch (_) {}
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

// =======================================================
// UNIFORMS: MATERIAL / LUZ / FOG / MODE
// =======================================================
function setMaterial(mat) {
  gl.uniform3fv(program.uKa, mat.Ka);
  gl.uniform3fv(program.uKd, mat.Kd);
  gl.uniform3fv(program.uKs, mat.Ks);
  gl.uniform1f(program.uAlpha, mat.alpha);
}

function setMaterialBack(mat) {
  gl.uniform3fv(program.uKaB, mat.Ka);
  gl.uniform3fv(program.uKdB, mat.Kd);
  gl.uniform3fv(program.uKsB, mat.Ks);
  gl.uniform1f(program.uAlphaB, mat.alpha);
}

function normalize3(v) {
  var len = Math.hypot(v[0], v[1], v[2]);
  if (len < 1e-8) return [0, 0, 1];
  return [v[0]/len, v[1]/len, v[2]/len];
}

function setLightAndMode() {
  gl.uniform1i(program.uMode, state.shaderMode);

  // Luz: en coordenadas de ojo (fija respecto a la cámara, como en las diapositivas)
  // Elegida para que (1,1,1) apunte hacia el origen y funcione "de serie".
  gl.uniform3fv(program.uLightPos, [-10.0, 10.0, -10.0]);

  gl.uniform3fv(program.uLa, [0.20, 0.20, 0.20]);
  gl.uniform3fv(program.uLd, [1.00, 1.00, 1.00]);
  gl.uniform3fv(program.uLs, [1.00, 1.00, 1.00]);

  var d = normalize3(state.dir);
  gl.uniform3fv(program.uDir, d);
  gl.uniform1f(program.uExp, state.exponent);
  gl.uniform1f(program.uCut, state.cutoff);

  // Fog (para que se note MUCHO)
  gl.uniform1f(program.uFogMin, 6.0);
  gl.uniform1f(program.uFogMax, 26.0);
  gl.uniform3fv(program.uFogCol, [0.15, 0.15, 0.15]);

  // Two-sided (materiales muy distintos)
  setMaterialBack(MatBackTwo);
}

// =======================================================
// DIBUJO MALLAS
// =======================================================
function drawMesh(mesh) {
  gl.bindBuffer(gl.ARRAY_BUFFER, mesh.vbo);

  // Pos (3) + Normal (3)
  gl.vertexAttribPointer(program.aPos, 3, gl.FLOAT, false, 6 * 4, 0);
  gl.vertexAttribPointer(program.aNor, 3, gl.FLOAT, false, 6 * 4, 3 * 4);

  gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, mesh.ibo);
  gl.drawElements(gl.TRIANGLES, mesh.indices.length, gl.UNSIGNED_SHORT, 0);

  gl.bindBuffer(gl.ARRAY_BUFFER, null);
  gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, null);
}

function setModelMatrixAndDraw(mesh, modelMatrix) {
  // MV = V * M
  var mv = mat4.create();
  mat4.multiply(mv, viewMatrix, modelMatrix);

  // Normal matrix
  var nrm = mat3.create();
  mat3.normalFromMat4(nrm, mv);

  gl.uniformMatrix4fv(program.uMV, false, mv);
  gl.uniformMatrix3fv(program.uNrm, false, nrm);

  drawMesh(mesh);
}

// =======================================================
// MODELO: CASCOS (5 cubos) + PLANO
// =======================================================
function makeTRS(pos, rotDeg, scale) {
  rotDeg = rotDeg || [0,0,0];

  var S = mat4.create();
  mat4.fromScaling(S, scale);

  var R = mat4.create();
  mat4.rotateX(R, R, rotDeg[0] * Math.PI/180);
  mat4.rotateY(R, R, rotDeg[1] * Math.PI/180);
  mat4.rotateZ(R, R, rotDeg[2] * Math.PI/180);

  var T = mat4.create();
  mat4.fromTranslation(T, pos);

  var TR = mat4.create();
  mat4.multiply(TR, T, R);

  var M = mat4.create();
  mat4.multiply(M, TR, S);
  return M;
}

function drawPlane() {
  // Un pelín por debajo para evitar z-fighting con la base de los cascos
  var M = mat4.create();
  mat4.fromTranslation(M, [0, -1.52, 0]);

  // Material del plano (siempre)
  setMaterial(MatPlane);

  setModelMatrixAndDraw(planeMesh, M);
}

function drawCascos() {
  // Bases (2): 2x3x3
  setMaterial(MatEarcup);
  setModelMatrixAndDraw(cubeMesh, makeTRS([-3, 0.0, 0.0], [0,0,0], [2, 3, 3]));
  setModelMatrixAndDraw(cubeMesh, makeTRS([ 3, 0.0, 0.0], [0,0,0], [2, 3, 3]));

  // Brazos (2): 1x2x1
  setMaterial(MatHeadband);
  setModelMatrixAndDraw(cubeMesh, makeTRS([-2.5, 2.5, 0.0], [0,0,0], [1, 2, 1]));
  setModelMatrixAndDraw(cubeMesh, makeTRS([ 2.5, 2.5, 0.0], [0,0,0], [1, 2, 1]));

  // Diadema (1): 6x1x1
  setMaterial(MatHeadband);
  setModelMatrixAndDraw(cubeMesh, makeTRS([0.0, 4.0, 0.0], [0,0,0], [6, 1, 1]));
}

// =======================================================
// ESCENA
// =======================================================
function drawScene() {
  gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

  updateViewProj();
  setLightAndMode();

  // En modo Two-sided, el material frontal lo ponemos muy vivo también
  if (state.shaderMode === 4) {
    setMaterial(MatFrontTwo);
    setMaterialBack(MatBackTwo);
  } else {
    setMaterialBack(MatBackTwo); // no molesta
  }

  // 1) plano (para apreciar spotlight)
  drawPlane();

  // 2) cascos
  drawCascos();
}

// =======================================================
// UI
// =======================================================
function initUI() {
  var title = document.getElementById("shaderTitle");
  var sel = document.getElementById("shaderSelect");

  var dirX = document.getElementById("dirX");
  var dirY = document.getElementById("dirY");
  var dirZ = document.getElementById("dirZ");

  var expSlider = document.getElementById("expSlider");
  var expVal = document.getElementById("expVal");

  var cutoffSlider = document.getElementById("cutoffSlider");
  var cutoffVal = document.getElementById("cutoffVal");

  function syncTitle() {
    title.textContent = sel.options[sel.selectedIndex].text;
    document.title = title.textContent;
  }

  sel.addEventListener("change", function() {
    state.shaderMode = parseInt(sel.value, 10) || 0;
    syncTitle();
    requestAnimationFrame(drawScene);
  });

  function onDirChange() {
    state.dir[0] = parseFloat(dirX.value) || 0;
    state.dir[1] = parseFloat(dirY.value) || 0;
    state.dir[2] = parseFloat(dirZ.value) || 0;
    requestAnimationFrame(drawScene);
  }

  dirX.addEventListener("input", onDirChange);
  dirY.addEventListener("input", onDirChange);
  dirZ.addEventListener("input", onDirChange);

  expSlider.addEventListener("input", function() {
    state.exponent = parseFloat(expSlider.value) || 0;
    expVal.textContent = String(expSlider.value);
    requestAnimationFrame(drawScene);
  });

  cutoffSlider.addEventListener("input", function() {
    state.cutoff = parseFloat(cutoffSlider.value) || 0;
    cutoffVal.textContent = String(cutoffSlider.value);
    requestAnimationFrame(drawScene);
  });

  // Atajos opcionales (1..5)
  window.addEventListener("keydown", function(e) {
    if (e.key === "1") sel.value = "0";
    else if (e.key === "2") sel.value = "1";
    else if (e.key === "3") sel.value = "2";
    else if (e.key === "4") sel.value = "3";
    else if (e.key === "5") sel.value = "4";
    else return;

    state.shaderMode = parseInt(sel.value, 10) || 0;
    syncTitle();
    requestAnimationFrame(drawScene);
  });

  expVal.textContent = String(expSlider.value);
  cutoffVal.textContent = String(cutoffSlider.value);
  syncTitle();
}

// =======================================================
// INIT
// =======================================================
function initWebGL() {
  gl = getWebGLContext();
  if (!gl) {
    alert("WebGL 2.0 no está disponible");
    return;
  }

  initShaders();
  if (!program) return;

  initMeshBuffers(cubeMesh);
  initMeshBuffers(planeMesh);

  initRendering();
  initUI();

  // Eventos de cámara
  const canvas = gl.canvas;
  canvas.addEventListener('pointerdown', onPointerDown);
  canvas.addEventListener('pointermove', onPointerMove);
  canvas.addEventListener('pointerup', onPointerUp);
  canvas.addEventListener('wheel', onWheel, {passive:false});
  window.addEventListener('resize', onResize);

  onResize();
}

initWebGL();
