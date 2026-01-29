extends Node

## GuildManager
## Manages player guilds and fellowships

signal guild_created(guild_id: String, guild_name: String)
signal guild_disbanded(guild_id: String)
signal member_joined(guild_id: String, player_id: String)
signal member_left(guild_id: String, player_id: String)
signal guild_level_up(guild_id: String, new_level: int)

var guilds: Dictionary = {}  # guild_id -> GuildResource
var player_guild: String = ""  # Current player's guild ID

func _ready() -> void:
	print("🏰 GuildManager initialized")

func create_guild(guild_name: String, guild_tag: String, leader_id: String) -> String:
	"""Create a new guild"""
	# Validate guild name and tag
	if guild_name.length() < 3 or guild_name.length() > 30:
		print("Guild name must be 3-30 characters")
		return ""
	
	if guild_tag.length() < 2 or guild_tag.length() > 5:
		print("Guild tag must be 2-5 characters")
		return ""
	
	# Check if already in a guild
	if player_guild != "":
		print("Already in a guild")
		return ""
	
	# Create guild
	var guild_id = "guild_" + str(Time.get_ticks_msec())
	var guild = GuildResource.new(guild_id, guild_name, guild_tag, leader_id)
	guild.add_member(leader_id)
	
	guilds[guild_id] = guild
	player_guild = guild_id
	
	guild_created.emit(guild_id, guild_name)
	EventBus.achievement_unlocked.emit("guild_founder", "Founded a Guild")
	
	print("Guild created: ", guild_name, " [", guild_tag, "]")
	return guild_id

func disband_guild(guild_id: String, player_id: String) -> bool:
	"""Disband a guild (leader only)"""
	if guild_id not in guilds:
		return false
	
	var guild = guilds[guild_id]
	
	# Check if player is leader
	if not guild.is_leader(player_id):
		print("Only guild leader can disband guild")
		return false
	
	# Remove guild
	guilds.erase(guild_id)
	if player_guild == guild_id:
		player_guild = ""
	
	guild_disbanded.emit(guild_id)
	print("Guild disbanded: ", guild.guild_name)
	return true

func join_guild(guild_id: String, player_id: String) -> bool:
	"""Join a guild"""
	if guild_id not in guilds:
		print("Guild not found")
		return false
	
	if player_guild != "":
		print("Already in a guild")
		return false
	
	var guild = guilds[guild_id]
	
	if not guild.add_member(player_id):
		print("Could not join guild (full or already member)")
		return false
	
	player_guild = guild_id
	member_joined.emit(guild_id, player_id)
	
	print("Joined guild: ", guild.guild_name)
	return true

func leave_guild(guild_id: String, player_id: String) -> bool:
	"""Leave a guild"""
	if guild_id not in guilds:
		return false
	
	var guild = guilds[guild_id]
	
	# Leader cannot leave (must transfer or disband)
	if guild.is_leader(player_id):
		print("Leader must transfer leadership or disband guild")
		return false
	
	if not guild.remove_member(player_id):
		return false
	
	if player_guild == guild_id:
		player_guild = ""
	
	member_left.emit(guild_id, player_id)
	print("Left guild: ", guild.guild_name)
	return true

func add_guild_experience(guild_id: String, amount: int) -> void:
	"""Add experience to a guild"""
	if guild_id not in guilds:
		return
	
	var guild = guilds[guild_id]
	var old_level = guild.guild_level
	
	guild.add_experience(amount)
	
	if guild.guild_level > old_level:
		guild_level_up.emit(guild_id, guild.guild_level)
		EventBus.achievement_unlocked.emit(
			"guild_level_" + str(guild.guild_level),
			"Guild reached level " + str(guild.guild_level)
		)
		print("Guild leveled up to ", guild.guild_level)

func promote_to_officer(guild_id: String, player_id: String, promoter_id: String) -> bool:
	"""Promote a member to officer"""
	if guild_id not in guilds:
		return false
	
	var guild = guilds[guild_id]
	
	# Only leader can promote
	if not guild.is_leader(promoter_id):
		print("Only guild leader can promote members")
		return false
	
	if player_id not in guild.members:
		print("Player not in guild")
		return false
	
	if player_id not in guild.officers:
		guild.officers.append(player_id)
		print("Promoted to officer")
		return true
	
	return false

func get_guild(guild_id: String) -> GuildResource:
	"""Get a guild by ID"""
	return guilds.get(guild_id, null)

func get_player_guild() -> GuildResource:
	"""Get current player's guild"""
	if player_guild.is_empty():
		return null
	return guilds.get(player_guild, null)

func is_in_guild() -> bool:
	"""Check if player is in a guild"""
	return not player_guild.is_empty()

func save_data() -> Dictionary:
	"""Save guild data"""
	var guild_data = {}
	for guild_id in guilds:
		var guild = guilds[guild_id]
		guild_data[guild_id] = {
			"guild_name": guild.guild_name,
			"guild_tag": guild.guild_tag,
			"leader_id": guild.leader_id,
			"members": guild.members,
			"officers": guild.officers,
			"guild_level": guild.guild_level,
			"guild_experience": guild.guild_experience
		}
	
	return {
		"guilds": guild_data,
		"player_guild": player_guild
	}

func load_data(data: Dictionary) -> void:
	"""Load guild data"""
	player_guild = data.get("player_guild", "")
	
	var guild_data = data.get("guilds", {})
	for guild_id in guild_data:
		var saved = guild_data[guild_id]
		var guild = GuildResource.new(
			guild_id,
			saved.get("guild_name", ""),
			saved.get("guild_tag", ""),
			saved.get("leader_id", "")
		)
		guild.members = saved.get("members", [])
		guild.officers = saved.get("officers", [])
		guild.guild_level = saved.get("guild_level", 1)
		guild.guild_experience = saved.get("guild_experience", 0)
		guilds[guild_id] = guild
