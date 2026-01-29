extends Node

## SpecializationManager
## Manages combat specializations and skill trees

signal specialization_chosen(spec_id: String)
signal ability_unlocked(spec_id: String, ability_id: String)

var specializations: Dictionary = {}  # spec_id -> SpecializationResource
var current_specialization: String = ""
var unlocked_abilities: Array[String] = []
var specialization_level: int = 0

func _ready() -> void:
	pass

func register_specialization(spec: SpecializationResource) -> void:
	"""Register a specialization"""
	if spec.specialization_id.is_empty():
		push_error("Cannot register specialization with empty ID")
		return
	
	specializations[spec.specialization_id] = spec
	print("Specialization registered: ", spec.specialization_name)

func choose_specialization(spec_id: String) -> bool:
	"""Choose a combat specialization"""
	if spec_id not in specializations:
		push_error("Specialization not found: " + spec_id)
		return false
	
	if not current_specialization.is_empty():
		print("Already chosen a specialization: ", current_specialization)
		return false
	
	current_specialization = spec_id
	specialization_chosen.emit(spec_id)
	
	# Apply passive bonuses
	_apply_specialization_bonuses(spec_id)
	
	print("Chosen specialization: ", specializations[spec_id].specialization_name)
	return true

func unlock_ability(ability_id: String) -> bool:
	"""Unlock a specialization ability"""
	if current_specialization.is_empty():
		print("No specialization chosen")
		return false
	
	var spec = specializations[current_specialization]
	
	if ability_id not in spec.abilities:
		print("Ability not in specialization: ", ability_id)
		return false
	
	var required_level = spec.abilities[ability_id]
	if specialization_level < required_level:
		print("Level ", required_level, " required for ability")
		return false
	
	if ability_id in unlocked_abilities:
		print("Ability already unlocked")
		return false
	
	unlocked_abilities.append(ability_id)
	ability_unlocked.emit(current_specialization, ability_id)
	print("Unlocked ability: ", ability_id)
	return true

func increase_specialization_level() -> void:
	"""Increase specialization level (called on player level up)"""
	specialization_level += 1
	print("Specialization level increased to ", specialization_level)

func _apply_specialization_bonuses(spec_id: String) -> void:
	"""Apply passive bonuses from specialization"""
	var spec = specializations[spec_id]
	
	# TODO: Apply bonuses to player stats when player system is updated
	print("Applied bonuses: +", spec.attack_bonus, " attack, +", spec.defense_bonus, " defense")

func get_specialization(spec_id: String) -> SpecializationResource:
	"""Get a specialization by ID"""
	return specializations.get(spec_id, null)

func get_current_specialization() -> SpecializationResource:
	"""Get the current specialization"""
	if current_specialization.is_empty():
		return null
	return specializations.get(current_specialization, null)

func has_ability(ability_id: String) -> bool:
	"""Check if player has unlocked an ability"""
	return ability_id in unlocked_abilities

func save_data() -> Dictionary:
	"""Save specialization data"""
	return {
		"current_specialization": current_specialization,
		"unlocked_abilities": unlocked_abilities,
		"specialization_level": specialization_level
	}

func load_data(data: Dictionary) -> void:
	"""Load specialization data"""
	current_specialization = data.get("current_specialization", "")
	unlocked_abilities = data.get("unlocked_abilities", [])
	specialization_level = data.get("specialization_level", 0)
	
	# Reapply bonuses
	if not current_specialization.is_empty():
		_apply_specialization_bonuses(current_specialization)
