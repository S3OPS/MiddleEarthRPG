extends Node

## RaidManager
## Manages raid dungeons and instances

signal raid_started(raid_id: String)
signal raid_completed(raid_id: String)
signal boss_defeated(raid_id: String, boss_name: String)

var raids: Dictionary = {}  # raid_id -> RaidDungeonResource
var active_raid: String = ""
var raid_progress: Dictionary = {}  # raid_id -> bosses_defeated

func _ready() -> void:
	print("🏰 RaidManager initialized")

func register_raid(raid: RaidDungeonResource) -> void:
	"""Register a raid dungeon"""
	if raid.raid_id.is_empty():
		push_error("Cannot register raid with empty ID")
		return
	
	raids[raid.raid_id] = raid
	print("Raid registered: ", raid.raid_name)

func can_start_raid(raid_id: String, player_count: int) -> Dictionary:
	"""Check if raid can be started"""
	var result = {
		"can_start": false,
		"reason": ""
	}
	
	if raid_id not in raids:
		result["reason"] = "Raid not found"
		return result
	
	var raid = raids[raid_id]
	
	# Check lockout
	if raid.is_locked_out():
		result["reason"] = "Raid is on lockout"
		return result
	
	# Check player count
	if player_count < raid.min_players:
		result["reason"] = "Need at least " + str(raid.min_players) + " players"
		return result
	
	if player_count > raid.max_players:
		result["reason"] = "Maximum " + str(raid.max_players) + " players allowed"
		return result
	
	# Check level requirement
	if GameManager.player_level < raid.required_level:
		result["reason"] = "Level " + str(raid.required_level) + " required"
		return result
	
	result["can_start"] = true
	return result

func start_raid(raid_id: String, player_count: int) -> bool:
	"""Start a raid instance"""
	var check = can_start_raid(raid_id, player_count)
	if not check["can_start"]:
		print("Cannot start raid: ", check["reason"])
		return false
	
	active_raid = raid_id
	raid_progress[raid_id] = 0
	
	raid_started.emit(raid_id)
	print("Raid started: ", raids[raid_id].raid_name)
	return true

func defeat_boss(raid_id: String, boss_name: String) -> void:
	"""Mark a boss as defeated"""
	if raid_id not in raids:
		return
	
	if raid_id in raid_progress:
		raid_progress[raid_id] += 1
	else:
		raid_progress[raid_id] = 1
	
	boss_defeated.emit(raid_id, boss_name)
	
	var raid = raids[raid_id]
	if raid_progress[raid_id] >= raid.boss_count:
		complete_raid(raid_id)

func complete_raid(raid_id: String) -> void:
	"""Complete a raid"""
	if raid_id not in raids:
		return
	
	var raid = raids[raid_id]
	raid.mark_cleared()
	
	raid_completed.emit(raid_id)
	EventBus.achievement_unlocked.emit(
		"raid_" + raid_id,
		"Cleared " + raid.raid_name
	)
	
	active_raid = ""
	print("Raid completed: ", raid.raid_name)

func get_raid(raid_id: String) -> RaidDungeonResource:
	"""Get a raid by ID"""
	return raids.get(raid_id, null)

func get_raid_progress(raid_id: String) -> int:
	"""Get current progress in raid"""
	return raid_progress.get(raid_id, 0)

func save_data() -> Dictionary:
	"""Save raid data"""
	var raid_data = {}
	for raid_id in raids:
		raid_data[raid_id] = {
			"last_cleared": raids[raid_id].last_cleared
		}
	
	return {
		"raids": raid_data,
		"raid_progress": raid_progress
	}

func load_data(data: Dictionary) -> void:
	"""Load raid data"""
	raid_progress = data.get("raid_progress", {})
	
	var raid_data = data.get("raids", {})
	for raid_id in raid_data:
		if raid_id in raids:
			raids[raid_id].last_cleared = raid_data[raid_id].get("last_cleared", 0.0)
