extends Resource
class_name QuestResource

## A quest that the player can undertake
## Quests have objectives that must be completed and rewards upon completion

enum QuestStatus { NOT_STARTED, IN_PROGRESS, COMPLETED, FAILED }
enum ObjectiveType { KILL_ENEMIES, COLLECT_ITEMS, VISIT_LOCATION, TALK_TO_NPC }

## Quest identification
@export var quest_id: String = ""
@export var quest_name: String = ""
@export_multiline var description: String = ""

## Quest structure
@export var objectives: Array[Dictionary] = []  # Array of {type: ObjectiveType, target: String, current: int, required: int}
@export var prerequisites: Array[String] = []  # Quest IDs that must be completed first
@export var level_requirement: int = 1

## Quest rewards
@export var xp_reward: int = 0
@export var gold_reward: int = 0
@export var item_rewards: Array[String] = []  # Item IDs to give as rewards

## Quest state
var status: QuestStatus = QuestStatus.NOT_STARTED
var objectives_completed: int = 0

## Check if all prerequisites are met
func are_prerequisites_met(completed_quests: Array[String]) -> bool:
	for prereq in prerequisites:
		if not prereq in completed_quests:
			return false
	return true

## Check if all objectives are completed
func are_all_objectives_complete() -> bool:
	for objective in objectives:
		if objective["current"] < objective["required"]:
			return false
	return true

## Update an objective's progress
func update_objective(objective_index: int, progress: int) -> void:
	if objective_index < 0 or objective_index >= objectives.size():
		return
	
	var obj = objectives[objective_index]
	var was_completed = obj["current"] >= obj["required"]
	
	objectives[objective_index]["current"] = min(
		objectives[objective_index]["current"] + progress,
		objectives[objective_index]["required"]
	)
	
	# Only increment counter when objective transitions from incomplete to complete
	var is_now_completed = objectives[objective_index]["current"] >= objectives[objective_index]["required"]
	if is_now_completed and not was_completed:
		objectives_completed += 1

## Get objective description string
func get_objective_description(objective_index: int) -> String:
	if objective_index < 0 or objective_index >= objectives.size():
		return ""
	
	var obj = objectives[objective_index]
	var type_str = ObjectiveType.keys()[obj["type"]]
	return "%s: %d/%d" % [obj["target"], obj["current"], obj["required"]]

## Get quest progress percentage
func get_progress_percentage() -> float:
	if objectives.is_empty():
		return 0.0
	return (float(objectives_completed) / float(objectives.size())) * 100.0
