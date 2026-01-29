extends Node

## Sample Factions Data
## Defines the sample factions for reputation system

const FactionResource = preload("res://scripts/resources/faction_resource.gd")

static func create_sample_factions() -> Array[FactionResource]:
	"""Create sample factions for reputation tracking"""
	var factions: Array[FactionResource] = []
	
	# Faction 1: Hobbits of the Shire
	var hobbits = FactionResource.new(
		"shire_hobbits",
		"Hobbits of the Shire",
		"The peaceful folk of the Shire, who value comfort, good food, and friendly neighbors."
	)
	hobbits.current_reputation = 100  # Start friendly with hobbits
	hobbits.update_reputation_tier()
	factions.append(hobbits)
	
	# Faction 2: Rohirrim
	var rohirrim = FactionResource.new(
		"rohirrim",
		"The Rohirrim",
		"The horse-lords of Rohan, proud warriors who defend their lands with honor and courage."
	)
	rohirrim.current_reputation = 0  # Start neutral
	rohirrim.update_reputation_tier()
	factions.append(rohirrim)
	
	# Faction 3: Elves of Rivendell
	var elves = FactionResource.new(
		"rivendell_elves",
		"Elves of Rivendell",
		"The wise and ancient elves of Rivendell, keepers of lore and masters of magic."
	)
	elves.current_reputation = 50  # Start slightly friendly
	elves.update_reputation_tier()
	factions.append(elves)
	
	# Faction 4: Rangers of the North
	var rangers = FactionResource.new(
		"rangers_north",
		"Rangers of the North",
		"Secretive protectors of the wild lands, skilled trackers and warriors led by Aragorn."
	)
	rangers.current_reputation = 0  # Start neutral
	rangers.update_reputation_tier()
	factions.append(rangers)
	
	# Faction 5: Dwarves of Erebor
	var dwarves = FactionResource.new(
		"dwarves_erebor",
		"Dwarves of Erebor",
		"The sturdy dwarves who reclaimed the Lonely Mountain, master craftsmen and fierce warriors."
	)
	dwarves.current_reputation = 0  # Start neutral
	dwarves.update_reputation_tier()
	factions.append(dwarves)
	
	# Faction 6: Forces of Mordor (Enemy)
	var mordor = FactionResource.new(
		"forces_mordor",
		"Forces of Mordor",
		"The dark armies of Sauron, orcs, trolls, and worse creatures that threaten all of Middle-earth."
	)
	mordor.current_reputation = -1000  # Start hostile
	mordor.update_reputation_tier()
	factions.append(mordor)
	
	return factions
