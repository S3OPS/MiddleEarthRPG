extends Resource
class_name FactionResource

## Faction Resource
## Defines a faction with reputation tracking

@export var faction_id: String = ""
@export var faction_name: String = ""
@export var description: String = ""
@export var current_reputation: int = 0
@export var reputation_tier: String = "neutral"  # hostile, unfriendly, neutral, friendly, honored, exalted

# Reputation thresholds
const REPUTATION_THRESHOLDS = {
	"hostile": -3000,
	"unfriendly": -500,
	"neutral": 0,
	"friendly": 1000,
	"honored": 3000,
	"exalted": 6000
}

func _init(
	p_id: String = "",
	p_name: String = "",
	p_desc: String = ""
):
	faction_id = p_id
	faction_name = p_name
	description = p_desc
	update_reputation_tier()

func add_reputation(amount: int) -> void:
	current_reputation += amount
	update_reputation_tier()

func update_reputation_tier() -> void:
	if current_reputation >= REPUTATION_THRESHOLDS["exalted"]:
		reputation_tier = "exalted"
	elif current_reputation >= REPUTATION_THRESHOLDS["honored"]:
		reputation_tier = "honored"
	elif current_reputation >= REPUTATION_THRESHOLDS["friendly"]:
		reputation_tier = "friendly"
	elif current_reputation >= REPUTATION_THRESHOLDS["neutral"]:
		reputation_tier = "neutral"
	elif current_reputation >= REPUTATION_THRESHOLDS["unfriendly"]:
		reputation_tier = "unfriendly"
	else:
		reputation_tier = "hostile"

func get_reputation_progress() -> float:
	# Returns 0.0-1.0 progress to next tier
	var tiers = ["hostile", "unfriendly", "neutral", "friendly", "honored", "exalted"]
	var current_index = tiers.find(reputation_tier)
	
	if current_index == tiers.size() - 1:
		return 1.0  # Max tier
	
	var current_threshold = REPUTATION_THRESHOLDS[reputation_tier]
	var next_threshold = REPUTATION_THRESHOLDS[tiers[current_index + 1]]
	var progress = float(current_reputation - current_threshold) / float(next_threshold - current_threshold)
	
	return clamp(progress, 0.0, 1.0)
