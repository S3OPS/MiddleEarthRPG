extends Node
## Global save/load manager
## Handles persistent data storage using Godot's FileAccess

const SAVE_DIR = "user://saves/"
const SAVE_FILE_EXTENSION = ".save"

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
		slot_index = data.get("slot_index", 0)
		save_date = data.get("save_date", "")
		play_time = data.get("play_time", 0.0)
		var pos = data.get("player_position", {"x": 0, "y": 0, "z": 0})
		player_position = Vector3(pos.x, pos.y, pos.z)
		var rot = data.get("player_rotation", {"x": 0, "y": 0, "z": 0})
		player_rotation = Vector3(rot.x, rot.y, rot.z)
		character_name = data.get("character_name", "Aragorn")
		level = data.get("level", 1)
		experience = data.get("experience", 0)
		experience_to_next_level = data.get("experience_to_next_level", 100)
		current_health = data.get("current_health", 100.0)
		max_health = data.get("max_health", 100.0)
		current_stamina = data.get("current_stamina", 100.0)
		max_stamina = data.get("max_stamina", 100.0)
		strength = data.get("strength", 10)
		wisdom = data.get("wisdom", 10)
		agility = data.get("agility", 10)
		inventory_items = data.get("inventory_items", [])
		equipped_items = data.get("equipped_items", {})
		active_quests = data.get("active_quests", [])
		completed_quests = data.get("completed_quests", [])
		discovered_locations = data.get("discovered_locations", [])
		opened_chests = data.get("opened_chests", [])
		enemies_defeated = data.get("enemies_defeated", 0)
		quests_completed = data.get("quests_completed", 0)
		treasures_found = data.get("treasures_found", 0)


func _ready() -> void:
	# Ensure save directory exists
	if not DirAccess.dir_exists_absolute(SAVE_DIR):
		DirAccess.make_dir_recursive_absolute(SAVE_DIR)
	print("💾 SaveManager initialized")


## Save game to specified slot
func save_game(slot_index: int) -> bool:
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
		push_error("Failed to open save file: " + file_path)
		EventBus.save_failed.emit("Failed to open save file")
		return false
	
	var json_string = JSON.stringify(save_data.to_dict(), "\t")
	file.store_string(json_string)
	file.close()
	
	print("💾 Game saved to slot ", slot_index)
	EventBus.game_saved.emit(slot_index)
	return true


## Load game from specified slot
func load_game(slot_index: int) -> bool:
	var file_path = _get_save_path(slot_index)
	
	if not FileAccess.file_exists(file_path):
		push_error("Save file does not exist: " + file_path)
		return false
	
	var file = FileAccess.open(file_path, FileAccess.READ)
	
	if file == null:
		push_error("Failed to open save file: " + file_path)
		return false
	
	var json_string = file.get_as_text()
	file.close()
	
	var json = JSON.new()
	var parse_result = json.parse(json_string)
	
	if parse_result != OK:
		push_error("Failed to parse save file JSON")
		return false
	
	var save_data = SaveData.new()
	save_data.from_dict(json.data)
	
	# Apply loaded data to game
	_apply_save_data(save_data)
	
	print("💾 Game loaded from slot ", slot_index)
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
	
	DirAccess.remove_absolute(file_path)
	print("🗑️ Save deleted from slot ", slot_index)
	return true


## Get save file path for slot
func _get_save_path(slot_index: int) -> String:
	return SAVE_DIR + "slot_" + str(slot_index) + SAVE_FILE_EXTENSION


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
