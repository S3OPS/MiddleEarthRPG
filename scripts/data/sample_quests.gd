extends Node

## Sample quests for testing the quest system
## These can be registered with QuestManager at game start

static func create_sample_quests() -> Array[QuestResource]:
	var quests: Array[QuestResource] = []
	
	# Quest 1: First Steps
	var quest1 = QuestResource.new()
	quest1.quest_id = "first_steps"
	quest1.quest_name = "First Steps in Middle-earth"
	quest1.description = "Begin your journey by defeating some enemies and exploring the land."
	quest1.level_requirement = 1
	quest1.objectives = [
		{
			"type": QuestResource.ObjectiveType.KILL_ENEMIES,
			"target": "any",
			"current": 0,
			"required": 3
		},
		{
			"type": QuestResource.ObjectiveType.VISIT_LOCATION,
			"target": "Rohan Plains",
			"current": 0,
			"required": 1
		}
	]
	quest1.xp_reward = 100
	quest1.gold_reward = 50
	quests.append(quest1)
	
	# Quest 2: Orc Menace
	var quest2 = QuestResource.new()
	quest2.quest_id = "orc_menace"
	quest2.quest_name = "The Orc Menace"
	quest2.description = "The orcs threaten the peaceful lands. Defeat 10 orcs to protect the realm."
	quest2.level_requirement = 2
	quest2.prerequisites = ["first_steps"]
	quest2.objectives = [
		{
			"type": QuestResource.ObjectiveType.KILL_ENEMIES,
			"target": "Orc Scout",
			"current": 0,
			"required": 10
		}
	]
	quest2.xp_reward = 250
	quest2.gold_reward = 100
	quest2.item_rewards = ["health_potion"]
	quests.append(quest2)
	
	# Quest 3: Treasure Hunter
	var quest3 = QuestResource.new()
	quest3.quest_id = "treasure_hunter"
	quest3.quest_name = "Treasure Hunter"
	quest3.description = "Seek out the hidden treasures scattered across Middle-earth."
	quest3.level_requirement = 1
	quest3.objectives = [
		{
			"type": QuestResource.ObjectiveType.COLLECT_ITEMS,
			"target": "treasure_chest",
			"current": 0,
			"required": 5
		}
	]
	quest3.xp_reward = 200
	quest3.gold_reward = 150
	quests.append(quest3)
	
	# Quest 4: Meet the Wizard
	var quest4 = QuestResource.new()
	quest4.quest_id = "meet_wizard"
	quest4.quest_name = "Seeking Wisdom"
	quest4.description = "Find and speak with the wise wizard Gandalf to learn more about your quest."
	quest4.level_requirement = 1
	quest4.objectives = [
		{
			"type": QuestResource.ObjectiveType.TALK_TO_NPC,
			"target": "gandalf",
			"current": 0,
			"required": 1
		}
	]
	quest4.xp_reward = 150
	quest4.gold_reward = 75
	quest4.item_rewards = ["wizard_staff"]
	quests.append(quest4)
	
	# Quest 5: The Journey Begins
	var quest5 = QuestResource.new()
	quest5.quest_id = "journey_begins"
	quest5.quest_name = "The Journey Begins"
	quest5.description = "Explore all the major regions of Middle-earth."
	quest5.level_requirement = 3
	quest5.prerequisites = ["first_steps"]
	quest5.objectives = [
		{
			"type": QuestResource.ObjectiveType.VISIT_LOCATION,
			"target": "The Shire",
			"current": 0,
			"required": 1
		},
		{
			"type": QuestResource.ObjectiveType.VISIT_LOCATION,
			"target": "Rohan Plains",
			"current": 0,
			"required": 1
		},
		{
			"type": QuestResource.ObjectiveType.VISIT_LOCATION,
			"target": "Mordor",
			"current": 0,
			"required": 1
		}
	]
	quest5.xp_reward = 500
	quest5.gold_reward = 250
	quest5.item_rewards = ["ring_of_power"]
	quests.append(quest5)
	
	return quests
