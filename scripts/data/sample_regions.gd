extends Node

## Sample Regions Data
## Defines the sample regions for the game world

const RegionResource = preload("res://scripts/resources/region_resource.gd")

static func create_sample_regions() -> Array[RegionResource]:
	"""Create sample regions for the game"""
	var regions: Array[RegionResource] = []
	
	# Region 1: The Shire
	var shire = RegionResource.new(
		"the_shire",
		"The Shire",
		"A peaceful land of rolling hills and comfortable hobbit-holes. The hobbits here are friendly folk who enjoy simple pleasures.",
		1
	)
	shire.climate = "temperate"
	shire.danger_level = 1
	shire.has_fast_travel = true
	shire.skybox_color = Color(0.4, 0.7, 0.9)  # Light blue
	shire.ambient_light = Color(1.0, 0.95, 0.8)  # Warm sunlight
	regions.append(shire)
	
	# Region 2: Rohan
	var rohan = RegionResource.new(
		"rohan",
		"The Plains of Rohan",
		"Vast grasslands where the horse-lords ride. The Rohirrim are proud warriors, masters of mounted combat.",
		5
	)
	rohan.climate = "temperate"
	rohan.danger_level = 3
	rohan.has_fast_travel = true
	rohan.skybox_color = Color(0.6, 0.8, 0.9)  # Clear blue
	rohan.ambient_light = Color(1.0, 0.9, 0.7)  # Golden sunlight
	regions.append(rohan)
	
	# Region 3: Mordor
	var mordor = RegionResource.new(
		"mordor",
		"The Land of Mordor",
		"A desolate wasteland shrouded in darkness. The domain of Sauron, where few dare to tread.",
		10
	)
	mordor.climate = "hot"
	mordor.danger_level = 5
	mordor.has_fast_travel = false  # Too dangerous for casual fast travel
	mordor.skybox_color = Color(0.3, 0.2, 0.2)  # Dark red
	mordor.ambient_light = Color(0.5, 0.3, 0.3)  # Dim red light
	mordor.fog_enabled = true
	mordor.fog_color = Color(0.4, 0.2, 0.2)
	regions.append(mordor)
	
	# Region 4: Rivendell
	var rivendell = RegionResource.new(
		"rivendell",
		"Rivendell",
		"The Last Homely House, a sanctuary of the elves. A place of wisdom, beauty, and ancient magic.",
		3
	)
	rivendell.climate = "mystical"
	rivendell.danger_level = 1
	rivendell.has_fast_travel = true
	rivendell.skybox_color = Color(0.5, 0.7, 1.0)  # Ethereal blue
	rivendell.ambient_light = Color(0.9, 0.95, 1.0)  # Cool magical light
	regions.append(rivendell)
	
	return regions
