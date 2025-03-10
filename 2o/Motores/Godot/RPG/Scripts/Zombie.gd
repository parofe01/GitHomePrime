extends CharacterBody2D

var speed = 60
var health = 10000
var playerInRange = false
var player_chase = false
var player = null
@onready var animator = $AnimatedSprite2D

func _physics_process(delta):
		dealWithDamage() 
		updateCanvas()
		if player_chase:
			position += (player.position - position)/speed
			animator.play("Move")
			
			if(player.position.x < position.x):
				animator.flip_h = true
			else:
				animator.flip_h = false
		else:
			animator.play("Idle")

func _on_detection_area_body_entered(body: Node2D) -> void:
	player = body
	player_chase = true

func _on_detection_area_body_exited(body: Node2D) -> void:
	player = null
	player_chase = false

func enemy():
	pass

func zombie():
	pass

func dealWithDamage():
	if playerInRange && Global.playerCurrentAttack:
		if Global.boosted:
			health -= 1000
		else:
			health -= 20
		if health <= 0:
			animator.play("Death")
			queue_free()

func _on_zombie_hitbox_body_shape_entered(body_rid: RID, body: Node2D, body_shape_index: int, local_shape_index: int) -> void:
	if body.has_method("player"):
		playerInRange = true

func _on_zombie_hitbox_body_shape_exited(body_rid: RID, body: Node2D, body_shape_index: int, local_shape_index: int) -> void:
	if body.has_method("player"):
		playerInRange = false

func updateCanvas():
	var healthbar = $HealthBar
	healthbar.value = health
	if health >= 10000 or health <= 0:
		healthbar.visible = false
	else:
		healthbar.visible = true
