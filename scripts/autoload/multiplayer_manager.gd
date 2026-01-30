extends Node

## MultiplayerManager
## Manages multiplayer networking and co-op gameplay

signal player_joined(player_id: String, player_name: String)
signal player_left(player_id: String)
signal party_formed(party_id: String)
signal party_disbanded(party_id: String)

# Multiplayer state
var is_host: bool = false
var is_connected: bool = false
var max_players: int = 4
var current_players: Dictionary = {}  # player_id -> player_data

# Party system
var current_party: String = ""
var party_members: Array[String] = []
var party_leader: String = ""

func _ready() -> void:
	print("🌐 MultiplayerManager initialized")

func create_session(session_name: String, max_players_count: int = 4) -> bool:
	"""Create a new multiplayer session"""
	# Validate max_players_count
	if max_players_count <= 0:
		push_error("MultiplayerManager: max_players_count must be positive, got %d" % max_players_count)
		return false
	
	if max_players_count > 32:
		push_warning("MultiplayerManager: max_players_count %d exceeds recommended limit, clamping to 32" % max_players_count)
		max_players_count = 32
	
	is_host = true
	is_connected = true
	max_players = max_players_count
	
	# Add host as first player
	var player_id = "host"
	current_players[player_id] = {
		"name": "Host Player",
		"level": GameManager.player_level,
		"health": 100,
		"is_host": true
	}
	
	print("Created multiplayer session: ", session_name)
	return true

func join_session(session_id: String) -> bool:
	"""Join an existing multiplayer session"""
	if current_players.size() >= max_players:
		print("Session is full")
		return false
	
	is_host = false
	is_connected = true
	
	var player_id = "player_" + str(Time.get_ticks_msec())
	current_players[player_id] = {
		"name": "Player",
		"level": GameManager.player_level,
		"health": 100,
		"is_host": false
	}
	
	player_joined.emit(player_id, "Player")
	print("Joined multiplayer session: ", session_id)
	return true

func leave_session() -> void:
	"""Leave the current multiplayer session"""
	is_connected = false
	
	if is_host:
		# Disband session if host leaves
		for player_id in current_players:
			if player_id != "host":
				player_left.emit(player_id)
		current_players.clear()
		print("Host left - session disbanded")
	else:
		print("Left multiplayer session")
	
	is_host = false

func form_party(player_ids: Array[String]) -> String:
	"""Form a party with specified players"""
	if current_party != "":
		print("Already in a party")
		return ""
	
	var party_id = "party_" + str(Time.get_ticks_msec())
	current_party = party_id
	party_members = player_ids.duplicate()
	
	# Check array bounds before accessing first element
	if party_members.size() > 0:
		party_leader = party_members[0]
	else:
		push_warning("MultiplayerManager: Forming party with no members")
		party_leader = ""
	
	party_formed.emit(party_id)
	print("Party formed: ", party_id)
	return party_id

func disband_party() -> void:
	"""Disband the current party"""
	if current_party == "":
		return
	
	var old_party = current_party
	current_party = ""
	party_members.clear()
	party_leader = ""
	
	party_disbanded.emit(old_party)
	print("Party disbanded")

func get_difficulty_multiplier() -> float:
	"""Get difficulty multiplier based on player count"""
	var player_count = current_players.size()
	if player_count <= 1:
		return 1.0
	
	# Scale difficulty with player count
	return 1.0 + (player_count - 1) * 0.5

func get_loot_multiplier() -> float:
	"""Get loot multiplier for party play"""
	if party_members.size() > 1:
		return 1.2  # 20% bonus loot for party play
	return 1.0

func is_in_party() -> bool:
	"""Check if player is in a party"""
	return current_party != ""

func save_data() -> Dictionary:
	"""Save multiplayer data"""
	return {
		"current_party": current_party,
		"party_members": party_members
	}

func load_data(data: Dictionary) -> void:
	"""Load multiplayer data"""
	current_party = data.get("current_party", "")
	party_members = data.get("party_members", [])
