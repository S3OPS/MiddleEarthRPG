extends Node
## Centralized game balance constants
## Ported from Unity's GameConstants.cs
## Modify these values to tune gameplay without changing code

# ========================================
# COMBAT SETTINGS
# ========================================

## Base chance for critical hit (15%)
const CRITICAL_HIT_BASE_CHANCE: float = 0.15

## Damage multiplier for critical hits (2x)
const CRITICAL_HIT_DAMAGE_MULTIPLIER: float = 2.0

## Strength stat contribution to base damage per point
const STRENGTH_DAMAGE_MULTIPLIER: float = 2.0

## Combo damage increase per hit (20%)
const COMBO_DAMAGE_BONUS: float = 0.2

## Stamina cost for special ability
const SPECIAL_ABILITY_STAMINA_COST: float = 30.0

## Attack cooldown in seconds
const ATTACK_COOLDOWN: float = 0.5

## Special ability cooldown in seconds
const SPECIAL_COOLDOWN: float = 5.0

## Attack range in units (raycast distance)
const ATTACK_RANGE: float = 3.0

## Special ability AOE radius in units
const SPECIAL_AOE_RADIUS: float = 4.0


# ========================================
# ENEMY AI SETTINGS
# ========================================

## Health percentage when enemy starts fleeing (20%)
const ENEMY_FLEE_HEALTH_PERCENT: float = 0.2

## Enemy movement speed in units per second
const ENEMY_MOVE_SPEED: float = 2.0

## Enemy patrol wander distance in units
const ENEMY_WANDER_DISTANCE: float = 5.0

## Enemy player detection range in units
const ENEMY_DETECTION_RANGE: float = 10.0

## Enemy attack range in units
const ENEMY_ATTACK_RANGE: float = 5.0

## Enemy attack cooldown in seconds
const ENEMY_ATTACK_COOLDOWN: float = 2.0

## Enemy wander interval in seconds
const ENEMY_WANDER_INTERVAL: float = 3.0

## Enemy flee speed multiplier
const ENEMY_FLEE_SPEED_MULTIPLIER: float = 1.5

## Enemy patrol speed multiplier
const ENEMY_PATROL_SPEED_MULTIPLIER: float = 0.5

## Enemy flee distance
const ENEMY_FLEE_DISTANCE: float = 10.0

## Enemy waypoint arrival threshold
const ENEMY_WAYPOINT_ARRIVAL_THRESHOLD: float = 1.0

## Enemy death fade duration
const ENEMY_DEATH_FADE_DURATION: float = 0.5


# ========================================
# PLAYER SETTINGS
# ========================================

## Player walk speed in units per second
const PLAYER_WALK_SPEED: float = 5.0

## Player sprint speed multiplier
const PLAYER_SPRINT_MULTIPLIER: float = 1.5

## Player jump velocity
const PLAYER_JUMP_VELOCITY: float = 4.5

## Camera mouse sensitivity
const CAMERA_MOUSE_SENSITIVITY: float = 0.003

## Stamina regeneration per second
const STAMINA_REGEN_RATE: float = 10.0

## Stamina regeneration delay after use (seconds)
const STAMINA_REGEN_DELAY: float = 2.0

## Stamina drain rate while sprinting
const SPRINT_STAMINA_DRAIN_RATE: float = 5.0

## Player base attack damage
const PLAYER_BASE_ATTACK_DAMAGE: float = 10.0

## Player special attack base damage
const PLAYER_SPECIAL_ATTACK_DAMAGE: float = 30.0


# ========================================
# PROGRESSION SETTINGS
# ========================================

## XP scaling factor for level up requirements
const XP_SCALING_FACTOR: float = 1.5

## Health bonus per level up
const LEVELUP_HEALTH_BONUS: float = 20.0

## Stamina bonus per level up
const LEVELUP_STAMINA_BONUS: float = 10.0

## Stat increase per level (strength, wisdom, agility)
const LEVELUP_STAT_INCREASE: int = 2


# ========================================
# PARTICLE VFX SETTINGS
# ========================================

## Particle count for normal hit effects
const NORMAL_HIT_PARTICLE_COUNT: int = 8

## Particle count for critical hit effects
const CRITICAL_HIT_PARTICLE_COUNT: int = 15

## Particle count for special ability effects
const SPECIAL_PARTICLE_COUNT: int = 30

## Particle count for level-up effect
const LEVELUP_PARTICLE_COUNT: int = 20

## Particle count for treasure effect
const TREASURE_PARTICLE_COUNT: int = 12

## Particle count for quest complete effect
const QUEST_PARTICLE_COUNT: int = 8

## Particle lifetime for normal hit effects in seconds
const PARTICLE_LIFETIME: float = 0.5

## Particle lifetime for special ability effects in seconds
const SPECIAL_PARTICLE_LIFETIME: float = 0.8

## Particle lifetime for level-up effects in seconds
const LEVELUP_PARTICLE_LIFETIME: float = 1.5


# ========================================
# UI SETTINGS
# ========================================

## Notification display time in seconds
const NOTIFICATION_DURATION: float = 3.0

## Quest notification display time in seconds
const QUEST_NOTIFICATION_DURATION: float = 5.0

## Achievement notification display time in seconds
const ACHIEVEMENT_NOTIFICATION_DURATION: float = 4.0


# ========================================
# WORLD SETTINGS
# ========================================

## Day/night cycle duration in seconds (24 minutes = 1 day)
const DAY_NIGHT_CYCLE_DURATION: float = 1440.0

## Weather change interval in seconds
const WEATHER_CHANGE_INTERVAL: float = 300.0

## Treasure chest respawn time in seconds
const TREASURE_RESPAWN_TIME: float = 600.0


# ========================================
# SAVE SYSTEM SETTINGS
# ========================================

## Number of save slots available
const MAX_SAVE_SLOTS: int = 5

## Auto-save interval in seconds
const AUTO_SAVE_INTERVAL: float = 300.0


# ========================================
# HELPER FUNCTIONS
# ========================================

## Get the sprint speed based on walk speed
func get_sprint_speed() -> float:
	return PLAYER_WALK_SPEED * PLAYER_SPRINT_MULTIPLIER

## Calculate damage with strength modifier
func calculate_damage(base_damage: float, strength: int) -> float:
	return base_damage + (strength * STRENGTH_DAMAGE_MULTIPLIER)

## Check if attack is critical hit
func is_critical_hit() -> bool:
	return randf() < CRITICAL_HIT_BASE_CHANCE

## Apply critical hit multiplier to damage
func apply_critical_multiplier(damage: float) -> float:
	return damage * CRITICAL_HIT_DAMAGE_MULTIPLIER
