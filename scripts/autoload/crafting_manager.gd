extends Node

## CraftingManager
## Manages crafting recipes and crafting operations

signal recipe_discovered(recipe_id: String)
signal crafting_started(recipe_id: String)
signal crafting_completed(recipe_id: String, item_id: String, quantity: int)
signal crafting_failed(recipe_id: String, reason: String)

var recipes: Dictionary = {}  # recipe_id -> RecipeResource
var discovered_recipes: Array[String] = []
var crafting_skill: int = 0

func _ready() -> void:
	pass

func register_recipe(recipe: RecipeResource) -> void:
	"""Register a crafting recipe"""
	if recipe.recipe_id.is_empty():
		push_error("Cannot register recipe with empty ID")
		return
	
	recipes[recipe.recipe_id] = recipe
	print("Recipe registered: ", recipe.recipe_name)

func discover_recipe(recipe_id: String) -> void:
	"""Discover a new recipe"""
	if recipe_id not in recipes:
		push_error("Recipe not found: " + recipe_id)
		return
	
	if recipe_id in discovered_recipes:
		return  # Already discovered
	
	discovered_recipes.append(recipe_id)
	recipe_discovered.emit(recipe_id)
	EventBus.achievement_unlocked.emit("recipe_" + recipe_id, "Discovered: " + recipes[recipe_id].recipe_name)
	print("Recipe discovered: ", recipes[recipe_id].recipe_name)

func can_craft(recipe_id: String) -> Dictionary:
	"""Check if player can craft a recipe"""
	var result = {
		"can_craft": false,
		"reason": ""
	}
	
	if recipe_id not in recipes:
		result["reason"] = "Recipe not found"
		return result
	
	if recipe_id not in discovered_recipes:
		result["reason"] = "Recipe not discovered"
		return result
	
	var recipe = recipes[recipe_id]
	
	# Check level requirement
	if GameManager.player_level < recipe.required_level:
		result["reason"] = "Level " + str(recipe.required_level) + " required"
		return result
	
	# Check crafting skill requirement
	if crafting_skill < recipe.crafting_skill_required:
		result["reason"] = "Crafting skill " + str(recipe.crafting_skill_required) + " required"
		return result
	
	# Check ingredients
	for item_id in recipe.ingredients:
		var required = recipe.ingredients[item_id]
		if not InventoryManager.has_item(item_id, required):
			result["reason"] = "Missing ingredient: " + item_id
			return result
	
	# TODO: Check crafting station requirement when implemented
	
	result["can_craft"] = true
	return result

func craft_item(recipe_id: String) -> bool:
	"""Craft an item from a recipe"""
	var check = can_craft(recipe_id)
	if not check["can_craft"]:
		crafting_failed.emit(recipe_id, check["reason"])
		print("Cannot craft: ", check["reason"])
		return false
	
	var recipe = recipes[recipe_id]
	
	# Remove ingredients
	for item_id in recipe.ingredients:
		var required = recipe.ingredients[item_id]
		InventoryManager.remove_item(item_id, required)
	
	crafting_started.emit(recipe_id)
	
	# TODO: Add crafting time delay when animation system exists
	# For now, craft instantly
	
	# Add output item
	var output_item = get_node("/root/Main/GameInitializer").get_item(recipe.output_item_id)
	if output_item:
		InventoryManager.add_item(output_item, recipe.output_quantity)
	
	crafting_completed.emit(recipe_id, recipe.output_item_id, recipe.output_quantity)
	
	# Increase crafting skill
	increase_crafting_skill(1)
	
	print("Crafted: ", recipe.recipe_name)
	return true

func increase_crafting_skill(amount: int) -> void:
	"""Increase crafting skill level"""
	crafting_skill += amount
	print("Crafting skill increased to ", crafting_skill)

func get_recipe(recipe_id: String) -> RecipeResource:
	"""Get a recipe by ID"""
	return recipes.get(recipe_id, null)

func get_discovered_recipes() -> Array[String]:
	"""Get all discovered recipes"""
	return discovered_recipes

func get_recipes_by_category(category: String) -> Array[RecipeResource]:
	"""Get all recipes in a category"""
	var result: Array[RecipeResource] = []
	for recipe_id in discovered_recipes:
		var recipe = recipes[recipe_id]
		if recipe.category == category:
			result.append(recipe)
	return result

func save_data() -> Dictionary:
	"""Save crafting data"""
	return {
		"discovered_recipes": discovered_recipes,
		"crafting_skill": crafting_skill
	}

func load_data(data: Dictionary) -> void:
	"""Load crafting data"""
	discovered_recipes = data.get("discovered_recipes", [])
	crafting_skill = data.get("crafting_skill", 0)
