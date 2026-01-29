extends Node

## Manages all quests in the game
## Tracks active quests, completed quests, and quest progression
## Communicates with EventBus for quest-related events

var active_quests: Dictionary = {}  # quest_id -> QuestResource
var completed_quests: Array[String] = []  # Array of completed quest IDs
var available_quests: Dictionary = {}  # quest_id -> QuestResource

func _ready() -> void:
	# Connect to relevant events
	EventBus.enemy_died.connect(_on_enemy_died)
	EventBus.item_collected.connect(_on_item_collected)
	EventBus.location_discovered.connect(_on_location_discovered)
	EventBus.npc_talked.connect(_on_npc_talked)

## Register a quest as available
func register_quest(quest: QuestResource) -> void:
	if not quest or quest.quest_id.is_empty():
		push_error("Cannot register invalid quest")
		return
	
	available_quests[quest.quest_id] = quest
	EventBus.quest_available.emit(quest.quest_id, quest.quest_name)

## Start a quest
func start_quest(quest_id: String) -> bool:
	if not available_quests.has(quest_id):
		push_warning("Quest not found: " + quest_id)
		return false
	
	var quest = available_quests[quest_id]
	
	# Check prerequisites
	if not quest.are_prerequisites_met(completed_quests):
		push_warning("Quest prerequisites not met: " + quest_id)
		return false
	
	# Check level requirement
	if not GameManager.player_stats:
		push_warning("Player stats not available for quest: " + quest_id)
		return false
	if GameManager.player_stats.level < quest.level_requirement:
		push_warning("Player level too low for quest: " + quest_id)
		return false
	
	# Start the quest
	quest.status = QuestResource.QuestStatus.IN_PROGRESS
	active_quests[quest_id] = quest
	available_quests.erase(quest_id)
	
	EventBus.quest_started.emit(quest_id, quest.quest_name)
	return true

## Complete a quest
func complete_quest(quest_id: String) -> void:
	if not active_quests.has(quest_id):
		return
	
	var quest = active_quests[quest_id]
	quest.status = QuestResource.QuestStatus.COMPLETED
	completed_quests.append(quest_id)
	active_quests.erase(quest_id)
	
	# Grant rewards
	if quest.xp_reward > 0:
		GameManager.add_experience(quest.xp_reward)
	
	if quest.gold_reward > 0:
		GameManager.add_gold(quest.gold_reward)
	
	for item_id in quest.item_rewards:
		EventBus.item_rewarded.emit(item_id)
	
	EventBus.quest_completed.emit(quest_id, quest.quest_name)

## Fail a quest
func fail_quest(quest_id: String) -> void:
	if not active_quests.has(quest_id):
		return
	
	var quest = active_quests[quest_id]
	quest.status = QuestResource.QuestStatus.FAILED
	active_quests.erase(quest_id)
	
	EventBus.quest_failed.emit(quest_id, quest.quest_name)

## Update quest progress for enemy kills
func _on_enemy_died(enemy_type: String, position: Vector3) -> void:
	for quest_id in active_quests:
		var quest = active_quests[quest_id]
		for i in range(quest.objectives.size()):
			var obj = quest.objectives[i]
			if obj["type"] == QuestResource.ObjectiveType.KILL_ENEMIES:
				if obj["target"] == enemy_type or obj["target"] == "any":
					quest.update_objective(i, 1)
					EventBus.quest_objective_updated.emit(quest_id, i)
					
					if quest.are_all_objectives_complete():
						complete_quest(quest_id)

## Update quest progress for item collection
func _on_item_collected(item_id: String) -> void:
	for quest_id in active_quests:
		var quest = active_quests[quest_id]
		for i in range(quest.objectives.size()):
			var obj = quest.objectives[i]
			if obj["type"] == QuestResource.ObjectiveType.COLLECT_ITEMS:
				if obj["target"] == item_id:
					quest.update_objective(i, 1)
					EventBus.quest_objective_updated.emit(quest_id, i)
					
					if quest.are_all_objectives_complete():
						complete_quest(quest_id)

## Update quest progress for location visits
func _on_location_discovered(location_name: String) -> void:
	for quest_id in active_quests:
		var quest = active_quests[quest_id]
		for i in range(quest.objectives.size()):
			var obj = quest.objectives[i]
			if obj["type"] == QuestResource.ObjectiveType.VISIT_LOCATION:
				if obj["target"] == location_name:
					quest.update_objective(i, 1)
					EventBus.quest_objective_updated.emit(quest_id, i)
					
					if quest.are_all_objectives_complete():
						complete_quest(quest_id)

## Update quest progress for NPC conversations
func _on_npc_talked(npc_id: String) -> void:
	for quest_id in active_quests:
		var quest = active_quests[quest_id]
		for i in range(quest.objectives.size()):
			var obj = quest.objectives[i]
			if obj["type"] == QuestResource.ObjectiveType.TALK_TO_NPC:
				if obj["target"] == npc_id:
					quest.update_objective(i, 1)
					EventBus.quest_objective_updated.emit(quest_id, i)
					
					if quest.are_all_objectives_complete():
						complete_quest(quest_id)

## Get all active quests
func get_active_quests() -> Array[QuestResource]:
	var quests: Array[QuestResource] = []
	for quest_id in active_quests:
		quests.append(active_quests[quest_id])
	return quests

## Get quest by ID
func get_quest(quest_id: String) -> QuestResource:
	if active_quests.has(quest_id):
		return active_quests[quest_id]
	if available_quests.has(quest_id):
		return available_quests[quest_id]
	return null

## Check if quest is completed
func is_quest_completed(quest_id: String) -> bool:
	return quest_id in completed_quests
