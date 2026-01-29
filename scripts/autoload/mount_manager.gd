extends Node

## MountManager
## Manages player mounts

signal mount_acquired(mount_id: String, mount_name: String)
signal mount_equipped(mount_id: String)
signal mount_unequipped()

var mounts: Dictionary = {}  # mount_id -> MountResource
var owned_mounts: Array[String] = []
var active_mount: String = ""

func _ready() -> void:
	print("🐴 MountManager initialized")

func register_mount(mount: MountResource) -> void:
	"""Register a mount"""
	if mount.mount_id.is_empty():
		push_error("Cannot register mount with empty ID")
		return
	
	mounts[mount.mount_id] = mount
	print("Mount registered: ", mount.mount_name)

func can_purchase_mount(mount_id: String) -> Dictionary:
	"""Check if player can purchase a mount"""
	var result = {
		"can_purchase": false,
		"reason": ""
	}
	
	if mount_id not in mounts:
		result["reason"] = "Mount not found"
		return result
	
	var mount = mounts[mount_id]
	
	if mount.is_owned or mount_id in owned_mounts:
		result["reason"] = "Already owned"
		return result
	
	if GameManager.player_level < mount.required_level:
		result["reason"] = "Level " + str(mount.required_level) + " required"
		return result
	
	if not mount.required_quest.is_empty():
		if not QuestManager.is_quest_completed(mount.required_quest):
			result["reason"] = "Quest required: " + mount.required_quest
			return result
	
	if GameManager.player_gold < mount.purchase_cost:
		result["reason"] = "Not enough gold (need " + str(mount.purchase_cost) + ")"
		return result
	
	result["can_purchase"] = true
	return result

func purchase_mount(mount_id: String) -> bool:
	"""Purchase a mount"""
	var check = can_purchase_mount(mount_id)
	if not check["can_purchase"]:
		print("Cannot purchase mount: ", check["reason"])
		return false
	
	var mount = mounts[mount_id]
	
	# Deduct cost
	GameManager.add_gold(-mount.purchase_cost)
	
	# Add to owned
	mount.is_owned = true
	owned_mounts.append(mount_id)
	
	mount_acquired.emit(mount_id, mount.mount_name)
	EventBus.achievement_unlocked.emit("first_mount", "Acquired your first mount")
	
	print("Mount purchased: ", mount.mount_name)
	return true

func equip_mount(mount_id: String) -> bool:
	"""Equip a mount"""
	if mount_id not in owned_mounts:
		print("Mount not owned")
		return false
	
	active_mount = mount_id
	mount_equipped.emit(mount_id)
	
	print("Mount equipped: ", mounts[mount_id].mount_name)
	return true

func unequip_mount() -> void:
	"""Unequip current mount"""
	if active_mount.is_empty():
		return
	
	active_mount = ""
	mount_unequipped.emit()
	print("Mount unequipped")

func get_mount(mount_id: String) -> MountResource:
	"""Get a mount by ID"""
	return mounts.get(mount_id, null)

func get_active_mount() -> MountResource:
	"""Get currently equipped mount"""
	if active_mount.is_empty():
		return null
	return mounts.get(active_mount, null)

func get_speed_bonus() -> float:
	"""Get speed bonus from active mount"""
	var mount = get_active_mount()
	if mount == null:
		return 1.0
	return mount.speed_bonus

func save_data() -> Dictionary:
	"""Save mount data"""
	return {
		"owned_mounts": owned_mounts,
		"active_mount": active_mount
	}

func load_data(data: Dictionary) -> void:
	"""Load mount data"""
	owned_mounts = data.get("owned_mounts", [])
	active_mount = data.get("active_mount", "")
	
	# Update owned status
	for mount_id in owned_mounts:
		if mount_id in mounts:
			mounts[mount_id].is_owned = true
