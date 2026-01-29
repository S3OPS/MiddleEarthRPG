extends Resource
class_name RecipeResource

## Recipe Resource
## Defines a crafting recipe with ingredients and output

@export var recipe_id: String = ""
@export var recipe_name: String = ""
@export var description: String = ""
@export var category: String = "general"  # weapon, armor, consumable, material, misc
@export var crafting_skill_required: int = 0
@export var crafting_time: float = 1.0  # seconds

# Ingredients: Dictionary of item_id -> quantity needed
@export var ingredients: Dictionary = {}

# Output
@export var output_item_id: String = ""
@export var output_quantity: int = 1

# Requirements
@export var required_station: String = ""  # forge, workbench, alchemy_table, etc.
@export var required_level: int = 1

func _init(
	p_id: String = "",
	p_name: String = "",
	p_output: String = "",
	p_category: String = "general"
):
	recipe_id = p_id
	recipe_name = p_name
	output_item_id = p_output
	category = p_category

func add_ingredient(item_id: String, quantity: int) -> void:
	ingredients[item_id] = quantity

func has_required_ingredients(inventory_check: Callable) -> bool:
	"""Check if player has all required ingredients"""
	for item_id in ingredients:
		var required = ingredients[item_id]
		if not inventory_check.call(item_id, required):
			return false
	return true
