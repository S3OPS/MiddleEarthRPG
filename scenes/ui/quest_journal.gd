extends Control

## Quest Journal UI Panel
## Displays active quests and their objectives

@onready var quest_list = $Panel/MarginContainer/VBoxContainer/ScrollContainer/QuestList
@onready var close_button = $Panel/MarginContainer/VBoxContainer/TopBar/CloseButton

var is_visible: bool = false

func _ready() -> void:
	# Connect signals
	EventBus.quest_started.connect(_on_quest_started)
	EventBus.quest_completed.connect(_on_quest_completed)
	EventBus.quest_objective_updated.connect(_on_quest_objective_updated)
	EventBus.ui_panel_toggled.connect(_on_ui_panel_toggled)
	
	close_button.pressed.connect(_on_close_pressed)
	
	# Start hidden
	hide()
	is_visible = false

func _input(event: InputEvent) -> void:
	if event.is_action_pressed("toggle_quest_journal"):
		toggle_visibility()

func toggle_visibility() -> void:
	is_visible = !is_visible
	visible = is_visible
	
	if is_visible:
		refresh_quest_list()
		# Pause game when UI is open
		Input.mouse_mode = Input.MOUSE_MODE_VISIBLE
	else:
		Input.mouse_mode = Input.MOUSE_MODE_CAPTURED

func refresh_quest_list() -> void:
	# Clear existing quest entries
	for child in quest_list.get_children():
		child.queue_free()
	
	# Get all active quests
	var active_quests = QuestManager.get_active_quests()
	
	if active_quests.is_empty():
		var no_quests = Label.new()
		no_quests.text = "No active quests"
		no_quests.horizontal_alignment = HORIZONTAL_ALIGNMENT_CENTER
		quest_list.add_child(no_quests)
		return
	
	# Add each quest
	for quest in active_quests:
		var quest_panel = _create_quest_panel(quest)
		quest_list.add_child(quest_panel)

func _create_quest_panel(quest: QuestResource) -> PanelContainer:
	var panel = PanelContainer.new()
	panel.size_flags_horizontal = Control.SIZE_EXPAND_FILL
	
	var vbox = VBoxContainer.new()
	panel.add_child(vbox)
	
	# Quest name
	var name_label = Label.new()
	name_label.text = quest.quest_name
	name_label.add_theme_font_size_override("font_size", 20)
	vbox.add_child(name_label)
	
	# Quest description
	var desc_label = Label.new()
	desc_label.text = quest.description
	desc_label.autowrap_mode = TextServer.AUTOWRAP_WORD
	desc_label.custom_minimum_size = Vector2(400, 0)
	vbox.add_child(desc_label)
	
	# Objectives
	var obj_label = Label.new()
	obj_label.text = "Objectives:"
	obj_label.add_theme_font_size_override("font_size", 16)
	vbox.add_child(obj_label)
	
	for i in range(quest.objectives.size()):
		var obj_text = Label.new()
		var obj = quest.objectives[i]
		var check_mark = "✓" if obj["current"] >= obj["required"] else "•"
		obj_text.text = "  %s %s" % [check_mark, quest.get_objective_description(i)]
		obj_text.modulate = Color.GREEN if obj["current"] >= obj["required"] else Color.WHITE
		vbox.add_child(obj_text)
	
	# Progress bar
	var progress = ProgressBar.new()
	progress.value = quest.get_progress_percentage()
	progress.show_percentage = true
	vbox.add_child(progress)
	
	return panel

func _on_quest_started(_quest_id: String, _quest_name: String) -> void:
	if is_visible:
		refresh_quest_list()

func _on_quest_completed(_quest_id: String, _quest_name: String) -> void:
	if is_visible:
		refresh_quest_list()

func _on_quest_objective_updated(_quest_id: String, _objective_index: int) -> void:
	if is_visible:
		refresh_quest_list()

func _on_ui_panel_toggled(panel_name: String, should_show: bool) -> void:
	if panel_name == "quest_journal":
		is_visible = should_show
		visible = should_show
		if should_show:
			refresh_quest_list()

func _on_close_pressed() -> void:
	toggle_visibility()
