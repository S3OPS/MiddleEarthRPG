extends Resource
class_name WaypointResource

## Waypoint Resource
## Defines a fast travel waypoint in the game world

@export var waypoint_id: String = ""
@export var waypoint_name: String = ""
@export var region_id: String = ""
@export var description: String = ""
@export var position: Vector3 = Vector3.ZERO
@export var discovered: bool = false
@export var unlocked: bool = false
@export var required_quest: String = ""

# Costs
@export var travel_cost: int = 0  # Gold cost to travel here
@export var requires_mount: bool = false

func _init(
	p_id: String = "",
	p_name: String = "",
	p_region: String = "",
	p_pos: Vector3 = Vector3.ZERO
):
	waypoint_id = p_id
	waypoint_name = p_name
	region_id = p_region
	position = p_pos
