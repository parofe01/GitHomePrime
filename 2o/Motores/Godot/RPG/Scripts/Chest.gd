extends Node2D

var claimed = false

@onready var animator = $AnimatedSprite2D
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	animator.play("CofreCerrado")


func _on_area_2d_body_entered(body: Node2D) -> void:
	if !claimed:
		if body.has_method("player"):
			animator.play("CofreAbierto")
			Global.boosted = true
