extends Resource
class_name HousingResource

## Housing Resource
## Defines a player housing/base

@export var house_id: String = ""
@export var house_name: String = ""
@export var house_type: String = "cottage"  # cottage, mansion, tower
@export var region_id: String = ""
@export var location: Vector3 = Vector3.ZERO

# Status
@export var is_owned: bool = false
@export var purchase_cost: int = 5000
@export var upgrade_level: int = 0
@export var max_upgrade_level: int = 5

# Storage
@export var storage_capacity: int = 100
@export var stored_items: Dictionary = {}  # item_id -> quantity

# Decorations
@export var decorations: Array[String] = []
@export var max_decorations: int = 20

func _init(
	p_id: String = "",
	p_name: String = "",
	p_type: String = "cottage"
):
	house_id = p_id
	house_name = p_name
	house_type = p_type

func can_upgrade() -> bool:
	"""Check if house can be upgraded"""
	return upgrade_level < max_upgrade_level

func get_upgrade_cost() -> int:
	"""Get cost to upgrade to next level"""
	return 1000 * (upgrade_level + 1)
