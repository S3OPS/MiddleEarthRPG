extends Node

## Sample Waypoints Data
## Defines the sample fast travel waypoints

const WaypointResource = preload("res://scripts/resources/waypoint_resource.gd")

static func create_sample_waypoints() -> Array[WaypointResource]:
	"""Create sample waypoints for fast travel"""
	var waypoints: Array[WaypointResource] = []
	
	# Shire Waypoints
	var hobbiton = WaypointResource.new(
		"hobbiton",
		"Hobbiton",
		"the_shire",
		Vector3(10, 0, 10)
	)
	hobbiton.description = "The heart of the Shire, home to Bag End"
	hobbiton.travel_cost = 0  # Free starting location
	waypoints.append(hobbiton)
	
	var bywater = WaypointResource.new(
		"bywater",
		"Bywater",
		"the_shire",
		Vector3(15, 0, 5)
	)
	bywater.description = "A small village known for its Green Dragon Inn"
	bywater.travel_cost = 10
	waypoints.append(bywater)
	
	# Rohan Waypoints
	var edoras = WaypointResource.new(
		"edoras",
		"Edoras",
		"rohan",
		Vector3(50, 5, 50)
	)
	edoras.description = "The capital of Rohan, seat of King Théoden"
	edoras.travel_cost = 50
	edoras.required_quest = "reach_rohan"
	waypoints.append(edoras)
	
	var helms_deep = WaypointResource.new(
		"helms_deep",
		"Helm's Deep",
		"rohan",
		Vector3(45, 8, 60)
	)
	helms_deep.description = "The great fortress of Rohan, last refuge in times of war"
	helms_deep.travel_cost = 75
	helms_deep.required_quest = "defense_of_rohan"
	waypoints.append(helms_deep)
	
	# Mordor Waypoints (no fast travel within Mordor, but border camps)
	var black_gate = WaypointResource.new(
		"black_gate",
		"The Black Gate",
		"mordor",
		Vector3(100, 0, 100)
	)
	black_gate.description = "The main entrance to Mordor, heavily guarded"
	black_gate.travel_cost = 0  # Cannot fast travel here
	black_gate.unlocked = false
	waypoints.append(black_gate)
	
	# Rivendell Waypoint
	var rivendell = WaypointResource.new(
		"rivendell",
		"Rivendell",
		"rivendell",
		Vector3(30, 10, 30)
	)
	rivendell.description = "The sanctuary of Elrond, a haven of wisdom and peace"
	rivendell.travel_cost = 100
	rivendell.required_quest = "council_of_elrond"
	waypoints.append(rivendell)
	
	return waypoints
