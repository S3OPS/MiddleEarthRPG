extends Node

## Game initializer that loads sample data for Phase 3 systems
## This script should be added to the main scene

var sample_quests_script = preload("res://scripts/data/sample_quests.gd")
var sample_items_script = preload("res://scripts/data/sample_items.gd")
var sample_dialogues_script = preload("res://scripts/data/sample_dialogues.gd")
var sample_regions_script = preload("res://scripts/data/sample_regions.gd")
var sample_waypoints_script = preload("res://scripts/data/sample_waypoints.gd")
var sample_factions_script = preload("res://scripts/data/sample_factions.gd")
var sample_regional_quests_script = preload("res://scripts/data/sample_regional_quests.gd")
var sample_recipes_script = preload("res://scripts/data/sample_recipes.gd")
var sample_specializations_script = preload("res://scripts/data/sample_specializations.gd")
var sample_companions_script = preload("res://scripts/data/sample_companions.gd")
var sample_seasonal_events_script = preload("res://scripts/data/sample_seasonal_events.gd")

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
	
	# Register Phase 5 regions
	var regions = sample_regions_script.create_sample_regions()
	for region in regions:
		RegionManager.register_region(region)
	print("✅ Registered %d regions" % regions.size())
	
	# Register Phase 5 waypoints
	var waypoints = sample_waypoints_script.create_sample_waypoints()
	for waypoint in waypoints:
		FastTravelManager.register_waypoint(waypoint)
	print("✅ Registered %d waypoints" % waypoints.size())
	
	# Register Phase 5 factions
	var factions = sample_factions_script.create_sample_factions()
	for faction in factions:
		FactionManager.register_faction(faction)
	print("✅ Registered %d factions" % factions.size())
	
	# Register Phase 5 regional quests
	var regional_quests = sample_regional_quests_script.create_regional_quests()
	for quest in regional_quests:
		QuestManager.register_quest(quest)
	print("✅ Registered %d regional quests" % regional_quests.size())
	
	# Register Phase 6 recipes
	var recipes = sample_recipes_script.create_sample_recipes()
	for recipe in recipes:
		CraftingManager.register_recipe(recipe)
	print("✅ Registered %d crafting recipes" % recipes.size())
	
	# Register Phase 6 specializations
	var specializations = sample_specializations_script.create_sample_specializations()
	for spec in specializations:
		SpecializationManager.register_specialization(spec)
	print("✅ Registered %d specializations" % specializations.size())
	
	# Register Phase 6 companions
	var companions = sample_companions_script.create_sample_companions()
	for companion in companions:
		CompanionManager.register_companion(companion)
	print("✅ Registered %d companions" % companions.size())
	
	# Register Phase 7 seasonal events
	var events = sample_seasonal_events_script.create_sample_events()
	for event in events:
		SeasonalEventManager.register_event(event)
	print("✅ Registered %d seasonal events" % events.size())
	
	# Give player some starting items for testing
	_give_starting_items()
	
	# Start the first quest automatically
	if QuestManager != null:
		# Check if quest exists before starting it
		if QuestManager.has_method("is_quest_registered"):
			if QuestManager.is_quest_registered("first_steps"):
				QuestManager.start_quest("first_steps")
			else:
				push_warning("Quest 'first_steps' not registered")
		else:
			# If no validation method available, try to start anyway
			QuestManager.start_quest("first_steps")
	
	# Set starting region (The Shire)
	if RegionManager != null:
		# Check if region exists before entering it
		if RegionManager.has_method("is_region_registered"):
			if RegionManager.is_region_registered("the_shire"):
				RegionManager.enter_region("the_shire")
			else:
				push_warning("Region 'the_shire' not registered")
		else:
			# If no validation method available, try to enter anyway
			RegionManager.enter_region("the_shire")
	
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
