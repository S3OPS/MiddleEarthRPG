extends Node
class_name PlayerCombatComponent
## Handles player combat, attacks, and special abilities
## Separated component for modular design

signal attack_performed(target: Node, damage: float, is_critical: bool)
signal special_attack_performed(targets: Array, damage: float)

var _character: CharacterBody3D
var _stats: CharacterStats
var _attack_raycast: RayCast3D

# Combat state
var can_attack: bool = true
var attack_cooldown_timer: float = 0.0
var can_use_special: bool = true
var special_cooldown_timer: float = 0.0


## Initialize the combat component
func initialize(character: CharacterBody3D, stats: CharacterStats, raycast: RayCast3D) -> void:
	_character = character
	_stats = stats
	_attack_raycast = raycast
	
	# Configure attack raycast
	if _attack_raycast:
		_attack_raycast.target_position = Vector3(0, 0, -Constants.ATTACK_RANGE)


## Process combat input and cooldowns
func process_combat(delta: float) -> void:
	_update_cooldowns(delta)
	_handle_combat_input()


## Update attack and special cooldowns
func _update_cooldowns(delta: float) -> void:
	if not can_attack:
		attack_cooldown_timer -= delta
		if attack_cooldown_timer <= 0:
			can_attack = true
	
	if not can_use_special:
		special_cooldown_timer -= delta
		if special_cooldown_timer <= 0:
			can_use_special = true


## Handle combat input
func _handle_combat_input() -> void:
	if Input.is_action_just_pressed("attack") and can_attack:
		perform_attack()
	
	if Input.is_action_just_pressed("special_attack") and can_use_special:
		perform_special_attack()


## Perform a normal attack
func perform_attack() -> void:
	if not _is_attack_valid():
		return
	
	print("⚔️ Player attacks!")
	can_attack = false
	attack_cooldown_timer = Constants.ATTACK_COOLDOWN
	
	var target = _get_attack_target()
	if target:
		_deal_attack_damage(target)


## Check if attack can be performed
func _is_attack_valid() -> bool:
	return _attack_raycast != null and _attack_raycast.is_colliding()


## Get the target of the attack
func _get_attack_target() -> Node:
	if not _attack_raycast:
		return null
	
	var target = _attack_raycast.get_collider()
	if target and target.has_method("take_damage"):
		return target
	return null


## Deal damage to target
func _deal_attack_damage(target: Node) -> void:
	var damage = Constants.calculate_damage(Constants.PLAYER_BASE_ATTACK_DAMAGE, _stats.strength)
	var is_critical = Constants.is_critical_hit()
	
	if is_critical:
		damage = Constants.apply_critical_multiplier(damage)
		print("💥 CRITICAL HIT! Damage: %.1f" % damage)
	
	target.take_damage(damage)
	EventBus.damage_dealt.emit(target, damage, is_critical)
	attack_performed.emit(target, damage, is_critical)


## Perform special AOE attack
func perform_special_attack() -> void:
	# Check if player has enough stamina
	if not _stats.use_stamina(Constants.SPECIAL_ABILITY_STAMINA_COST):
		print("⚠️ Not enough stamina for special attack!")
		return
	
	print("✨ Player uses SPECIAL ATTACK!")
	can_use_special = false
	special_cooldown_timer = Constants.SPECIAL_COOLDOWN
	
	# Deal AOE damage to nearby enemies
	_deal_aoe_damage()


## Deal area of effect damage to nearby enemies
func _deal_aoe_damage() -> void:
	var targets = _get_nearby_enemies()
	var damage = Constants.calculate_damage(Constants.PLAYER_SPECIAL_ATTACK_DAMAGE, _stats.strength)
	
	for target in targets:
		if target.has_method("take_damage"):
			target.take_damage(damage)
			EventBus.damage_dealt.emit(target, damage, false)
	
	special_attack_performed.emit(targets, damage)


## Get all enemies within special attack range
func _get_nearby_enemies() -> Array:
	var space_state = _character.get_world_3d().direct_space_state
	var query = PhysicsShapeQueryParameters3D.new()
	var sphere = SphereShape3D.new()
	sphere.radius = Constants.SPECIAL_AOE_RADIUS
	query.shape = sphere
	query.transform = _character.global_transform
	query.collision_mask = 4  # Enemy layer
	
	var results = space_state.intersect_shape(query)
	var enemies = []
	for result in results:
		enemies.append(result.collider)
	return enemies


## Check if can attack
func is_attack_ready() -> bool:
	return can_attack


## Check if special is ready
func is_special_ready() -> bool:
	return can_use_special


## Get attack cooldown progress (0-1)
func get_attack_cooldown_progress() -> float:
	if can_attack:
		return 1.0
	return 1.0 - (attack_cooldown_timer / Constants.ATTACK_COOLDOWN)


## Get special cooldown progress (0-1)
func get_special_cooldown_progress() -> float:
	if can_use_special:
		return 1.0
	return 1.0 - (special_cooldown_timer / Constants.SPECIAL_COOLDOWN)
