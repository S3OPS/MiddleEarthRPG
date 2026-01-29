extends Resource
class_name RegionResource

## Region Resource
## Defines a region in the game world with its properties

@export var region_id: String = ""
@export var region_name: String = ""
@export var description: String = ""
@export var required_level: int = 1
@export var climate: String = "temperate"  # temperate, cold, hot, mystical
@export var danger_level: int = 1  # 1-5
@export var has_fast_travel: bool = false
@export var discovered: bool = false

# Visual properties
@export var skybox_color: Color = Color.SKY_BLUE
@export var ambient_light: Color = Color.WHITE
@export var fog_enabled: bool = false
@export var fog_color: Color = Color.WHITE

# Faction reputation requirements
@export var required_faction: String = ""
@export var required_reputation: int = 0

func _init(
	p_id: String = "",
	p_name: String = "",
	p_desc: String = "",
	p_level: int = 1
):
	region_id = p_id
	region_name = p_name
	description = p_desc
	required_level = p_level
