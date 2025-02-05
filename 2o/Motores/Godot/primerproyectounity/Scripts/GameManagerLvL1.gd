extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	_TEST()
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
	
func _TEST() -> void:
	print("hola")
	var num1 = 1
	if (num1 == 1):
		print(num1)
	
