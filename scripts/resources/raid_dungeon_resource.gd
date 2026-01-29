extends Resource
class_name RaidDungeonResource

## Raid Dungeon Resource
## Defines a raid dungeon instance

@export var raid_id: String = ""
@export var raid_name: String = ""
@export var description: String = ""
@export var difficulty: String = "normal"  # normal, heroic, mythic
@export var min_players: int = 6
@export var max_players: int = 10
@export var required_level: int = 15

# Boss encounters
@export var boss_count: int = 0
@export var bosses: Array[String] = []

# Rewards
@export var loot_table: Dictionary = {}  # item_id -> drop_chance
@export var guaranteed_rewards: Array[String] = []
@export var raid_tokens: int = 0

# Lockout
@export var lockout_duration: float = 604800.0  # 1 week in seconds
@export var last_cleared: float = 0.0

func _init(
	p_id: String = "",
	p_name: String = "",
	p_min: int = 6,
	p_max: int = 10
):
	raid_id = p_id
	raid_name = p_name
	min_players = p_min
	max_players = p_max

func is_locked_out() -> bool:
	"""Check if raid is on lockout"""
	if last_cleared == 0.0:
		return false
	
	var current_time = Time.get_ticks_msec() / 1000.0
	return (current_time - last_cleared) < lockout_duration

func mark_cleared() -> void:
	"""Mark raid as cleared (starts lockout)"""
	last_cleared = Time.get_ticks_msec() / 1000.0
