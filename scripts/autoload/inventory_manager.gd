extends Node

## Manages the player's inventory and equipment
## Handles item collection, usage, and equipment

const MAX_INVENTORY_SIZE = 100

# Inventory structure: item_id -> {item: InventoryItem, quantity: int}
var inventory: Dictionary = {}
var equipped_items: Dictionary = {}  # slot_name -> item_id

func _ready() -> void:
	# Connect to relevant events
	EventBus.item_rewarded.connect(_on_item_rewarded)
	print("🎒 InventoryManager initialized")

## Add an item to inventory
func add_item(item: InventoryItem, quantity: int = 1) -> bool:
	if not item or quantity <= 0:
		push_error("Invalid item or quantity")
		return false
	
	# Check if inventory has space for new unique items
	if not inventory.has(item.item_id) and inventory.size() >= MAX_INVENTORY_SIZE:
		EventBus.inventory_full.emit()
		EventBus.show_notification("Inventory is full!", "warning")
		return false
	
	# Add or update item
	if inventory.has(item.item_id):
		# Item exists, increase quantity if stackable
		if item.stackable:
			var max_add = item.max_stack_size - inventory[item.item_id]["quantity"]
			var added = min(quantity, max_add)
			inventory[item.item_id]["quantity"] += added
			quantity -= added
		else:
			# Non-stackable item already exists
			EventBus.show_notification("Already have " + item.item_name, "warning")
			return false
	else:
		# New item
		inventory[item.item_id] = {
			"item": item,
			"quantity": min(quantity, item.max_stack_size if item.stackable else 1)
		}
		quantity -= inventory[item.item_id]["quantity"]
	
	EventBus.item_added.emit(item.item_id, item.item_name, inventory[item.item_id]["quantity"])
	EventBus.item_collected.emit(item.item_id)
	EventBus.show_notification("+" + str(inventory[item.item_id]["quantity"]) + " " + item.item_name, "success")
	
	# If there's leftover quantity, couldn't add all
	return quantity == 0

## Remove an item from inventory
func remove_item(item_id: String, quantity: int = 1) -> bool:
	if not inventory.has(item_id) or quantity <= 0:
		return false
	
	var item_data = inventory[item_id]
	if item_data["quantity"] < quantity:
		return false
	
	# If item is equipped, unequip it first
	for slot_name in equipped_items:
		if equipped_items[slot_name] == item_id:
			unequip_item(slot_name)
			break
	
	item_data["quantity"] -= quantity
	
	if item_data["quantity"] <= 0:
		inventory.erase(item_id)
	
	EventBus.item_removed.emit(item_id, quantity)
	return true

## Check if player has an item
func has_item(item_id: String, quantity: int = 1) -> bool:
	if not inventory.has(item_id):
		return false
	return inventory[item_id]["quantity"] >= quantity

## Get item quantity
func get_item_quantity(item_id: String) -> int:
	if not inventory.has(item_id):
		return 0
	return inventory[item_id]["quantity"]

## Use an item (consumable or equipment)
func use_item(item_id: String) -> bool:
	if not inventory.has(item_id):
		return false
	
	var item_data = inventory[item_id]
	var item: InventoryItem = item_data["item"]
	
	match item.type:
		InventoryItem.ItemType.CONSUMABLE:
			_use_consumable(item)
			remove_item(item_id, 1)
			return true
		InventoryItem.ItemType.EQUIPMENT:
			return equip_item(item_id)
		_:
			EventBus.show_notification("Cannot use " + item.item_name, "warning")
			return false

## Use a consumable item
func _use_consumable(item: InventoryItem) -> void:
	var player = GameManager.get_player()
	if not player:
		return
	if not GameManager.player_stats:
		return
	
	if item.health_restore > 0:
		GameManager.player_stats.heal(item.health_restore)
		EventBus.show_notification("Restored %d health" % item.health_restore, "success")
	
	if item.stamina_restore > 0:
		GameManager.player_stats.restore_stamina(item.stamina_restore)
		EventBus.show_notification("Restored %d stamina" % item.stamina_restore, "success")

## Equip an item
func equip_item(item_id: String) -> bool:
	if not inventory.has(item_id):
		return false
	
	var item: InventoryItem = inventory[item_id]["item"]
	if item.type != InventoryItem.ItemType.EQUIPMENT:
		return false
	
	var slot_name = _get_equipment_slot_name(item)
	
	# Unequip current item in slot if exists
	if equipped_items.has(slot_name):
		unequip_item(slot_name)
	
	# Equip new item
	equipped_items[slot_name] = item_id
	_apply_equipment_bonuses(item, true)
	
	EventBus.item_equipped.emit(item_id, slot_name)
	EventBus.show_notification("Equipped " + item.item_name, "success")
	return true

## Unequip an item from a slot
func unequip_item(slot_name: String) -> bool:
	if not equipped_items.has(slot_name):
		return false
	
	var item_id = equipped_items[slot_name]
	if not inventory.has(item_id):
		equipped_items.erase(slot_name)
		return false
	
	var item: InventoryItem = inventory[item_id]["item"]
	_apply_equipment_bonuses(item, false)
	equipped_items.erase(slot_name)
	
	EventBus.item_unequipped.emit(item_id, slot_name)
	EventBus.show_notification("Unequipped " + item.item_name, "info")
	return true

## Get equipment slot name for an item
func _get_equipment_slot_name(item: InventoryItem) -> String:
	match item.equipment_slot:
		InventoryItem.EquipmentSlot.WEAPON:
			return "weapon"
		InventoryItem.EquipmentSlot.ARMOR:
			return "armor"
		InventoryItem.EquipmentSlot.ACCESSORY:
			return "accessory"
		_:
			return "accessory"

## Apply or remove equipment bonuses
func _apply_equipment_bonuses(item: InventoryItem, apply: bool) -> void:
	if not GameManager.player_stats:
		return
	
	var multiplier = 1 if apply else -1
	
	if item.attack_bonus != 0:
		GameManager.player_stats.strength += item.attack_bonus * multiplier
	
	if item.defense_bonus != 0:
		GameManager.player_stats.defense += item.defense_bonus * multiplier
	
	if item.health_bonus != 0:
		GameManager.player_stats.max_health += item.health_bonus * multiplier
		if apply:
			GameManager.player_stats.current_health += item.health_bonus
		else:
			GameManager.player_stats.current_health = min(
				GameManager.player_stats.current_health,
				GameManager.player_stats.max_health
			)
	
	if item.stamina_bonus != 0:
		GameManager.player_stats.max_stamina += item.stamina_bonus * multiplier
		if apply:
			GameManager.player_stats.current_stamina += item.stamina_bonus
		else:
			GameManager.player_stats.current_stamina = min(
				GameManager.player_stats.current_stamina,
				GameManager.player_stats.max_stamina
			)

## Get all items in inventory
func get_all_items() -> Array[Dictionary]:
	var items: Array[Dictionary] = []
	for item_id in inventory:
		items.append(inventory[item_id])
	return items

## Get equipped item in slot
func get_equipped_item(slot_name: String) -> InventoryItem:
	if not equipped_items.has(slot_name):
		return null
	
	var item_id = equipped_items[slot_name]
	if not inventory.has(item_id):
		return null
	
	return inventory[item_id]["item"]

## Handle item rewards from quests
func _on_item_rewarded(item_id: String) -> void:
	# Try to get item from GameInitializer's database
	var game_init = get_tree().root.get_node_or_null("GameInitializer")
	if game_init and game_init.has_method("get_item"):
		var item = game_init.get_item(item_id)
		if item and item is InventoryItem:
			add_item(item, 1)
			return
	
	# Fallback: Create a placeholder if database is not available
	push_warning("Item database not found, creating placeholder for: " + item_id)
	var item = InventoryItem.new()
	item.item_id = item_id
	item.item_name = "Quest Reward"
	item.description = "A reward from completing a quest"
	add_item(item, 1)

## Clear inventory
func clear_inventory() -> void:
	# Unequip all items first to remove bonuses
	for slot_name in equipped_items.keys():
		unequip_item(slot_name)
	
	inventory.clear()
	equipped_items.clear()
