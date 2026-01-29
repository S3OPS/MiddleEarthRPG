extends Node

## Sample Seasonal Events Data
## Defines seasonal and limited-time events for Phase 7

const SeasonalEventResource = preload("res://scripts/resources/seasonal_event_resource.gd")

static func create_sample_events() -> Array[SeasonalEventResource]:
	"""Create sample seasonal events"""
	var events: Array[SeasonalEventResource] = []
	
	# ========================================
	# SEASONAL EVENTS
	# ========================================
	
	# Spring Festival
	var spring_festival = SeasonalEventResource.new(
		"spring_festival",
		"Spring Festival in the Shire",
		"seasonal"
	)
	spring_festival.description = "Celebrate spring with the hobbits! Special quests, bonus rewards, and festive decorations."
	spring_festival.start_date = "2026-03-01"
	spring_festival.end_date = "2026-03-31"
	spring_festival.duration_days = 30
	spring_festival.bonus_xp_multiplier = 1.5
	spring_festival.bonus_gold_multiplier = 1.25
	spring_festival.event_quests = ["plant_party_tree", "spring_cleaning", "egg_hunt"]
	spring_festival.exclusive_items = ["spring_crown", "flower_wreath", "festival_lantern"]
	spring_festival.repeatable = true
	events.append(spring_festival)
	
	# Summer Solstice
	var summer_solstice = SeasonalEventResource.new(
		"summer_solstice",
		"Midsummer Night's Dream",
		"seasonal"
	)
	summer_solstice.description = "Join the elves in celebrating the longest day of the year with magic and wonder."
	summer_solstice.start_date = "2026-06-15"
	summer_solstice.end_date = "2026-06-30"
	summer_solstice.duration_days = 15
	summer_solstice.bonus_xp_multiplier = 2.0
	summer_solstice.bonus_gold_multiplier = 1.5
	summer_solstice.event_quests = ["elven_celebration", "starlight_dance", "magical_fireworks"]
	summer_solstice.exclusive_items = ["summer_robe", "starlight_staff", "moonstone_pendant"]
	summer_solstice.repeatable = true
	events.append(summer_solstice)
	
	# Harvest Festival
	var harvest_festival = SeasonalEventResource.new(
		"harvest_festival",
		"Harvest Festival",
		"seasonal"
	)
	harvest_festival.description = "Help gather the autumn harvest and celebrate the bounty with feasting and games."
	harvest_festival.start_date = "2026-09-01"
	harvest_festival.end_date = "2026-09-30"
	harvest_festival.duration_days = 30
	harvest_festival.bonus_xp_multiplier = 1.5
	harvest_festival.bonus_gold_multiplier = 1.5
	harvest_festival.event_quests = ["great_harvest", "pie_contest", "corn_maze"]
	harvest_festival.exclusive_items = ["harvest_basket", "golden_sickle", "autumn_cloak"]
	harvest_festival.repeatable = true
	events.append(harvest_festival)
	
	# Winter Solstice
	var winter_solstice = SeasonalEventResource.new(
		"winter_solstice",
		"Winter's Eve Celebration",
		"seasonal"
	)
	winter_solstice.description = "Brave the cold and celebrate the winter solstice with warmth, light, and friendship."
	winter_solstice.start_date = "2026-12-15"
	winter_solstice.end_date = "2026-12-31"
	winter_solstice.duration_days = 16
	winter_solstice.bonus_xp_multiplier = 2.0
	winter_solstice.bonus_gold_multiplier = 2.0
	winter_solstice.event_quests = ["light_the_beacons", "gift_exchange", "winter_feast"]
	winter_solstice.exclusive_items = ["winter_cape", "frost_blade", "yule_log"]
	winter_solstice.repeatable = true
	events.append(winter_solstice)
	
	# ========================================
	# HOLIDAY EVENTS
	# ========================================
	
	# Bilbo's Birthday
	var bilbos_birthday = SeasonalEventResource.new(
		"bilbos_birthday",
		"Bilbo's Birthday Party",
		"holiday"
	)
	bilbos_birthday.description = "Celebrate Bilbo's eleventy-first birthday with fireworks, food, and grand adventures!"
	bilbos_birthday.start_date = "2026-09-22"
	bilbos_birthday.end_date = "2026-09-22"
	bilbos_birthday.duration_days = 1
	bilbos_birthday.bonus_xp_multiplier = 3.0
	bilbos_birthday.bonus_gold_multiplier = 2.0
	bilbos_birthday.event_quests = ["party_preparations", "gandalf_fireworks", "unexpected_party"]
	bilbos_birthday.exclusive_items = ["party_invitation", "birthday_cake", "special_fireworks"]
	bilbos_birthday.repeatable = true
	events.append(bilbos_birthday)
	
	# ========================================
	# LIMITED TIME EVENTS
	# ========================================
	
	# Dragon Attack
	var dragon_attack = SeasonalEventResource.new(
		"dragon_attack",
		"Smaug's Revenge",
		"limited_time"
	)
	dragon_attack.description = "A dragon threatens the land! Band together to defend against this terrible threat."
	dragon_attack.required_level = 8
	dragon_attack.duration_days = 7
	dragon_attack.bonus_xp_multiplier = 2.5
	dragon_attack.bonus_gold_multiplier = 3.0
	dragon_attack.event_quests = ["dragon_sighting", "defend_village", "slay_the_dragon"]
	dragon_attack.exclusive_items = ["dragon_scale_armor", "dragon_slayer_sword", "dragon_hoard_key"]
	dragon_attack.repeatable = false
	events.append(dragon_attack)
	
	# Orc Invasion
	var orc_invasion = SeasonalEventResource.new(
		"orc_invasion",
		"The Black Tide",
		"limited_time"
	)
	orc_invasion.description = "Mordor's armies march! Join the defense and push back the darkness."
	orc_invasion.required_level = 10
	orc_invasion.duration_days = 14
	orc_invasion.bonus_xp_multiplier = 2.0
	orc_invasion.bonus_gold_multiplier = 2.5
	orc_invasion.event_quests = ["defend_the_walls", "counter_attack", "heroes_stand"]
	orc_invasion.exclusive_items = ["defender_shield", "heroes_banner", "orc_slayer_blade"]
	orc_invasion.repeatable = false
	events.append(orc_invasion)
	
	return events
