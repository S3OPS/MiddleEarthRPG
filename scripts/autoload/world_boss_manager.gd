extends Node

## WorldBossManager
## Manages world boss encounters

signal boss_spawned(boss_id: String, boss_name: String)
signal boss_defeated(boss_id: String, top_contributors: Array)
signal boss_despawned(boss_id: String)

var world_bosses: Dictionary = {}  # boss_id -> WorldBossResource
var active_boss: String = ""

func _ready() -> void:
	print("🐉 WorldBossManager initialized")
	
	# Check for boss spawns periodically
	var timer = Timer.new()
	timer.wait_time = 60.0  # Check every minute
	timer.timeout.connect(_check_boss_spawns)
	add_child(timer)
	timer.start()

func register_world_boss(boss: WorldBossResource) -> void:
	"""Register a world boss"""
	if boss.boss_id.is_empty():
		push_error("Cannot register boss with empty ID")
		return
	
	world_bosses[boss.boss_id] = boss
	print("World boss registered: ", boss.boss_name)

func _check_boss_spawns() -> void:
	"""Check if any bosses should spawn"""
	for boss_id in world_bosses:
		var boss = world_bosses[boss_id]
		if boss.can_spawn() and randf() < 0.1:  # 10% chance per check
			spawn_boss(boss_id)

func spawn_boss(boss_id: String) -> bool:
	"""Manually spawn a world boss"""
	if boss_id not in world_bosses:
		return false
	
	var boss = world_bosses[boss_id]
	
	if not boss.can_spawn():
		print("Boss cannot spawn yet")
		return false
	
	boss.spawn()
	active_boss = boss_id
	
	boss_spawned.emit(boss_id, boss.boss_name)
	EventBus.notification_requested.emit(
		"WORLD BOSS: " + boss.boss_name + " has appeared!",
		10.0,
		"warning"
	)
	
	print("World boss spawned: ", boss.boss_name)
	return true

func defeat_boss(boss_id: String) -> bool:
	"""Defeat a world boss"""
	if boss_id not in world_bosses:
		return false
	
	var boss = world_bosses[boss_id]
	
	if not boss.is_spawned:
		return false
	
	boss.despawn()
	
	# Get top contributors
	var top_contributors = boss.get_top_contributors(10)
	
	boss_defeated.emit(boss_id, top_contributors)
	EventBus.achievement_unlocked.emit(
		"world_boss_" + boss_id,
		"Defeated " + boss.boss_name
	)
	EventBus.notification_requested.emit(
		boss.boss_name + " has been defeated!",
		5.0,
		"success"
	)
	
	if active_boss == boss_id:
		active_boss = ""
	
	print("World boss defeated: ", boss.boss_name)
	return true

func despawn_boss(boss_id: String) -> bool:
	"""Despawn a world boss (timeout/escape)"""
	if boss_id not in world_bosses:
		return false
	
	var boss = world_bosses[boss_id]
	boss.despawn()
	
	boss_despawned.emit(boss_id)
	
	if active_boss == boss_id:
		active_boss = ""
	
	print("World boss despawned: ", boss.boss_name)
	return true

func record_damage(boss_id: String, player_id: String, damage: int) -> void:
	"""Record damage dealt to a world boss"""
	if boss_id not in world_bosses:
		return
	
	var boss = world_bosses[boss_id]
	boss.add_participant(player_id, damage)

func get_world_boss(boss_id: String) -> WorldBossResource:
	"""Get a world boss by ID"""
	return world_bosses.get(boss_id, null)

func get_active_boss() -> WorldBossResource:
	"""Get the currently active world boss"""
	if active_boss.is_empty():
		return null
	return world_bosses.get(active_boss, null)

func save_data() -> Dictionary:
	"""Save world boss data"""
	var boss_data = {}
	for boss_id in world_bosses:
		var boss = world_bosses[boss_id]
		boss_data[boss_id] = {
			"last_spawn_time": boss.last_spawn_time,
			"is_spawned": boss.is_spawned
		}
	
	return {
		"world_bosses": boss_data
	}

func load_data(data: Dictionary) -> void:
	"""Load world boss data"""
	var boss_data = data.get("world_bosses", {})
	for boss_id in boss_data:
		if boss_id in world_bosses:
			var saved = boss_data[boss_id]
			world_bosses[boss_id].last_spawn_time = saved.get("last_spawn_time", 0.0)
			world_bosses[boss_id].is_spawned = saved.get("is_spawned", false)
