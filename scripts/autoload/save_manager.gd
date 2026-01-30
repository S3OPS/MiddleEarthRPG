extends Node
## Global save/load manager
## Handles persistent data storage using Godot's FileAccess

const SAVE_DIR = "user://saves/"
const SAVE_FILE_EXTENSION = ".save"
const MAX_SAVE_SLOTS = 10  # Limit to prevent abuse
const MAX_SAVE_FILE_SIZE = 1048576  # 1MB limit for save files
const SAVE_VERSION = 1  # Version for backward compatibility

## Save data structure
class SaveData:
	var slot_index: int = 0
	var save_date: String = ""
	var play_time: float = 0.0
	
	# Player data
	var player_position: Vector3 = Vector3.ZERO
	var player_rotation: Vector3 = Vector3.ZERO
	var character_name: String = "Aragorn"
	var level: int = 1
	var experience: int = 0
	var experience_to_next_level: int = 100
	var current_health: float = 100.0
	var max_health: float = 100.0
	var current_stamina: float = 100.0
	var max_stamina: float = 100.0
	var strength: int = 10
	var wisdom: int = 10
	var agility: int = 10
	
	# Inventory data
	var inventory_items: Array = []
	var equipped_items: Dictionary = {}
	
	# Quest data
	var active_quests: Array = []
	var completed_quests: Array = []
	
	# World data
	var discovered_locations: Array = []
	var opened_chests: Array = []
	
	# Statistics
	var enemies_defeated: int = 0
	var quests_completed: int = 0
	var treasures_found: int = 0
	
	func to_dict() -> Dictionary:
		return {
			"version": SAVE_VERSION,  # Add version for compatibility
			"slot_index": slot_index,
			"save_date": save_date,
			"play_time": play_time,
			"player_position": {"x": player_position.x, "y": player_position.y, "z": player_position.z},
			"player_rotation": {"x": player_rotation.x, "y": player_rotation.y, "z": player_rotation.z},
			"character_name": character_name,
			"level": level,
			"experience": experience,
			"experience_to_next_level": experience_to_next_level,
			"current_health": current_health,
			"max_health": max_health,
			"current_stamina": current_stamina,
			"max_stamina": max_stamina,
			"strength": strength,
			"wisdom": wisdom,
			"agility": agility,
			"inventory_items": inventory_items,
			"equipped_items": equipped_items,
			"active_quests": active_quests,
			"completed_quests": completed_quests,
			"discovered_locations": discovered_locations,
			"opened_chests": opened_chests,
			"enemies_defeated": enemies_defeated,
			"quests_completed": quests_completed,
			"treasures_found": treasures_found
		}
	
	func from_dict(data: Dictionary) -> void:
		# Validate version for compatibility
		var version = data.get("version", 0)
		if version > SAVE_VERSION:
			push_warning("Save file version %d is newer than supported version %d" % [version, SAVE_VERSION])
		
		slot_index = _validate_int(data.get("slot_index", 0), 0, MAX_SAVE_SLOTS)
		save_date = _sanitize_string(data.get("save_date", ""))
		play_time = _validate_float(data.get("play_time", 0.0), 0.0, 1000000.0)
		
		# Validate and clamp position values
		var pos = data.get("player_position", {"x": 0, "y": 0, "z": 0})
		player_position = Vector3(
			_validate_float(pos.get("x", 0), -10000, 10000),
			_validate_float(pos.get("y", 0), -10000, 10000),
			_validate_float(pos.get("z", 0), -10000, 10000)
		)
		
		var rot = data.get("player_rotation", {"x": 0, "y": 0, "z": 0})
		player_rotation = Vector3(
			_validate_float(rot.get("x", 0), -PI, PI),
			_validate_float(rot.get("y", 0), -PI, PI),
			_validate_float(rot.get("z", 0), -PI, PI)
		)
		
		character_name = _sanitize_string(data.get("character_name", "Aragorn"))
		level = _validate_int(data.get("level", 1), 1, 100)
		experience = _validate_int(data.get("experience", 0), 0, 1000000)
		experience_to_next_level = _validate_int(data.get("experience_to_next_level", 100), 1, 1000000)
		current_health = _validate_float(data.get("current_health", 100.0), 0.0, 10000.0)
		max_health = _validate_float(data.get("max_health", 100.0), 1.0, 10000.0)
		current_stamina = _validate_float(data.get("current_stamina", 100.0), 0.0, 10000.0)
		max_stamina = _validate_float(data.get("max_stamina", 100.0), 1.0, 10000.0)
		strength = _validate_int(data.get("strength", 10), 1, 1000)
		wisdom = _validate_int(data.get("wisdom", 10), 1, 1000)
		agility = _validate_int(data.get("agility", 10), 1, 1000)
		
		# Validate arrays (ensure they are arrays, not malicious objects)
		inventory_items = _validate_array(data.get("inventory_items", []))
		equipped_items = _validate_dict(data.get("equipped_items", {}))
		active_quests = _validate_array(data.get("active_quests", []))
		completed_quests = _validate_array(data.get("completed_quests", []))
		discovered_locations = _validate_array(data.get("discovered_locations", []))
		opened_chests = _validate_array(data.get("opened_chests", []))
		
		enemies_defeated = _validate_int(data.get("enemies_defeated", 0), 0, 1000000)
		quests_completed = _validate_int(data.get("quests_completed", 0), 0, 10000)
		treasures_found = _validate_int(data.get("treasures_found", 0), 0, 10000)
	
	
	## Validate and clamp integer value
	static func _validate_int(value: Variant, min_val: int, max_val: int) -> int:
		if not (value is int or value is float):
			return min_val
		return clampi(int(value), min_val, max_val)
	
	
	## Validate and clamp float value
	static func _validate_float(value: Variant, min_val: float, max_val: float) -> float:
		if not (value is float or value is int):
			return min_val
		return clampf(float(value), min_val, max_val)
	
	
	## Sanitize string to prevent injection
	static func _sanitize_string(value: Variant) -> String:
		if not value is String:
			return ""
		# Remove potential control characters and limit length
		var sanitized = value.strip_edges()
		if sanitized.length() > 100:
			sanitized = sanitized.substr(0, 100)
		return sanitized
	
	
	## Validate array type
	static func _validate_array(value: Variant) -> Array:
		if value is Array:
			return value
		return []
	
	
	## Validate dictionary type
	static func _validate_dict(value: Variant) -> Dictionary:
		if value is Dictionary:
			return value
		return {}


func _ready() -> void:
	# Ensure save directory exists
	if not DirAccess.dir_exists_absolute(SAVE_DIR):
		DirAccess.make_dir_recursive_absolute(SAVE_DIR)
	print("💾 SaveManager initialized")


## Save game to specified slot
func save_game(slot_index: int) -> bool:
	# Validate slot index
	if not _is_valid_slot(slot_index):
		push_error("Invalid save slot index: %d" % slot_index)
		EventBus.save_failed.emit("Invalid slot index")
		return false
	
	var save_data = SaveData.new()
	save_data.slot_index = slot_index
	save_data.save_date = Time.get_datetime_string_from_system()
	save_data.play_time = GameManager.total_play_time
	
	# Get player data if available
	var player = GameManager.get_player()
	if player != null and player.has_method("get_save_data"):
		var player_data = player.get_save_data()
		for key in player_data.keys():
			if key in save_data:
				save_data[key] = player_data[key]
	
	# Copy game statistics
	save_data.enemies_defeated = GameManager.enemies_defeated
	save_data.quests_completed = GameManager.quests_completed
	save_data.treasures_found = GameManager.treasures_found
	
	var file_path = _get_save_path(slot_index)
	var file = FileAccess.open(file_path, FileAccess.WRITE)
	
	if file == null:
		push_error("Failed to open save file: %s (Error: %d)" % [file_path, FileAccess.get_open_error()])
		EventBus.save_failed.emit("Failed to open save file")
		return false
	
	var json_string = JSON.stringify(save_data.to_dict(), "\t")
	file.store_string(json_string)
	
	# Validate that write succeeded by checking for errors
	if FileAccess.get_open_error() != OK:
		file.close()
		push_error("Failed to write save file: %s (Error: %d)" % [file_path, FileAccess.get_open_error()])
		EventBus.save_failed.emit("Failed to write save file")
		return false
	
	file.close()
	
	print("💾 Game saved to slot %d" % slot_index)
	EventBus.game_saved.emit(slot_index)
	return true


## Load game from specified slot
func load_game(slot_index: int) -> bool:
	# Validate slot index
	if not _is_valid_slot(slot_index):
		push_error("Invalid save slot index: %d" % slot_index)
		return false
	
	var file_path = _get_save_path(slot_index)
	
	if not FileAccess.file_exists(file_path):
		push_error("Save file does not exist: %s" % file_path)
		return false
	
	# Check file size for security using file handle
	var file_check = FileAccess.open(file_path, FileAccess.READ)
	if file_check == null:
		push_error("Failed to open save file for size check: %s" % file_path)
		return false
	
	var file_size = file_check.get_length()
	file_check.close()
	
	if file_size > MAX_SAVE_FILE_SIZE:
		push_error("Save file too large (potential corruption): %d bytes" % file_size)
		return false
	
	# Open file for reading
	var file = FileAccess.open(file_path, FileAccess.READ)
	
	if file == null:
		push_error("Failed to open save file: %s (Error: %d)" % [file_path, FileAccess.get_open_error()])
		return false
	
	var json_string = file.get_as_text()
	file.close()
	
	var json = JSON.new()
	var parse_result = json.parse(json_string)
	
	if parse_result != OK:
		push_error("Failed to parse save file JSON: %s" % json.get_error_message())
		return false
	
	# Validate that data is a dictionary
	if not json.data is Dictionary:
		push_error("Invalid save file format: expected Dictionary")
		return false
	
	var save_data = SaveData.new()
	save_data.from_dict(json.data)
	
	# Apply loaded data to game
	_apply_save_data(save_data)
	
	print("💾 Game loaded from slot %d" % slot_index)
	EventBus.game_loaded.emit(slot_index)
	return true


## Check if save exists in slot
func save_exists(slot_index: int) -> bool:
	return FileAccess.file_exists(_get_save_path(slot_index))


## Get save file info
func get_save_info(slot_index: int) -> Dictionary:
	if not save_exists(slot_index):
		return {}
	
	var file_path = _get_save_path(slot_index)
	var file = FileAccess.open(file_path, FileAccess.READ)
	
	if file == null:
		return {}
	
	var json_string = file.get_as_text()
	file.close()
	
	var json = JSON.new()
	if json.parse(json_string) != OK:
		return {}
	
	# Validate that json.data is a valid Dictionary before using it
	if json.data == null or not json.data is Dictionary:
		return {}
	
	var data = json.data
	return {
		"slot_index": data.get("slot_index", slot_index),
		"save_date": data.get("save_date", "Unknown"),
		"play_time": data.get("play_time", 0.0),
		"level": data.get("level", 1),
		"character_name": data.get("character_name", "Unknown")
	}


## Delete save from slot
func delete_save(slot_index: int) -> bool:
	var file_path = _get_save_path(slot_index)
	
	if not FileAccess.file_exists(file_path):
		return false
	
	var delete_result = DirAccess.remove_absolute(file_path)
	if delete_result != OK:
		push_error("Failed to delete save file: %s (Error: %d)" % [file_path, delete_result])
		return false
	
	print("🗑️ Save deleted from slot ", slot_index)
	return true


## Get save file path for slot
func _get_save_path(slot_index: int) -> String:
	# Sanitize slot index to prevent directory traversal
	var sanitized_index = clampi(slot_index, 0, MAX_SAVE_SLOTS)
	return SAVE_DIR + "slot_%d%s" % [sanitized_index, SAVE_FILE_EXTENSION]


## Validate slot index
func _is_valid_slot(slot_index: int) -> bool:
	return slot_index >= 0 and slot_index < MAX_SAVE_SLOTS


## Apply loaded save data to the game
func _apply_save_data(save_data: SaveData) -> void:
	# Update game manager
	GameManager.total_play_time = save_data.play_time
	GameManager.enemies_defeated = save_data.enemies_defeated
	GameManager.quests_completed = save_data.quests_completed
	GameManager.treasures_found = save_data.treasures_found
	
	# Apply to player if available
	var player = GameManager.get_player()
	if player != null and player.has_method("apply_save_data"):
		player.apply_save_data(save_data)
