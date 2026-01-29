extends Node

## DifficultyManager
## Manages game difficulty settings and balance tuning

signal difficulty_changed(old_difficulty: String, new_difficulty: String)

enum Difficulty {
	EASY,
	NORMAL,
	HARD,
	NIGHTMARE
}

var current_difficulty: String = "normal"

# Difficulty multipliers
var difficulty_settings = {
	"easy": {
		"name": "Easy",
		"description": "Relaxed experience for enjoying the story",
		"enemy_health_multiplier": 0.75,
		"enemy_damage_multiplier": 0.75,
		"player_damage_multiplier": 1.25,
		"xp_multiplier": 1.0,
		"gold_multiplier": 1.0,
		"stamina_drain_multiplier": 0.75
	},
	"normal": {
		"name": "Normal",
		"description": "Balanced challenge for most players",
		"enemy_health_multiplier": 1.0,
		"enemy_damage_multiplier": 1.0,
		"player_damage_multiplier": 1.0,
		"xp_multiplier": 1.0,
		"gold_multiplier": 1.0,
		"stamina_drain_multiplier": 1.0
	},
	"hard": {
		"name": "Hard",
		"description": "Tough combat for experienced players",
		"enemy_health_multiplier": 1.5,
		"enemy_damage_multiplier": 1.5,
		"player_damage_multiplier": 0.85,
		"xp_multiplier": 1.25,
		"gold_multiplier": 1.25,
		"stamina_drain_multiplier": 1.25
	},
	"nightmare": {
		"name": "Nightmare",
		"description": "Brutal difficulty for true veterans",
		"enemy_health_multiplier": 2.0,
		"enemy_damage_multiplier": 2.0,
		"player_damage_multiplier": 0.75,
		"xp_multiplier": 1.5,
		"gold_multiplier": 1.5,
		"stamina_drain_multiplier": 1.5
	}
}

func _ready() -> void:
	print("🎮 Difficulty set to: ", difficulty_settings[current_difficulty]["name"])

func set_difficulty(difficulty: String) -> bool:
	"""Set game difficulty"""
	if difficulty not in difficulty_settings:
		push_error("Invalid difficulty: " + difficulty)
		return false
	
	var old_difficulty = current_difficulty
	current_difficulty = difficulty
	difficulty_changed.emit(old_difficulty, current_difficulty)
	
	print("Difficulty changed to: ", difficulty_settings[current_difficulty]["name"])
	return true

func get_difficulty() -> String:
	"""Get current difficulty"""
	return current_difficulty

func get_setting(setting_name: String) -> float:
	"""Get a specific difficulty setting multiplier"""
	if current_difficulty not in difficulty_settings:
		return 1.0
	
	var settings = difficulty_settings[current_difficulty]
	return settings.get(setting_name, 1.0)

func get_enemy_health_multiplier() -> float:
	return get_setting("enemy_health_multiplier")

func get_enemy_damage_multiplier() -> float:
	return get_setting("enemy_damage_multiplier")

func get_player_damage_multiplier() -> float:
	return get_setting("player_damage_multiplier")

func get_xp_multiplier() -> float:
	return get_setting("xp_multiplier")

func get_gold_multiplier() -> float:
	return get_setting("gold_multiplier")

func get_stamina_drain_multiplier() -> float:
	return get_setting("stamina_drain_multiplier")

func get_all_difficulties() -> Array:
	"""Get list of all difficulties"""
	return difficulty_settings.keys()

func get_difficulty_info(difficulty: String) -> Dictionary:
	"""Get information about a specific difficulty"""
	return difficulty_settings.get(difficulty, {})

func save_data() -> Dictionary:
	"""Save difficulty data"""
	return {
		"difficulty": current_difficulty
	}

func load_data(data: Dictionary) -> void:
	"""Load difficulty data"""
	var saved_difficulty = data.get("difficulty", "normal")
	set_difficulty(saved_difficulty)
