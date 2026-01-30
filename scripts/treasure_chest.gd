extends StaticBody3D

## Treasure Chest
## Interactable object that contains loot

@export var chest_id: String = "chest_01"
@export var loot_table: LootTable = null
@export var interaction_range: float = 2.0
@export var is_opened: bool = false

var player_in_range: bool = false

@onready var mesh_instance = $MeshInstance3D
@onready var interaction_area = $InteractionArea
@onready var lid = $Lid

func _ready() -> void:
	# Connect signals
	if interaction_area:
		interaction_area.body_entered.connect(_on_body_entered)
		interaction_area.body_exited.connect(_on_body_exited)
	
	# Load chest state from save if needed
	_load_chest_state()

func _input(event: InputEvent) -> void:
	if player_in_range and not is_opened and event.is_action_pressed("interact"):
		open_chest()

func _on_body_entered(body: Node3D) -> void:
	if body.is_in_group("player"):
		player_in_range = true

func _on_body_exited(body: Node3D) -> void:
	if body.is_in_group("player"):
		player_in_range = false

func open_chest() -> void:
	if is_opened:
		return
	
	is_opened = true
	
	# Play opening animation
	_play_open_animation()
	
	# Generate loot
	if loot_table:
		var drops = loot_table.get_random_drops()
		var gold = loot_table.get_random_gold()
		
		# Add items to inventory
		for drop in drops:
			InventoryManager.add_item(drop.item_id, drop.quantity)
		
		# Add gold
		if gold > 0:
			GameManager.add_gold(gold)
		
		# Emit event
		EventBus.chest_opened.emit(chest_id, drops, gold)
	
	# Save chest state
	_save_chest_state()

func _play_open_animation() -> void:
	if lid:
		var tween = create_tween()
		tween.tween_property(lid, "rotation_degrees:x", -90, 0.5)

func _load_chest_state() -> void:
	# Check if this chest was already opened
	# This would integrate with SaveManager in a full implementation
	pass

func _save_chest_state() -> void:
	# Save that this chest was opened
	# This would integrate with SaveManager in a full implementation
	pass
