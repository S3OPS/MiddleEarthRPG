extends Node
## Global event bus for signal-based communication
## Replaces Unity's EventBus pattern with Godot signals

# ========================================
# PLAYER SIGNALS
# ========================================

## Emitted when player health changes
signal player_health_changed(current_health: float, max_health: float)

## Emitted when player stamina changes
signal player_stamina_changed(current_stamina: float, max_stamina: float)

## Emitted when player gains experience
signal player_experience_gained(amount: int, total: int)

## Emitted when player levels up
signal player_level_up(new_level: int)

## Emitted when player dies
signal player_died()

## Emitted when player respawns
signal player_respawned()


# ========================================
# COMBAT SIGNALS
# ========================================

## Emitted when damage is dealt
signal damage_dealt(target: Node, amount: float, is_critical: bool)

## Emitted when an entity takes damage
signal damage_taken(entity: Node, amount: float, attacker: Node)

## Emitted when an enemy dies
signal enemy_killed(enemy: Node, experience_reward: int)

## Emitted when combat starts
signal combat_started(enemy: Node)

## Emitted when combat ends
signal combat_ended()


# ========================================
# QUEST SIGNALS
# ========================================

## Emitted when a quest is started
signal quest_started(quest_id: String, quest_name: String)

## Emitted when a quest objective is updated
signal quest_objective_updated(quest_id: String, objective_index: int, completed: bool)

## Emitted when a quest is completed
signal quest_completed(quest_id: String, quest_name: String, reward_xp: int)

## Emitted when a quest is failed
signal quest_failed(quest_id: String, quest_name: String)


# ========================================
# INVENTORY SIGNALS
# ========================================

## Emitted when an item is added to inventory
signal item_added(item_id: String, item_name: String, quantity: int)

## Emitted when an item is removed from inventory
signal item_removed(item_id: String, quantity: int)

## Emitted when an item is equipped
signal item_equipped(item_id: String, slot: String)

## Emitted when an item is unequipped
signal item_unequipped(item_id: String, slot: String)

## Emitted when inventory is full
signal inventory_full()


# ========================================
# UI SIGNALS
# ========================================

## Emitted when a notification should be displayed
signal notification_requested(message: String, duration: float, type: String)

## Emitted when UI panel should be toggled
signal ui_panel_toggled(panel_name: String, is_visible: bool)


# ========================================
# WORLD SIGNALS
# ========================================

## Emitted when time of day changes
signal time_of_day_changed(hour: int, is_day: bool)

## Emitted when weather changes
signal weather_changed(weather_type: String)

## Emitted when player discovers a location
signal location_discovered(location_name: String)


# ========================================
# SAVE/LOAD SIGNALS
# ========================================

## Emitted when game is saved
signal game_saved(slot_index: int)

## Emitted when game is loaded
signal game_loaded(slot_index: int)

## Emitted when save fails
signal save_failed(error_message: String)


# ========================================
# ACHIEVEMENT SIGNALS
# ========================================

## Emitted when an achievement is unlocked
signal achievement_unlocked(achievement_id: String, achievement_name: String)


# ========================================
# HELPER METHODS
# ========================================

## Print debug info when signals are emitted (for development)
var debug_mode: bool = false

func _ready() -> void:
	if debug_mode:
		_connect_debug_listeners()

func _connect_debug_listeners() -> void:
	player_level_up.connect(func(level): print("🎉 Player leveled up to level ", level))
	quest_completed.connect(func(id, name, xp): print("✅ Quest completed: ", name, " (", xp, " XP)"))
	enemy_killed.connect(func(enemy, xp): print("⚔️ Enemy killed: +", xp, " XP"))
	achievement_unlocked.connect(func(id, name): print("🏆 Achievement unlocked: ", name))
