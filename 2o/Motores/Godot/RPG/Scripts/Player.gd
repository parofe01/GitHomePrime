extends CharacterBody2D

''' State Machine '''
enum State { 
	IDLE_UP, IDLE_DOWN, IDLE_LEFT, IDLE_RIGHT, 
	WALK_UP, WALK_DOWN, WALK_LEFT, WALK_RIGHT, 
	ATTACK_UP, ATTACK_DOWN, ATTACK_LEFT, ATTACK_RIGHT, 
	DIE 
}

var currentState = State.IDLE_DOWN

''' Variables '''

@export var speed = 100

func StateMachine():
	match currentState:
		State.IDLE_UP:
			Idle_Up()
		State.IDLE_DOWN:
			Idle_Down()
		State.IDLE_LEFT:
			Idle_Left()
		State.IDLE_RIGHT:
			Idle_Right()
		State.WALK_UP:
			Walk_Up()
		State.WALK_DOWN:
			Walk_Down()
		State.WALK_LEFT:
			Walk_Left()
		State.WALK_RIGHT:
			Walk_Right()
		State.ATTACK_UP:
			Attack_Up()
		State.ATTACK_DOWN:
			Attack_Down()
		State.ATTACK_LEFT:
			Attack_Left()
		State.ATTACK_RIGHT:
			Attack_Right()
		State.DIE:
			Die()


	
func _physics_process(delta):
	playerMovement(delta)
	
func playerMovement(delta):
	if (Input.is_action_pressed("ui_right")):
		velocity.x = speed
		velocity.y = 0
	elif (Input.is_action_pressed("ui_left")):
		velocity.x = -speed
		velocity.y = 0
	elif (Input.is_action_pressed("ui_up")):
		velocity.x = 0
		velocity.y = -speed
	elif (Input.is_action_pressed("ui_down")):
		velocity.x = 0
		velocity.y = speed
	else:
		velocity = Vector2.ZERO
	
	move_and_slide()
	
	pass

func Idle_Up():
	pass

func Idle_Down():
	pass

func Idle_Left():
	pass

func Idle_Right():
	pass

func Walk_Up():
	pass

func Walk_Down():
	pass

func Walk_Left():
	pass

func Walk_Right():
	pass

func Attack_Up():
	pass

func Attack_Down():
	pass

func Attack_Left():
	pass

func Attack_Right():
	pass

func Die():
	pass
