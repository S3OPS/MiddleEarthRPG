extends Node

## CompanionManager
## Manages companion/hireling system

signal companion_hired(companion_id: String)
signal companion_dismissed(companion_id: String)
signal loyalty_changed(companion_id: String, old_value: int, new_value: int)

var companions: Dictionary = {}  # companion_id -> CompanionResource
var active_companions: Array[String] = []
var max_active_companions: int = 1

func _ready() -> void:
	pass

func register_companion(companion: CompanionResource) -> void:
	"""Register a companion"""
	if companion.companion_id.is_empty():
		push_error("Cannot register companion with empty ID")
		return
	
	companions[companion.companion_id] = companion
	print("Companion registered: ", companion.companion_name)

func can_hire(companion_id: String) -> Dictionary:
	"""Check if player can hire a companion"""
	var result = {
		"can_hire": false,
		"reason": ""
	}
	
	if companion_id not in companions:
		result["reason"] = "Companion not found"
		return result
	
	var companion = companions[companion_id]
	
	if companion.is_hired:
		result["reason"] = "Already hired"
		return result
	
	# Check level requirement
	if player_stats and player_stats.level < companion.required_level:
		result["reason"] = "Level " + str(companion.required_level) + " required"
		return result
	
	# Check quest requirement
	if not companion.required_quest.is_empty():
		if not QuestManager.is_quest_completed(companion.required_quest):
			result["reason"] = "Quest required: " + companion.required_quest
			return result
	
	# Check gold cost
	if GameManager.gold < companion.hire_cost:
		result["reason"] = "Not enough gold (need " + str(companion.hire_cost) + ")"
		return result
	
	# Check max companions
	if active_companions.size() >= max_active_companions:
		result["reason"] = "Maximum companions reached"
		return result
	
	result["can_hire"] = true
	return result

func hire_companion(companion_id: String) -> bool:
	"""Hire a companion"""
	var check = can_hire(companion_id)
	if not check["can_hire"]:
		print("Cannot hire: ", check["reason"])
		return false
	
	var companion = companions[companion_id]
	
	# Deduct hire cost
	GameManager.remove_gold(companion.hire_cost)
	
	# Hire companion
	companion.is_hired = true
	active_companions.append(companion_id)
	companion_hired.emit(companion_id)
	
	print("Hired companion: ", companion.companion_name)
	return true

func dismiss_companion(companion_id: String) -> bool:
	"""Dismiss a hired companion"""
	if companion_id not in companions:
		push_error("Companion not found: " + companion_id)
		return false
	
	if companion_id not in active_companions:
		push_error("Companion not active: " + companion_id)
		return false
	
	var companion = companions[companion_id]
	companion.is_hired = false
	active_companions.erase(companion_id)
	companion_dismissed.emit(companion_id)
	
	print("Dismissed companion: ", companion.companion_name)
	return true

func change_loyalty(companion_id: String, amount: int) -> void:
	"""Change companion loyalty"""
	if companion_id not in companions:
		push_error("Companion not found: " + companion_id)
		return
	
	var companion = companions[companion_id]
	var old_loyalty = companion.loyalty
	companion.loyalty = clamp(companion.loyalty + amount, 0, 100)
	
	loyalty_changed.emit(companion_id, old_loyalty, companion.loyalty)
	
	if amount > 0:
		print(companion.companion_name, " loyalty increased to ", companion.loyalty)
	else:
		print(companion.companion_name, " loyalty decreased to ", companion.loyalty)
	
	# Check for desertion
	if companion.loyalty <= 0 and companion.is_hired:
		print(companion.companion_name, " has left due to low loyalty!")
		dismiss_companion(companion_id)

func get_companion(companion_id: String) -> CompanionResource:
	"""Get a companion by ID"""
	return companions.get(companion_id, null)

func get_active_companions() -> Array[CompanionResource]:
	"""Get all active companions"""
	var result: Array[CompanionResource] = []
	for companion_id in active_companions:
		result.append(companions[companion_id])
	return result

func level_up_companion(companion_id: String) -> void:
	"""Level up a companion"""
	if companion_id not in companions:
		push_error("Companion not found: " + companion_id)
		return
	
	var companion = companions[companion_id]
	companion.level += 1
	companion.max_health += 10
	companion.attack += 2
	companion.defense += 1
	print(companion.companion_name, " leveled up to ", companion.level)

func save_data() -> Dictionary:
	"""Save companion data"""
	var companion_data = {}
	for companion_id in companions:
		var companion = companions[companion_id]
		companion_data[companion_id] = {
			"is_hired": companion.is_hired,
			"loyalty": companion.loyalty,
			"level": companion.level
		}
	
	return {
		"companions": companion_data,
		"active_companions": active_companions
	}

func load_data(data: Dictionary) -> void:
	"""Load companion data"""
	var companion_data = data.get("companions", {})
	active_companions = data.get("active_companions", [])
	
	for companion_id in companion_data:
		if companion_id in companions:
			var saved = companion_data[companion_id]
			companions[companion_id].is_hired = saved.get("is_hired", false)
			companions[companion_id].loyalty = saved.get("loyalty", 50)
			companions[companion_id].level = saved.get("level", 1)
