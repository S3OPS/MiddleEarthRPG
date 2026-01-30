extends Node

## Sample Regional Quests Data
## Defines region-specific quests for Phase 5

const QuestResource = preload("res://scripts/resources/quest_resource.gd")

static func create_regional_quests() -> Array[QuestResource]:
	"""Create regional quests for the new regions"""
	var quests: Array[QuestResource] = []
	
	# ========================================
	# THE SHIRE QUESTS
	# ========================================
	
	# Quest 1: Trouble at Hobbiton
	var hobbiton_quest = QuestResource.new()
	hobbiton_quest.quest_id = "trouble_at_hobbiton"
	hobbiton_quest.quest_name = "Trouble at Hobbiton"
	hobbiton_quest.description = "The hobbits of Hobbiton are troubled by wolves near the village. Help them by clearing the threat."
	hobbiton_quest.level_requirement = 1
	hobbiton_quest.objectives = [
		{"type": QuestResource.ObjectiveType.KILL_ENEMIES, "target": "Defeat Wolves", "current": 0, "required": 5, "data": {"enemy_type": "Wolf"}},
		{"type": QuestResource.ObjectiveType.TALK_TO_NPC, "target": "Report to the Mayor", "current": 0, "required": 1, "data": {"npc_id": "mayor_hobbiton"}}
	]
	hobbiton_quest.xp_reward = 100
	hobbiton_quest.gold_reward = 50
	hobbiton_quest.item_rewards = ["healing_potion", "healing_potion", "healing_potion"]
	quests.append(hobbiton_quest)
	
	# Quest 2: The Missing Pipe
	var pipe_quest = QuestResource.new()
	pipe_quest.quest_id = "missing_pipe"
	pipe_quest.quest_name = "The Missing Pipe"
	pipe_quest.description = "Old Toby has lost his favorite pipe somewhere in the Shire. Help him find it."
	pipe_quest.level_requirement = 1
	pipe_quest.objectives = [
		{"type": QuestResource.ObjectiveType.COLLECT_ITEMS, "target": "Find Old Toby's Pipe", "current": 0, "required": 1, "data": {"item_id": "old_pipe"}},
		{"type": QuestResource.ObjectiveType.TALK_TO_NPC, "target": "Return the Pipe to Old Toby", "current": 0, "required": 1, "data": {"npc_id": "old_toby"}}
	]
	pipe_quest.xp_reward = 75
	pipe_quest.gold_reward = 25
	quests.append(pipe_quest)
	
	# ========================================
	# ROHAN QUESTS
	# ========================================
	
	# Quest 3: Reach Rohan
	var reach_rohan = QuestResource.new()
	reach_rohan.quest_id = "reach_rohan"
	reach_rohan.quest_name = "Journey to Rohan"
	reach_rohan.description = "Travel to the plains of Rohan and meet with the horse-lords. A dangerous journey lies ahead."
	reach_rohan.level_requirement = 5
	reach_rohan.objectives = [
		{"type": QuestResource.ObjectiveType.VISIT_LOCATION, "target": "Reach the Plains of Rohan", "current": 0, "required": 1, "data": {"location": "rohan_border"}},
		{"type": QuestResource.ObjectiveType.TALK_TO_NPC, "target": "Speak with a Rohirrim Scout", "current": 0, "required": 1, "data": {"npc_id": "rohirrim_scout"}}
	]
	reach_rohan.xp_reward = 500
	reach_rohan.gold_reward = 200
	reach_rohan.item_rewards = ["rohan_banner"]
	quests.append(reach_rohan)
	
	# Quest 4: Defense of Rohan
	var defense_rohan = QuestResource.new()
	defense_rohan.quest_id = "defense_of_rohan"
	defense_rohan.quest_name = "Defense of Rohan"
	defense_rohan.description = "Orcs are raiding the settlements of Rohan. Help the Rohirrim defend their lands."
	defense_rohan.level_requirement = 6
	defense_rohan.prerequisites = ["reach_rohan"]
	defense_rohan.objectives = [
		{"type": QuestResource.ObjectiveType.KILL_ENEMIES, "target": "Defeat Orc Raiders", "current": 0, "required": 15, "data": {"enemy_type": "Orc"}},
		{"type": QuestResource.ObjectiveType.VISIT_LOCATION, "target": "Reach Helm's Deep", "current": 0, "required": 1, "data": {"location": "helms_deep"}}
	]
	defense_rohan.xp_reward = 800
	defense_rohan.gold_reward = 300
	defense_rohan.item_rewards = ["rohirrim_spear"]
	quests.append(defense_rohan)
	
	# Quest 5: Riders of Rohan
	var riders_quest = QuestResource.new()
	riders_quest.quest_id = "riders_of_rohan"
	riders_quest.quest_name = "Riders of Rohan"
	riders_quest.description = "Prove your worth to the Rohirrim by completing their trials of horsemanship and combat."
	riders_quest.level_requirement = 7
	riders_quest.prerequisites = ["defense_of_rohan"]
	riders_quest.objectives = [
		{"type": QuestResource.ObjectiveType.KILL_ENEMIES, "target": "Defeat Wargs", "current": 0, "required": 10, "data": {"enemy_type": "Warg"}},
		{"type": QuestResource.ObjectiveType.COLLECT_ITEMS, "target": "Collect Horse Medallions", "current": 0, "required": 5, "data": {"item_id": "horse_medallion"}}
	]
	riders_quest.xp_reward = 1000
	riders_quest.gold_reward = 400
	riders_quest.item_rewards = ["rohirrim_armor"]
	quests.append(riders_quest)
	
	# ========================================
	# RIVENDELL QUESTS
	# ========================================
	
	# Quest 6: Council of Elrond
	var council_quest = QuestResource.new()
	council_quest.quest_id = "council_of_elrond"
	council_quest.quest_name = "The Council of Elrond"
	council_quest.description = "You have been summoned to Rivendell for a council of great importance. The fate of Middle-earth hangs in the balance."
	council_quest.level_requirement = 3
	council_quest.objectives = [
		{"type": QuestResource.ObjectiveType.VISIT_LOCATION, "target": "Reach Rivendell", "current": 0, "required": 1, "data": {"location": "rivendell"}},
		{"type": QuestResource.ObjectiveType.TALK_TO_NPC, "target": "Attend the Council", "current": 0, "required": 1, "data": {"npc_id": "elrond"}}
	]
	council_quest.xp_reward = 400
	council_quest.gold_reward = 150
	quests.append(council_quest)
	
	# Quest 7: Elven Wisdom
	var elven_wisdom = QuestResource.new()
	elven_wisdom.quest_id = "elven_wisdom"
	elven_wisdom.quest_name = "Elven Wisdom"
	elven_wisdom.description = "The elves of Rivendell offer to teach you their ancient knowledge. Complete their trials to prove yourself worthy."
	elven_wisdom.level_requirement = 4
	elven_wisdom.prerequisites = ["council_of_elrond"]
	elven_wisdom.objectives = [
		{"type": QuestResource.ObjectiveType.COLLECT_ITEMS, "target": "Gather Ancient Texts", "current": 0, "required": 3, "data": {"item_id": "ancient_text"}},
		{"type": QuestResource.ObjectiveType.KILL_ENEMIES, "target": "Defeat Corrupted Spirits", "current": 0, "required": 8, "data": {"enemy_type": "Spirit"}}
	]
	elven_wisdom.xp_reward = 600
	elven_wisdom.gold_reward = 250
	elven_wisdom.item_rewards = ["elven_blade"]
	quests.append(elven_wisdom)
	
	# ========================================
	# MORDOR QUESTS
	# ========================================
	
	# Quest 8: Shadow of Mordor
	var shadow_quest = QuestResource.new()
	shadow_quest.quest_id = "shadow_of_mordor"
	shadow_quest.quest_name = "Into the Shadow"
	shadow_quest.description = "Dark forces stir in Mordor. Scout the borders and gather intelligence on Sauron's armies."
	shadow_quest.level_requirement = 10
	shadow_quest.objectives = [
		{"type": QuestResource.ObjectiveType.VISIT_LOCATION, "target": "Reach the Black Gate", "current": 0, "required": 1, "data": {"location": "black_gate"}},
		{"type": QuestResource.ObjectiveType.KILL_ENEMIES, "target": "Defeat Mordor Orcs", "current": 0, "required": 20, "data": {"enemy_type": "MordorOrc"}}
	]
	shadow_quest.xp_reward = 1500
	shadow_quest.gold_reward = 500
	shadow_quest.item_rewards = ["scouting_report"]
	quests.append(shadow_quest)
	
	# Quest 9: The Ring's Pull
	var ring_quest = QuestResource.new()
	ring_quest.quest_id = "rings_pull"
	ring_quest.quest_name = "The Ring's Pull"
	ring_quest.description = "The One Ring's influence grows stronger as you approach Mount Doom. You must resist its corruption."
	ring_quest.level_requirement = 12
	ring_quest.prerequisites = ["shadow_of_mordor"]
	ring_quest.objectives = [
		{"type": QuestResource.ObjectiveType.VISIT_LOCATION, "target": "Climb Mount Doom", "current": 0, "required": 1, "data": {"location": "mount_doom"}},
		{"type": QuestResource.ObjectiveType.KILL_ENEMIES, "target": "Defeat the Nazgûl", "current": 0, "required": 1, "data": {"enemy_type": "Nazgul"}}
	]
	ring_quest.xp_reward = 3000
	ring_quest.gold_reward = 1000
	ring_quest.item_rewards = ["mithril_coat"]
	quests.append(ring_quest)
	
	# ========================================
	# EXPLORATION QUESTS
	# ========================================
	
	# Quest 10: Explorer of Middle-earth
	var explorer_quest = QuestResource.new()
	explorer_quest.quest_id = "explorer_middle_earth"
	explorer_quest.quest_name = "Explorer of Middle-earth"
	explorer_quest.description = "Discover all the major regions of Middle-earth and become a renowned explorer."
	explorer_quest.level_requirement = 1
	explorer_quest.objectives = [
		{"type": QuestResource.ObjectiveType.VISIT_LOCATION, "target": "Discover The Shire", "current": 0, "required": 1, "data": {"location": "the_shire"}},
		{"type": QuestResource.ObjectiveType.VISIT_LOCATION, "target": "Discover Rohan", "current": 0, "required": 1, "data": {"location": "rohan"}},
		{"type": QuestResource.ObjectiveType.VISIT_LOCATION, "target": "Discover Rivendell", "current": 0, "required": 1, "data": {"location": "rivendell"}},
		{"type": QuestResource.ObjectiveType.VISIT_LOCATION, "target": "Discover Mordor", "current": 0, "required": 1, "data": {"location": "mordor"}}
	]
	explorer_quest.xp_reward = 2000
	explorer_quest.gold_reward = 800
	explorer_quest.item_rewards = ["explorer_cloak"]
	quests.append(explorer_quest)
	
	# Quest 11: Waypoint Master
	var waypoint_quest = QuestResource.new()
	waypoint_quest.quest_id = "waypoint_master"
	waypoint_quest.quest_name = "Waypoint Master"
	waypoint_quest.description = "Unlock all fast travel waypoints across Middle-earth."
	waypoint_quest.level_requirement = 1
	waypoint_quest.objectives = [
		{"type": QuestResource.ObjectiveType.COLLECT_ITEMS, "target": "Unlock Waypoints", "current": 0, "required": 6, "data": {"item_id": "waypoint_unlock"}}
	]
	waypoint_quest.xp_reward = 1200
	waypoint_quest.gold_reward = 400
	waypoint_quest.item_rewards = ["travel_cloak"]
	quests.append(waypoint_quest)
	
	# Quest 12: Friend to All
	var friend_quest = QuestResource.new()
	friend_quest.quest_id = "friend_to_all"
	friend_quest.quest_name = "Friend to All"
	friend_quest.description = "Gain the friendship of all major factions in Middle-earth."
	friend_quest.level_requirement = 1
	friend_quest.objectives = [
		{"type": QuestResource.ObjectiveType.TALK_TO_NPC, "target": "Reach Friendly with Shire Hobbits", "current": 0, "required": 1, "data": {"npc_id": "faction_check"}},
		{"type": QuestResource.ObjectiveType.TALK_TO_NPC, "target": "Reach Friendly with Rohirrim", "current": 0, "required": 1, "data": {"npc_id": "faction_check"}},
		{"type": QuestResource.ObjectiveType.TALK_TO_NPC, "target": "Reach Friendly with Rivendell Elves", "current": 0, "required": 1, "data": {"npc_id": "faction_check"}}
	]
	friend_quest.xp_reward = 2500
	friend_quest.gold_reward = 1000
	friend_quest.item_rewards = ["diplomacy_ring"]
	quests.append(friend_quest)
	
	return quests
