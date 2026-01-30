extends CharacterBody3D

## Base NPC Class
## Handles NPC interactions, dialogue triggering, and quest giving

@export var npc_name: String = "NPC"
@export var dialogue_id: String = ""
@export var interaction_range: float = 3.0
@export var quest_to_give: String = ""

var player_in_range: bool = false
var player_ref: Node3D = null

@onready var interaction_area = $InteractionArea
@onready var mesh_instance = $MeshInstance3D
@onready var interaction_prompt = $InteractionPrompt

func _ready() -> void:
	# Connect area signals
	if interaction_area:
		interaction_area.body_entered.connect(_on_body_entered)
		interaction_area.body_exited.connect(_on_body_exited)
	
	# Hide interaction prompt
	if interaction_prompt:
		interaction_prompt.hide()
	
	# Set up collision area size
	if interaction_area:
		var sphere_shape = interaction_area.get_child(0) as CollisionShape3D
		if sphere_shape and sphere_shape.shape is SphereShape3D:
			sphere_shape.shape.radius = interaction_range

func _input(event: InputEvent) -> void:
	if player_in_range and event.is_action_pressed("interact"):
		interact()

func _on_body_entered(body: Node3D) -> void:
	if body.is_in_group("player"):
		player_in_range = true
		player_ref = body
		show_interaction_prompt()
		highlight_npc(true)

func _on_body_exited(body: Node3D) -> void:
	if body.is_in_group("player"):
		player_in_range = false
		player_ref = null
		hide_interaction_prompt()
		highlight_npc(false)

func show_interaction_prompt() -> void:
	if interaction_prompt:
		interaction_prompt.show()

func hide_interaction_prompt() -> void:
	if interaction_prompt:
		interaction_prompt.hide()

func highlight_npc(enabled: bool) -> void:
	if not mesh_instance:
		return
	
	var material = mesh_instance.get_active_material(0)
	if material:
		if enabled:
			# Add a slight glow/brightness
			material.emission_enabled = true
			material.emission = Color(0.8, 0.7, 0.4, 1.0)
			material.emission_energy_multiplier = 0.3
		else:
			material.emission_enabled = false

func interact() -> void:
	if not player_in_range:
		return
	
	# Start dialogue if available
	if dialogue_id != "":
		var game_init = get_tree().root.get_node_or_null("GameInitializer")
		if game_init and game_init.has_method("get_dialogue"):
			var dialogue = game_init.get_dialogue(dialogue_id)
			if dialogue:
				DialogueManager.start_dialogue(dialogue)
				EventBus.npc_interacted.emit(npc_name)
	
	# Give quest if available and not already active
	if quest_to_give != "":
		if not QuestManager.is_quest_active(quest_to_give):
			QuestManager.start_quest(quest_to_give)
			print("Quest started: ", quest_to_give)

func get_npc_name() -> String:
	return npc_name
