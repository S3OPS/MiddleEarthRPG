extends Node

## Sample dialogues for testing the dialogue system

static func create_sample_dialogues() -> Dictionary:
	var dialogues: Dictionary = {}
	
	# Gandalf dialogue
	var gandalf_dialogue = DialogueResource.new()
	gandalf_dialogue.dialogue_id = "gandalf_greeting"
	gandalf_dialogue.npc_id = "gandalf"
	gandalf_dialogue.npc_name = "Gandalf the Grey"
	gandalf_dialogue.lines_data = [
		{
			"speaker": "Gandalf the Grey",
			"text": "Ah, a new adventurer! Welcome to Middle-earth, young one.",
			"next": 1
		},
		{
			"speaker": "Gandalf the Grey",
			"text": "The darkness grows in the East. We need brave souls like you to stand against it.",
			"next": 2
		},
		{
			"speaker": "Gandalf the Grey",
			"text": "Will you help us in our quest to protect these lands?",
			"choices": [
				{"text": "I will help in any way I can!", "next": 3},
				{"text": "What's in it for me?", "next": 4},
				{"text": "I'm not sure I'm ready yet...", "next": 5}
			]
		},
		{
			"speaker": "Gandalf the Grey",
			"text": "Excellent! Your courage does you credit. Seek out the orcs that threaten our borders and defeat them.",
			"next": -1
		},
		{
			"speaker": "Gandalf the Grey",
			"text": "Ha! A pragmatic one, I see. There will be gold and glory aplenty for those who succeed.",
			"next": 3
		},
		{
			"speaker": "Gandalf the Grey",
			"text": "That's alright. Take your time to prepare. When you're ready, return to me.",
			"next": -1
		}
	]
	gandalf_dialogue._parse_lines_data()
	dialogues[gandalf_dialogue.dialogue_id] = gandalf_dialogue
	
	# Legolas dialogue
	var legolas_dialogue = DialogueResource.new()
	legolas_dialogue.dialogue_id = "legolas_greeting"
	legolas_dialogue.npc_id = "legolas"
	legolas_dialogue.npc_name = "Legolas"
	legolas_dialogue.lines_data = [
		{
			"speaker": "Legolas",
			"text": "Greetings, traveler. I am Legolas of the Woodland Realm.",
			"next": 1
		},
		{
			"speaker": "Legolas",
			"text": "The forest whispers of growing danger. Have you seen any orcs in your travels?",
			"choices": [
				{"text": "Yes, I've encountered several.", "next": 2},
				{"text": "Not yet, but I'll keep watch.", "next": 3}
			]
		},
		{
			"speaker": "Legolas",
			"text": "Good. Stay vigilant. The orcs grow bolder with each passing day.",
			"next": -1
		},
		{
			"speaker": "Legolas",
			"text": "That is wise. May your eyes be keen and your arrows true.",
			"next": -1
		}
	]
	legolas_dialogue._parse_lines_data()
	dialogues[legolas_dialogue.dialogue_id] = legolas_dialogue
	
	# Gimli dialogue
	var gimli_dialogue = DialogueResource.new()
	gimli_dialogue.dialogue_id = "gimli_greeting"
	gimli_dialogue.npc_id = "gimli"
	gimli_dialogue.npc_name = "Gimli, son of Glóin"
	gimli_dialogue.lines_data = [
		{
			"speaker": "Gimli",
			"text": "Aye! Another warrior joins the fray! Welcome, friend!",
			"next": 1
		},
		{
			"speaker": "Gimli",
			"text": "I've been counting my orc kills. Forty-two so far! Can you beat that?",
			"choices": [
				{"text": "Challenge accepted!", "next": 2},
				{"text": "That's... a lot of orcs.", "next": 3}
			]
		},
		{
			"speaker": "Gimli",
			"text": "Ha! That's the spirit! May your axe stay sharp!",
			"next": -1
		},
		{
			"speaker": "Gimli",
			"text": "Aye, and there's plenty more where they came from, unfortunately.",
			"next": -1
		}
	]
	gimli_dialogue._parse_lines_data()
	dialogues[gimli_dialogue.dialogue_id] = gimli_dialogue
	
	# Merchant dialogue
	var merchant_dialogue = DialogueResource.new()
	merchant_dialogue.dialogue_id = "merchant_greeting"
	merchant_dialogue.npc_id = "merchant"
	merchant_dialogue.npc_name = "Traveling Merchant"
	merchant_dialogue.lines_data = [
		{
			"speaker": "Traveling Merchant",
			"text": "Welcome, welcome! Looking to buy or sell?",
			"choices": [
				{"text": "Show me your wares.", "next": 1},
				{"text": "Just browsing.", "next": 2},
				{"text": "Goodbye.", "next": -1}
			]
		},
		{
			"speaker": "Traveling Merchant",
			"text": "Excellent! I have potions, weapons, armor... all at reasonable prices!",
			"next": -1
		},
		{
			"speaker": "Traveling Merchant",
			"text": "Take your time! Let me know if you need anything.",
			"next": -1
		}
	]
	merchant_dialogue._parse_lines_data()
	dialogues[merchant_dialogue.dialogue_id] = merchant_dialogue
	
	# Tutorial NPC
	var tutorial_dialogue = DialogueResource.new()
	tutorial_dialogue.dialogue_id = "tutorial_guide"
	tutorial_dialogue.npc_id = "guide"
	tutorial_dialogue.npc_name = "Friendly Guide"
	tutorial_dialogue.lines_data = [
		{
			"speaker": "Friendly Guide",
			"text": "Hello there! New to these parts?",
			"next": 1
		},
		{
			"speaker": "Friendly Guide",
			"text": "Let me give you some tips: Use WASD to move, Shift to sprint, and mouse to look around.",
			"next": 2
		},
		{
			"speaker": "Friendly Guide",
			"text": "For combat, left-click to attack and right-click for a special AOE attack.",
			"next": 3
		},
		{
			"speaker": "Friendly Guide",
			"text": "Press I for inventory, J for quests, C for character sheet, and M for the map.",
			"next": 4
		},
		{
			"speaker": "Friendly Guide",
			"text": "Good luck on your adventure! May your blade stay sharp and your heart stay true!",
			"next": -1
		}
	]
	tutorial_dialogue._parse_lines_data()
	dialogues[tutorial_dialogue.dialogue_id] = tutorial_dialogue
	
	return dialogues
