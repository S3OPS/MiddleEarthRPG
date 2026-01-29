extends Resource
class_name SpecializationResource

## Specialization Resource
## Defines a combat specialization/skill tree

@export var specialization_id: String = ""
@export var specialization_name: String = ""
@export var description: String = ""
@export var icon_path: String = ""

# Passive bonuses
@export var attack_bonus: int = 0
@export var defense_bonus: int = 0
@export var health_bonus: int = 0
@export var stamina_bonus: int = 0
@export var critical_chance_bonus: float = 0.0

# Active abilities (ability_id -> unlock level)
@export var abilities: Dictionary = {}

func _init(
	p_id: String = "",
	p_name: String = "",
	p_desc: String = ""
):
	specialization_id = p_id
	specialization_name = p_name
	description = p_desc

func add_ability(ability_id: String, unlock_level: int) -> void:
	abilities[ability_id] = unlock_level
