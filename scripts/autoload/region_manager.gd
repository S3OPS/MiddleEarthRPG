extends Node

## RegionManager
## Manages regions and player location in the world

signal region_entered(region_id: String)
signal region_exited(region_id: String)
signal region_discovered(region_id: String)

var regions: Dictionary = {}  # region_id -> RegionResource
var current_region: String = ""
var discovered_regions: Array[String] = []

func _ready() -> void:
	pass

func register_region(region: RegionResource) -> void:
	"""Register a new region"""
	if region.region_id.is_empty():
		push_error("Cannot register region with empty ID")
		return
	
	regions[region.region_id] = region
	print("Region registered: ", region.region_name)

func discover_region(region_id: String) -> void:
	"""Mark a region as discovered"""
	if region_id not in regions:
		push_error("Region not found: " + region_id)
		return
	
	if region_id in discovered_regions:
		return  # Already discovered
	
	discovered_regions.append(region_id)
	regions[region_id].discovered = true
	region_discovered.emit(region_id, regions[region_id].region_name)
	EventBus.region_discovered.emit(region_id, regions[region_id].region_name)
	EventBus.achievement_unlocked.emit("discover_" + region_id, "Discovered " + regions[region_id].region_name)
	print("Region discovered: ", regions[region_id].region_name)

func enter_region(region_id: String) -> void:
	"""Enter a new region"""
	if region_id not in regions:
		push_error("Region not found: " + region_id)
		return
	
	# Exit previous region
	if not current_region.is_empty() and current_region != region_id:
		region_exited.emit(current_region)
	
	current_region = region_id
	region_entered.emit(region_id)
	
	# Auto-discover when entering
	if region_id not in discovered_regions:
		discover_region(region_id)
	
	print("Entered region: ", regions[region_id].region_name)

func get_region(region_id: String) -> RegionResource:
	"""Get a region by ID"""
	return regions.get(region_id, null)

func get_current_region() -> RegionResource:
	"""Get the current region"""
	if current_region.is_empty():
		return null
	return regions.get(current_region, null)

func is_region_discovered(region_id: String) -> bool:
	"""Check if a region is discovered"""
	return region_id in discovered_regions

func get_discovered_regions() -> Array[String]:
	"""Get all discovered regions"""
	return discovered_regions

func can_enter_region(region_id: String) -> bool:
	"""Check if player meets requirements to enter a region"""
	var region: RegionResource = get_region(region_id)
	if region == null:
		return false
	
	# Check level requirement
	if GameManager and GameManager.player_level < region.required_level:
		return false
	
	# Check faction requirement
	if not region.required_faction.is_empty():
		if not FactionManager.has_reputation(region.required_faction, region.required_reputation):
			return false
	
	return true

func save_data() -> Dictionary:
	"""Save region data"""
	return {
		"current_region": current_region,
		"discovered_regions": discovered_regions
	}

func load_data(data: Dictionary) -> void:
	"""Load region data"""
	current_region = data.get("current_region", "")
	discovered_regions = data.get("discovered_regions", [])
	
	# Update region discovered status
	for region_id in discovered_regions:
		if region_id in regions:
			regions[region_id].discovered = true
