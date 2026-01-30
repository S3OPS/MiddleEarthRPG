extends Node

## ArenaManager
## Manages PvP arena matches and rankings

signal match_started(match_id: String)
signal match_ended(match_id: String, winner_team: int)
signal ranking_updated(player_id: String, new_rating: int)

var active_matches: Dictionary = {}  # match_id -> ArenaMatchResource
var queue: Array[String] = []  # Players in queue
var player_ratings: Dictionary = {}  # player_id -> rating
var leaderboard: Array = []

const STARTING_RATING: int = 1000
const RATING_K_FACTOR: int = 32

func _ready() -> void:
	print("⚔️ ArenaManager initialized")

func join_queue(player_id: String, match_type: String = "1v1") -> bool:
	"""Join arena matchmaking queue"""
	if player_id in queue:
		print("Already in queue")
		return false
	
	queue.append(player_id)
	
	# Initialize rating if needed
	if player_id not in player_ratings:
		player_ratings[player_id] = STARTING_RATING
	
	print("Joined ", match_type, " queue")
	
	# Try to create a match
	_try_create_match(match_type)
	
	return true

func leave_queue(player_id: String) -> bool:
	"""Leave arena matchmaking queue"""
	if player_id not in queue:
		return false
	
	queue.erase(player_id)
	print("Left queue")
	return true

func _try_create_match(match_type: String) -> void:
	"""Try to create a match from queue"""
	var required_players = 2
	if match_type == "2v2":
		required_players = 4
	elif match_type == "3v3":
		required_players = 6
	
	if queue.size() < required_players:
		return
	
	# Create match with queued players
	var arena_match = ArenaMatchResource.new(match_type, "arena_1")
	
	var players_per_team = required_players / 2
	for i in range(players_per_team):
		if queue.size() > 0:
			arena_match.add_player_to_team(queue.pop_front(), 1)
	
	for i in range(players_per_team):
		if queue.size() > 0:
			arena_match.add_player_to_team(queue.pop_front(), 2)
	
	if arena_match.is_ready():
		active_matches[arena_match.match_id] = arena_match
		start_match(arena_match.match_id)

func start_match(match_id: String) -> bool:
	"""Start an arena match"""
	if match_id not in active_matches:
		return false
	
	var arena_match = active_matches[match_id]
	arena_match.status = "active"
	
	match_started.emit(match_id)
	print("Arena match started: ", match_id)
	return true

func end_match(match_id: String, winner_team: int) -> bool:
	"""End an arena match"""
	if match_id not in active_matches:
		return false
	
	var arena_match = active_matches[match_id]
	arena_match.status = "completed"
	arena_match.winner_team = winner_team
	
	# Update ratings
	_update_ratings(arena_match)
	
	match_ended.emit(match_id, winner_team)
	
	# Award achievements
	for player_id in (arena_match.team1 if winner_team == 1 else arena_match.team2):
		EventBus.achievement_unlocked.emit(
			"arena_victory",
			"Victory in the Arena"
		)
	
	# Clean up
	active_matches.erase(match_id)
	
	print("Arena match ended, winner: team ", winner_team)
	return true

func _update_ratings(arena_match: ArenaMatchResource) -> void:
	"""Update player ratings based on match result"""
	var team1_avg = _get_team_average_rating(arena_match.team1)
	var team2_avg = _get_team_average_rating(arena_match.team2)
	
	# Calculate expected scores
	var expected1 = 1.0 / (1.0 + pow(10.0, (team2_avg - team1_avg) / 400.0))
	var expected2 = 1.0 - expected1
	
	# Actual scores
	var score1 = 1.0 if arena_match.winner_team == 1 else 0.0
	var score2 = 1.0 - score1
	
	# Update ratings for each player
	for player_id in arena_match.team1:
		var old_rating = player_ratings.get(player_id, STARTING_RATING)
		var new_rating = int(old_rating + RATING_K_FACTOR * (score1 - expected1))
		player_ratings[player_id] = new_rating
		arena_match.rating_change[player_id] = new_rating - old_rating
		ranking_updated.emit(player_id, new_rating)
	
	for player_id in arena_match.team2:
		var old_rating = player_ratings.get(player_id, STARTING_RATING)
		var new_rating = int(old_rating + RATING_K_FACTOR * (score2 - expected2))
		player_ratings[player_id] = new_rating
		arena_match.rating_change[player_id] = new_rating - old_rating
		ranking_updated.emit(player_id, new_rating)
	
	_update_leaderboard()

func _get_team_average_rating(team: Array[String]) -> float:
	"""Get average rating of a team"""
	var total = 0.0
	for player_id in team:
		total += player_ratings.get(player_id, STARTING_RATING)
	return total / team.size() if team.size() > 0 else STARTING_RATING

func _update_leaderboard() -> void:
	"""Update the leaderboard"""
	leaderboard.clear()
	for player_id in player_ratings:
		leaderboard.append({
			"player_id": player_id,
			"rating": player_ratings[player_id]
		})
	
	leaderboard.sort_custom(func(a, b): return a["rating"] > b["rating"])

func get_player_rating(player_id: String) -> int:
	"""Get player's arena rating"""
	return player_ratings.get(player_id, STARTING_RATING)

func get_leaderboard(limit: int = 100) -> Array:
	"""Get top players on leaderboard"""
	return leaderboard.slice(0, min(limit, leaderboard.size()))

func save_data() -> Dictionary:
	"""Save arena data"""
	return {
		"player_ratings": player_ratings
	}

func load_data(data: Dictionary) -> void:
	"""Load arena data"""
	player_ratings = data.get("player_ratings", {})
	_update_leaderboard()
