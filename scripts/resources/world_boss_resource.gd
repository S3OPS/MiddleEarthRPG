extends Resource
class_name WorldBossResource

## World Boss Resource
## Defines a world boss encounter

@export var boss_id: String = ""
@export var boss_name: String = ""
@export var description: String = ""
@export var spawn_location: Vector3 = Vector3.ZERO
@export var region_id: String = ""

# Stats
@export var level: int = 20
@export var health: int = 100000
@export var damage: int = 500
@export var armor: int = 200

# Spawning
@export var spawn_cooldown: float = 86400.0  # 24 hours
@export var last_spawn_time: float = 0.0
@export var is_spawned: bool = false

# Participation
@export var participants: Array[String] = []
@export var damage_dealt: Dictionary = {}  # player_id -> damage_amount

# Rewards
@export var loot_table: Dictionary = {}
@export var guaranteed_drops: Array[String] = []

func _init(
	p_id: String = "",
	p_name: String = "",
	p_region: String = ""
):
	boss_id = p_id
	boss_name = p_name
	region_id = p_region

func can_spawn() -> bool:
	"""Check if boss can spawn"""
	if is_spawned:
		return false
	
	if last_spawn_time == 0.0:
		return true
	
	var current_time = Time.get_ticks_msec() / 1000.0
	return (current_time - last_spawn_time) >= spawn_cooldown

func spawn() -> void:
	"""Spawn the world boss"""
	is_spawned = true
	last_spawn_time = Time.get_ticks_msec() / 1000.0
	participants.clear()
	damage_dealt.clear()

func despawn() -> void:
	"""Despawn the world boss"""
	is_spawned = false

func add_participant(player_id: String, damage: int) -> void:
	"""Track player participation"""
	if player_id not in participants:
		participants.append(player_id)
	
	if player_id in damage_dealt:
		damage_dealt[player_id] += damage
	else:
		damage_dealt[player_id] = damage

func get_top_contributors(count: int = 10) -> Array:
	"""Get top damage dealers"""
	var sorted_contributors = []
	for player_id in damage_dealt:
		sorted_contributors.append({
			"player_id": player_id,
			"damage": damage_dealt[player_id]
		})
	
	sorted_contributors.sort_custom(func(a, b): return a["damage"] > b["damage"])
	
	return sorted_contributors.slice(0, min(count, sorted_contributors.size()))
