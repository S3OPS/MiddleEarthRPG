extends Node
class_name EnemyAIComponent
## Handles enemy AI state machine and decision making
## Separated component for modular design

enum State {
	PATROL,
	CHASE,
	ATTACK,
	FLEE,
	DEAD
}

signal state_changed(old_state: State, new_state: State)
signal attack_initiated
signal target_lost
signal target_acquired

var _enemy: CharacterBody3D
var _nav_agent: NavigationAgent3D
var _player: Node3D

# AI State
var current_state: State = State.PATROL

# Patrol
var spawn_position: Vector3 = Vector3.ZERO
var patrol_target: Vector3 = Vector3.ZERO
var patrol_timer: float = 0.0
var next_patrol_time: float = 0.0

# Attack
var attack_cooldown_timer: float = 0.0

# Cached values for performance
var _attack_range_sqr: float = 0.0
var _detection_range_sqr: float = 0.0


## Initialize the AI component
func initialize(enemy: CharacterBody3D, nav_agent: NavigationAgent3D, spawn_pos: Vector3) -> void:
	_enemy = enemy
	_nav_agent = nav_agent
	spawn_position = spawn_pos
	
	# Cache frequently used values
	_attack_range_sqr = Constants.ENEMY_ATTACK_RANGE * Constants.ENEMY_ATTACK_RANGE
	_detection_range_sqr = Constants.ENEMY_DETECTION_RANGE * Constants.ENEMY_DETECTION_RANGE
	
	# Get player reference
	_player = GameManager.get_player()
	
	# Set initial patrol point
	_set_new_patrol_point()


## Process AI logic and state transitions
func process_ai(delta: float, current_health: float, max_health: float) -> void:
	if current_state == State.DEAD or not _player:
		return
	
	# Update cooldowns
	if attack_cooldown_timer > 0:
		attack_cooldown_timer -= delta
	
	# Determine new state based on conditions
	var new_state = _determine_state(current_health, max_health)
	if new_state != current_state:
		_change_state(new_state)
	
	# Execute state behavior
	_execute_state_behavior(delta)


## Determine the appropriate state based on conditions
func _determine_state(current_health: float, max_health: float) -> State:
	if current_state == State.DEAD:
		return State.DEAD
	
	var distance_sqr = _enemy.global_position.distance_squared_to(_player.global_position)
	var flee_threshold = max_health * Constants.ENEMY_FLEE_HEALTH_PERCENT
	
	# Priority: Flee > Attack > Chase > Patrol
	if current_health <= flee_threshold:
		return State.FLEE
	elif distance_sqr <= _attack_range_sqr:
		return State.ATTACK
	elif distance_sqr <= _detection_range_sqr:
		return State.CHASE
	else:
		return State.PATROL


## Change state and emit signal
func _change_state(new_state: State) -> void:
	var old_state = current_state
	current_state = new_state
	state_changed.emit(old_state, new_state)
	
	# Handle state entry events
	match new_state:
		State.CHASE:
			target_acquired.emit()
		State.PATROL:
			if old_state == State.CHASE or old_state == State.ATTACK:
				target_lost.emit()
		State.ATTACK:
			attack_initiated.emit()


## Execute behavior for current state
func _execute_state_behavior(delta: float) -> void:
	match current_state:
		State.PATROL:
			_patrol(delta)
		State.CHASE:
			_chase_player()
		State.ATTACK:
			_attack_player()
		State.FLEE:
			_flee_from_player()


## Patrol behavior
func _patrol(delta: float) -> void:
	patrol_timer += delta
	
	if patrol_timer >= next_patrol_time:
		# Check if reached patrol point
		if _enemy.global_position.distance_squared_to(patrol_target) < Constants.ENEMY_WAYPOINT_ARRIVAL_THRESHOLD:
			_set_new_patrol_point()
		else:
			# Update navigation target
			if _nav_agent:
				_nav_agent.target_position = patrol_target


## Set a new random patrol point
func _set_new_patrol_point() -> void:
	var random_offset = Vector2(
		randf_range(-Constants.ENEMY_WANDER_DISTANCE, Constants.ENEMY_WANDER_DISTANCE),
		randf_range(-Constants.ENEMY_WANDER_DISTANCE, Constants.ENEMY_WANDER_DISTANCE)
	)
	patrol_target = spawn_position + Vector3(random_offset.x, 0, random_offset.y)
	next_patrol_time = randf_range(
		Constants.ENEMY_WANDER_INTERVAL - 1.0,
		Constants.ENEMY_WANDER_INTERVAL + 2.0
	)
	patrol_timer = 0.0
	
	if _nav_agent:
		_nav_agent.target_position = patrol_target


## Chase player behavior
func _chase_player() -> void:
	if _nav_agent and _player:
		_nav_agent.target_position = _player.global_position


## Attack player behavior
func _attack_player() -> void:
	# Attacking is handled externally, just check cooldown
	pass


## Flee from player behavior
func _flee_from_player() -> void:
	if _nav_agent and _player:
		var flee_direction = (_enemy.global_position - _player.global_position).normalized()
		var flee_target = _enemy.global_position + flee_direction * Constants.ENEMY_FLEE_DISTANCE
		_nav_agent.target_position = flee_target


## Check if can attack
func can_attack() -> bool:
	return current_state == State.ATTACK and attack_cooldown_timer <= 0


## Start attack cooldown
func start_attack_cooldown() -> void:
	attack_cooldown_timer = Constants.ENEMY_ATTACK_COOLDOWN


## Get movement speed for current state
func get_movement_speed() -> float:
	match current_state:
		State.FLEE:
			return Constants.ENEMY_MOVE_SPEED * Constants.ENEMY_FLEE_SPEED_MULTIPLIER
		State.PATROL:
			return Constants.ENEMY_MOVE_SPEED * Constants.ENEMY_PATROL_SPEED_MULTIPLIER
		_:
			return Constants.ENEMY_MOVE_SPEED


## Set state to dead
func set_dead() -> void:
	_change_state(State.DEAD)


## Get player reference
func get_player() -> Node3D:
	return _player


## Check if should stop moving (for attack state)
func should_stop_moving() -> bool:
	return current_state == State.ATTACK
