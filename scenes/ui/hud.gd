extends CanvasLayer
## HUD overlay displaying player stats and performance metrics
## Shows health, stamina, XP, level, and optional performance data

@onready var health_bar: ProgressBar = $MarginContainer/VBoxContainer/HealthBar
@onready var health_label: Label = $MarginContainer/VBoxContainer/HealthBar/Label
@onready var stamina_bar: ProgressBar = $MarginContainer/VBoxContainer/StaminaBar
@onready var stamina_label: Label = $MarginContainer/VBoxContainer/StaminaBar/Label
@onready var xp_bar: ProgressBar = $MarginContainer/VBoxContainer/XPBar
@onready var xp_label: Label = $MarginContainer/VBoxContainer/XPBar/Label
@onready var level_label: Label = $MarginContainer/VBoxContainer/LevelLabel

var player: Node = null
var show_performance_stats: bool = false
var performance_overlay: Control = null


func _ready() -> void:
	# Create performance overlay control for drawing
	performance_overlay = Control.new()
	performance_overlay.set_anchors_preset(Control.PRESET_FULL_RECT)
	performance_overlay.mouse_filter = Control.MOUSE_FILTER_IGNORE
	performance_overlay.draw.connect(_draw_performance_overlay)
	add_child(performance_overlay)
	
	# Connect to EventBus signals
	if EventBus.player_health_changed.connect(_on_health_changed) != OK:
		push_error("Failed to connect player_health_changed signal")
	if EventBus.player_stamina_changed.connect(_on_stamina_changed) != OK:
		push_error("Failed to connect player_stamina_changed signal")
	if EventBus.player_experience_gained.connect(_on_experience_gained) != OK:
		push_error("Failed to connect player_experience_gained signal")
	if EventBus.player_level_up.connect(_on_level_up) != OK:
		push_error("Failed to connect player_level_up signal")
	
	# Get player reference
	await get_tree().process_frame
	player = GameManager.get_player()
	
	# Initialize UI if player exists
	if _validate_player():
		_update_all_stats()
	else:
		push_warning("HUD: Player not found or invalid stats")


func _input(event: InputEvent) -> void:
	# Toggle performance stats with F3 (ui_text_completion_query)
	# Note: Define custom "toggle_debug_stats" action in project settings for better control
	if event.is_action_pressed("ui_text_completion_query"):  # F3 by default
		show_performance_stats = not show_performance_stats
		if performance_overlay:
			performance_overlay.queue_redraw()


func _process(_delta: float) -> void:
	# Update performance display if enabled
	if show_performance_stats and performance_overlay:
		performance_overlay.queue_redraw()


func _draw_performance_overlay() -> void:
	if not show_performance_stats or not performance_overlay:
		return
	
	# Draw performance overlay
	var stats_text = _get_performance_text()
	var font = ThemeDB.fallback_font
	var font_size = 14
	var viewport_size = get_viewport().get_visible_rect().size
	var position = Vector2(10, viewport_size.y - 120)
	
	# Draw background
	var text_size = font.get_multiline_string_size(stats_text, HORIZONTAL_ALIGNMENT_LEFT, -1, font_size)
	performance_overlay.draw_rect(Rect2(position - Vector2(5, text_size.y + 5), text_size + Vector2(10, 10)), Color(0, 0, 0, 0.7))
	
	# Draw text
	performance_overlay.draw_multiline_string(font, position, stats_text, HORIZONTAL_ALIGNMENT_LEFT, -1, font_size, -1, Color.WHITE)


## Get performance statistics text
func _get_performance_text() -> String:
	var fps = Engine.get_frames_per_second()
	var memory = Performance.get_monitor(Performance.MEMORY_STATIC) / 1024.0 / 1024.0
	var objects = Performance.get_monitor(Performance.OBJECT_COUNT)
	var nodes = Performance.get_monitor(Performance.OBJECT_NODE_COUNT)
	var draw_calls = Performance.get_monitor(Performance.RENDER_TOTAL_DRAW_CALLS_IN_FRAME)
	var physics_3d = Performance.get_monitor(Performance.PHYSICS_3D_ACTIVE_OBJECTS)
	
	return """Performance Stats (F3 to toggle):
FPS: %d
Memory: %.1f MB
Objects: %d
Nodes: %d
Draw Calls: %d
Physics Objects: %d""" % [fps, memory, objects, nodes, draw_calls, physics_3d]


## Validate that player and stats exist
func _validate_player() -> bool:
	return player != null and is_instance_valid(player) and player.stats != null


## Update all UI elements with current stats
func _update_all_stats() -> void:
	if not _validate_player():
		return
	
	var stats = player.stats
	_update_health_bar(stats.current_health, stats.max_health)
	_update_stamina_bar(stats.current_stamina, stats.max_stamina)
	_update_experience_bar(stats.experience, stats.experience_to_next_level)
	_update_level_label(stats.level)


## Update health bar and label
func _update_health_bar(current: float, maximum: float) -> void:
	if not health_bar or not health_label:
		return
	
	health_bar.max_value = maximum
	health_bar.value = current
	health_label.text = "Health: %d / %d" % [int(current), int(maximum)]


## Update stamina bar and label
func _update_stamina_bar(current: float, maximum: float) -> void:
	if not stamina_bar or not stamina_label:
		return
	
	stamina_bar.max_value = maximum
	stamina_bar.value = current
	stamina_label.text = "Stamina: %d / %d" % [int(current), int(maximum)]


## Update experience bar and label
func _update_experience_bar(current: int, maximum: int) -> void:
	if not xp_bar or not xp_label:
		return
	
	xp_bar.max_value = maximum
	xp_bar.value = current
	xp_label.text = "XP: %d / %d" % [current, maximum]


## Update level label
func _update_level_label(level: int) -> void:
	if not level_label:
		return
	
	level_label.text = "Level %d" % level


## Signal handler for health changes
func _on_health_changed(current: float, maximum: float) -> void:
	_update_health_bar(current, maximum)


## Signal handler for stamina changes
func _on_stamina_changed(current: float, maximum: float) -> void:
	_update_stamina_bar(current, maximum)


## Signal handler for experience gained
func _on_experience_gained(_amount: int, _total: int) -> void:
	if not _validate_player():
		return
	
	var stats = player.stats
	_update_experience_bar(stats.experience, stats.experience_to_next_level)


## Signal handler for level up
func _on_level_up(new_level: int) -> void:
	_update_level_label(new_level)
	
	# Update XP bar after level up
	if _validate_player():
		_update_experience_bar(player.stats.experience, player.stats.experience_to_next_level)
