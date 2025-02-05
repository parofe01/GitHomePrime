extends AnimatableBody2D

@export var _vDesplazamiento = 0
@export var _vSpeed = 0

var _vPosInicial = Vector2(0,0)

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	
	_vPosInicial = position
	
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	var _vPos = _vPosInicial.x + _vDesplazamiento
	if _vPos >= position.x:
		_vSpeed *= -1
	_vPos = _vPosInicial.x - _vDesplazamiento
	if _vPos <= position.x:
		_vSpeed *= -1
	translate(Vector2.RIGHT * _vSpeed)
	pass
