extends Resource
class_name SeasonalEventResource

## Seasonal Event Resource
## Defines a seasonal or limited-time event

@export var event_id: String = ""
@export var event_name: String = ""
@export var description: String = ""
@export var event_type: String = "seasonal"  # seasonal, holiday, limited_time

# Duration
@export var start_date: String = ""  # ISO format: YYYY-MM-DD
@export var end_date: String = ""
@export var duration_days: int = 7

# Requirements
@export var required_level: int = 1
@export var repeatable: bool = false

# Rewards
@export var event_quests: Array[String] = []
@export var exclusive_items: Array[String] = []
@export var bonus_xp_multiplier: float = 1.5
@export var bonus_gold_multiplier: float = 1.5

# Status
@export var is_active: bool = false
@export var times_completed: int = 0

func _init(
	p_id: String = "",
	p_name: String = "",
	p_type: String = "seasonal"
):
	event_id = p_id
	event_name = p_name
	event_type = p_type

func check_if_active(current_date: String) -> bool:
	"""Check if event is currently active based on date"""
	# Simple date comparison (would need proper date parsing in production)
	if start_date.is_empty() or end_date.is_empty():
		return is_active
	
	return current_date >= start_date and current_date <= end_date
