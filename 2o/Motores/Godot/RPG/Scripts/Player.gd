extends CharacterBody2D

var enemyInRange = false
var enemyAttackCooldown = true
var health = 100
const speed = 100
var damage = 0
var boost = false
var deathPlayed = false
var attackInProgress = false
enum Dir {Right, Left, Up, Down}
var dir = Dir.Down
enum State {Idle, Walk, Attack, Death}
var state = State.Idle
@onready var animator = $AnimatedSprite2D
@onready var timer = $AttackCooldown
func _ready():
	pass

func _physics_process(delta):
	if state != State.Death and !attackInProgress:
		player_movement(delta)
	play_anim()
	enemy_attack()
	updateCanvas()

func player_movement(delta):
	if Input.is_action_pressed("ui_right"):
		dir = Dir.Right;
		state = State.Walk
		velocity.x = speed
		velocity.y = 0
	elif Input.is_action_pressed("ui_left"):
		dir = Dir.Left;
		state = State.Walk
		velocity.x = -speed
		velocity.y = 0
	elif Input.is_action_pressed("ui_down"):
		dir = Dir.Down;
		state = State.Walk
		velocity.y = speed
		velocity.x = 0
	elif Input.is_action_pressed("ui_up"):
		dir = Dir.Up;
		state = State.Walk
		velocity.y = -speed
		velocity.x = 0
	elif  Input.is_action_just_pressed("Attack"):
		Global.playerCurrentAttack = true
		attackInProgress = true
		state = State.Attack
		$DealAttackTimer.start()
	else:
		state = State.Idle
		velocity.x = 0
		velocity.y = 0
	
	move_and_slide()

func play_anim():
	if dir == Dir.Left:
		animator.flip_h = true
	else:
		animator.flip_h = false
	match state:
		State.Idle:
			match dir:
				Dir.Up:
					animator.play("Idle_Back")
				Dir.Down:
					animator.play("Idle_Front")
				Dir.Left:
					animator.play("Idle_Side")
				Dir.Right:
					animator.play("Idle_Side")
		State.Walk:
			match dir:
				Dir.Up:
					animator.play("Walk_Back")
				Dir.Down:
					animator.play("Walk_Front")
				Dir.Left:
					animator.play("Walk_Side")
				Dir.Right:
					animator.play("Walk_Side")
		State.Attack:
			match dir:
				Dir.Up:
					animator.play("Attack_Back")
				Dir.Down:
					animator.play("Attack_Front")
				Dir.Left:
					animator.play("Attack_Side")
				Dir.Right:
					animator.play("Attack_Side")
		State.Death:
			if !deathPlayed:
				animator.play("Die")
				deathPlayed = true
			

func player():
	pass

func _on_player_hitbox_body_entered(body: Node2D) -> void:
	if body.has_method("enemy"):
		enemyInRange = true
	if body.has_method("slime"):
		damage = 10
	if body.has_method("zombie"):
		damage = 25

func _on_player_hitbox_body_exited(body: Node2D) -> void:
	if body.has_method("enemy"):
		enemyInRange = false
	if body.has_method("slime"):
		damage = 0
	if body.has_method("zombie"):
		damage = 0

func enemy_attack():
	if enemyInRange && enemyAttackCooldown:
		health -= damage
		enemyAttackCooldown = false
		timer.start()
		print(health)
		if (health <= 0):
			health = 0
			state = State.Death

func _on_attack_cooldown_timeout() -> void:
	enemyAttackCooldown = true

func _on_deal_attack_timer_timeout() -> void:
	$DealAttackTimer.stop()
	Global.playerCurrentAttack = false
	attackInProgress = false

func updateCanvas():
	var healthbar = $HealthBar
	healthbar.value = health
	if health >= 100 or health <= 0:
		healthbar.visible = false
	else:
		healthbar.visible = true
		
	var boostedMessage = $Boost
	if Global.boosted:
		boostedMessage.visible = true
	else:
		boostedMessage.visible = false

func _on_renen_timer_timeout() -> void:
	if state != State.Death:
		health += 10
		if health > 100:
			health = 100
