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
		if randf() <= entry.drop_chance:
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
		drop_chance = p_chance
		quantity_min = p_qty_min
		quantity_max = p_qty_max
