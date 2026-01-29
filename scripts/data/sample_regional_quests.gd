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
	var hobbiton_quest = QuestResource.new(
		"trouble_at_hobbiton",
		"Trouble at Hobbiton",
		"The hobbits of Hobbiton are troubled by wolves near the village. Help them by clearing the threat.",
		1
	)
	hobbiton_quest.add_objective("kill_enemies", "Defeat Wolves", 5, {"enemy_type": "Wolf"})
	hobbiton_quest.add_objective("talk_to_npc", "Report to the Mayor", 1, {"npc_id": "mayor_hobbiton"})
	hobbiton_quest.set_rewards(100, 50)
	hobbiton_quest.add_item_reward("healing_potion", 3)
	hobbiton_quest.faction_rewards["shire_hobbits"] = 100
	quests.append(hobbiton_quest)
	
	# Quest 2: The Missing Pipe
	var pipe_quest = QuestResource.new(
		"missing_pipe",
		"The Missing Pipe",
		"Old Toby has lost his favorite pipe somewhere in the Shire. Help him find it.",
		1
	)
	pipe_quest.add_objective("collect_items", "Find Old Toby's Pipe", 1, {"item_id": "old_pipe"})
	pipe_quest.add_objective("talk_to_npc", "Return the Pipe to Old Toby", 1, {"npc_id": "old_toby"})
	pipe_quest.set_rewards(75, 25)
	pipe_quest.faction_rewards["shire_hobbits"] = 50
	quests.append(pipe_quest)
	
	# ========================================
	# ROHAN QUESTS
	# ========================================
	
	# Quest 3: Reach Rohan
	var reach_rohan = QuestResource.new(
		"reach_rohan",
		"Journey to Rohan",
		"Travel to the plains of Rohan and meet with the horse-lords. A dangerous journey lies ahead.",
		5
	)
	reach_rohan.add_objective("visit_location", "Reach the Plains of Rohan", 1, {"location": "rohan_border"})
	reach_rohan.add_objective("talk_to_npc", "Speak with a Rohirrim Scout", 1, {"npc_id": "rohirrim_scout"})
	reach_rohan.set_rewards(500, 200)
	reach_rohan.add_item_reward("rohan_banner", 1)
	reach_rohan.faction_rewards["rohirrim"] = 200
	quests.append(reach_rohan)
	
	# Quest 4: Defense of Rohan
	var defense_rohan = QuestResource.new(
		"defense_of_rohan",
		"Defense of Rohan",
		"Orcs are raiding the settlements of Rohan. Help the Rohirrim defend their lands.",
		6
	)
	defense_rohan.add_prerequisite("reach_rohan")
	defense_rohan.add_objective("kill_enemies", "Defeat Orc Raiders", 15, {"enemy_type": "Orc"})
	defense_rohan.add_objective("visit_location", "Reach Helm's Deep", 1, {"location": "helms_deep"})
	defense_rohan.set_rewards(800, 300)
	defense_rohan.add_item_reward("rohirrim_spear", 1)
	defense_rohan.faction_rewards["rohirrim"] = 500
	quests.append(defense_rohan)
	
	# Quest 5: Riders of Rohan
	var riders_quest = QuestResource.new(
		"riders_of_rohan",
		"Riders of Rohan",
		"Prove your worth to the Rohirrim by completing their trials of horsemanship and combat.",
		7
	)
	riders_quest.add_prerequisite("defense_of_rohan")
	riders_quest.add_objective("kill_enemies", "Defeat Wargs", 10, {"enemy_type": "Warg"})
	riders_quest.add_objective("collect_items", "Collect Horse Medallions", 5, {"item_id": "horse_medallion"})
	riders_quest.set_rewards(1000, 400)
	riders_quest.add_item_reward("rohirrim_armor", 1)
	riders_quest.faction_rewards["rohirrim"] = 750
	quests.append(riders_quest)
	
	# ========================================
	# RIVENDELL QUESTS
	# ========================================
	
	# Quest 6: Council of Elrond
	var council_quest = QuestResource.new(
		"council_of_elrond",
		"The Council of Elrond",
		"You have been summoned to Rivendell for a council of great importance. The fate of Middle-earth hangs in the balance.",
		3
	)
	council_quest.add_objective("visit_location", "Reach Rivendell", 1, {"location": "rivendell"})
	council_quest.add_objective("talk_to_npc", "Attend the Council", 1, {"npc_id": "elrond"})
	council_quest.set_rewards(400, 150)
	council_quest.faction_rewards["rivendell_elves"] = 300
	quests.append(council_quest)
	
	# Quest 7: Elven Wisdom
	var elven_wisdom = QuestResource.new(
		"elven_wisdom",
		"Elven Wisdom",
		"The elves of Rivendell offer to teach you their ancient knowledge. Complete their trials to prove yourself worthy.",
		4
	)
	elven_wisdom.add_prerequisite("council_of_elrond")
	elven_wisdom.add_objective("collect_items", "Gather Ancient Texts", 3, {"item_id": "ancient_text"})
	elven_wisdom.add_objective("kill_enemies", "Defeat Corrupted Spirits", 8, {"enemy_type": "Spirit"})
	elven_wisdom.set_rewards(600, 250)
	elven_wisdom.add_item_reward("elven_blade", 1)
	elven_wisdom.faction_rewards["rivendell_elves"] = 500
	quests.append(elven_wisdom)
	
	# ========================================
	# MORDOR QUESTS
	# ========================================
	
	# Quest 8: Shadow of Mordor
	var shadow_quest = QuestResource.new(
		"shadow_of_mordor",
		"Into the Shadow",
		"Dark forces stir in Mordor. Scout the borders and gather intelligence on Sauron's armies.",
		10
	)
	shadow_quest.add_objective("visit_location", "Reach the Black Gate", 1, {"location": "black_gate"})
	shadow_quest.add_objective("kill_enemies", "Defeat Mordor Orcs", 20, {"enemy_type": "MordorOrc"})
	shadow_quest.set_rewards(1500, 500)
	shadow_quest.add_item_reward("scouting_report", 1)
	shadow_quest.faction_rewards["rangers_north"] = 800
	quests.append(shadow_quest)
	
	# Quest 9: The Ring's Pull
	var ring_quest = QuestResource.new(
		"rings_pull",
		"The Ring's Pull",
		"The One Ring's influence grows stronger as you approach Mount Doom. You must resist its corruption.",
		12
	)
	ring_quest.add_prerequisite("shadow_of_mordor")
	ring_quest.add_objective("visit_location", "Climb Mount Doom", 1, {"location": "mount_doom"})
	ring_quest.add_objective("kill_enemies", "Defeat the Nazgûl", 1, {"enemy_type": "Nazgul"})
	ring_quest.set_rewards(3000, 1000)
	ring_quest.add_item_reward("mithril_coat", 1)
	quests.append(ring_quest)
	
	# ========================================
	# EXPLORATION QUESTS
	# ========================================
	
	# Quest 10: Explorer of Middle-earth
	var explorer_quest = QuestResource.new(
		"explorer_middle_earth",
		"Explorer of Middle-earth",
		"Discover all the major regions of Middle-earth and become a renowned explorer.",
		1
	)
	explorer_quest.add_objective("visit_location", "Discover The Shire", 1, {"location": "the_shire"})
	explorer_quest.add_objective("visit_location", "Discover Rohan", 1, {"location": "rohan"})
	explorer_quest.add_objective("visit_location", "Discover Rivendell", 1, {"location": "rivendell"})
	explorer_quest.add_objective("visit_location", "Discover Mordor", 1, {"location": "mordor"})
	explorer_quest.set_rewards(2000, 800)
	explorer_quest.add_item_reward("explorer_cloak", 1)
	quests.append(explorer_quest)
	
	# Quest 11: Waypoint Master
	var waypoint_quest = QuestResource.new(
		"waypoint_master",
		"Waypoint Master",
		"Unlock all fast travel waypoints across Middle-earth.",
		1
	)
	waypoint_quest.add_objective("collect_items", "Unlock Waypoints", 6, {"item_id": "waypoint_unlock"})
	waypoint_quest.set_rewards(1200, 400)
	waypoint_quest.add_item_reward("travel_cloak", 1)
	quests.append(waypoint_quest)
	
	# Quest 12: Friend to All
	var friend_quest = QuestResource.new(
		"friend_to_all",
		"Friend to All",
		"Gain the friendship of all major factions in Middle-earth.",
		1
	)
	friend_quest.add_objective("talk_to_npc", "Reach Friendly with Shire Hobbits", 1, {"npc_id": "faction_check"})
	friend_quest.add_objective("talk_to_npc", "Reach Friendly with Rohirrim", 1, {"npc_id": "faction_check"})
	friend_quest.add_objective("talk_to_npc", "Reach Friendly with Rivendell Elves", 1, {"npc_id": "faction_check"})
	friend_quest.set_rewards(2500, 1000)
	friend_quest.add_item_reward("diplomacy_ring", 1)
	quests.append(friend_quest)
	
	return quests
