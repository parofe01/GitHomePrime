function initShader() {
    // Paso 1: crear y compilar shaders
    var vertexShader = gl.createShader(gl.VERTEX_SHADER);
    gl.shaderSource(vertexShader, document.getElementById('myVertexShader').text);
    gl.compileShader(vertexShader);

    var fragmentShader = gl.createShader(gl.FRAGMENT_SHADER);
    gl.shaderSource(fragmentShader, document.getElementById('myFragmentShader').text);
    gl.compileShader(fragmentShader);

    // Paso 2: crear el programa y adjuntar los shaders
    program = gl.createProgram();
    gl.attachShader(program, vertexShader);
    gl.attachShader(program, fragmentShader);

    // Paso 3: enlazar y usar el programa
    gl.linkProgram(program);
    gl.useProgram(program);

    // Obtener la ubicación del atributo de posición de vértice
    program.vertexPositionAttribute = gl.getAttribLocation(program, "VertexPosition");
    gl.enableVertexAttribArray(program.vertexPositionAttribute);
}
