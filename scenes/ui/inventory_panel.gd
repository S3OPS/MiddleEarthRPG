extends Control

## Inventory UI Panel
## Displays player's inventory in a grid layout

@onready var item_grid = $Panel/MarginContainer/VBoxContainer/ScrollContainer/ItemGrid
@onready var close_button = $Panel/MarginContainer/VBoxContainer/TopBar/CloseButton
@onready var info_panel = $Panel/MarginContainer/VBoxContainer/InfoPanel
@onready var item_name_label = $Panel/MarginContainer/VBoxContainer/InfoPanel/VBoxContainer/ItemName
@onready var item_desc_label = $Panel/MarginContainer/VBoxContainer/InfoPanel/VBoxContainer/ItemDescription

const GRID_COLUMNS = 10

var is_visible: bool = false
var selected_item_id: String = ""

func _ready() -> void:
	# Connect signals
	EventBus.item_added.connect(_on_item_changed)
	EventBus.item_removed.connect(_on_item_removed)
	EventBus.ui_panel_toggled.connect(_on_ui_panel_toggled)
	
	close_button.pressed.connect(_on_close_pressed)
	
	# Start hidden
	hide()
	is_visible = false
	info_panel.hide()

func _input(event: InputEvent) -> void:
	if event.is_action_pressed("toggle_inventory"):
		toggle_visibility()

func toggle_visibility() -> void:
	is_visible = !is_visible
	visible = is_visible
	
	if is_visible:
		refresh_inventory()
		Input.mouse_mode = Input.MOUSE_MODE_VISIBLE
	else:
		Input.mouse_mode = Input.MOUSE_MODE_CAPTURED

func refresh_inventory() -> void:
	# Clear existing items
	for child in item_grid.get_children():
		child.queue_free()
	
	# Get all items
	var items = InventoryManager.get_all_items()
	
	if items.is_empty():
		var no_items = Label.new()
		no_items.text = "Inventory is empty"
		no_items.horizontal_alignment = HORIZONTAL_ALIGNMENT_CENTER
		item_grid.add_child(no_items)
		return
	
	# Add each item as a button
	for item_data in items:
		var item: InventoryItem = item_data["item"]
		var quantity: int = item_data["quantity"]
		var button = _create_item_button(item, quantity)
		item_grid.add_child(button)

func _create_item_button(item: InventoryItem, quantity: int) -> Button:
	var button = Button.new()
	button.custom_minimum_size = Vector2(64, 64)
	button.text = item.item_name.substr(0, 1).to_upper() + "\n" + str(quantity)
	button.modulate = item.get_rarity_color()
	
	# Connect button press
	button.pressed.connect(func(): _on_item_selected(item))
	
	return button

func _on_item_selected(item: InventoryItem) -> void:
	selected_item_id = item.item_id
	
	# Show item info
	info_panel.show()
	item_name_label.text = item.item_name
	item_desc_label.text = item.description
	item_desc_label.text += "\n\nType: " + item.get_type_string()
	item_desc_label.text += "\nRarity: " + InventoryItem.Rarity.keys()[item.rarity]
	item_desc_label.text += "\nValue: %d gold" % item.value
	
	if item.type == InventoryItem.ItemType.EQUIPMENT:
		item_desc_label.text += "\n\nStats:"
		if item.attack_bonus > 0:
			item_desc_label.text += "\n  Attack: +" + str(item.attack_bonus)
		if item.defense_bonus > 0:
			item_desc_label.text += "\n  Defense: +" + str(item.defense_bonus)
		if item.health_bonus > 0:
			item_desc_label.text += "\n  Health: +" + str(item.health_bonus)
		if item.stamina_bonus > 0:
			item_desc_label.text += "\n  Stamina: +" + str(item.stamina_bonus)
	
	if item.type == InventoryItem.ItemType.CONSUMABLE:
		item_desc_label.text += "\n\nEffects:"
		if item.health_restore > 0:
			item_desc_label.text += "\n  Restores %d health" % item.health_restore
		if item.stamina_restore > 0:
			item_desc_label.text += "\n  Restores %d stamina" % item.stamina_restore

func _on_item_changed(_item_id: String, _item_name: String, _quantity: int) -> void:
	if is_visible:
		refresh_inventory()

func _on_item_removed(_item_id: String, _quantity: int) -> void:
	if is_visible:
		refresh_inventory()

func _on_ui_panel_toggled(panel_name: String, should_show: bool) -> void:
	if panel_name == "inventory":
		is_visible = should_show
		visible = should_show
		if should_show:
			refresh_inventory()

func _on_close_pressed() -> void:
	toggle_visibility()
