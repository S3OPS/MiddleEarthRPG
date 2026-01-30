extends Node

## Sample Specializations Data
## Defines combat specializations for Phase 6

const SpecializationRes = preload("res://scripts/resources/specialization_resource.gd")

static func create_sample_specializations() -> Array[SpecializationResource]:
	"""Create sample combat specializations"""
	var specializations: Array[SpecializationResource] = []
	
	# ========================================
	# WARRIOR SPECIALIZATION
	# ========================================
	
	var warrior = SpecializationRes.new(
		"warrior",
		"Warrior",
		"Masters of melee combat, warriors excel in close-quarters battle with heavy weapons and armor."
	)
	warrior.attack_bonus = 5
	warrior.defense_bonus = 3
	warrior.health_bonus = 50
	warrior.stamina_bonus = 20
	warrior.critical_chance_bonus = 0.05
	
	# Warrior abilities
	warrior.add_ability("shield_bash", 3)
	warrior.add_ability("whirlwind_attack", 5)
	warrior.add_ability("berserk_rage", 8)
	warrior.add_ability("last_stand", 10)
	
	specializations.append(warrior)
	
	# ========================================
	# RANGER SPECIALIZATION
	# ========================================
	
	var ranger = SpecializationRes.new(
		"ranger",
		"Ranger",
		"Swift and deadly, rangers are masters of ranged combat and wilderness survival."
	)
	ranger.attack_bonus = 4
	ranger.defense_bonus = 2
	ranger.health_bonus = 30
	ranger.stamina_bonus = 40
	ranger.critical_chance_bonus = 0.15
	
	# Ranger abilities
	ranger.add_ability("multishot", 3)
	ranger.add_ability("camouflage", 5)
	ranger.add_ability("hunters_mark", 8)
	ranger.add_ability("volley", 10)
	
	specializations.append(ranger)
	
	# ========================================
	# MAGE SPECIALIZATION
	# ========================================
	
	var mage = SpecializationRes.new(
		"mage",
		"Mage",
		"Wielders of arcane power, mages command devastating spells and mystical energies."
	)
	mage.attack_bonus = 6
	mage.defense_bonus = 1
	mage.health_bonus = 20
	mage.stamina_bonus = 60
	mage.critical_chance_bonus = 0.10
	
	# Mage abilities
	mage.add_ability("fireball", 3)
	mage.add_ability("frost_nova", 5)
	mage.add_ability("lightning_storm", 8)
	mage.add_ability("meteor", 10)
	
	specializations.append(mage)
	
	return specializations
