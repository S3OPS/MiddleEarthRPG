extends Node

## FactionManager
## Manages faction reputation and relationships

signal reputation_changed(faction_id: String, amount: int, new_total: int)
signal reputation_tier_changed(faction_id: String, old_tier: String, new_tier: String)

var factions: Dictionary = {}  # faction_id -> FactionResource

func _ready() -> void:
	pass

func register_faction(faction: FactionResource) -> void:
	"""Register a new faction"""
	if faction.faction_id.is_empty():
		push_error("Cannot register faction with empty ID")
		return
	
	factions[faction.faction_id] = faction
	print("Faction registered: ", faction.faction_name)

func add_reputation(faction_id: String, amount: int) -> void:
	"""Add or subtract reputation with a faction"""
	if faction_id not in factions:
		push_error("Faction not found: " + faction_id)
		return
	
	var faction = factions[faction_id]
	var old_tier = faction.reputation_tier
	var old_reputation = faction.current_reputation
	
	faction.add_reputation(amount)
	
	reputation_changed.emit(faction_id, amount, faction.current_reputation)
	
	# Check if tier changed
	if faction.reputation_tier != old_tier:
		reputation_tier_changed.emit(faction_id, old_tier, faction.reputation_tier)
		EventBus.achievement_unlocked.emit(
			"faction_" + faction_id + "_" + faction.reputation_tier,
			"Reached " + faction.reputation_tier.capitalize() + " with " + faction.faction_name
		)
		print("Reputation tier changed: ", faction.faction_name, " - ", old_tier, " -> ", faction.reputation_tier)
	
	if amount > 0:
		print("Gained ", amount, " reputation with ", faction.faction_name, " (", faction.current_reputation, ")")
	else:
		print("Lost ", abs(amount), " reputation with ", faction.faction_name, " (", faction.current_reputation, ")")

func get_faction(faction_id: String) -> FactionResource:
	"""Get a faction by ID"""
	return factions.get(faction_id, null)

func get_reputation(faction_id: String) -> int:
	"""Get current reputation with a faction"""
	var faction = get_faction(faction_id)
	if faction == null:
		return 0
	return faction.current_reputation

func get_reputation_tier(faction_id: String) -> String:
	"""Get current reputation tier with a faction"""
	var faction = get_faction(faction_id)
	if faction == null:
		return "neutral"
	return faction.reputation_tier

func has_reputation(faction_id: String, required_amount: int) -> bool:
	"""Check if player has at least the required reputation"""
	return get_reputation(faction_id) >= required_amount

func has_reputation_tier(faction_id: String, required_tier: String) -> bool:
	"""Check if player has at least the required reputation tier"""
	var faction = get_faction(faction_id)
	if faction == null:
		return false
	
	var tiers = ["hostile", "unfriendly", "neutral", "friendly", "honored", "exalted"]
	var current_index = tiers.find(faction.reputation_tier)
	var required_index = tiers.find(required_tier)
	
	return current_index >= required_index

func get_all_factions() -> Array[FactionResource]:
	"""Get all registered factions"""
	var result: Array[FactionResource] = []
	for faction_id in factions:
		result.append(factions[faction_id])
	return result

func save_data() -> Dictionary:
	"""Save faction data"""
	var faction_data = {}
	for faction_id in factions:
		faction_data[faction_id] = {
			"reputation": factions[faction_id].current_reputation,
			"tier": factions[faction_id].reputation_tier
		}
	
	return {
		"factions": faction_data
	}

func load_data(data: Dictionary) -> void:
	"""Load faction data"""
	var faction_data = data.get("factions", {})
	
	for faction_id in faction_data:
		if faction_id in factions:
			var saved = faction_data[faction_id]
			factions[faction_id].current_reputation = saved.get("reputation", 0)
			factions[faction_id].update_reputation_tier()
