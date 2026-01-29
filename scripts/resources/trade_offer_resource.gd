extends Resource
class_name TradeOfferResource

## Trade Offer Resource
## Defines a trading offer between players

@export var trade_id: String = ""
@export var sender_id: String = ""
@export var receiver_id: String = ""
@export var status: String = "pending"  # pending, accepted, declined, cancelled

# Sender's offer
@export var sender_items: Dictionary = {}  # item_id -> quantity
@export var sender_gold: int = 0

# Receiver's offer
@export var receiver_items: Dictionary = {}  # item_id -> quantity
@export var receiver_gold: int = 0

@export var created_time: float = 0.0
@export var expiry_time: float = 300.0  # 5 minutes default

func _init(
	p_sender: String = "",
	p_receiver: String = ""
):
	trade_id = str(Time.get_ticks_msec())
	sender_id = p_sender
	receiver_id = p_receiver
	created_time = Time.get_ticks_msec() / 1000.0
	status = "pending"

func is_expired() -> bool:
	"""Check if trade offer has expired"""
	var current_time = Time.get_ticks_msec() / 1000.0
	return (current_time - created_time) > expiry_time

func add_sender_item(item_id: String, quantity: int) -> void:
	"""Add item to sender's offer"""
	sender_items[item_id] = quantity

func add_receiver_item(item_id: String, quantity: int) -> void:
	"""Add item to receiver's offer"""
	receiver_items[item_id] = quantity

func get_total_value() -> int:
	"""Get total gold value of trade"""
	return sender_gold + receiver_gold
