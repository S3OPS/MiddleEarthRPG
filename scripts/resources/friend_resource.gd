extends Resource
class_name FriendResource

## Friend Resource
## Defines a friend relationship between players

@export var friend_id: String = ""
@export var friend_name: String = ""
@export var friend_level: int = 1
@export var friend_class: String = ""
@export var is_online: bool = false
@export var last_seen: String = ""
@export var friendship_date: String = ""

# Status
@export var status: String = "offline"  # online, offline, busy, away
@export var current_location: String = ""

func _init(
	p_id: String = "",
	p_name: String = ""
):
	friend_id = p_id
	friend_name = p_name
	friendship_date = Time.get_date_string_from_system()
	last_seen = Time.get_datetime_string_from_system()
