extends Resource
class_name PetResource

## Pet Resource
## Defines a collectible companion pet

@export var pet_id: String = ""
@export var pet_name: String = ""
@export var description: String = ""
@export var pet_type: String = "common"  # common, uncommon, rare, epic, legendary

# Appearance
@export var model_path: String = ""
@export var scale: float = 1.0

# Acquisition
@export var is_collected: bool = false
@export var acquisition_method: String = ""  # drop, quest, purchase, achievement

func _init(
	p_id: String = "",
	p_name: String = "",
	p_type: String = "common"
):
	pet_id = p_id
	pet_name = p_name
	pet_type = p_type
