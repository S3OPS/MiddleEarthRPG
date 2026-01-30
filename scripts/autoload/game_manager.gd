extends Node
## Global game manager singleton
## Handles game state, scene transitions, and high-level game logic
## Provides centralized player reference and game statistics

enum GameState {
	MENU,
	PLAYING,
	PAUSED,
	GAME_OVER
}

var current_state: GameState = GameState.MENU
var player: Node = null
var player_stats: CharacterStats = null
var current_scene: Node = null

# Game statistics
var total_play_time: float = 0.0
var enemies_defeated: int = 0
var quests_completed: int = 0
var treasures_found: int = 0
var gold: int = 0


func _ready() -> void:
	print("🎮 GameManager initialized")
	process_mode = Node.PROCESS_MODE_ALWAYS  # Run even when paused


func _process(delta: float) -> void:
	if current_state == GameState.PLAYING:
		total_play_time += delta


## Set the game state and handle transitions
func set_game_state(new_state: GameState) -> void:
	var old_state: GameState = current_state
	current_state = new_state
	
	_handle_state_transition(new_state)
	
	print("Game state changed: %s -> %s" % [
		GameState.keys()[old_state],
		GameState.keys()[new_state]
	])


## Handle state transitions
func _handle_state_transition(new_state: GameState) -> void:
	match new_state:
		GameState.PAUSED:
			Engine.time_scale = 0.0
			get_tree().paused = true
		GameState.PLAYING:
			Engine.time_scale = 1.0
			get_tree().paused = false
		GameState.GAME_OVER:
			_handle_game_over()


## Pause the game
func pause_game() -> void:
	set_game_state(GameState.PAUSED)


## Resume the game
func resume_game() -> void:
	set_game_state(GameState.PLAYING)


## Start a new game
func start_new_game() -> void:
	print("🎮 Starting new game...")
	_reset_statistics()
	set_game_state(GameState.PLAYING)
	EventBus.player_respawned.emit()


## Reset all game statistics
func _reset_statistics() -> void:
	total_play_time = 0.0
	enemies_defeated = 0
	quests_completed = 0
	treasures_found = 0
	gold = 0


## Handle game over
func _handle_game_over() -> void:
	print("💀 Game Over")
	await get_tree().create_timer(3.0).timeout
	# Could load game over screen here


## Register the player reference
func register_player(player_node: Node) -> void:
	if not is_instance_valid(player_node):
		push_error("Attempted to register invalid player node")
		return
	
	player = player_node
	
	# Try to get player stats
	if player_node.has_method("get_stats"):
		player_stats = player_node.get_stats()
	
	print("✅ Player registered with GameManager")


## Get the player reference safely
func get_player() -> Node:
	if not is_instance_valid(player):
		return null
	return player


## Check if player is valid
func has_valid_player() -> bool:
	return player != null and is_instance_valid(player)


## Increment enemies defeated counter
func increment_enemies_defeated() -> void:
	enemies_defeated += 1


## Increment quests completed counter
func increment_quests_completed() -> void:
	quests_completed += 1


## Increment treasures found counter
func increment_treasures_found() -> void:
	treasures_found += 1


## Add gold to player
func add_gold(amount: int) -> void:
	if amount <= 0:
		return
	gold += amount
	EventBus.show_notification("Found %d gold!" % amount, "success")


## Remove gold from player
func remove_gold(amount: int) -> bool:
	if amount <= 0 or gold < amount:
		return false
	gold -= amount
	return true


## Add experience to player
func add_experience(amount: int) -> void:
	if not player_stats or amount <= 0:
		return
	player_stats.gain_experience(amount)


## Get formatted play time as HH:MM:SS
func get_formatted_play_time() -> String:
	var total_seconds = int(total_play_time)
	var hours = total_seconds / 3600
	var minutes = (total_seconds % 3600) / 60
	var seconds = total_seconds % 60
	return "%02d:%02d:%02d" % [hours, minutes, seconds]


## Get game statistics dictionary
func get_statistics() -> Dictionary:
	return {
		"play_time": total_play_time,
		"play_time_formatted": get_formatted_play_time(),
		"enemies_defeated": enemies_defeated,
		"quests_completed": quests_completed,
		"treasures_found": treasures_found,
	}


## Quit the game
func quit_game() -> void:
	print("👋 Quitting game...")
	get_tree().quit()
