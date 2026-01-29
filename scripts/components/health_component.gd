extends Node
class_name HealthComponent
## Reusable health and damage component
## Can be used by players, enemies, and other entities

signal health_changed(current: float, maximum: float)
signal damage_taken(amount: float, remaining_health: float)
signal healed(amount: float, current_health: float)
signal died
signal health_depleted

@export var max_health: float = 100.0
@export var current_health: float = 100.0
@export var is_invulnerable: bool = false
@export var regenerate_health: bool = false
@export var regen_rate: float = 1.0  # HP per second

var _is_dead: bool = false


func _ready() -> void:
	current_health = max_health


func _process(delta: float) -> void:
	if regenerate_health and current_health < max_health and not _is_dead:
		heal(regen_rate * delta)


## Take damage
func take_damage(amount: float) -> bool:
	if _is_dead or is_invulnerable or amount <= 0:
		return false
	
	current_health = max(0, current_health - amount)
	damage_taken.emit(amount, current_health)
	health_changed.emit(current_health, max_health)
	
	if current_health <= 0:
		_die()
		return true
	
	return false


## Heal health
func heal(amount: float) -> void:
	if _is_dead or amount <= 0:
		return
	
	var old_health = current_health
	current_health = min(max_health, current_health + amount)
	
	if current_health > old_health:
		healed.emit(amount, current_health)
		health_changed.emit(current_health, max_health)


## Set health to maximum
func heal_full() -> void:
	heal(max_health - current_health)


## Handle death
func _die() -> void:
	if _is_dead:
		return
	
	_is_dead = true
	health_depleted.emit()
	died.emit()


## Revive with specified health
func revive(health_amount: float = -1.0) -> void:
	_is_dead = false
	
	if health_amount > 0:
		current_health = min(health_amount, max_health)
	else:
		current_health = max_health
	
	health_changed.emit(current_health, max_health)


## Set maximum health and optionally heal to full
func set_max_health(new_max: float, heal_to_full: bool = false) -> void:
	max_health = new_max
	
	if heal_to_full:
		current_health = max_health
	else:
		current_health = min(current_health, max_health)
	
	health_changed.emit(current_health, max_health)


## Get current health
func get_current_health() -> float:
	return current_health


## Get max health
func get_max_health() -> float:
	return max_health


## Get health percentage (0-1)
func get_health_percent() -> float:
	if max_health <= 0:
		return 0.0
	return current_health / max_health


## Check if dead
func is_dead() -> bool:
	return _is_dead


## Check if at full health
func is_full_health() -> bool:
	return current_health >= max_health


## Check if health is low (below threshold)
func is_health_low(threshold: float = 0.25) -> bool:
	return get_health_percent() <= threshold


## Set invulnerability
func set_invulnerable(invulnerable: bool) -> void:
	is_invulnerable = invulnerable


## Enable health regeneration
func enable_regen(rate: float = 1.0) -> void:
	regenerate_health = true
	regen_rate = rate


## Disable health regeneration
func disable_regen() -> void:
	regenerate_health = false
