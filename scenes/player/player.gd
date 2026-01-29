extends CharacterBody3D
## Player character controller
## Implements movement, camera, and basic interactions

@export var stats: CharacterStats

@onready var camera_pivot: Node3D = $CameraPivot
@onready var camera: Camera3D = $CameraPivot/Camera3D
@onready var attack_raycast: RayCast3D = $AttackRaycast

# Movement
var move_speed: float = Constants.PLAYER_WALK_SPEED
var is_sprinting: bool = false

# Camera
var camera_rotation: Vector2 = Vector2.ZERO
var mouse_captured: bool = true

# Combat
var can_attack: bool = true
var attack_cooldown_timer: float = 0.0
var can_use_special: bool = true
var special_cooldown_timer: float = 0.0

# Stamina regeneration
var stamina_regen_timer: float = 0.0


func _ready() -> void:
	# Create default stats if not assigned
	if stats == null:
		stats = CharacterStats.new()
	
	# Capture mouse
	Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
	
	# Register with GameManager
	GameManager.register_player(self)
	
	# Set up attack raycast
	if attack_raycast:
		attack_raycast.target_position = Vector3(0, 0, -Constants.ATTACK_RANGE)
	
	print("✅ Player initialized: ", stats.character_name)


## Handle taking damage from enemies
func take_damage(damage: float) -> void:
	if stats:
		stats.take_damage(damage)


func _input(event: InputEvent) -> void:
	# Handle mouse movement for camera
	if event is InputEventMouseMotion and mouse_captured:
		camera_rotation.x -= event.relative.y * Constants.CAMERA_MOUSE_SENSITIVITY
		camera_rotation.y -= event.relative.x * Constants.CAMERA_MOUSE_SENSITIVITY
		camera_rotation.x = clamp(camera_rotation.x, -PI/2, PI/2)
	
	# Toggle mouse capture
	if event.is_action_pressed("ui_cancel"):
		mouse_captured = not mouse_captured
		Input.mouse_mode = Input.MOUSE_MODE_CAPTURED if mouse_captured else Input.MOUSE_MODE_VISIBLE


func _physics_process(delta: float) -> void:
	# Update camera rotation
	camera_pivot.rotation.x = camera_rotation.x
	rotation.y = camera_rotation.y
	
	# Handle movement
	_handle_movement(delta)
	
	# Handle combat
	_handle_combat(delta)
	
	# Handle stamina regeneration
	_handle_stamina_regen(delta)
	
	# Apply movement
	move_and_slide()


func _handle_movement(delta: float) -> void:
	# Get input direction
	var input_dir := Input.get_vector("move_left", "move_right", "move_forward", "move_backward")
	var direction := (transform.basis * Vector3(input_dir.x, 0, input_dir.y)).normalized()
	
	# Handle sprinting
	is_sprinting = Input.is_action_pressed("sprint") and stats.current_stamina > 0
	
	if is_sprinting:
		move_speed = Constants.get_sprint_speed()
		# Drain stamina while sprinting
		stats.use_stamina(Constants.SPRINT_STAMINA_DRAIN_RATE * delta)
		stamina_regen_timer = Constants.STAMINA_REGEN_DELAY
	else:
		move_speed = Constants.PLAYER_WALK_SPEED
	
	# Apply movement
	if direction:
		velocity.x = direction.x * move_speed
		velocity.z = direction.z * move_speed
	else:
		velocity.x = move_toward(velocity.x, 0, move_speed)
		velocity.z = move_toward(velocity.z, 0, move_speed)
	
	# Handle jumping
	if Input.is_action_just_pressed("jump") and is_on_floor():
		velocity.y = Constants.PLAYER_JUMP_VELOCITY
	
	# Apply gravity
	if not is_on_floor():
		velocity.y -= ProjectSettings.get_setting("physics/3d/default_gravity") * delta


func _handle_combat(delta: float) -> void:
	# Update cooldowns
	if not can_attack:
		attack_cooldown_timer -= delta
		if attack_cooldown_timer <= 0:
			can_attack = true
	
	if not can_use_special:
		special_cooldown_timer -= delta
		if special_cooldown_timer <= 0:
			can_use_special = true
	
	# Handle attack input
	if Input.is_action_just_pressed("attack") and can_attack:
		_perform_attack()
	
	# Handle special attack input
	if Input.is_action_just_pressed("special_attack") and can_use_special:
		_perform_special_attack()


func _handle_stamina_regen(delta: float) -> void:
	if stamina_regen_timer > 0:
		stamina_regen_timer -= delta
	elif stats.current_stamina < stats.max_stamina:
		stats.restore_stamina(Constants.STAMINA_REGEN_RATE * delta)


func _perform_attack() -> void:
	print("⚔️ Player attacks!")
	can_attack = false
	attack_cooldown_timer = Constants.ATTACK_COOLDOWN
	
	# Check for hit using raycast
	if attack_raycast and attack_raycast.is_colliding():
		var target = attack_raycast.get_collider()
		if target and target.has_method("take_damage"):
			var damage = Constants.calculate_damage(Constants.PLAYER_BASE_ATTACK_DAMAGE, stats.strength)
			var is_critical = Constants.is_critical_hit()
			
			if is_critical:
				damage = Constants.apply_critical_multiplier(damage)
				print("💥 CRITICAL HIT! Damage: ", damage)
			
			target.take_damage(damage)
			EventBus.damage_dealt.emit(target, damage, is_critical)


func _perform_special_attack() -> void:
	# Check if player has enough stamina
	if not stats.use_stamina(Constants.SPECIAL_ABILITY_STAMINA_COST):
		print("⚠️ Not enough stamina for special attack!")
		return
	
	print("✨ Player uses SPECIAL ATTACK!")
	can_use_special = false
	special_cooldown_timer = Constants.SPECIAL_COOLDOWN
	stamina_regen_timer = Constants.STAMINA_REGEN_DELAY
	
	# Deal AOE damage to nearby enemies
	var space_state = get_world_3d().direct_space_state
	var query = PhysicsShapeQueryParameters3D.new()
	var sphere = SphereShape3D.new()
	sphere.radius = Constants.SPECIAL_AOE_RADIUS
	query.shape = sphere
	query.transform = global_transform
	query.collision_mask = 4  # Enemy layer
	
	var results = space_state.intersect_shape(query)
	for result in results:
		var target = result.collider
		if target and target.has_method("take_damage"):
			var damage = Constants.calculate_damage(Constants.PLAYER_SPECIAL_ATTACK_DAMAGE, stats.strength)
			target.take_damage(damage)
			EventBus.damage_dealt.emit(target, damage, false)


## Get save data for this player
func get_save_data() -> Dictionary:
	var data = stats.get_save_data()
	data["player_position"] = global_position
	data["player_rotation"] = rotation
	return data


## Apply save data to this player
func apply_save_data(save_data) -> void:
	if save_data is Dictionary:
		# Apply position
		var pos = save_data.get("player_position", Vector3.ZERO)
		if pos is Vector3:
			global_position = pos
		
		# Apply rotation
		var rot = save_data.get("player_rotation", Vector3.ZERO)
		if rot is Vector3:
			rotation = rot
			camera_rotation.y = rot.y
		
		# Apply stats
		stats.apply_save_data(save_data)
	elif save_data.has_method("get_save_data"):
		# SaveData object
		global_position = save_data.player_position
		rotation = save_data.player_rotation
		camera_rotation.y = save_data.player_rotation.y
		stats.apply_save_data(save_data.get_save_data())
