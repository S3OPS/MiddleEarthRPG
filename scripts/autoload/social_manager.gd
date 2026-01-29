extends Node

## SocialManager
## Manages friends list and social features

signal friend_added(friend_id: String, friend_name: String)
signal friend_removed(friend_id: String)
signal friend_online(friend_id: String)
signal friend_offline(friend_id: String)

var friends: Dictionary = {}  # friend_id -> FriendResource
var friend_requests: Array[String] = []
var blocked_players: Array[String] = []

func _ready() -> void:
	print("👥 SocialManager initialized")

func send_friend_request(player_id: String, player_name: String) -> bool:
	"""Send a friend request to a player"""
	if player_id in friends:
		print("Already friends with this player")
		return false
	
	if player_id in friend_requests:
		print("Friend request already sent")
		return false
	
	if player_id in blocked_players:
		print("Player is blocked")
		return false
	
	friend_requests.append(player_id)
	print("Friend request sent to: ", player_name)
	return true

func accept_friend_request(player_id: String, player_name: String) -> bool:
	"""Accept a friend request"""
	if player_id not in friend_requests:
		print("No pending friend request from this player")
		return false
	
	var friend = FriendResource.new(player_id, player_name)
	friends[player_id] = friend
	friend_requests.erase(player_id)
	
	friend_added.emit(player_id, player_name)
	EventBus.achievement_unlocked.emit("first_friend", "Made your first friend")
	
	print("Friend added: ", player_name)
	return true

func remove_friend(player_id: String) -> bool:
	"""Remove a friend"""
	if player_id not in friends:
		return false
	
	var friend_name = friends[player_id].friend_name
	friends.erase(player_id)
	
	friend_removed.emit(player_id)
	print("Friend removed: ", friend_name)
	return true

func block_player(player_id: String) -> bool:
	"""Block a player"""
	if player_id in blocked_players:
		return false
	
	blocked_players.append(player_id)
	
	# Remove from friends if present
	if player_id in friends:
		remove_friend(player_id)
	
	# Remove pending requests
	friend_requests.erase(player_id)
	
	print("Player blocked: ", player_id)
	return true

func unblock_player(player_id: String) -> bool:
	"""Unblock a player"""
	if player_id not in blocked_players:
		return false
	
	blocked_players.erase(player_id)
	print("Player unblocked: ", player_id)
	return true

func update_friend_status(player_id: String, is_online: bool) -> void:
	"""Update a friend's online status"""
	if player_id not in friends:
		return
	
	var friend = friends[player_id]
	friend.is_online = is_online
	
	if is_online:
		friend.status = "online"
		friend_online.emit(player_id)
	else:
		friend.status = "offline"
		friend.last_seen = Time.get_datetime_string_from_system()
		friend_offline.emit(player_id)

func get_online_friends() -> Array[FriendResource]:
	"""Get all online friends"""
	var online_friends: Array[FriendResource] = []
	for friend_id in friends:
		var friend = friends[friend_id]
		if friend.is_online:
			online_friends.append(friend)
	return online_friends

func get_all_friends() -> Array[FriendResource]:
	"""Get all friends"""
	var all_friends: Array[FriendResource] = []
	for friend_id in friends:
		all_friends.append(friends[friend_id])
	return all_friends

func is_friend(player_id: String) -> bool:
	"""Check if a player is a friend"""
	return player_id in friends

func is_blocked(player_id: String) -> bool:
	"""Check if a player is blocked"""
	return player_id in blocked_players

func save_data() -> Dictionary:
	"""Save social data"""
	var friend_data = {}
	for friend_id in friends:
		var friend = friends[friend_id]
		friend_data[friend_id] = {
			"friend_name": friend.friend_name,
			"friend_level": friend.friend_level,
			"friendship_date": friend.friendship_date
		}
	
	return {
		"friends": friend_data,
		"blocked_players": blocked_players
	}

func load_data(data: Dictionary) -> void:
	"""Load social data"""
	blocked_players = data.get("blocked_players", [])
	
	var friend_data = data.get("friends", {})
	for friend_id in friend_data:
		var saved = friend_data[friend_id]
		var friend = FriendResource.new(friend_id, saved.get("friend_name", ""))
		friend.friend_level = saved.get("friend_level", 1)
		friend.friendship_date = saved.get("friendship_date", "")
		friends[friend_id] = friend
