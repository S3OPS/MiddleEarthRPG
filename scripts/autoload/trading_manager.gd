extends Node

## TradingManager
## Manages player-to-player trading

signal trade_offer_created(trade_id: String, sender_id: String, receiver_id: String)
signal trade_accepted(trade_id: String)
signal trade_declined(trade_id: String)
signal trade_cancelled(trade_id: String)

var active_trades: Dictionary = {}  # trade_id -> TradeOfferResource
var player_trades: Array[String] = []  # Active trades for current player

func _ready() -> void:
	print("💰 TradingManager initialized")

func create_trade_offer(receiver_id: String) -> String:
	"""Create a new trade offer"""
	var sender_id = "current_player"  # TODO: Get from actual player system
	
	var trade = TradeOfferResource.new(sender_id, receiver_id)
	active_trades[trade.trade_id] = trade
	player_trades.append(trade.trade_id)
	
	trade_offer_created.emit(trade.trade_id, sender_id, receiver_id)
	print("Trade offer created: ", trade.trade_id)
	return trade.trade_id

func add_item_to_trade(trade_id: String, item_id: String, quantity: int, is_sender: bool) -> bool:
	"""Add an item to a trade offer"""
	if trade_id not in active_trades:
		return false
	
	var trade = active_trades[trade_id]
	
	# Check if item exists in inventory
	if not InventoryManager.has_item(item_id, quantity):
		print("Not enough items in inventory")
		return false
	
	if is_sender:
		trade.add_sender_item(item_id, quantity)
	else:
		trade.add_receiver_item(item_id, quantity)
	
	print("Added ", quantity, "x ", item_id, " to trade")
	return true

func add_gold_to_trade(trade_id: String, gold_amount: int, is_sender: bool) -> bool:
	"""Add gold to a trade offer"""
	if trade_id not in active_trades:
		return false
	
	var trade = active_trades[trade_id]
	
	# Check if player has enough gold
	if GameManager.player_gold < gold_amount:
		print("Not enough gold")
		return false
	
	if is_sender:
		trade.sender_gold = gold_amount
	else:
		trade.receiver_gold = gold_amount
	
	print("Added ", gold_amount, " gold to trade")
	return true

func accept_trade(trade_id: String) -> bool:
	"""Accept a trade offer"""
	if trade_id not in active_trades:
		return false
	
	var trade = active_trades[trade_id]
	
	# Check if trade has expired
	if trade.is_expired():
		cancel_trade(trade_id)
		print("Trade has expired")
		return false
	
	# Execute trade
	if not _execute_trade(trade):
		print("Trade execution failed")
		return false
	
	trade.status = "accepted"
	trade_accepted.emit(trade_id)
	
	# Clean up
	active_trades.erase(trade_id)
	player_trades.erase(trade_id)
	
	print("Trade accepted and completed")
	return true

func decline_trade(trade_id: String) -> bool:
	"""Decline a trade offer"""
	if trade_id not in active_trades:
		return false
	
	var trade = active_trades[trade_id]
	trade.status = "declined"
	
	trade_declined.emit(trade_id)
	
	# Clean up
	active_trades.erase(trade_id)
	player_trades.erase(trade_id)
	
	print("Trade declined")
	return true

func cancel_trade(trade_id: String) -> bool:
	"""Cancel a trade offer"""
	if trade_id not in active_trades:
		return false
	
	var trade = active_trades[trade_id]
	trade.status = "cancelled"
	
	trade_cancelled.emit(trade_id)
	
	# Clean up
	active_trades.erase(trade_id)
	player_trades.erase(trade_id)
	
	print("Trade cancelled")
	return true

func _execute_trade(trade: TradeOfferResource) -> bool:
	"""Execute the trade transaction"""
	# Remove items from sender
	for item_id in trade.sender_items:
		var quantity = trade.sender_items[item_id]
		if not InventoryManager.has_item(item_id, quantity):
			return false
		InventoryManager.remove_item(item_id, quantity)
	
	# Remove gold from sender
	if trade.sender_gold > 0:
		GameManager.add_gold(-trade.sender_gold)
	
	# Add items to sender from receiver
	for item_id in trade.receiver_items:
		var quantity = trade.receiver_items[item_id]
		var item = get_node("/root/Main/GameInitializer").get_item(item_id)
		if item:
			InventoryManager.add_item(item, quantity)
	
	# Add gold to sender from receiver
	if trade.receiver_gold > 0:
		GameManager.add_gold(trade.receiver_gold)
	
	return true

func get_active_trades() -> Array[TradeOfferResource]:
	"""Get all active trades"""
	var trades: Array[TradeOfferResource] = []
	for trade_id in player_trades:
		if trade_id in active_trades:
			trades.append(active_trades[trade_id])
	return trades

func save_data() -> Dictionary:
	"""Save trading data"""
	return {
		"player_trades": player_trades
	}

func load_data(data: Dictionary) -> void:
	"""Load trading data"""
	player_trades = data.get("player_trades", [])
