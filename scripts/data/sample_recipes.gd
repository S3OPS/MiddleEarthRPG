extends Node

## Sample Recipes Data
## Defines sample crafting recipes for Phase 6

const RecipeRes = preload("res://scripts/resources/recipe_resource.gd")

static func create_sample_recipes() -> Array[RecipeResource]:
	"""Create sample crafting recipes"""
	var recipes: Array[RecipeResource] = []
	
	# ========================================
	# WEAPON RECIPES
	# ========================================
	
	# Iron Sword
	var iron_sword_recipe = RecipeRes.new(
		"recipe_iron_sword",
		"Iron Sword",
		"iron_sword",
		"weapon"
	)
	iron_sword_recipe.add_ingredient("iron_ore", 3)
	iron_sword_recipe.add_ingredient("wood", 1)
	iron_sword_recipe.required_level = 1
	iron_sword_recipe.crafting_skill_required = 5
	iron_sword_recipe.required_station = "forge"
	recipes.append(iron_sword_recipe)
	
	# Steel Sword
	var steel_sword_recipe = RecipeRes.new(
		"recipe_steel_sword",
		"Steel Sword",
		"steel_sword",
		"weapon"
	)
	steel_sword_recipe.add_ingredient("steel_ore", 3)
	steel_sword_recipe.add_ingredient("wood", 1)
	steel_sword_recipe.add_ingredient("leather", 1)
	steel_sword_recipe.required_level = 5
	steel_sword_recipe.crafting_skill_required = 15
	steel_sword_recipe.required_station = "forge"
	recipes.append(steel_sword_recipe)
	
	# Elven Blade
	var elven_blade_recipe = RecipeRes.new(
		"recipe_elven_blade",
		"Elven Blade",
		"elven_blade",
		"weapon"
	)
	elven_blade_recipe.add_ingredient("mithril_ore", 2)
	elven_blade_recipe.add_ingredient("ancient_wood", 1)
	elven_blade_recipe.add_ingredient("starlight_essence", 1)
	elven_blade_recipe.required_level = 10
	elven_blade_recipe.crafting_skill_required = 30
	elven_blade_recipe.required_station = "elven_forge"
	recipes.append(elven_blade_recipe)
	
	# ========================================
	# ARMOR RECIPES
	# ========================================
	
	# Leather Armor
	var leather_armor_recipe = RecipeRes.new(
		"recipe_leather_armor",
		"Leather Armor",
		"leather_armor",
		"armor"
	)
	leather_armor_recipe.add_ingredient("leather", 5)
	leather_armor_recipe.add_ingredient("thread", 3)
	leather_armor_recipe.required_level = 2
	leather_armor_recipe.crafting_skill_required = 8
	leather_armor_recipe.required_station = "workbench"
	recipes.append(leather_armor_recipe)
	
	# Chain Mail
	var chain_mail_recipe = RecipeRes.new(
		"recipe_chain_mail",
		"Chain Mail",
		"chain_mail",
		"armor"
	)
	chain_mail_recipe.add_ingredient("iron_ore", 6)
	chain_mail_recipe.add_ingredient("leather", 2)
	chain_mail_recipe.required_level = 5
	chain_mail_recipe.crafting_skill_required = 18
	chain_mail_recipe.required_station = "forge"
	recipes.append(chain_mail_recipe)
	
	# Mithril Armor
	var mithril_armor_recipe = RecipeRes.new(
		"recipe_mithril_armor",
		"Mithril Armor",
		"mithril_coat",
		"armor"
	)
	mithril_armor_recipe.add_ingredient("mithril_ore", 8)
	mithril_armor_recipe.add_ingredient("elven_thread", 4)
	mithril_armor_recipe.required_level = 12
	mithril_armor_recipe.crafting_skill_required = 40
	mithril_armor_recipe.required_station = "dwarven_forge"
	recipes.append(mithril_armor_recipe)
	
	# ========================================
	# CONSUMABLE RECIPES
	# ========================================
	
	# Health Potion
	var health_potion_recipe = RecipeRes.new(
		"recipe_health_potion",
		"Health Potion",
		"health_potion",
		"consumable"
	)
	health_potion_recipe.add_ingredient("healing_herb", 2)
	health_potion_recipe.add_ingredient("water", 1)
	health_potion_recipe.output_quantity = 3
	health_potion_recipe.required_level = 1
	health_potion_recipe.crafting_skill_required = 3
	health_potion_recipe.required_station = "alchemy_table"
	recipes.append(health_potion_recipe)
	
	# Stamina Potion
	var stamina_potion_recipe = RecipeRes.new(
		"recipe_stamina_potion",
		"Stamina Potion",
		"stamina_potion",
		"consumable"
	)
	stamina_potion_recipe.add_ingredient("energy_flower", 2)
	stamina_potion_recipe.add_ingredient("water", 1)
	stamina_potion_recipe.output_quantity = 3
	stamina_potion_recipe.required_level = 1
	stamina_potion_recipe.crafting_skill_required = 3
	stamina_potion_recipe.required_station = "alchemy_table"
	recipes.append(stamina_potion_recipe)
	
	# Greater Health Potion
	var greater_health_recipe = RecipeRes.new(
		"recipe_greater_health_potion",
		"Greater Health Potion",
		"greater_health_potion",
		"consumable"
	)
	greater_health_recipe.add_ingredient("rare_healing_herb", 3)
	greater_health_recipe.add_ingredient("crystal_water", 1)
	greater_health_recipe.add_ingredient("magic_essence", 1)
	greater_health_recipe.output_quantity = 2
	greater_health_recipe.required_level = 8
	greater_health_recipe.crafting_skill_required = 25
	greater_health_recipe.required_station = "advanced_alchemy"
	recipes.append(greater_health_recipe)
	
	# ========================================
	# MATERIAL RECIPES
	# ========================================
	
	# Steel Ingot (from ore)
	var steel_ingot_recipe = RecipeRes.new(
		"recipe_steel_ingot",
		"Steel Ingot",
		"steel_ore",
		"material"
	)
	steel_ingot_recipe.add_ingredient("iron_ore", 2)
	steel_ingot_recipe.add_ingredient("coal", 1)
	steel_ingot_recipe.output_quantity = 1
	steel_ingot_recipe.required_level = 3
	steel_ingot_recipe.crafting_skill_required = 10
	steel_ingot_recipe.required_station = "forge"
	recipes.append(steel_ingot_recipe)
	
	# Leather (from hide)
	var leather_recipe = RecipeRes.new(
		"recipe_leather",
		"Leather",
		"leather",
		"material"
	)
	leather_recipe.add_ingredient("animal_hide", 3)
	leather_recipe.output_quantity = 2
	leather_recipe.required_level = 1
	leather_recipe.crafting_skill_required = 2
	leather_recipe.required_station = "tanning_rack"
	recipes.append(leather_recipe)
	
	return recipes
