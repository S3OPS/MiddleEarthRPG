extends Node

## Manages dialogue conversations with NPCs
## Handles dialogue flow, choices, and events

signal dialogue_started(dialogue_id: String)
signal dialogue_line_changed(line: DialogueResource.DialogueLine)
signal dialogue_ended(dialogue_id: String)

var current_dialogue: DialogueResource = null
var current_line_index: int = 0
var is_dialogue_active: bool = false

func _ready() -> void:
	print("💬 DialogueManager initialized")

## Start a dialogue conversation
func start_dialogue(dialogue: DialogueResource) -> bool:
	if not dialogue or dialogue.lines.is_empty():
		push_error("Cannot start invalid or empty dialogue")
		return false
	
	if is_dialogue_active:
		push_warning("Dialogue already active, ending current dialogue")
		end_dialogue()
	
	current_dialogue = dialogue
	current_line_index = 0
	is_dialogue_active = true
	
	dialogue_started.emit(dialogue.dialogue_id)
	EventBus.ui_panel_toggled.emit("dialogue", true)
	
	# Show first line
	show_current_line()
	
	# Emit NPC talked event for quest tracking
	EventBus.npc_talked.emit(dialogue.npc_id)
	
	return true

## Show the current dialogue line
func show_current_line() -> void:
	if not is_dialogue_active or not current_dialogue:
		return
	
	var line = current_dialogue.get_line(current_line_index)
	if line:
		dialogue_line_changed.emit(line)

## Advance to the next dialogue line (no choices)
func advance_dialogue() -> void:
	if not is_dialogue_active or not current_dialogue:
		return
	
	var current_line = current_dialogue.get_line(current_line_index)
	if not current_line:
		end_dialogue()
		return
	
	# If line has choices, don't auto-advance
	if not current_line.choices.is_empty():
		return
	
	# Move to next line
	current_line_index = current_line.next_line_index
	
	if current_dialogue.is_dialogue_end(current_line_index):
		end_dialogue()
	else:
		show_current_line()

## Select a dialogue choice
func select_choice(choice_index: int) -> void:
	if not is_dialogue_active or not current_dialogue:
		return
	
	var current_line = current_dialogue.get_line(current_line_index)
	if not current_line or choice_index < 0 or choice_index >= current_line.choices.size():
		push_error("Invalid choice index: %d" % choice_index)
		return
	
	var choice = current_line.choices[choice_index]
	
	# Check choice condition
	if choice.condition_func and not choice.condition_func.call():
		EventBus.show_notification("Cannot select this option", "warning")
		return
	
	# Move to next line based on choice
	current_line_index = choice.next_line_index
	
	if current_dialogue.is_dialogue_end(current_line_index):
		end_dialogue()
	else:
		show_current_line()

## End the current dialogue
func end_dialogue() -> void:
	if not is_dialogue_active:
		return
	
	var dialogue_id = current_dialogue.dialogue_id if current_dialogue else ""
	
	is_dialogue_active = false
	dialogue_ended.emit(dialogue_id)
	EventBus.ui_panel_toggled.emit("dialogue", false)
	
	current_dialogue = null
	current_line_index = 0

## Get the current dialogue line
func get_current_line() -> DialogueResource.DialogueLine:
	if not is_dialogue_active or not current_dialogue:
		return null
	return current_dialogue.get_line(current_line_index)

## Skip the current dialogue
func skip_dialogue() -> void:
	if is_dialogue_active:
		end_dialogue()
