extends Node

## FastTravelManager
## Manages fast travel waypoints and travel between locations
## Handles waypoint discovery, unlocking, and teleportation mechanics

signal waypoint_discovered(waypoint_id: String)
signal waypoint_unlocked(waypoint_id: String)
signal travel_started(from_waypoint: String, to_waypoint: String)
signal travel_completed(waypoint_id: String)

# Dictionary of all waypoints: waypoint_id -> WaypointResource
var waypoints: Dictionary = {}
var discovered_waypoints: Array[String] = []
var unlocked_waypoints: Array[String] = []
var current_waypoint: String = ""


func _ready() -> void:
	if OS.is_debug_build():
		print("🗺️ FastTravelManager initialized")


## Register a new waypoint location
## @param waypoint: The waypoint resource to register
func register_waypoint(waypoint: WaypointResource) -> void:
	if not waypoint or waypoint.waypoint_id.is_empty():
		push_error("FastTravelManager: Cannot register waypoint with empty or null ID")
		return
	
	waypoints[waypoint.waypoint_id] = waypoint
	if OS.is_debug_build():
		print("FastTravelManager: Waypoint registered - %s" % waypoint.waypoint_name)

## Mark a waypoint as discovered (visible but may not be unlocked)
## @param waypoint_id: The ID of the waypoint to discover
func discover_waypoint(waypoint_id: String) -> void:
	if waypoint_id not in waypoints:
		push_error("FastTravelManager: Waypoint not found - %s" % waypoint_id)
		return
	
	if waypoint_id in discovered_waypoints:
		return  # Already discovered
	
	discovered_waypoints.append(waypoint_id)
	waypoints[waypoint_id].discovered = true
	waypoint_discovered.emit(waypoint_id, waypoints[waypoint_id].waypoint_name)
	EventBus.waypoint_discovered.emit(waypoint_id, waypoints[waypoint_id].waypoint_name)
	EventBus.achievement_unlocked.emit(
		"discover_waypoint_%s" % waypoint_id,
		"Discovered %s" % waypoints[waypoint_id].waypoint_name
	)
	if OS.is_debug_build():
		print("FastTravelManager: Waypoint discovered - %s" % waypoints[waypoint_id].waypoint_name)
	
	# Auto-unlock if no quest requirements
	if waypoints[waypoint_id].required_quest.is_empty():
		unlock_waypoint(waypoint_id)

## Unlock a waypoint for fast travel
## @param waypoint_id: The ID of the waypoint to unlock
func unlock_waypoint(waypoint_id: String) -> void:
	if waypoint_id not in waypoints:
		push_error("FastTravelManager: Waypoint not found - %s" % waypoint_id)
		return
	
	if waypoint_id in unlocked_waypoints:
		return  # Already unlocked
	
	var waypoint: WaypointResource = waypoints[waypoint_id]
	
	# Check quest requirement
	if not waypoint.required_quest.is_empty():
		if not QuestManager.is_quest_completed(waypoint.required_quest):
			if OS.is_debug_build():
				print("FastTravelManager: Waypoint requires quest - %s" % waypoint.required_quest)
			return
	
	unlocked_waypoints.append(waypoint_id)
	waypoint.unlocked = true
	waypoint_unlocked.emit(waypoint_id)
	if OS.is_debug_build():
		print("FastTravelManager: Waypoint unlocked - %s" % waypoint.waypoint_name)


## Check if player can travel to a specific waypoint
## @param waypoint_id: The ID of the waypoint to check
## @return: Dictionary with "can_travel" (bool) and "reason" (String) keys
func can_travel_to(waypoint_id: String) -> Dictionary:
	var result := {
		"can_travel": false,
		"reason": ""
	}
	
	if waypoint_id not in waypoints:
		result["reason"] = "Waypoint not found"
		return result
	
	if waypoint_id not in unlocked_waypoints:
		result["reason"] = "Waypoint not unlocked"
		return result
	
	var waypoint: WaypointResource = waypoints[waypoint_id]
	
	# Null safety check
	if not waypoint:
		result["reason"] = "Waypoint data is invalid"
		return result
	
	# Check gold cost
	if waypoint.travel_cost > 0:
		if GameManager.gold < waypoint.travel_cost:
			result["reason"] = "Not enough gold (need %d)" % waypoint.travel_cost
			return result
	
	# Check mount requirement (for future mount system)
	if waypoint.requires_mount:
		# Check if player has an active mount
		if not MountManager or MountManager.get_active_mount() == null:
			result["reason"] = "Requires a mount"
			return result
	
	result["can_travel"] = true
	return result

## Travel to a waypoint location
## @param waypoint_id: The ID of the waypoint to travel to
## @return: true if travel was successful, false otherwise
func travel_to(waypoint_id: String) -> bool:
	var check: Dictionary = can_travel_to(waypoint_id)
	if not check["can_travel"]:
		if OS.is_debug_build():
			print("FastTravelManager: Cannot travel - %s" % check["reason"])
		return false
	
	var waypoint: WaypointResource = waypoints.get(waypoint_id, null)
	
	# Add null check even after can_travel_to validation
	if waypoint == null:
		push_error("FastTravelManager: Waypoint became null - %s" % waypoint_id)
		return false
	
	# Deduct travel cost
	if waypoint.travel_cost > 0:
		# Validate gold was successfully removed
		if not GameManager.remove_gold(waypoint.travel_cost):
			if OS.is_debug_build():
				print("FastTravelManager: Failed to deduct travel cost")
			return false
	
	travel_started.emit(current_waypoint, waypoint_id)
	current_waypoint = waypoint_id
	
	# Future: Add travel animation/screen transition
	# For now, complete travel immediately
	travel_completed.emit(waypoint_id)
	
	if OS.is_debug_build():
		print("FastTravelManager: Traveled to - %s" % waypoint.waypoint_name)
	return true


## Get a waypoint by ID
## @param waypoint_id: The ID of the waypoint to retrieve
## @return: The WaypointResource or null if not found
func get_waypoint(waypoint_id: String) -> WaypointResource:
	return waypoints.get(waypoint_id, null)


## Get all unlocked waypoints
## @return: Array of waypoint IDs
func get_unlocked_waypoints() -> Array[String]:
	return unlocked_waypoints


## Set unlocked waypoints (used by save system)
## @param waypoint_ids: Array of waypoint IDs to set as unlocked
func set_unlocked_waypoints(waypoint_ids: Array) -> void:
	unlocked_waypoints.clear()
	for id in waypoint_ids:
		if id is String:
			unlocked_waypoints.append(id)
			# Update waypoint status
			if id in waypoints:
				waypoints[id].unlocked = true


## Get all waypoints in a specific region
## @param region_id: The ID of the region to filter by
## @return: Array of WaypointResource objects
func get_waypoints_in_region(region_id: String) -> Array[WaypointResource]:
	var result: Array[WaypointResource] = []
	for waypoint_id in waypoints:
		var waypoint: WaypointResource = waypoints[waypoint_id]
		if waypoint.region_id == region_id:
			result.append(waypoint)
	return result


## Save fast travel data for persistence
## @return: Dictionary containing all fast travel data
func save_data() -> Dictionary:
	return {
		"discovered_waypoints": discovered_waypoints,
		"unlocked_waypoints": unlocked_waypoints,
		"current_waypoint": current_waypoint
	}


## Load fast travel data from save
## @param data: Dictionary containing fast travel data
func load_data(data: Dictionary) -> void:
	discovered_waypoints = data.get("discovered_waypoints", [])
	unlocked_waypoints = data.get("unlocked_waypoints", [])
	current_waypoint = data.get("current_waypoint", "")
	
	# Update waypoint status based on loaded data
	for waypoint_id in discovered_waypoints:
		if waypoint_id in waypoints:
			waypoints[waypoint_id].discovered = true
	
	for waypoint_id in unlocked_waypoints:
		if waypoint_id in waypoints:
			waypoints[waypoint_id].unlocked = true
