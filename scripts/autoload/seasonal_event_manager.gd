extends Node

## SeasonalEventManager
## Manages seasonal events and limited-time content

signal event_started(event_id: String)
signal event_ended(event_id: String)
signal event_completed(event_id: String)

var events: Dictionary = {}  # event_id -> SeasonalEventResource
var active_events: Array[String] = []
var completed_events: Array[String] = []

func _ready() -> void:
	# Check for active events on startup
	_check_active_events()

func register_event(event: SeasonalEventResource) -> void:
	"""Register a seasonal event"""
	if event.event_id.is_empty():
		push_error("Cannot register event with empty ID")
		return
	
	events[event.event_id] = event
	print("Event registered: ", event.event_name)

func start_event(event_id: String) -> bool:
	"""Manually start an event"""
	if event_id not in events:
		push_error("Event not found: " + event_id)
		return false
	
	if event_id in active_events:
		print("Event already active")
		return false
	
	var event = events[event_id]
	event.is_active = true
	active_events.append(event_id)
	event_started.emit(event_id)
	
	print("Event started: ", event.event_name)
	return true

func end_event(event_id: String) -> void:
	"""End an active event"""
	if event_id not in active_events:
		return
	
	var event = events[event_id]
	event.is_active = false
	active_events.erase(event_id)
	event_ended.emit(event_id)
	
	print("Event ended: ", event.event_name)

func complete_event(event_id: String) -> void:
	"""Mark an event as completed by the player"""
	if event_id not in events:
		return
	
	var event = events[event_id]
	event.times_completed += 1
	
	if not event.repeatable and event_id not in completed_events:
		completed_events.append(event_id)
	
	event_completed.emit(event_id)
	EventBus.achievement_unlocked.emit("event_" + event_id, "Completed " + event.event_name)
	
	print("Event completed: ", event.event_name)

func is_event_active(event_id: String) -> bool:
	"""Check if an event is currently active"""
	return event_id in active_events

func get_active_events() -> Array[SeasonalEventResource]:
	"""Get all currently active events"""
	var result: Array[SeasonalEventResource] = []
	for event_id in active_events:
		result.append(events[event_id])
	return result

func get_event_multipliers() -> Dictionary:
	"""Get combined multipliers from all active events"""
	var multipliers = {
		"xp": 1.0,
		"gold": 1.0
	}
	
	for event_id in active_events:
		var event = events[event_id]
		multipliers["xp"] *= event.bonus_xp_multiplier
		multipliers["gold"] *= event.bonus_gold_multiplier
	
	return multipliers

func _check_active_events() -> void:
	"""Check which events should be active based on date"""
	var current_date = Time.get_date_string_from_system()
	
	for event_id in events:
		var event = events[event_id]
		if event.check_if_active(current_date):
			if event_id not in active_events:
				start_event(event_id)
		else:
			if event_id in active_events:
				end_event(event_id)

func save_data() -> Dictionary:
	"""Save event data"""
	var event_data = {}
	for event_id in events:
		event_data[event_id] = {
			"is_active": events[event_id].is_active,
			"times_completed": events[event_id].times_completed
		}
	
	return {
		"events": event_data,
		"completed_events": completed_events
	}

func load_data(data: Dictionary) -> void:
	"""Load event data"""
	var event_data = data.get("events", {})
	completed_events = data.get("completed_events", [])
	
	for event_id in event_data:
		if event_id in events:
			var saved = event_data[event_id]
			events[event_id].is_active = saved.get("is_active", false)
			events[event_id].times_completed = saved.get("times_completed", 0)
			
			if events[event_id].is_active:
				active_events.append(event_id)
