extends Resource
class_name DialogueResource

## Represents a dialogue conversation with branching choices

## Single dialogue line with optional choices
class DialogueLine:
	var speaker_name: String = ""
	var text: String = ""
	var choices: Array[DialogueChoice] = []
	var next_line_index: int = -1  # -1 means end of dialogue

## Dialogue choice that leads to another line
class DialogueChoice:
	var choice_text: String = ""
	var next_line_index: int = -1
	var condition_func: Callable = func(): return true  # Optional condition

## Dialogue metadata
@export var dialogue_id: String = ""
@export var npc_id: String = ""
@export var npc_name: String = ""

## Dialogue lines - stored as dictionaries for export
@export var lines_data: Array[Dictionary] = []

var lines: Array[DialogueLine] = []

## Initialize dialogue from exported data
func _init() -> void:
	_parse_lines_data()

## Parse exported line data into DialogueLine objects
func _parse_lines_data() -> void:
	lines.clear()
	for line_dict in lines_data:
		var line = DialogueLine.new()
		line.speaker_name = line_dict.get("speaker", npc_name)
		line.text = line_dict.get("text", "")
		line.next_line_index = line_dict.get("next", -1)
		
		# Parse choices
		var choices_data = line_dict.get("choices", [])
		for choice_dict in choices_data:
			var choice = DialogueChoice.new()
			choice.choice_text = choice_dict.get("text", "")
			choice.next_line_index = choice_dict.get("next", -1)
			line.choices.append(choice)
		
		lines.append(line)

## Get a specific dialogue line
func get_line(index: int) -> DialogueLine:
	if index < 0 or index >= lines.size():
		return null
	return lines[index]

## Get the first line
func get_first_line() -> DialogueLine:
	return get_line(0)

## Check if dialogue has ended
func is_dialogue_end(line_index: int) -> bool:
	return line_index == -1 or line_index >= lines.size()
