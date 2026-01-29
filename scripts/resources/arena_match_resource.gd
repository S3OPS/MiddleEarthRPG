extends Resource
class_name ArenaMatchResource

## Arena Match Resource
## Defines a PvP arena match

@export var match_id: String = ""
@export var match_type: String = "1v1"  # 1v1, 2v2, 3v3
@export var status: String = "waiting"  # waiting, active, completed
@export var map_name: String = ""

# Teams
@export var team1: Array[String] = []
@export var team2: Array[String] = []

# Results
@export var winner_team: int = 0  # 0 = none, 1 = team1, 2 = team2
@export var match_duration: float = 0.0

# Ratings
@export var rating_change: Dictionary = {}  # player_id -> rating_delta

func _init(
	p_type: String = "1v1",
	p_map: String = ""
):
	match_id = "match_" + str(Time.get_ticks_msec())
	match_type = p_type
	map_name = p_map
	status = "waiting"

func add_player_to_team(player_id: String, team: int) -> bool:
	"""Add a player to a team"""
	var max_per_team = 1
	if match_type == "2v2":
		max_per_team = 2
	elif match_type == "3v3":
		max_per_team = 3
	
	if team == 1:
		if team1.size() >= max_per_team:
			return false
		team1.append(player_id)
	elif team == 2:
		if team2.size() >= max_per_team:
			return false
		team2.append(player_id)
	else:
		return false
	
	return true

func is_ready() -> bool:
	"""Check if match is ready to start"""
	var required_per_team = 1
	if match_type == "2v2":
		required_per_team = 2
	elif match_type == "3v3":
		required_per_team = 3
	
	return team1.size() == required_per_team and team2.size() == required_per_team
