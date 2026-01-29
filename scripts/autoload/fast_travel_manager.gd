extends Node

## FastTravelManager
## Manages fast travel waypoints and travel between locations

signal waypoint_discovered(waypoint_id: String)
signal waypoint_unlocked(waypoint_id: String)
signal travel_started(from_waypoint: String, to_waypoint: String)
signal travel_completed(waypoint_id: String)

var waypoints: Dictionary = {}  # waypoint_id -> WaypointResource
var discovered_waypoints: Array[String] = []
var unlocked_waypoints: Array[String] = []
var current_waypoint: String = ""

func _ready() -> void:
	pass

func register_waypoint(waypoint: WaypointResource) -> void:
	"""Register a new waypoint"""
	if waypoint.waypoint_id.is_empty():
		push_error("Cannot register waypoint with empty ID")
		return
	
	waypoints[waypoint.waypoint_id] = waypoint
	print("Waypoint registered: ", waypoint.waypoint_name)

func discover_waypoint(waypoint_id: String) -> void:
	"""Mark a waypoint as discovered"""
	if waypoint_id not in waypoints:
		push_error("Waypoint not found: " + waypoint_id)
		return
	
	if waypoint_id in discovered_waypoints:
		return  # Already discovered
	
	discovered_waypoints.append(waypoint_id)
	waypoints[waypoint_id].discovered = true
	waypoint_discovered.emit(waypoint_id)
	EventBus.achievement_unlocked.emit("discover_waypoint_" + waypoint_id, "Discovered " + waypoints[waypoint_id].waypoint_name)
	print("Waypoint discovered: ", waypoints[waypoint_id].waypoint_name)
	
	# Auto-unlock if no requirements
	if waypoints[waypoint_id].required_quest.is_empty():
		unlock_waypoint(waypoint_id)

func unlock_waypoint(waypoint_id: String) -> void:
	"""Unlock a waypoint for fast travel"""
	if waypoint_id not in waypoints:
		push_error("Waypoint not found: " + waypoint_id)
		return
	
	if waypoint_id in unlocked_waypoints:
		return  # Already unlocked
	
	var waypoint = waypoints[waypoint_id]
	
	# Check quest requirement
	if not waypoint.required_quest.is_empty():
		if not QuestManager.is_quest_completed(waypoint.required_quest):
			print("Waypoint requires quest: ", waypoint.required_quest)
			return
	
	unlocked_waypoints.append(waypoint_id)
	waypoint.unlocked = true
	waypoint_unlocked.emit(waypoint_id)
	print("Waypoint unlocked: ", waypoint.waypoint_name)

func can_travel_to(waypoint_id: String) -> Dictionary:
	"""Check if player can travel to a waypoint"""
	var result = {
		"can_travel": false,
		"reason": ""
	}
	
	if waypoint_id not in waypoints:
		result["reason"] = "Waypoint not found"
		return result
	
	if waypoint_id not in unlocked_waypoints:
		result["reason"] = "Waypoint not unlocked"
		return result
	
	var waypoint = waypoints[waypoint_id]
	
	# Check gold cost
	if waypoint.travel_cost > 0:
		if GameManager.player_gold < waypoint.travel_cost:
			result["reason"] = "Not enough gold (need " + str(waypoint.travel_cost) + ")"
			return result
	
	# Check mount requirement
	if waypoint.requires_mount:
		# TODO: Check if player has mount when mount system is implemented
		pass
	
	result["can_travel"] = true
	return result

func travel_to(waypoint_id: String) -> bool:
	"""Travel to a waypoint"""
	var check = can_travel_to(waypoint_id)
	if not check["can_travel"]:
		print("Cannot travel: ", check["reason"])
		return false
	
	var waypoint = waypoints[waypoint_id]
	
	# Deduct travel cost
	if waypoint.travel_cost > 0:
		GameManager.add_gold(-waypoint.travel_cost)
	
	travel_started.emit(current_waypoint, waypoint_id)
	current_waypoint = waypoint_id
	
	# TODO: Trigger travel animation/screen transition
	# For now, just complete immediately
	travel_completed.emit(waypoint_id)
	
	print("Traveled to: ", waypoint.waypoint_name)
	return true

func get_waypoint(waypoint_id: String) -> WaypointResource:
	"""Get a waypoint by ID"""
	return waypoints.get(waypoint_id, null)

func get_unlocked_waypoints() -> Array[String]:
	"""Get all unlocked waypoints"""
	return unlocked_waypoints

func get_waypoints_in_region(region_id: String) -> Array[WaypointResource]:
	"""Get all waypoints in a specific region"""
	var result: Array[WaypointResource] = []
	for waypoint_id in waypoints:
		var waypoint = waypoints[waypoint_id]
		if waypoint.region_id == region_id:
			result.append(waypoint)
	return result

func save_data() -> Dictionary:
	"""Save fast travel data"""
	return {
		"discovered_waypoints": discovered_waypoints,
		"unlocked_waypoints": unlocked_waypoints,
		"current_waypoint": current_waypoint
	}

func load_data(data: Dictionary) -> void:
	"""Load fast travel data"""
	discovered_waypoints = data.get("discovered_waypoints", [])
	unlocked_waypoints = data.get("unlocked_waypoints", [])
	current_waypoint = data.get("current_waypoint", "")
	
	# Update waypoint status
	for waypoint_id in discovered_waypoints:
		if waypoint_id in waypoints:
			waypoints[waypoint_id].discovered = true
	
	for waypoint_id in unlocked_waypoints:
		if waypoint_id in waypoints:
			waypoints[waypoint_id].unlocked = true
