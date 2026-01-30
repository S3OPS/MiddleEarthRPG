extends Node3D

## Item Pickup Visual
## Floating item that can be picked up by the player

@export var item_id: String = ""
@export var quantity: int = 1
@export var auto_pickup_range: float = 1.5
@export var float_height: float = 0.3
@export var float_speed: float = 2.0
@export var rotate_speed: float = 2.0

var player_in_range: bool = false
var initial_y: float = 0.0

@onready var mesh_instance = $MeshInstance3D
@onready var pickup_area = $PickupArea

func _ready() -> void:
	initial_y = position.y
	
	# Connect area signals
	if pickup_area:
		pickup_area.body_entered.connect(_on_body_entered)
	
	# Start floating animation
	_start_float_animation()

func _process(delta: float) -> void:
	# Rotate the item
	rotate_y(rotate_speed * delta)

func _start_float_animation() -> void:
	var tween = create_tween()
	tween.set_loops()
	tween.set_trans(Tween.TRANS_SINE)
	tween.set_ease(Tween.EASE_IN_OUT)
	
	tween.tween_property(self, "position:y", initial_y + float_height, 1.0)
	tween.tween_property(self, "position:y", initial_y - float_height, 1.0)

func _on_body_entered(body: Node3D) -> void:
	if body.is_in_group("player"):
		pickup_item()

func pickup_item() -> void:
	# Get item from GameInitializer's database
	var game_init = get_tree().root.get_node_or_null("GameInitializer")
	var item: InventoryItem = null
	
	if game_init and game_init.has_method("get_item"):
		item = game_init.get_item(item_id)
		if not item:
			push_warning("Failed to get item from database: " + item_id)
	
	if not item:
		# Fallback: Create a placeholder item
		push_warning("Item database not found, creating placeholder for: " + item_id)
		item = InventoryItem.new()
		item.item_id = item_id
		item.item_name = "Unknown Item"
		item.description = "An item found in the world"
	
	# Add item to inventory
	var success = InventoryManager.add_item(item, quantity)
	
	if success:
		# Play pickup animation
		_play_pickup_animation()
		
		# Emit pickup event
		EventBus.item_picked_up.emit(item_id, quantity)
		
		# Remove the pickup after animation
		await get_tree().create_timer(0.3).timeout
		queue_free()

func _play_pickup_animation() -> void:
	var tween = create_tween()
	tween.set_parallel(true)
	tween.tween_property(self, "position:y", position.y + 1.0, 0.3)
	tween.tween_property(self, "scale", Vector3.ZERO, 0.3)
	tween.tween_property(mesh_instance, "transparency", 1.0, 0.3)

func set_item(p_item_id: String, p_quantity: int = 1) -> void:
	item_id = p_item_id
	quantity = p_quantity
	
	# You could customize the mesh based on item type here
	# For now, we'll use a simple sphere
