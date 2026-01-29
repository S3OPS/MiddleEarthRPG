extends Node

## Sample Companions Data
## Defines companion characters for Phase 6

const CompanionResource = preload("res://scripts/resources/companion_resource.gd")

static func create_sample_companions() -> Array[CompanionResource]:
	"""Create sample companions"""
	var companions: Array[CompanionResource] = []
	
	# ========================================
	# WARRIOR COMPANIONS
	# ========================================
	
	var boromir = CompanionResource.new(
		"boromir",
		"Boromir",
		"warrior"
	)
	boromir.description = "Captain of Gondor, brave and noble, but tempted by the Ring."
	boromir.level = 5
	boromir.max_health = 150
	boromir.attack = 15
	boromir.defense = 12
	boromir.hire_cost = 500
	boromir.daily_cost = 20
	boromir.required_level = 5
	boromir.abilities = ["shield_bash", "battle_cry", "last_stand"]
	companions.append(boromir)
	
	var gimli = CompanionResource.new(
		"gimli_companion",
		"Gimli",
		"warrior"
	)
	gimli.description = "Dwarf warrior of Erebor, fierce in battle and loyal to his friends."
	gimli.level = 6
	gimli.max_health = 180
	gimli.attack = 18
	gimli.defense = 15
	gimli.hire_cost = 750
	gimli.daily_cost = 30
	gimli.required_level = 6
	gimli.required_quest = "council_of_elrond"
	gimli.abilities = ["axe_throw", "stone_skin", "cleave"]
	companions.append(gimli)
	
	# ========================================
	# RANGER COMPANIONS
	# ========================================
	
	var legolas_companion = CompanionResource.new(
		"legolas_companion",
		"Legolas",
		"ranger"
	)
	legolas_companion.description = "Elven archer of Mirkwood, deadly accurate and incredibly swift."
	legolas_companion.level = 7
	legolas_companion.max_health = 120
	legolas_companion.attack = 20
	legolas_companion.defense = 8
	legolas_companion.hire_cost = 1000
	legolas_companion.daily_cost = 40
	legolas_companion.required_level = 7
	legolas_companion.required_quest = "council_of_elrond"
	legolas_companion.abilities = ["rapid_fire", "piercing_shot", "elven_reflexes"]
	companions.append(legolas_companion)
	
	var aragorn = CompanionResource.new(
		"aragorn",
		"Aragorn",
		"ranger"
	)
	aragorn.description = "Heir of Isildur, Ranger of the North, destined to be King of Gondor."
	aragorn.level = 10
	aragorn.max_health = 200
	aragorn.attack = 22
	aragorn.defense = 14
	aragorn.hire_cost = 2000
	aragorn.daily_cost = 75
	aragorn.required_level = 10
	aragorn.required_quest = "reach_rohan"
	aragorn.abilities = ["kings_valor", "andúril_strike", "leadership"]
	companions.append(aragorn)
	
	# ========================================
	# MAGE COMPANIONS
	# ========================================
	
	var gandalf_companion = CompanionResource.new(
		"gandalf_companion",
		"Gandalf the Grey",
		"mage"
	)
	gandalf_companion.description = "Wise wizard of the Istari, wielder of Narya and friend to all free peoples."
	gandalf_companion.level = 15
	gandalf_companion.max_health = 150
	gandalf_companion.attack = 25
	gandalf_companion.defense = 10
	gandalf_companion.hire_cost = 5000
	gandalf_companion.daily_cost = 100
	gandalf_companion.required_level = 8
	gandalf_companion.required_quest = "council_of_elrond"
	gandalf_companion.abilities = ["staff_strike", "fireworks", "you_shall_not_pass"]
	companions.append(gandalf_companion)
	
	var saruman = CompanionResource.new(
		"saruman",
		"Saruman the White",
		"mage"
	)
	saruman.description = "Leader of the Istari, powerful but corrupted by desire for the Ring."
	saruman.level = 20
	saruman.max_health = 180
	saruman.attack = 30
	saruman.defense = 12
	saruman.hire_cost = 10000
	saruman.daily_cost = 200
	saruman.required_level = 15
	saruman.required_quest = "shadow_of_mordor"
	saruman.abilities = ["voice_of_saruman", "mind_control", "lightning_bolt"]
	companions.append(saruman)
	
	return companions
