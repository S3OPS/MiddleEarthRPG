extends Node

## Sample items for testing the inventory system

static func create_sample_items() -> Dictionary:
	var items: Dictionary = {}
	
	# Consumables
	var health_potion = InventoryItem.new()
	health_potion.item_id = "health_potion"
	health_potion.item_name = "Health Potion"
	health_potion.description = "A magical potion that restores health."
	health_potion.type = InventoryItem.ItemType.CONSUMABLE
	health_potion.rarity = InventoryItem.Rarity.COMMON
	health_potion.stackable = true
	health_potion.max_stack_size = 20
	health_potion.value = 25
	health_potion.health_restore = 50
	items[health_potion.item_id] = health_potion
	
	var stamina_potion = InventoryItem.new()
	stamina_potion.item_id = "stamina_potion"
	stamina_potion.item_name = "Stamina Potion"
	stamina_potion.description = "A refreshing potion that restores stamina."
	stamina_potion.type = InventoryItem.ItemType.CONSUMABLE
	stamina_potion.rarity = InventoryItem.Rarity.COMMON
	stamina_potion.stackable = true
	stamina_potion.max_stack_size = 20
	stamina_potion.value = 20
	stamina_potion.stamina_restore = 50
	items[stamina_potion.item_id] = stamina_potion
	
	var super_potion = InventoryItem.new()
	super_potion.item_id = "super_potion"
	super_potion.item_name = "Super Potion"
	super_potion.description = "A powerful potion that fully restores health and stamina."
	super_potion.type = InventoryItem.ItemType.CONSUMABLE
	super_potion.rarity = InventoryItem.Rarity.RARE
	super_potion.stackable = true
	super_potion.max_stack_size = 10
	super_potion.value = 100
	super_potion.health_restore = 100
	super_potion.stamina_restore = 100
	items[super_potion.item_id] = super_potion
	
	# Weapons
	var iron_sword = InventoryItem.new()
	iron_sword.item_id = "iron_sword"
	iron_sword.item_name = "Iron Sword"
	iron_sword.description = "A basic iron sword. Reliable and sturdy."
	iron_sword.type = InventoryItem.ItemType.EQUIPMENT
	iron_sword.rarity = InventoryItem.Rarity.COMMON
	iron_sword.stackable = false
	iron_sword.value = 50
	iron_sword.attack_bonus = 5
	items[iron_sword.item_id] = iron_sword
	
	var steel_sword = InventoryItem.new()
	steel_sword.item_id = "steel_sword"
	steel_sword.item_name = "Steel Sword"
	steel_sword.description = "A well-crafted steel sword with a sharp edge."
	steel_sword.type = InventoryItem.ItemType.EQUIPMENT
	steel_sword.rarity = InventoryItem.Rarity.UNCOMMON
	steel_sword.stackable = false
	steel_sword.value = 150
	steel_sword.attack_bonus = 10
	items[steel_sword.item_id] = steel_sword
	
	var elven_blade = InventoryItem.new()
	elven_blade.item_id = "elven_blade"
	elven_blade.item_name = "Elven Blade"
	elven_blade.description = "A masterfully crafted blade by the elves of Rivendell."
	elven_blade.type = InventoryItem.ItemType.EQUIPMENT
	elven_blade.rarity = InventoryItem.Rarity.RARE
	elven_blade.stackable = false
	elven_blade.value = 500
	elven_blade.attack_bonus = 20
	elven_blade.stamina_bonus = 10
	items[elven_blade.item_id] = elven_blade
	
	var anduril = InventoryItem.new()
	anduril.item_id = "anduril"
	anduril.item_name = "Andúril, Flame of the West"
	anduril.description = "The legendary sword reforged from the shards of Narsil."
	anduril.type = InventoryItem.ItemType.EQUIPMENT
	anduril.rarity = InventoryItem.Rarity.LEGENDARY
	anduril.stackable = false
	anduril.value = 2000
	anduril.attack_bonus = 35
	anduril.health_bonus = 20
	anduril.stamina_bonus = 15
	items[anduril.item_id] = anduril
	
	# Armor
	var leather_armor = InventoryItem.new()
	leather_armor.item_id = "leather_armor"
	leather_armor.item_name = "Leather Armor"
	leather_armor.description = "Basic leather protection."
	leather_armor.type = InventoryItem.ItemType.EQUIPMENT
	leather_armor.rarity = InventoryItem.Rarity.COMMON
	leather_armor.stackable = false
	leather_armor.value = 40
	leather_armor.defense_bonus = 5
	items[leather_armor.item_id] = leather_armor
	
	var chainmail = InventoryItem.new()
	chainmail.item_id = "chainmail"
	chainmail.item_name = "Chainmail Armor"
	chainmail.description = "Interlocking metal rings provide solid defense."
	chainmail.type = InventoryItem.ItemType.EQUIPMENT
	chainmail.rarity = InventoryItem.Rarity.UNCOMMON
	chainmail.stackable = false
	chainmail.value = 120
	chainmail.defense_bonus = 10
	items[chainmail.item_id] = chainmail
	
	var mithril_coat = InventoryItem.new()
	mithril_coat.item_id = "mithril_coat"
	mithril_coat.item_name = "Mithril Coat"
	mithril_coat.description = "A legendary armor made of the precious mithril. Light yet impenetrable."
	mithril_coat.type = InventoryItem.ItemType.EQUIPMENT
	mithril_coat.rarity = InventoryItem.Rarity.LEGENDARY
	mithril_coat.stackable = false
	mithril_coat.value = 3000
	mithril_coat.defense_bonus = 40
	mithril_coat.health_bonus = 50
	items[mithril_coat.item_id] = mithril_coat
	
	# Accessories
	var ring_of_power = InventoryItem.new()
	ring_of_power.item_id = "ring_of_power"
	ring_of_power.item_name = "Ring of Power"
	ring_of_power.description = "A mysterious ring that grants great power to its wearer."
	ring_of_power.type = InventoryItem.ItemType.EQUIPMENT
	ring_of_power.rarity = InventoryItem.Rarity.EPIC
	ring_of_power.stackable = false
	ring_of_power.value = 1000
	ring_of_power.attack_bonus = 15
	ring_of_power.defense_bonus = 15
	ring_of_power.health_bonus = 30
	ring_of_power.stamina_bonus = 30
	items[ring_of_power.item_id] = ring_of_power
	
	# Quest items
	var treasure_chest = InventoryItem.new()
	treasure_chest.item_id = "treasure_chest"
	treasure_chest.item_name = "Treasure Chest"
	treasure_chest.description = "A chest full of treasures."
	treasure_chest.type = InventoryItem.ItemType.QUEST_ITEM
	treasure_chest.rarity = InventoryItem.Rarity.UNCOMMON
	treasure_chest.stackable = false
	treasure_chest.value = 200
	items[treasure_chest.item_id] = treasure_chest
	
	var wizard_staff = InventoryItem.new()
	wizard_staff.item_id = "wizard_staff"
	wizard_staff.item_name = "Wizard's Staff"
	wizard_staff.description = "A staff imbued with magical power, gifted by Gandalf."
	wizard_staff.type = InventoryItem.ItemType.EQUIPMENT
	wizard_staff.rarity = InventoryItem.Rarity.EPIC
	wizard_staff.stackable = false
	wizard_staff.value = 800
	wizard_staff.attack_bonus = 25
	wizard_staff.stamina_bonus = 40
	items[wizard_staff.item_id] = wizard_staff
	
	return items
