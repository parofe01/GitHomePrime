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

	if _vPosInicial.x + _vDesplazamiento >= position.x:
		_vSpeed *= -1
	
	if _vPosInicial.x - _vDesplazamiento <= position.x:
		_vSpeed *= -1
	position.x += _vSpeed * delta

	pass
