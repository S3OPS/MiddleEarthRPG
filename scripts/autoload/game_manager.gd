extends Node
## Global game manager singleton
## Handles game state, scene transitions, and high-level game logic

enum GameState {
	MENU,
	PLAYING,
	PAUSED,
	GAME_OVER
}

var current_state: GameState = GameState.MENU
var player: Node = null
var current_scene: Node = null

# Game statistics
var total_play_time: float = 0.0
var enemies_defeated: int = 0
var quests_completed: int = 0
var treasures_found: int = 0


func _ready() -> void:
	print("🎮 GameManager initialized")
	process_mode = Node.PROCESS_MODE_ALWAYS  # Run even when paused


func _process(delta: float) -> void:
	if current_state == GameState.PLAYING:
		total_play_time += delta


## Set the game state
func set_game_state(new_state: GameState) -> void:
	var old_state = current_state
	current_state = new_state
	
	match new_state:
		GameState.PAUSED:
			Engine.time_scale = 0.0
			get_tree().paused = true
		GameState.PLAYING:
			Engine.time_scale = 1.0
			get_tree().paused = false
		GameState.GAME_OVER:
			_handle_game_over()
	
	print("Game state changed: ", GameState.keys()[old_state], " -> ", GameState.keys()[new_state])


## Pause the game
func pause_game() -> void:
	set_game_state(GameState.PAUSED)


## Resume the game
func resume_game() -> void:
	set_game_state(GameState.PLAYING)


## Start a new game
func start_new_game() -> void:
	print("🎮 Starting new game...")
	total_play_time = 0.0
	enemies_defeated = 0
	quests_completed = 0
	treasures_found = 0
	set_game_state(GameState.PLAYING)
	EventBus.player_respawned.emit()


## Handle game over
func _handle_game_over() -> void:
	print("💀 Game Over")
	await get_tree().create_timer(3.0).timeout
	# Could load game over screen here


## Register the player reference
func register_player(player_node: Node) -> void:
	player = player_node
	print("✅ Player registered with GameManager")


## Get the player reference
func get_player() -> Node:
	return player


## Increment enemies defeated counter
func increment_enemies_defeated() -> void:
	enemies_defeated += 1


## Increment quests completed counter
func increment_quests_completed() -> void:
	quests_completed += 1


## Increment treasures found counter
func increment_treasures_found() -> void:
	treasures_found += 1


## Get formatted play time
func get_formatted_play_time() -> String:
	var hours = int(total_play_time) / 3600
	var minutes = (int(total_play_time) % 3600) / 60
	var seconds = int(total_play_time) % 60
	return "%02d:%02d:%02d" % [hours, minutes, seconds]


## Quit the game
func quit_game() -> void:
	print("👋 Quitting game...")
	get_tree().quit()
