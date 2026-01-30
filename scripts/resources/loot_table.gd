extends Resource
class_name LootTable

## LootTable Resource
## Defines drop chances and item pools for enemies and treasure chests

## Item drop entries
@export var loot_entries: Array = []
@export var gold_min: int = 0
@export var gold_max: int = 10
@export var guaranteed_gold: bool = false

func get_random_drops() -> Array[Dictionary]:
	var drops: Array[Dictionary] = []
	
	# Roll for each loot entry
	for entry in loot_entries:
		# Validate entry has required properties
		if not (entry is Dictionary or entry is LootEntry):
			continue
		if not (entry.has("drop_chance") if entry is Dictionary else true):
			continue
		
		# Validate drop_chance is in valid range (0.0 to 1.0)
		var drop_chance = entry.drop_chance if (entry is LootEntry or entry.has("drop_chance")) else 0.5
		if drop_chance < 0.0 or drop_chance > 1.0:
			push_error("Invalid drop_chance in loot entry: %f (must be 0.0-1.0)" % drop_chance)
			drop_chance = clampf(drop_chance, 0.0, 1.0)
		
		# Use the validated/clamped drop_chance
		if randf() <= drop_chance:
			# Validate quantity_min <= quantity_max before randi_range
			if entry.quantity_min > entry.quantity_max:
				push_error("Invalid quantity range in loot entry: %d > %d" % [entry.quantity_min, entry.quantity_max])
				continue
			
			var quantity = randi_range(entry.quantity_min, entry.quantity_max)
			drops.append({
				"item_id": entry.item_id,
				"quantity": quantity
			})
	
	return drops

func get_random_gold() -> int:
	if guaranteed_gold or randf() <= 0.7:  # 70% chance for gold
		return randi_range(gold_min, gold_max)
	return 0

## Inner class for loot entries
class LootEntry:
	var item_id: String = ""
	var drop_chance: float = 0.5
	var quantity_min: int = 1
	var quantity_max: int = 1
	
	func _init(p_item_id: String = "", p_chance: float = 0.5, p_qty_min: int = 1, p_qty_max: int = 1):
		item_id = p_item_id
		# Validate and clamp drop_chance to valid range
		if p_chance < 0.0 or p_chance > 1.0:
			push_warning("LootEntry: drop_chance %f out of range, clamping to 0.0-1.0" % p_chance)
			drop_chance = clampf(p_chance, 0.0, 1.0)
		else:
			drop_chance = p_chance
		quantity_min = p_qty_min
		quantity_max = p_qty_max
