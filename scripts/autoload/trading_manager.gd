extends Node

## TradingManager
## Manages player-to-player trading system
## Handles trade offers, item/gold exchange, and trade lifecycle

signal trade_offer_created(trade_id: String, sender_id: String, receiver_id: String)
signal trade_accepted(trade_id: String)
signal trade_declined(trade_id: String)
signal trade_cancelled(trade_id: String)

# Dictionary of active trades: trade_id -> TradeOfferResource
var active_trades: Dictionary = {}
# Array of trade IDs for current player
var player_trades: Array[String] = []


func _ready() -> void:
	if OS.is_debug_build():
		print("💰 TradingManager initialized")


## Create a new trade offer
## @param receiver_id: The ID of the player to receive the trade offer
## @return: The unique trade ID
func create_trade_offer(receiver_id: String) -> String:
	var sender_id: String = "current_player"  # Future: Get from actual player system
	
	var trade: TradeOfferResource = TradeOfferResource.new(sender_id, receiver_id)
	active_trades[trade.trade_id] = trade
	player_trades.append(trade.trade_id)
	
	trade_offer_created.emit(trade.trade_id, sender_id, receiver_id)
	if OS.is_debug_build():
		print("TradingManager: Trade offer created - %s" % trade.trade_id)
	return trade.trade_id

## Add an item to a trade offer
## @param trade_id: The ID of the trade to modify
## @param item_id: The ID of the item to add
## @param quantity: The quantity of items to add
## @param is_sender: true if adding to sender's side, false for receiver's side
## @return: true if item was added successfully, false otherwise
func add_item_to_trade(trade_id: String, item_id: String, quantity: int, is_sender: bool) -> bool:
	if trade_id not in active_trades:
		return false
	
	var trade: TradeOfferResource = active_trades[trade_id]
	
	# Verify item exists in inventory
	if not InventoryManager.has_item(item_id, quantity):
		if OS.is_debug_build():
			print("TradingManager: Not enough items in inventory")
		return false
	
	if is_sender:
		trade.add_sender_item(item_id, quantity)
	else:
		trade.add_receiver_item(item_id, quantity)
	
	if OS.is_debug_build():
		print("TradingManager: Added %dx %s to trade" % [quantity, item_id])
	return true


## Add gold to a trade offer
## @param trade_id: The ID of the trade to modify
## @param gold_amount: The amount of gold to add
## @param is_sender: true if adding to sender's side, false for receiver's side
## @return: true if gold was added successfully, false otherwise
func add_gold_to_trade(trade_id: String, gold_amount: int, is_sender: bool) -> bool:
	if trade_id not in active_trades:
		return false
	
	var trade: TradeOfferResource = active_trades[trade_id]
	
	# Verify player has enough gold
	if GameManager.gold < gold_amount:
		if OS.is_debug_build():
			print("TradingManager: Not enough gold")
		return false
	
	if is_sender:
		trade.sender_gold = gold_amount
	else:
		trade.receiver_gold = gold_amount
	
	if OS.is_debug_build():
		print("TradingManager: Added %d gold to trade" % gold_amount)
	return true

## Accept a trade offer and execute the transaction
## @param trade_id: The ID of the trade to accept
## @return: true if trade was accepted successfully, false otherwise
func accept_trade(trade_id: String) -> bool:
	if trade_id not in active_trades:
		return false
	
	var trade: TradeOfferResource = active_trades[trade_id]
	
	# Check if trade has expired
	if trade.is_expired():
		cancel_trade(trade_id)
		if OS.is_debug_build():
			print("TradingManager: Trade has expired")
		return false
	
	# Execute the trade transaction
	if not _execute_trade(trade):
		if OS.is_debug_build():
			print("TradingManager: Trade execution failed")
		return false
	
	trade.status = "accepted"
	trade_accepted.emit(trade_id)
	
	# Clean up completed trade
	active_trades.erase(trade_id)
	player_trades.erase(trade_id)
	
	if OS.is_debug_build():
		print("TradingManager: Trade accepted and completed")
	return true


## Decline a trade offer
## @param trade_id: The ID of the trade to decline
## @return: true if trade was declined successfully, false otherwise
func decline_trade(trade_id: String) -> bool:
	if trade_id not in active_trades:
		return false
	
	var trade: TradeOfferResource = active_trades[trade_id]
	trade.status = "declined"
	
	trade_declined.emit(trade_id)
	
	# Clean up declined trade
	active_trades.erase(trade_id)
	player_trades.erase(trade_id)
	
	if OS.is_debug_build():
		print("TradingManager: Trade declined")
	return true


## Cancel a trade offer
## @param trade_id: The ID of the trade to cancel
## @return: true if trade was cancelled successfully, false otherwise
func cancel_trade(trade_id: String) -> bool:
	if trade_id not in active_trades:
		return false
	
	var trade: TradeOfferResource = active_trades[trade_id]
	trade.status = "cancelled"
	
	trade_cancelled.emit(trade_id)
	
	# Clean up cancelled trade
	active_trades.erase(trade_id)
	player_trades.erase(trade_id)
	
	if OS.is_debug_build():
		print("TradingManager: Trade cancelled")
	return true


## Execute the trade transaction (private helper)
## @param trade: The trade resource to execute
## @return: true if execution was successful, false otherwise
func _execute_trade(trade: TradeOfferResource) -> bool:
	# Remove items from sender's inventory
	for item_id in trade.sender_items:
		var quantity: int = trade.sender_items[item_id]
		if not InventoryManager.has_item(item_id, quantity):
			return false
		InventoryManager.remove_item(item_id, quantity)
	
	# Remove gold from sender
	if trade.sender_gold > 0:
		if not GameManager.remove_gold(trade.sender_gold):
			return false
	
	# Add items to sender from receiver
	for item_id in trade.receiver_items:
		var quantity: int = trade.receiver_items[item_id]
		var game_initializer: Node = get_node_or_null("/root/Main/GameInitializer")
		if game_initializer and game_initializer.has_method("get_item"):
			var item = game_initializer.get_item(item_id)
			if item:
				InventoryManager.add_item(item, quantity)
	
	# Add gold to sender from receiver
	if trade.receiver_gold > 0:
		GameManager.add_gold(trade.receiver_gold)
	
	return true


## Get all active trades for the current player
## @return: Array of TradeOfferResource objects
func get_active_trades() -> Array[TradeOfferResource]:
	var trades: Array[TradeOfferResource] = []
	for trade_id in player_trades:
		if trade_id in active_trades:
			trades.append(active_trades[trade_id])
	return trades


## Save trading data for persistence
## @return: Dictionary containing trading data
func save_data() -> Dictionary:
	return {
		"player_trades": player_trades
	}


## Load trading data from save
## @param data: Dictionary containing trading data
func load_data(data: Dictionary) -> void:
	player_trades = data.get("player_trades", [])
