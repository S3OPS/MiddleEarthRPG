extends Resource
class_name InventoryItem

## Represents an item that can be collected and stored in the inventory

enum ItemType { CONSUMABLE, EQUIPMENT, QUEST_ITEM, MATERIAL, MISC }
enum Rarity { COMMON, UNCOMMON, RARE, EPIC, LEGENDARY }
enum EquipmentSlot { WEAPON, ARMOR, ACCESSORY }

## Item identification
@export var item_id: String = ""
@export var item_name: String = ""
@export_multiline var description: String = ""

## Item properties
@export var type: ItemType = ItemType.MISC
@export var rarity: Rarity = Rarity.COMMON
@export var equipment_slot: EquipmentSlot = EquipmentSlot.WEAPON  # For equipment items
@export var icon_path: String = ""
@export var stackable: bool = true
@export var max_stack_size: int = 99
@export var value: int = 0  # Gold value

## Effects (for consumables)
@export var health_restore: int = 0
@export var stamina_restore: int = 0

## Equipment stats (for equipment items)
@export var attack_bonus: int = 0
@export var defense_bonus: int = 0
@export var health_bonus: int = 0
@export var stamina_bonus: int = 0

## Get rarity color for UI
func get_rarity_color() -> Color:
	match rarity:
		Rarity.COMMON:
			return Color.WHITE
		Rarity.UNCOMMON:
			return Color.GREEN
		Rarity.RARE:
			return Color.BLUE
		Rarity.EPIC:
			return Color.PURPLE
		Rarity.LEGENDARY:
			return Color.ORANGE
	return Color.WHITE

## Get type string
func get_type_string() -> String:
	return ItemType.keys()[type]

## Check if item can be used
func is_usable() -> bool:
	return type == ItemType.CONSUMABLE or type == ItemType.EQUIPMENT
