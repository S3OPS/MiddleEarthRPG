extends Node
class_name PlayerMovementComponent
## Handles player movement, sprinting, and jumping
## Separated component for modular design

signal movement_started
signal movement_stopped
signal sprint_started
signal sprint_stopped
signal jumped

var _character: CharacterBody3D
var _stats: CharacterStats

# Movement state
var move_speed: float = Constants.PLAYER_WALK_SPEED
var is_sprinting: bool = false

# Stamina for sprinting
var stamina_regen_timer: float = 0.0

# Cached values
var _gravity: float = 0.0
var _sprint_speed: float = 0.0


## Initialize the movement component
func initialize(character: CharacterBody3D, stats: CharacterStats) -> bool:
	if not is_instance_valid(character) or not is_instance_valid(stats):
		push_error("PlayerMovementComponent: Invalid initialization parameters")
		return false
	
	_character = character
	_stats = stats
	
	# Cache values
	_gravity = ProjectSettings.get_setting("physics/3d/default_gravity")
	_sprint_speed = Constants.get_sprint_speed()
	
	return true


## Process movement input and update velocity, then apply it
func process_movement(delta: float) -> void:
	if not _character or not _stats:
		return
	
	_handle_sprinting(delta)
	_apply_movement_input()
	_handle_jumping()
	_apply_gravity(delta)
	_handle_stamina_regeneration(delta)
	
	# CRITICAL FIX: Apply the calculated velocity to the character
	# This line was missing, causing movement to not work despite velocity being calculated
	if _character:
		_character.move_and_slide()


## Handle sprint input and stamina drain
func _handle_sprinting(delta: float) -> void:
	var wants_to_sprint = Input.is_action_pressed("sprint") and _stats.current_stamina > 0
	
	if wants_to_sprint and not is_sprinting:
		is_sprinting = true
		move_speed = _sprint_speed
		sprint_started.emit()
	elif not wants_to_sprint and is_sprinting:
		is_sprinting = false
		move_speed = Constants.PLAYER_WALK_SPEED
		sprint_stopped.emit()
	
	if is_sprinting:
		_stats.use_stamina(Constants.SPRINT_STAMINA_DRAIN_RATE * delta)
		stamina_regen_timer = Constants.STAMINA_REGEN_DELAY


## Apply movement from WASD input
func _apply_movement_input() -> void:
	var input_dir := Input.get_vector("move_left", "move_right", "move_forward", "move_backward")
	var direction := (_character.transform.basis * Vector3(input_dir.x, 0, input_dir.y)).normalized()
	
	if direction:
		_character.velocity.x = direction.x * move_speed
		_character.velocity.z = direction.z * move_speed
	else:
		_character.velocity.x = move_toward(_character.velocity.x, 0, move_speed)
		_character.velocity.z = move_toward(_character.velocity.z, 0, move_speed)


## Handle jump input
func _handle_jumping() -> void:
	if Input.is_action_just_pressed("jump") and _character.is_on_floor():
		_character.velocity.y = Constants.PLAYER_JUMP_VELOCITY
		jumped.emit()


## Apply gravity
func _apply_gravity(delta: float) -> void:
	if not _character.is_on_floor():
		_character.velocity.y -= _gravity * delta


## Handle stamina regeneration
func _handle_stamina_regeneration(delta: float) -> void:
	# CRITICAL FIX: Add null check for _stats before accessing it
	if not _stats:
		push_warning("PlayerMovementComponent: _stats is null in _handle_stamina_regeneration")
		return
	
	if stamina_regen_timer > 0:
		stamina_regen_timer -= delta
	elif _stats.current_stamina < _stats.max_stamina:
		_stats.restore_stamina(Constants.STAMINA_REGEN_RATE * delta)


## Stop sprinting immediately
func stop_sprinting() -> void:
	if is_sprinting:
		is_sprinting = false
		move_speed = Constants.PLAYER_WALK_SPEED
		sprint_stopped.emit()


## Get current movement speed
func get_current_speed() -> float:
	return move_speed


## Check if currently sprinting
func is_currently_sprinting() -> bool:
	return is_sprinting
