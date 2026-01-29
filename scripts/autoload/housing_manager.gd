extends Node

## HousingManager
## Manages player housing and bases

signal house_purchased(house_id: String, house_name: String)
signal house_upgraded(house_id: String, new_level: int)
signal item_stored(house_id: String, item_id: String, quantity: int)
signal item_retrieved(house_id: String, item_id: String, quantity: int)

var houses: Dictionary = {}  # house_id -> HousingResource
var owned_houses: Array[String] = []
var primary_house: String = ""

func _ready() -> void:
	print("🏠 HousingManager initialized")

func register_house(house: HousingResource) -> void:
	"""Register a house"""
	if house.house_id.is_empty():
		push_error("Cannot register house with empty ID")
		return
	
	houses[house.house_id] = house
	print("House registered: ", house.house_name)

func can_purchase_house(house_id: String) -> Dictionary:
	"""Check if player can purchase a house"""
	var result = {
		"can_purchase": false,
		"reason": ""
	}
	
	if house_id not in houses:
		result["reason"] = "House not found"
		return result
	
	var house = houses[house_id]
	
	if house.is_owned or house_id in owned_houses:
		result["reason"] = "Already owned"
		return result
	
	if GameManager.player_gold < house.purchase_cost:
		result["reason"] = "Not enough gold (need " + str(house.purchase_cost) + ")"
		return result
	
	result["can_purchase"] = true
	return result

func purchase_house(house_id: String) -> bool:
	"""Purchase a house"""
	var check = can_purchase_house(house_id)
	if not check["can_purchase"]:
		print("Cannot purchase house: ", check["reason"])
		return false
	
	var house = houses[house_id]
	
	# Deduct cost
	GameManager.add_gold(-house.purchase_cost)
	
	# Add to owned
	house.is_owned = true
	owned_houses.append(house_id)
	
	# Set as primary if first house
	if primary_house.is_empty():
		primary_house = house_id
	
	house_purchased.emit(house_id, house.house_name)
	EventBus.achievement_unlocked.emit("home_owner", "Purchased your first house")
	
	print("House purchased: ", house.house_name)
	return true

func upgrade_house(house_id: String) -> bool:
	"""Upgrade a house"""
	if house_id not in owned_houses:
		print("House not owned")
		return false
	
	var house = houses[house_id]
	
	if not house.can_upgrade():
		print("House at maximum level")
		return false
	
	var cost = house.get_upgrade_cost()
	if GameManager.player_gold < cost:
		print("Not enough gold to upgrade (need ", cost, ")")
		return false
	
	# Deduct cost
	GameManager.add_gold(-cost)
	
	# Upgrade
	house.upgrade_level += 1
	house.storage_capacity += 25  # +25 storage per level
	house.max_decorations += 5   # +5 decoration slots per level
	
	house_upgraded.emit(house_id, house.upgrade_level)
	
	print("House upgraded to level ", house.upgrade_level)
	return true

func store_item(house_id: String, item_id: String, quantity: int) -> bool:
	"""Store an item in house storage"""
	if house_id not in owned_houses:
		return false
	
	var house = houses[house_id]
	
	# Check if player has the item
	if not InventoryManager.has_item(item_id, quantity):
		print("Not enough items to store")
		return false
	
	# Check storage capacity
	var current_storage = 0
	for stored_id in house.stored_items:
		current_storage += house.stored_items[stored_id]
	
	if current_storage + quantity > house.storage_capacity:
		print("Storage is full")
		return false
	
	# Remove from inventory
	InventoryManager.remove_item(item_id, quantity)
	
	# Add to storage
	if item_id in house.stored_items:
		house.stored_items[item_id] += quantity
	else:
		house.stored_items[item_id] = quantity
	
	item_stored.emit(house_id, item_id, quantity)
	print("Stored ", quantity, "x ", item_id)
	return true

func retrieve_item(house_id: String, item_id: String, quantity: int) -> bool:
	"""Retrieve an item from house storage"""
	if house_id not in owned_houses:
		return false
	
	var house = houses[house_id]
	
	# Check if item is in storage
	if item_id not in house.stored_items:
		print("Item not in storage")
		return false
	
	if house.stored_items[item_id] < quantity:
		print("Not enough items in storage")
		return false
	
	# Remove from storage
	house.stored_items[item_id] -= quantity
	if house.stored_items[item_id] == 0:
		house.stored_items.erase(item_id)
	
	# Add to inventory
	var item = get_node("/root/Main/GameInitializer").get_item(item_id)
	if item:
		InventoryManager.add_item(item, quantity)
	
	item_retrieved.emit(house_id, item_id, quantity)
	print("Retrieved ", quantity, "x ", item_id)
	return true

func get_house(house_id: String) -> HousingResource:
	"""Get a house by ID"""
	return houses.get(house_id, null)

func get_primary_house() -> HousingResource:
	"""Get primary house"""
	if primary_house.is_empty():
		return null
	return houses.get(primary_house, null)

func save_data() -> Dictionary:
	"""Save housing data"""
	var house_data = {}
	for house_id in owned_houses:
		var house = houses[house_id]
		house_data[house_id] = {
			"upgrade_level": house.upgrade_level,
			"stored_items": house.stored_items,
			"decorations": house.decorations
		}
	
	return {
		"owned_houses": owned_houses,
		"primary_house": primary_house,
		"houses": house_data
	}

func load_data(data: Dictionary) -> void:
	"""Load housing data"""
	owned_houses = data.get("owned_houses", [])
	primary_house = data.get("primary_house", "")
	
	var house_data = data.get("houses", {})
	for house_id in house_data:
		if house_id in houses:
			var saved = house_data[house_id]
			houses[house_id].is_owned = true
			houses[house_id].upgrade_level = saved.get("upgrade_level", 0)
			houses[house_id].stored_items = saved.get("stored_items", {})
			houses[house_id].decorations = saved.get("decorations", [])
