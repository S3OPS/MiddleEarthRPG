extends Resource
class_name GuildResource

## Guild Resource
## Defines a player guild/fellowship

@export var guild_id: String = ""
@export var guild_name: String = ""
@export var guild_tag: String = ""  # 3-5 character tag
@export var description: String = ""
@export var created_date: String = ""

# Leadership
@export var leader_id: String = ""
@export var officers: Array[String] = []

# Members
@export var members: Array[String] = []
@export var max_members: int = 50
@export var member_count: int = 0

# Guild Stats
@export var guild_level: int = 1
@export var guild_experience: int = 0
@export var guild_gold: int = 0

# Guild Hall
@export var has_guild_hall: bool = false
@export var guild_hall_level: int = 0

# Perks and Bonuses
@export var active_perks: Array[String] = []
@export var xp_bonus: float = 0.0
@export var gold_bonus: float = 0.0

func _init(
	p_id: String = "",
	p_name: String = "",
	p_tag: String = "",
	p_leader: String = ""
):
	guild_id = p_id
	guild_name = p_name
	guild_tag = p_tag
	leader_id = p_leader
	created_date = Time.get_date_string_from_system()

func add_member(player_id: String) -> bool:
	"""Add a member to the guild"""
	if member_count >= max_members:
		return false
	
	if player_id in members:
		return false
	
	members.append(player_id)
	member_count = members.size()
	return true

func remove_member(player_id: String) -> bool:
	"""Remove a member from the guild"""
	if player_id not in members:
		return false
	
	members.erase(player_id)
	officers.erase(player_id)  # Remove from officers if present
	member_count = members.size()
	return true

func is_officer(player_id: String) -> bool:
	"""Check if player is an officer"""
	return player_id in officers

func is_leader(player_id: String) -> bool:
	"""Check if player is the leader"""
	return player_id == leader_id

func add_experience(amount: int) -> void:
	"""Add experience to guild and check for level up"""
	guild_experience += amount
	
	# Level up check (1000 XP per level)
	var xp_needed = guild_level * 1000
	while guild_experience >= xp_needed:
		guild_experience -= xp_needed
		guild_level += 1
		xp_needed = guild_level * 1000
