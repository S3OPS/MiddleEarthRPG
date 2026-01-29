extends Resource
class_name CompanionResource

## Companion Resource
## Defines a companion/hireling character

@export var companion_id: String = ""
@export var companion_name: String = ""
@export var description: String = ""
@export var companion_class: String = "warrior"  # warrior, rogue, mage, ranger

# Stats
@export var level: int = 1
@export var max_health: int = 100
@export var attack: int = 10
@export var defense: int = 5

# Hiring
@export var hire_cost: int = 100
@export var daily_cost: int = 10
@export var required_level: int = 1
@export var required_quest: String = ""

# Status
@export var is_hired: bool = false
@export var loyalty: int = 50  # 0-100

# Special abilities
@export var abilities: Array[String] = []

func _init(
	p_id: String = "",
	p_name: String = "",
	p_class: String = "warrior"
):
	companion_id = p_id
	companion_name = p_name
	companion_class = p_class
