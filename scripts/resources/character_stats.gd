extends Resource
class_name CharacterStats
## Character statistics resource
## Ported from Unity's CharacterStats.cs

@export var character_name: String = "Aragorn"
@export var level: int = 1
@export var max_health: float = 100.0
@export var current_health: float = 100.0
@export var max_stamina: float = 100.0
@export var current_stamina: float = 100.0
@export var strength: int = 10
@export var wisdom: int = 10
@export var agility: int = 10
@export var experience: int = 0
@export var experience_to_next_level: int = 100

## Emitted when stats change (for UI updates)
signal stats_changed()
signal health_changed(current: float, max: float)
signal stamina_changed(current: float, max: float)
signal level_up(new_level: int)


## Take damage
func take_damage(damage: float) -> void:
	current_health = maxf(0.0, current_health - damage)
	health_changed.emit(current_health, max_health)
	EventBus.player_health_changed.emit(current_health, max_health)
	
	if current_health <= 0:
		_handle_death()


## Heal health
func heal(amount: float) -> void:
	current_health = minf(max_health, current_health + amount)
	health_changed.emit(current_health, max_health)
	EventBus.player_health_changed.emit(current_health, max_health)


## Use stamina
func use_stamina(amount: float) -> bool:
	if current_stamina >= amount:
		current_stamina = maxf(0.0, current_stamina - amount)
		stamina_changed.emit(current_stamina, max_stamina)
		EventBus.player_stamina_changed.emit(current_stamina, max_stamina)
		return true
	return false


## Restore stamina
func restore_stamina(amount: float) -> void:
	current_stamina = minf(max_stamina, current_stamina + amount)
	stamina_changed.emit(current_stamina, max_stamina)
	EventBus.player_stamina_changed.emit(current_stamina, max_stamina)


## Gain experience
func gain_experience(amount: int) -> void:
	experience += amount
	EventBus.player_experience_gained.emit(amount, experience)
	
	# Check for level up
	while experience >= experience_to_next_level:
		_level_up()


## Handle level up
func _level_up() -> void:
	level += 1
	experience -= experience_to_next_level
	experience_to_next_level = int(experience_to_next_level * Constants.XP_SCALING_FACTOR)
	
	# Increase stats
	max_health += Constants.LEVELUP_HEALTH_BONUS
	current_health = max_health
	max_stamina += Constants.LEVELUP_STAMINA_BONUS
	current_stamina = max_stamina
	strength += Constants.LEVELUP_STAT_INCREASE
	wisdom += Constants.LEVELUP_STAT_INCREASE
	agility += Constants.LEVELUP_STAT_INCREASE
	
	print("🎉 LEVEL UP! You are now level ", level, "!")
	
	level_up.emit(level)
	EventBus.player_level_up.emit(level)
	stats_changed.emit()


## Handle player death
func _handle_death() -> void:
	print("💀 Player has died!")
	EventBus.player_died.emit()


## Respawn player with full health and stamina
func respawn() -> void:
	current_health = max_health
	current_stamina = max_stamina
	health_changed.emit(current_health, max_health)
	stamina_changed.emit(current_stamina, max_stamina)
	EventBus.player_respawned.emit()


## Get save data dictionary
func get_save_data() -> Dictionary:
	return {
		"character_name": character_name,
		"level": level,
		"max_health": max_health,
		"current_health": current_health,
		"max_stamina": max_stamina,
		"current_stamina": current_stamina,
		"strength": strength,
		"wisdom": wisdom,
		"agility": agility,
		"experience": experience,
		"experience_to_next_level": experience_to_next_level
	}


## Apply save data
func apply_save_data(data: Dictionary) -> void:
	character_name = data.get("character_name", "Aragorn")
	level = data.get("level", 1)
	max_health = data.get("max_health", 100.0)
	current_health = data.get("current_health", 100.0)
	max_stamina = data.get("max_stamina", 100.0)
	current_stamina = data.get("current_stamina", 100.0)
	strength = data.get("strength", 10)
	wisdom = data.get("wisdom", 10)
	agility = data.get("agility", 10)
	experience = data.get("experience", 0)
	experience_to_next_level = data.get("experience_to_next_level", 100)
	
	stats_changed.emit()
	health_changed.emit(current_health, max_health)
	stamina_changed.emit(current_stamina, max_stamina)
