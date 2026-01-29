extends Node

## PetManager
## Manages collectible pets

signal pet_collected(pet_id: String, pet_name: String)
signal pet_summoned(pet_id: String)
signal pet_dismissed()

var pets: Dictionary = {}  # pet_id -> PetResource
var collected_pets: Array[String] = []
var active_pet: String = ""

func _ready() -> void:
	print("🐾 PetManager initialized")

func register_pet(pet: PetResource) -> void:
	"""Register a pet"""
	if pet.pet_id.is_empty():
		push_error("Cannot register pet with empty ID")
		return
	
	pets[pet.pet_id] = pet
	print("Pet registered: ", pet.pet_name)

func collect_pet(pet_id: String) -> bool:
	"""Collect a pet"""
	if pet_id not in pets:
		print("Pet not found")
		return false
	
	if pet_id in collected_pets:
		print("Pet already collected")
		return false
	
	var pet = pets[pet_id]
	pet.is_collected = true
	collected_pets.append(pet_id)
	
	pet_collected.emit(pet_id, pet.pet_name)
	
	# Achievement for first pet
	if collected_pets.size() == 1:
		EventBus.achievement_unlocked.emit("first_pet", "Collected your first pet")
	
	# Achievement for collecting all pets
	if collected_pets.size() == pets.size():
		EventBus.achievement_unlocked.emit("pet_collector", "Collected all pets")
	
	print("Pet collected: ", pet.pet_name)
	return true

func summon_pet(pet_id: String) -> bool:
	"""Summon a pet"""
	if pet_id not in collected_pets:
		print("Pet not collected")
		return false
	
	# Dismiss current pet if any
	if not active_pet.is_empty():
		dismiss_pet()
	
	active_pet = pet_id
	pet_summoned.emit(pet_id)
	
	print("Pet summoned: ", pets[pet_id].pet_name)
	return true

func dismiss_pet() -> void:
	"""Dismiss current pet"""
	if active_pet.is_empty():
		return
	
	var old_pet = active_pet
	active_pet = ""
	pet_dismissed.emit()
	
	print("Pet dismissed: ", pets[old_pet].pet_name)

func get_pet(pet_id: String) -> PetResource:
	"""Get a pet by ID"""
	return pets.get(pet_id, null)

func get_active_pet() -> PetResource:
	"""Get currently summoned pet"""
	if active_pet.is_empty():
		return null
	return pets.get(active_pet, null)

func get_collected_pets() -> Array[PetResource]:
	"""Get all collected pets"""
	var result: Array[PetResource] = []
	for pet_id in collected_pets:
		result.append(pets[pet_id])
	return result

func get_collection_progress() -> Dictionary:
	"""Get collection progress"""
	return {
		"collected": collected_pets.size(),
		"total": pets.size(),
		"percentage": (float(collected_pets.size()) / float(pets.size())) * 100.0 if pets.size() > 0 else 0.0
	}

func save_data() -> Dictionary:
	"""Save pet data"""
	return {
		"collected_pets": collected_pets,
		"active_pet": active_pet
	}

func load_data(data: Dictionary) -> void:
	"""Load pet data"""
	collected_pets = data.get("collected_pets", [])
	active_pet = data.get("active_pet", "")
	
	# Update collected status
	for pet_id in collected_pets:
		if pet_id in pets:
			pets[pet_id].is_collected = true
