extends Node
## Global event bus for signal-based communication
## Central hub for game-wide events using Godot's signal system
## Replaces Unity's EventBus pattern with type-safe signals
##
## Usage:
##   To emit: EventBus.player_level_up.emit(new_level)
##   To listen: EventBus.player_level_up.connect(_on_player_level_up)

# ========================================
# PLAYER SIGNALS
# ========================================

## Emitted when player health changes
## @param current_health: Current health value
## @param max_health: Maximum health value
signal player_health_changed(current_health: float, max_health: float)

## Emitted when player stamina changes
## @param current_stamina: Current stamina value
## @param max_stamina: Maximum stamina value
signal player_stamina_changed(current_stamina: float, max_stamina: float)

## Emitted when player gains experience
## @param amount: Amount of experience gained
## @param total: Total experience after gain
signal player_experience_gained(amount: int, total: int)

## Emitted when player levels up
## @param new_level: The new level reached
signal player_level_up(new_level: int)

## Emitted when player's stat increases
## @param stat_name: Name of the stat (strength, wisdom, agility)
## @param old_value: Previous value
## @param new_value: New value after increase
signal player_stat_increased(stat_name: String, old_value: int, new_value: int)

## Emitted when player dies
signal player_died()

## Emitted when player respawns
signal player_respawned()

## Emitted when player starts sprinting
signal player_sprint_started()

## Emitted when player stops sprinting
signal player_sprint_stopped()


# ========================================
# COMBAT SIGNALS
# ========================================

## Emitted when damage is dealt to any target
## @param target: The entity receiving damage
## @param amount: Damage amount
## @param is_critical: Whether it was a critical hit
signal damage_dealt(target: Node, amount: float, is_critical: bool)

## Emitted when an entity takes damage
## @param entity: The entity taking damage
## @param amount: Damage amount
## @param attacker: The entity dealing damage
signal damage_taken(entity: Node, amount: float, attacker: Node)

## Emitted when an enemy dies
## @param enemy: The enemy that died
## @param experience_reward: XP rewarded for the kill
signal enemy_killed(enemy: Node, experience_reward: int)

## Emitted when combat starts with an enemy
## @param enemy: The enemy engaging in combat
signal combat_started(enemy: Node)

## Emitted when all combat ends
signal combat_ended()

## Emitted when player performs special attack
## @param targets: Array of enemies hit
signal special_attack_used(targets: Array)


# ========================================
# QUEST SIGNALS
# ========================================

## Emitted when a quest is started
## @param quest_id: Unique quest identifier
## @param quest_name: Display name of the quest
signal quest_started(quest_id: String, quest_name: String)

## Emitted when a quest objective is updated
## @param quest_id: Quest identifier
## @param objective_index: Index of the objective
## @param completed: Whether objective is completed
signal quest_objective_updated(quest_id: String, objective_index: int, completed: bool)

## Emitted when a quest is completed
## @param quest_id: Quest identifier
## @param quest_name: Display name
## @param reward_xp: Experience points rewarded
signal quest_completed(quest_id: String, quest_name: String, reward_xp: int)

## Emitted when a quest is failed
## @param quest_id: Quest identifier
## @param quest_name: Display name
signal quest_failed(quest_id: String, quest_name: String)


# ========================================
# INVENTORY SIGNALS
# ========================================

## Emitted when an item is added to inventory
## @param item_id: Unique item identifier
## @param item_name: Display name of item
## @param quantity: Amount added
signal item_added(item_id: String, item_name: String, quantity: int)

## Emitted when an item is removed from inventory
## @param item_id: Item identifier
## @param quantity: Amount removed
signal item_removed(item_id: String, quantity: int)

## Emitted when an item is equipped
## @param item_id: Item identifier
## @param slot: Equipment slot name
signal item_equipped(item_id: String, slot: String)

## Emitted when an item is unequipped
## @param item_id: Item identifier
## @param slot: Equipment slot name
signal item_unequipped(item_id: String, slot: String)

## Emitted when inventory is full
signal inventory_full()


# ========================================
# UI SIGNALS
# ========================================

## Emitted when a notification should be displayed
## @param message: Notification text
## @param duration: How long to show (seconds)
## @param type: Notification type (info, warning, error, success)
signal notification_requested(message: String, duration: float, type: String)

## Emitted when UI panel should be toggled
## @param panel_name: Name of the panel
## @param is_visible: Whether to show or hide
signal ui_panel_toggled(panel_name: String, is_visible: bool)


# ========================================
# WORLD SIGNALS
# ========================================

## Emitted when time of day changes
## @param hour: Current hour (0-23)
## @param is_day: Whether it's daytime
signal time_of_day_changed(hour: int, is_day: bool)

## Emitted when weather changes
## @param weather_type: Type of weather (clear, rain, snow, fog)
signal weather_changed(weather_type: String)

## Emitted when player discovers a location
## @param location_name: Name of discovered location
signal location_discovered(location_name: String)

## Emitted when player enters a new zone
## @param zone_name: Name of the zone
signal zone_entered(zone_name: String)


# ========================================
# SAVE/LOAD SIGNALS
# ========================================

## Emitted when game is saved successfully
## @param slot_index: Save slot number
signal game_saved(slot_index: int)

## Emitted when game is loaded successfully
## @param slot_index: Save slot number
signal game_loaded(slot_index: int)

## Emitted when save operation fails
## @param error_message: Description of the error
signal save_failed(error_message: String)


# ========================================
# ACHIEVEMENT SIGNALS
# ========================================

## Emitted when an achievement is unlocked
## @param achievement_id: Unique achievement identifier
## @param achievement_name: Display name
signal achievement_unlocked(achievement_id: String, achievement_name: String)


# ========================================
# PERFORMANCE SIGNALS
# ========================================

## Emitted when performance drops below threshold
## @param fps: Current frames per second
signal performance_warning(fps: float)

## Emitted when object pool is exhausted
## @param pool_name: Name of the pool
signal pool_exhausted(pool_name: String)


# ========================================
# HELPER METHODS
# ========================================

## Enable/disable debug logging of events
var debug_mode: bool = false


func _ready() -> void:
	print("📡 EventBus initialized")
	if debug_mode:
		_connect_debug_listeners()


## Connect debug listeners for development
func _connect_debug_listeners() -> void:
	player_level_up.connect(func(level): print("🎉 [EventBus] Player leveled up to level %d" % level))
	quest_completed.connect(func(id, name, xp): print("✅ [EventBus] Quest completed: %s (+%d XP)" % [name, xp]))
	enemy_killed.connect(func(enemy, xp): print("⚔️ [EventBus] Enemy killed: +%d XP" % xp))
	achievement_unlocked.connect(func(id, name): print("🏆 [EventBus] Achievement unlocked: %s" % name))
	damage_dealt.connect(func(target, amount, critical): 
		print("💥 [EventBus] Damage dealt: %.1f%s" % [amount, " (CRITICAL!)" if critical else ""]))


## Request a notification to be displayed
func show_notification(message: String, type: String = "info", duration: float = 3.0) -> void:
	notification_requested.emit(message, duration, type)


## Request a UI panel toggle
func toggle_panel(panel_name: String, visible: bool) -> void:
	ui_panel_toggled.emit(panel_name, visible)
