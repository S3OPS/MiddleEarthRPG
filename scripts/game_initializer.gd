extends Node

## Game initializer that loads sample data for Phase 3 systems
## This script should be added to the main scene

var sample_quests_script = preload("res://scripts/data/sample_quests.gd")
var sample_items_script = preload("res://scripts/data/sample_items.gd")
var sample_dialogues_script = preload("res://scripts/data/sample_dialogues.gd")

var item_database: Dictionary = {}
var dialogue_database: Dictionary = {}

func _ready() -> void:
	# Wait for autoload managers to be ready
	await get_tree().process_frame
	
	print("🎮 Initializing game data...")
	
	# Load sample items into database
	item_database = sample_items_script.create_sample_items()
	print("✅ Loaded %d items into database" % item_database.size())
	
	# Load sample dialogues into database
	dialogue_database = sample_dialogues_script.create_sample_dialogues()
	print("✅ Loaded %d dialogues into database" % dialogue_database.size())
	
	# Register sample quests with QuestManager
	var quests = sample_quests_script.create_sample_quests()
	for quest in quests:
		QuestManager.register_quest(quest)
	print("✅ Registered %d quests" % quests.size())
	
	# Give player some starting items for testing
	_give_starting_items()
	
	# Start the first quest automatically
	QuestManager.start_quest("first_steps")
	
	print("✅ Game initialization complete!")

func _give_starting_items() -> void:
	# Give player some starter items
	if item_database.has("health_potion"):
		InventoryManager.add_item(item_database["health_potion"], 3)
	
	if item_database.has("stamina_potion"):
		InventoryManager.add_item(item_database["stamina_potion"], 2)
	
	if item_database.has("iron_sword"):
		InventoryManager.add_item(item_database["iron_sword"], 1)

## Get an item from the database by ID
func get_item(item_id: String) -> InventoryItem:
	return item_database.get(item_id, null)

## Get a dialogue from the database by ID
func get_dialogue(dialogue_id: String) -> DialogueResource:
	return dialogue_database.get(dialogue_id, null)
