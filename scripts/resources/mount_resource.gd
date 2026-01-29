extends Resource
class_name MountResource

## Mount Resource
## Defines a rideable mount

@export var mount_id: String = ""
@export var mount_name: String = ""
@export var description: String = ""
@export var mount_type: String = "horse"  # horse, warg, eagle

# Stats
@export var speed_bonus: float = 1.5
@export var stamina_reduction: float = 0.5

# Acquisition
@export var is_owned: bool = false
@export var purchase_cost: int = 1000
@export var required_level: int = 10
@export var required_quest: String = ""

func _init(
	p_id: String = "",
	p_name: String = "",
	p_type: String = "horse"
):
	mount_id = p_id
	mount_name = p_name
	mount_type = p_type
