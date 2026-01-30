extends Node

## CraftingManager
## Manages crafting recipes and crafting operations
## Provides a system for players to create items from ingredients

signal recipe_discovered(recipe_id: String)
signal crafting_started(recipe_id: String)
signal crafting_completed(recipe_id: String, item_id: String, quantity: int)
signal crafting_failed(recipe_id: String, reason: String)

# Dictionary of all available recipes: recipe_id -> RecipeResource
var recipes: Dictionary = {}
var discovered_recipes: Array[String] = []
var crafting_skill: int = 0
var game_initializer: Node = null


func _ready() -> void:
	# Cache the GameInitializer reference to avoid fragile node path lookups
	game_initializer = get_node_or_null("/root/Main/GameInitializer")
	if game_initializer == null:
		push_error("CraftingManager: Failed to find GameInitializer node at /root/Main/GameInitializer")
	
	if OS.is_debug_build():
		print("🔨 CraftingManager initialized")


## Register a crafting recipe
## @param recipe: The recipe resource to register
func register_recipe(recipe: RecipeResource) -> void:
	if not recipe or recipe.recipe_id.is_empty():
		push_error("CraftingManager: Cannot register recipe with empty or null ID")
		return
	
	recipes[recipe.recipe_id] = recipe
	if OS.is_debug_build():
		print("CraftingManager: Recipe registered - %s" % recipe.recipe_name)

## Discover a new recipe
## @param recipe_id: The ID of the recipe to discover
func discover_recipe(recipe_id: String) -> void:
	if recipe_id not in recipes:
		push_error("CraftingManager: Recipe not found - %s" % recipe_id)
		return
	
	if recipe_id in discovered_recipes:
		return  # Already discovered
	
	discovered_recipes.append(recipe_id)
	recipe_discovered.emit(recipe_id)
	EventBus.achievement_unlocked.emit("recipe_%s" % recipe_id, "Discovered: %s" % recipes[recipe_id].recipe_name)
	if OS.is_debug_build():
		print("CraftingManager: Recipe discovered - %s" % recipes[recipe_id].recipe_name)

## Check if player can craft a recipe
## @param recipe_id: The ID of the recipe to check
## @return: Dictionary with "can_craft" (bool) and "reason" (String) keys
func can_craft(recipe_id: String) -> Dictionary:
	var result := {
		"can_craft": false,
		"reason": ""
	}
	
	if recipe_id not in recipes:
		result["reason"] = "Recipe not found"
		return result
	
	if recipe_id not in discovered_recipes:
		result["reason"] = "Recipe not discovered"
		return result
	
	var recipe: RecipeResource = recipes[recipe_id]
	
	# Null safety check - ensure recipe exists
	if recipe == null:
		result["reason"] = "Recipe data corrupted or missing"
		return result
	
	# Check level requirement
	if GameManager.player_stats:
		if GameManager.player_stats.level < recipe.required_level:
			result["reason"] = "Level %d required" % recipe.required_level
			return result
	else:
		# Cannot craft without player stats initialized
		result["reason"] = "Player not initialized"
		return result
	
	# Check crafting skill requirement
	if crafting_skill < recipe.crafting_skill_required:
		result["reason"] = "Crafting skill %d required" % recipe.crafting_skill_required
		return result
	
	# Check ingredients
	for item_id in recipe.ingredients:
		var required: int = recipe.ingredients[item_id]
		if not InventoryManager.has_item(item_id, required):
			result["reason"] = "Missing ingredient: %s" % item_id
			return result
	
	result["can_craft"] = true
	return result

## Craft an item from a recipe
## @param recipe_id: The ID of the recipe to craft
## @return: true if crafting was successful, false otherwise
func craft_item(recipe_id: String) -> bool:
	var check: Dictionary = can_craft(recipe_id)
	if not check["can_craft"]:
		crafting_failed.emit(recipe_id, check["reason"])
		if OS.is_debug_build():
			print("CraftingManager: Cannot craft - %s" % check["reason"])
		return false
	
	var recipe: RecipeResource = recipes[recipe_id]
	
	# Validate output item creation BEFORE removing ingredients
	if game_initializer == null:
		push_error("CraftingManager: GameInitializer not available for crafting completion")
		crafting_failed.emit(recipe_id, "Game system error")
		return false
	
	if not game_initializer.has_method("get_item"):
		push_error("CraftingManager: GameInitializer missing get_item method")
		crafting_failed.emit(recipe_id, "Game system error")
		return false
	
	var output_item = game_initializer.get_item(recipe.output_item_id)
	if output_item == null:
		push_error("CraftingManager: Failed to create output item - %s" % recipe.output_item_id)
		crafting_failed.emit(recipe_id, "Failed to create output item")
		return false
	
	# Only remove ingredients after validating output can be created
	for item_id in recipe.ingredients:
		var required: int = recipe.ingredients[item_id]
		if not InventoryManager.remove_item(item_id, required):
			push_error("CraftingManager: Failed to remove ingredient - %s" % item_id)
			crafting_failed.emit(recipe_id, "Failed to remove ingredients")
			return false
	
	crafting_started.emit(recipe_id)
	
	# Instant crafting for now - could be enhanced with crafting time/animation
	
	# Add output item to inventory
	if not InventoryManager.add_item(output_item, recipe.output_quantity):
		push_error("CraftingManager: Failed to add output item to inventory")
		crafting_failed.emit(recipe_id, "Inventory full")
		# TODO: Should restore ingredients here but that's complex
		return false
	
	crafting_completed.emit(recipe_id, recipe.output_item_id, recipe.output_quantity)
	
	# Increase crafting skill
	increase_crafting_skill(1)
	
	if OS.is_debug_build():
		print("CraftingManager: Crafted - %s" % recipe.recipe_name)
	return true

## Increase crafting skill level
## @param amount: The amount to increase the skill by
func increase_crafting_skill(amount: int) -> void:
	crafting_skill += amount
	if OS.is_debug_build():
		print("CraftingManager: Crafting skill increased to %d" % crafting_skill)


## Get a recipe by ID
## @param recipe_id: The ID of the recipe to retrieve
## @return: The RecipeResource or null if not found
func get_recipe(recipe_id: String) -> RecipeResource:
	return recipes.get(recipe_id, null)


## Get all discovered recipes
## @return: Array of recipe IDs
func get_discovered_recipes() -> Array[String]:
	return discovered_recipes


## Get all recipes in a specific category
## @param category: The category to filter by
## @return: Array of RecipeResource objects
func get_recipes_by_category(category: String) -> Array[RecipeResource]:
	var result: Array[RecipeResource] = []
	for recipe_id in discovered_recipes:
		var recipe: RecipeResource = recipes[recipe_id]
		if recipe.category == category:
			result.append(recipe)
	return result


## Save crafting data for persistence
## @return: Dictionary containing all crafting data
func save_data() -> Dictionary:
	return {
		"discovered_recipes": discovered_recipes,
		"crafting_skill": crafting_skill
	}


## Load crafting data from save
## @param data: Dictionary containing crafting data
func load_data(data: Dictionary) -> void:
	discovered_recipes = data.get("discovered_recipes", [])
	crafting_skill = data.get("crafting_skill", 0)
