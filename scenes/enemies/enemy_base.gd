extends CharacterBody3D
class_name Enemy
## Base enemy class with AI
## Ported from Unity's Enemy.cs

enum State {
	PATROL,
	CHASE,
	ATTACK,
	FLEE,
	DEAD
}

@export var enemy_name: String = "Orc Scout"
@export var max_health: float = 50.0
@export var damage_amount: int = 10
@export var experience_reward: int = 50
@export var gold_reward: int = 25

var current_health: float = 50.0
var current_state: State = State.PATROL
var player: Node3D = null

# AI timers
var attack_cooldown_timer: float = 0.0
var patrol_timer: float = 0.0
var next_patrol_time: float = 0.0

# Spawn position for patrol
var spawn_position: Vector3 = Vector3.ZERO
var patrol_target: Vector3 = Vector3.ZERO

@onready var nav_agent: NavigationAgent3D = $NavigationAgent3D
@onready var mesh_instance: MeshInstance3D = $MeshInstance3D


func _ready() -> void:
	spawn_position = global_position
	current_health = max_health
	_set_new_patrol_point()
	
	# Get player reference
	await get_tree().process_frame
	player = GameManager.get_player()
	
	# Configure NavigationAgent
	if nav_agent:
		nav_agent.max_speed = Constants.ENEMY_MOVE_SPEED
		nav_agent.path_desired_distance = 0.5
		nav_agent.target_desired_distance = 0.5


func _physics_process(delta: float) -> void:
	if current_state == State.DEAD or player == null:
		return
	
	# Update cooldowns
	if attack_cooldown_timer > 0:
		attack_cooldown_timer -= delta
	
	# Get distance to player (squared for performance)
	var distance_to_player = global_position.distance_squared_to(player.global_position)
	var attack_range_sqr = Constants.ENEMY_ATTACK_RANGE * Constants.ENEMY_ATTACK_RANGE
	var detection_range_sqr = Constants.ENEMY_DETECTION_RANGE * Constants.ENEMY_DETECTION_RANGE
	
	# Check if should flee
	if current_health <= max_health * Constants.ENEMY_FLEE_HEALTH_PERCENT:
		current_state = State.FLEE
	# Check if can attack
	elif distance_to_player <= attack_range_sqr:
		current_state = State.ATTACK
	# Check if should chase
	elif distance_to_player <= detection_range_sqr:
		current_state = State.CHASE
	else:
		current_state = State.PATROL
	
	# Execute state behavior
	match current_state:
		State.PATROL:
			_patrol(delta)
		State.CHASE:
			_chase_player()
		State.ATTACK:
			_attack_player()
		State.FLEE:
			_flee_from_player()
	
	# Move using navigation agent
	if nav_agent and not nav_agent.is_navigation_finished():
		var next_position = nav_agent.get_next_path_position()
		var direction = (next_position - global_position).normalized()
		direction.y = 0  # Keep movement on horizontal plane
		
		var speed = Constants.ENEMY_MOVE_SPEED
		if current_state == State.FLEE:
			speed *= Constants.ENEMY_FLEE_SPEED_MULTIPLIER
		elif current_state == State.PATROL:
			speed *= Constants.ENEMY_PATROL_SPEED_MULTIPLIER
		
		velocity = direction * speed
		
		# Look in movement direction
		if direction.length() > 0.1:
			var look_target = global_position + direction
			look_at(Vector3(look_target.x, global_position.y, look_target.z), Vector3.UP)
		
		move_and_slide()


func _patrol(delta: float) -> void:
	patrol_timer += delta
	
	if patrol_timer >= next_patrol_time:
		# Check if reached patrol point
		if global_position.distance_squared_to(patrol_target) < Constants.ENEMY_WAYPOINT_ARRIVAL_THRESHOLD:
			_set_new_patrol_point()
		else:
			# Update navigation target
			if nav_agent:
				nav_agent.target_position = patrol_target


func _set_new_patrol_point() -> void:
	# Generate random point around spawn position
	var random_offset = Vector2(
		randf_range(-Constants.ENEMY_WANDER_DISTANCE, Constants.ENEMY_WANDER_DISTANCE),
		randf_range(-Constants.ENEMY_WANDER_DISTANCE, Constants.ENEMY_WANDER_DISTANCE)
	)
	patrol_target = spawn_position + Vector3(random_offset.x, 0, random_offset.y)
	next_patrol_time = randf_range(Constants.ENEMY_WANDER_INTERVAL - 1.0, Constants.ENEMY_WANDER_INTERVAL + 2.0)
	patrol_timer = 0.0
	
	if nav_agent:
		nav_agent.target_position = patrol_target


func _chase_player() -> void:
	if nav_agent and player:
		nav_agent.target_position = player.global_position


func _attack_player() -> void:
	# Stop moving when attacking
	velocity = Vector3.ZERO
	
	# Look at player
	if player:
		var target_pos = player.global_position
		look_at(Vector3(target_pos.x, global_position.y, target_pos.z), Vector3.UP)
	
	# Attack if cooldown is ready
	if attack_cooldown_timer <= 0:
		attack_cooldown_timer = Constants.ENEMY_ATTACK_COOLDOWN
		_perform_attack()


func _perform_attack() -> void:
	print(enemy_name, " attacks the player for ", damage_amount, " damage!")
	
	if player and player.has_method("take_damage"):
		player.stats.take_damage(damage_amount)
	
	EventBus.combat_started.emit(self)


func _flee_from_player() -> void:
	if nav_agent and player:
		# Set target in opposite direction from player
		var flee_direction = (global_position - player.global_position).normalized()
		var flee_target = global_position + flee_direction * Constants.ENEMY_FLEE_DISTANCE
		nav_agent.target_position = flee_target


func take_damage(damage: float) -> void:
	if current_state == State.DEAD:
		return
	
	current_health -= damage
	print(enemy_name, " takes ", damage, " damage! (", current_health, "/", max_health, ")")
	
	# Visual feedback
	_flash_red()
	
	if current_health <= 0:
		_die()


func _flash_red() -> void:
	if mesh_instance:
		var material = mesh_instance.get_active_material(0)
		if material:
			var tween = create_tween()
			var original_color = material.albedo_color
			tween.tween_property(material, "albedo_color", Color.RED, 0.1)
			tween.tween_property(material, "albedo_color", original_color, 0.1)


func _die() -> void:
	current_state = State.DEAD
	print(enemy_name, " has been defeated!")
	
	# Award experience to player
	if player and player.has_method("gain_experience"):
		player.stats.gain_experience(experience_reward)
	
	# Emit signals
	EventBus.enemy_killed.emit(self, experience_reward)
	EventBus.combat_ended.emit()
	
	# Update game stats
	GameManager.increment_enemies_defeated()
	
	# Death animation (fade out)
	var tween = create_tween()
	tween.tween_property(self, "scale", Vector3.ZERO, Constants.ENEMY_DEATH_FADE_DURATION)
	tween.tween_callback(queue_free)
