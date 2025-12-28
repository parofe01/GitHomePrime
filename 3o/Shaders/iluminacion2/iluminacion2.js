"use strict";

let gl, program;

// ===================== MODELOS =====================
// Cubo con normales por cara (posición+normal intercalados)
const cubePN = {
  vertices: [
    // +Z
    -0.5,-0.5, 0.5,   0,0,1,  0.5,-0.5, 0.5,   0,0,1,  0.5, 0.5, 0.5,   0,0,1,  -0.5, 0.5, 0.5,   0,0,1,
    // +X
     0.5,-0.5, 0.5,   1,0,0,  0.5,-0.5,-0.5,   1,0,0,  0.5, 0.5,-0.5,   1,0,0,   0.5, 0.5, 0.5,   1,0,0,
    // -Z
     0.5,-0.5,-0.5,   0,0,-1, -0.5,-0.5,-0.5,  0,0,-1, -0.5, 0.5,-0.5,  0,0,-1,  0.5, 0.5,-0.5,   0,0,-1,
    // -X
    -0.5,-0.5,-0.5,  -1,0,0, -0.5,-0.5, 0.5,  -1,0,0, -0.5, 0.5, 0.5,  -1,0,0,  -0.5, 0.5,-0.5,  -1,0,0,
    // +Y
    -0.5, 0.5, 0.5,   0,1,0,  0.5, 0.5, 0.5,   0,1,0,  0.5, 0.5,-0.5,   0,1,0,  -0.5, 0.5,-0.5,   0,1,0,
    // -Y
    -0.5,-0.5,-0.5,   0,-1,0, 0.5,-0.5,-0.5,   0,-1,0, 0.5,-0.5, 0.5,   0,-1,0, -0.5,-0.5, 0.5,   0,-1,0,
  ],
  indices: [
     0,  1,  2,  0,  2,  3,
     4,  5,  6,  4,  6,  7,
     8,  9, 10,  8, 10, 11,
    12, 13, 14, 12, 14, 15,
    16, 17, 18, 16, 18, 19,
    20, 21, 22, 20, 22, 23
  ]
};

const planePN = {
  vertices: [
    -1, 0, -1,   0, 1, 0,
     1, 0, -1,   0, 1, 0,
     1, 0,  1,   0, 1, 0,
    -1, 0,  1,   0, 1, 0,
  ],
  indices: [0,1,2, 0,2,3]
};

// ===================== WEBGL BOILERPLATE =====================
function getWebGLContext() {
  const canvas = document.getElementById("myCanvas");
  try { return canvas.getContext("webgl2"); } catch (e) {}
  return null;
}

function compileShader(type, source) {
  const sh = gl.createShader(type);
  gl.shaderSource(sh, source);
  gl.compileShader(sh);
  if (!gl.getShaderParameter(sh, gl.COMPILE_STATUS)) {
    console.error(gl.getShaderInfoLog(sh));
    gl.deleteShader(sh);
    return null;
  }
  return sh;
}

function initShaders() {
  const vsSource = document.getElementById("myVertexShader").textContent;
  const fsSource = document.getElementById("myFragmentShader").textContent;

  const vs = compileShader(gl.VERTEX_SHADER, vsSource);
  const fs = compileShader(gl.FRAGMENT_SHADER, fsSource);
  if (!vs || !fs) return false;

  program = gl.createProgram();
  gl.attachShader(program, vs);
  gl.attachShader(program, fs);
  gl.linkProgram(program);
  if (!gl.getProgramParameter(program, gl.LINK_STATUS)) {
    console.error(gl.getProgramInfoLog(program));
    return false;
  }
  gl.useProgram(program);

  program.aPos = gl.getAttribLocation(program, "VertexPosition");
  program.aNor = gl.getAttribLocation(program, "VertexNormal");
  gl.enableVertexAttribArray(program.aPos);
  gl.enableVertexAttribArray(program.aNor);

  program.uMV   = gl.getUniformLocation(program, "modelViewMatrix");
  program.uP    = gl.getUniformLocation(program, "projectionMatrix");
  program.uN    = gl.getUniformLocation(program, "normalMatrix");
  program.uMode = gl.getUniformLocation(program, "uMode");

  // Light
  program.uLightPos = gl.getUniformLocation(program, "Light.Position");
  program.uLightDir = gl.getUniformLocation(program, "Light.Direction");
  program.uLightExp = gl.getUniformLocation(program, "Light.Exponent");
  program.uLightCut = gl.getUniformLocation(program, "Light.Cutoff");
  program.uAttA     = gl.getUniformLocation(program, "Light.AttA");
  program.uAttB     = gl.getUniformLocation(program, "Light.AttB");
  program.uAttC     = gl.getUniformLocation(program, "Light.AttC");
  program.uLa       = gl.getUniformLocation(program, "Light.La");
  program.uLd       = gl.getUniformLocation(program, "Light.Ld");
  program.uLs       = gl.getUniformLocation(program, "Light.Ls");

  // Materials
  program.uF_Ka = gl.getUniformLocation(program, "MaterialFront.Ka");
  program.uF_Kd = gl.getUniformLocation(program, "MaterialFront.Kd");
  program.uF_Ks = gl.getUniformLocation(program, "MaterialFront.Ks");
  program.uF_a  = gl.getUniformLocation(program, "MaterialFront.alpha");

  program.uB_Ka = gl.getUniformLocation(program, "MaterialBack.Ka");
  program.uB_Kd = gl.getUniformLocation(program, "MaterialBack.Kd");
  program.uB_Ks = gl.getUniformLocation(program, "MaterialBack.Ks");
  program.uB_a  = gl.getUniformLocation(program, "MaterialBack.alpha");

  // Fog
  program.uFogMin = gl.getUniformLocation(program, "Fog.minDist");
  program.uFogMax = gl.getUniformLocation(program, "Fog.maxDist");
  program.uFogCol = gl.getUniformLocation(program, "Fog.color");
  program.uFogEn  = gl.getUniformLocation(program, "Fog.enabled");
  program.uFogExp = gl.getUniformLocation(program, "Fog.useExp");

  return true;
}

function initBuffers(model) {
  model.vbo = gl.createBuffer();
  gl.bindBuffer(gl.ARRAY_BUFFER, model.vbo);
  gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(model.vertices), gl.STATIC_DRAW);

  model.ibo = gl.createBuffer();
  gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, model.ibo);
  gl.bufferData(gl.ELEMENT_ARRAY_BUFFER, new Uint16Array(model.indices), gl.STATIC_DRAW);

  model.indexCount = model.indices.length;
}

function bindModel(model) {
  gl.bindBuffer(gl.ARRAY_BUFFER, model.vbo);
  const stride = 6 * 4;
  gl.vertexAttribPointer(program.aPos, 3, gl.FLOAT, false, stride, 0);
  gl.vertexAttribPointer(program.aNor, 3, gl.FLOAT, false, stride, 3 * 4);
  gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, model.ibo);
}

function drawModel(model) {
  bindModel(model);
  gl.drawElements(gl.TRIANGLES, model.indexCount, gl.UNSIGNED_SHORT, 0);
}

function normalMatrixFromMV(mv) {
  const n = mat3.create();
  mat3.normalFromMat4(n, mv);
  return n;
}

function setMatrices(view, proj, model) {
  const mv = mat4.create();
  mat4.multiply(mv, view, model);
  gl.uniformMatrix4fv(program.uMV, false, mv);
  gl.uniformMatrix4fv(program.uP,  false, proj);
  gl.uniformMatrix3fv(program.uN,  false, normalMatrixFromMV(mv));
}

// ===================== CÁMARA ORBITAL =====================
const camera = {
  target: [0, 2, 0],
  distance: 10.0,
  yaw: 0.6,
  pitch: 0.35,
  fov: Math.PI / 4,
  near: 0.1,
  far: 100.0,
  minDistance: 2.0,
  maxDistance: 40.0
};

let isDragging = false;
let lastX = 0, lastY = 0;
let dragMode = "rotate";

function clamp(v, a, b) { return Math.max(a, Math.min(b, v)); }

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
  dragMode = (e.shiftKey || e.button === 1) ? "pan" : "rotate";
  e.target.setPointerCapture(e.pointerId);
}

function onPointerMove(e) {
  if (!isDragging) return;
  const dx = e.clientX - lastX;
  const dy = e.clientY - lastY;
  lastX = e.clientX;
  lastY = e.clientY;

  if (dragMode === "rotate") {
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
    for (let i = 0; i < 3; i++) {
      camera.target[i] -= right[i] * dx * panSpeed;
      camera.target[i] += up[i]    * dy * panSpeed;
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
  canvas.width  = canvas.clientWidth;
  canvas.height = canvas.clientHeight;
  gl.viewport(0, 0, canvas.width, canvas.height);
  requestAnimationFrame(drawScene);
}

// ===================== GRÚA =====================
function buildModelMatrix(pos, rotXYZ, scl) {
  const m = mat4.create();
  mat4.translate(m, m, pos);
  mat4.rotateX(m, m, rotXYZ[0]);
  mat4.rotateY(m, m, rotXYZ[1]);
  mat4.rotateZ(m, m, rotXYZ[2]);
  mat4.scale(m, m, scl);
  return m;
}

function drawCrane(view, proj) {
  // Ajuste de escala/centrado
  const worldScale = 0.35;
  const worldTranslate = [-1.8, 0.0, 0.0];

  const world = mat4.create();
  mat4.translate(world, world, worldTranslate);
  mat4.scale(world, world, [worldScale, worldScale, worldScale]);

  function drawPart(local) {
    const model = mat4.create();
    mat4.multiply(model, world, local);
    setMatrices(view, proj, model);
    drawModel(cubePN);
  }

  // Base
  drawPart(buildModelMatrix([0, 0.25, 0], [0,0,0], [2.5, 0.5, 2.5]));
  // Pie
  drawPart(buildModelMatrix([0, 5.0, 0], [0,0,0], [1.0, 10.0, 1.0]));
  // Brazo
  drawPart(buildModelMatrix([5.0, 10.5, 0], [0,0,0], [10.0, 1.0, 1.0]));
  // Contrapeso
  drawPart(buildModelMatrix([-1.0, 10.5, 0], [0,0,0], [1.4, 1.4, 1.4]));
  // Carga
  drawPart(buildModelMatrix([10.0, 7.0, 0], [0,0,0], [1.0, 1.0, 1.0]));
  // Cable
  drawPart(buildModelMatrix([10.0, 9.0, 0], [0,0,0], [0.12, 3.0, 0.12]));
}

// ===================== UI / UNIFORMS =====================
let mode = 4;

const ui = {
  shaderSelect: null,
  dirX: null, dirY: null, dirZ: null,
  exp: null, cut: null,
  expVal: null, cutVal: null,
  ground: null
};

function setUniformsFromUI() {
  // Modo
  gl.uniform1i(program.uMode, mode);

  // Luz base
  gl.uniform3f(program.uLa, 0.15, 0.15, 0.15);
  gl.uniform3f(program.uLd, 1.00, 1.00, 1.00);
  gl.uniform3f(program.uLs, 1.00, 1.00, 1.00);

  // Luz en espacio ojo: un punto "delante" para que se vea bien
  gl.uniform3f(program.uLightPos, 6.0, 10.0, 10.0);

  // Dirección (en espacio ojo) desde inputs
  let dx = parseFloat(ui.dirX.value);
  let dy = parseFloat(ui.dirY.value);
  let dz = parseFloat(ui.dirZ.value);
  const len = Math.hypot(dx, dy, dz) || 1.0;
  dx /= len; dy /= len; dz /= len;
  gl.uniform3f(program.uLightDir, dx, dy, dz);

  // Spotlight params
  const exponent = parseFloat(ui.exp.value);
  const cutoff = parseFloat(ui.cut.value);
  gl.uniform1f(program.uLightExp, exponent);
  gl.uniform1f(program.uLightCut, cutoff);

  // Atenuación (se usa en modo 4)
  gl.uniform1f(program.uAttA, 1.0);
  gl.uniform1f(program.uAttB, 0.05);
  gl.uniform1f(program.uAttC, 0.005);

  // Material front (azul para que se note como la referencia)
  gl.uniform3f(program.uF_Ka, 0.05, 0.05, 0.10);
  gl.uniform3f(program.uF_Kd, 0.10, 0.10, 0.90);
  gl.uniform3f(program.uF_Ks, 0.90, 0.90, 0.90);
  gl.uniform1f(program.uF_a,  60.0);

  // Material back (muy distinto)
  gl.uniform3f(program.uB_Ka, 0.15, 0.05, 0.00);
  gl.uniform3f(program.uB_Kd, 0.95, 0.45, 0.10);
  gl.uniform3f(program.uB_Ks, 1.00, 0.80, 0.30);
  gl.uniform1f(program.uB_a,  30.0);

  // Fog (solo modo 3)
  gl.uniform1f(program.uFogMin, 8.0);
  gl.uniform1f(program.uFogMax, 18.0);
  gl.uniform3f(program.uFogCol, 0.12, 0.12, 0.12);
  gl.uniform1i(program.uFogEn, (mode === 3) ? 1 : 0);
  gl.uniform1i(program.uFogExp, 0);

  gl.clearColor(0.12, 0.12, 0.12, 1.0);
}

// ===================== RENDER =====================
function drawScene() {
  gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

  const eye = getEyePosition();
  const view = mat4.create();
  mat4.lookAt(view, eye, camera.target, [0, 1, 0]);

  const proj = mat4.create();
  const aspect = gl.canvas.width / gl.canvas.height;
  mat4.perspective(proj, camera.fov, aspect, camera.near, camera.far);

  setUniformsFromUI();

  // Suelo (opcional), bajado 0.01 para evitar z-fighting
  if (ui.ground.checked) {
    const model = mat4.create();
    mat4.translate(model, model, [0, -0.01, 0]);
    mat4.scale(model, model, [6, 1, 6]);
    setMatrices(view, proj, model);
    drawModel(planePN);
  }

  // Grúa
  drawCrane(view, proj);
}

// ===================== INIT =====================
function hookUI() {
  ui.shaderSelect = document.getElementById("shaderSelect");
  ui.dirX = document.getElementById("dirX");
  ui.dirY = document.getElementById("dirY");
  ui.dirZ = document.getElementById("dirZ");
  ui.exp  = document.getElementById("exp");
  ui.cut  = document.getElementById("cut");
  ui.expVal = document.getElementById("expVal");
  ui.cutVal = document.getElementById("cutVal");
  ui.ground = document.getElementById("ground");

  function refreshLabels() {
    ui.expVal.textContent = ui.exp.value;
    ui.cutVal.textContent = ui.cut.value;
  }

  ui.shaderSelect.addEventListener("change", () => {
    mode = parseInt(ui.shaderSelect.value, 10);
    requestAnimationFrame(drawScene);
  });

  [ui.dirX, ui.dirY, ui.dirZ].forEach(el => el.addEventListener("input", () => requestAnimationFrame(drawScene)));

  ui.exp.addEventListener("input", () => { refreshLabels(); requestAnimationFrame(drawScene); });
  ui.cut.addEventListener("input", () => { refreshLabels(); requestAnimationFrame(drawScene); });
  ui.ground.addEventListener("change", () => requestAnimationFrame(drawScene));

  refreshLabels();
}

function initRendering() {
  gl.enable(gl.DEPTH_TEST);
  gl.disable(gl.CULL_FACE); // importante para poder ver algo en modo 5 si hay superficies "por detrás"
}

function initInput() {
  const canvas = gl.canvas;
  canvas.addEventListener("pointerdown", onPointerDown);
  canvas.addEventListener("pointermove", onPointerMove);
  canvas.addEventListener("pointerup", onPointerUp);
  canvas.addEventListener("wheel", onWheel, { passive: false });
  window.addEventListener("resize", onResize);
}

function initWebGL() {
  gl = getWebGLContext();
  if (!gl) {
    alert("WebGL 2.0 no está disponible");
    return;
  }

  if (!initShaders()) return;

  initBuffers(cubePN);
  initBuffers(planePN);

  initRendering();
  initInput();
  hookUI();

  onResize();
}

initWebGL();
