extends Node

## PrestigeManager
## Manages prestige system and paragon points

signal prestige_earned(prestige_level: int)
signal paragon_point_earned(total_points: int)
signal paragon_spent(stat_name: String, new_value: int)

var prestige_level: int = 0
var paragon_points: int = 0
var paragon_spent: Dictionary = {
	"strength": 0,
	"agility": 0,
	"intelligence": 0,
	"vitality": 0,
	"endurance": 0
}

const MAX_LEVEL: int = 20
const PRESTIGE_LEVEL_CAP: int = 10

func _ready() -> void:
	print("⭐ PrestigeManager initialized")

func can_prestige() -> bool:
	"""Check if player can prestige"""
	return GameManager.player_level >= MAX_LEVEL and prestige_level < PRESTIGE_LEVEL_CAP

func prestige() -> bool:
	"""Prestige the character"""
	if not can_prestige():
		print("Cannot prestige: requirements not met")
		return false
	
	prestige_level += 1
	
	# Reset level but keep gear and some progress
	GameManager.player_level = 1
	GameManager.player_experience = 0
	
	# Award paragon points
	var points_awarded = 5
	paragon_points += points_awarded
	
	prestige_earned.emit(prestige_level)
	EventBus.achievement_unlocked.emit(
		"prestige_" + str(prestige_level),
		"Reached Prestige " + str(prestige_level)
	)
	
	print("Prestiged to level ", prestige_level)
	return true

func award_paragon_point() -> void:
	"""Award a paragon point (on level up after prestige)"""
	if prestige_level == 0:
		return
	
	paragon_points += 1
	paragon_point_earned.emit(paragon_points)
	print("Paragon point earned, total: ", paragon_points)

func spend_paragon_point(stat_name: String) -> bool:
	"""Spend a paragon point on a stat"""
	if paragon_points <= 0:
		print("No paragon points available")
		return false
	
	if stat_name not in paragon_spent:
		print("Invalid stat name")
		return false
	
	paragon_points -= 1
	paragon_spent[stat_name] += 1
	
	paragon_spent.emit(stat_name, paragon_spent[stat_name])
	print("Paragon point spent on ", stat_name)
	return true

func reset_paragon_points(cost: int) -> bool:
	"""Reset all paragon points (costs gold)"""
	if GameManager.player_gold < cost:
		print("Not enough gold to reset")
		return false
	
	GameManager.add_gold(-cost)
	
	# Refund all spent points
	for stat in paragon_spent:
		paragon_points += paragon_spent[stat]
		paragon_spent[stat] = 0
	
	print("Paragon points reset")
	return true

func get_prestige_bonus() -> Dictionary:
	"""Get bonuses from prestige level"""
	return {
		"xp_multiplier": 1.0 + (prestige_level * 0.1),
		"gold_multiplier": 1.0 + (prestige_level * 0.05),
		"stat_bonus": prestige_level * 2
	}

func get_paragon_bonuses() -> Dictionary:
	"""Get total bonuses from paragon points"""
	return {
		"strength": paragon_spent["strength"] * 5,
		"agility": paragon_spent["agility"] * 5,
		"intelligence": paragon_spent["intelligence"] * 5,
		"vitality": paragon_spent["vitality"] * 10,
		"endurance": paragon_spent["endurance"] * 10
	}

func save_data() -> Dictionary:
	"""Save prestige data"""
	return {
		"prestige_level": prestige_level,
		"paragon_points": paragon_points,
		"paragon_spent": paragon_spent
	}

func load_data(data: Dictionary) -> void:
	"""Load prestige data"""
	prestige_level = data.get("prestige_level", 0)
	paragon_points = data.get("paragon_points", 0)
	paragon_spent = data.get("paragon_spent", {
		"strength": 0,
		"agility": 0,
		"intelligence": 0,
		"vitality": 0,
		"endurance": 0
	})
