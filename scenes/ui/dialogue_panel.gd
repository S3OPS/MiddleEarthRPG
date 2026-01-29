extends Control

## Dialogue UI Panel
## Displays NPC dialogue and player choices

@onready var dialogue_panel = $Panel
@onready var speaker_label = $Panel/MarginContainer/VBoxContainer/SpeakerName
@onready var text_label = $Panel/MarginContainer/VBoxContainer/DialogueText
@onready var choices_container = $Panel/MarginContainer/VBoxContainer/ChoicesContainer
@onready var continue_button = $Panel/MarginContainer/VBoxContainer/ContinueButton

var is_visible: bool = false

func _ready() -> void:
	# Connect signals
	DialogueManager.dialogue_started.connect(_on_dialogue_started)
	DialogueManager.dialogue_line_changed.connect(_on_dialogue_line_changed)
	DialogueManager.dialogue_ended.connect(_on_dialogue_ended)
	EventBus.ui_panel_toggled.connect(_on_ui_panel_toggled)
	
	continue_button.pressed.connect(_on_continue_pressed)
	
	# Start hidden
	hide()
	is_visible = false

func _input(event: InputEvent) -> void:
	if is_visible and event.is_action_pressed("ui_accept"):
		if continue_button.visible:
			_on_continue_pressed()

func _on_dialogue_started(_dialogue_id: String) -> void:
	is_visible = true
	visible = true
	Input.mouse_mode = Input.MOUSE_MODE_VISIBLE

func _on_dialogue_line_changed(line: DialogueResource.DialogueLine) -> void:
	if not line:
		return
	
	# Update speaker name
	speaker_label.text = line.speaker_name
	
	# Update dialogue text
	text_label.text = line.text
	
	# Clear existing choices
	for child in choices_container.get_children():
		child.queue_free()
	
	# Show choices or continue button
	if line.choices.is_empty():
		continue_button.show()
		choices_container.hide()
	else:
		continue_button.hide()
		choices_container.show()
		
		# Create choice buttons
		for i in range(line.choices.size()):
			var choice = line.choices[i]
			var button = Button.new()
			button.text = choice.choice_text
			button.size_flags_horizontal = Control.SIZE_EXPAND_FILL
			button.pressed.connect(func(): _on_choice_selected(i))
			choices_container.add_child(button)

func _on_choice_selected(choice_index: int) -> void:
	DialogueManager.select_choice(choice_index)

func _on_continue_pressed() -> void:
	DialogueManager.advance_dialogue()

func _on_dialogue_ended(_dialogue_id: String) -> void:
	is_visible = false
	visible = false
	Input.mouse_mode = Input.MOUSE_MODE_CAPTURED

func _on_ui_panel_toggled(panel_name: String, should_show: bool) -> void:
	if panel_name == "dialogue":
		is_visible = should_show
		visible = should_show
		if should_show:
			Input.mouse_mode = Input.MOUSE_MODE_VISIBLE
		else:
			Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
