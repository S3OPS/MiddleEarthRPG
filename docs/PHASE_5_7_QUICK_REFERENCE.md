# Phase 5-7 Quick Reference Guide

## 🚀 Quick Start for Developers

### Overview
This guide provides quick access to the Phase 5-7 systems for developers working on the Middle-earth Adventure RPG.

---

## 📋 Manager Reference

### Phase 5 Managers

#### RegionManager
```gdscript
# Register a region
RegionManager.register_region(region_resource)

# Enter a region
RegionManager.enter_region("the_shire")

# Check if discovered
RegionManager.is_region_discovered("rohan")

# Get current region
var current = RegionManager.get_current_region()
```

#### FastTravelManager
```gdscript
# Register waypoint
FastTravelManager.register_waypoint(waypoint_resource)

# Discover waypoint
FastTravelManager.discover_waypoint("hobbiton")

# Travel to waypoint
FastTravelManager.travel_to("edoras")

# Check if can travel
var check = FastTravelManager.can_travel_to("rivendell")
if check["can_travel"]:
    # Travel is possible
```

#### FactionManager
```gdscript
# Register faction
FactionManager.register_faction(faction_resource)

# Add reputation
FactionManager.add_reputation("shire_hobbits", 100)

# Get reputation tier
var tier = FactionManager.get_reputation_tier("rohirrim")

# Check reputation requirement
if FactionManager.has_reputation("rivendell_elves", 1000):
    # Unlock content
```

### Phase 6 Managers

#### CraftingManager
```gdscript
# Register recipe
CraftingManager.register_recipe(recipe_resource)

# Discover recipe
CraftingManager.discover_recipe("recipe_iron_sword")

# Check if can craft
var check = CraftingManager.can_craft("recipe_iron_sword")
if check["can_craft"]:
    CraftingManager.craft_item("recipe_iron_sword")

# Increase crafting skill
CraftingManager.increase_crafting_skill(5)
```

#### SpecializationManager
```gdscript
# Register specialization
SpecializationManager.register_specialization(spec_resource)

# Choose specialization (one-time choice)
SpecializationManager.choose_specialization("warrior")

# Unlock ability
SpecializationManager.unlock_ability("shield_bash")

# Check if has ability
if SpecializationManager.has_ability("whirlwind_attack"):
    # Use ability
```

#### CompanionManager
```gdscript
# Register companion
CompanionManager.register_companion(companion_resource)

# Hire companion
if CompanionManager.hire_companion("boromir"):
    # Successfully hired

# Dismiss companion
CompanionManager.dismiss_companion("boromir")

# Change loyalty
CompanionManager.change_loyalty("gimli_companion", 10)

# Get active companions
var companions = CompanionManager.get_active_companions()
```

### Phase 7 Managers

#### SeasonalEventManager
```gdscript
# Register event
SeasonalEventManager.register_event(event_resource)

# Start event manually
SeasonalEventManager.start_event("spring_festival")

# Complete event
SeasonalEventManager.complete_event("spring_festival")

# Get active events
var active = SeasonalEventManager.get_active_events()

# Get multipliers
var multipliers = SeasonalEventManager.get_event_multipliers()
var bonus_xp = base_xp * multipliers["xp"]
```

#### DifficultyManager
```gdscript
# Set difficulty
DifficultyManager.set_difficulty("hard")

# Get current difficulty
var current = DifficultyManager.get_difficulty()

# Get multipliers
var enemy_hp_mult = DifficultyManager.get_enemy_health_multiplier()
var player_dmg_mult = DifficultyManager.get_player_damage_multiplier()

# Apply to gameplay
enemy.health = base_health * enemy_hp_mult
player.damage = base_damage * player_dmg_mult
```

#### AccessibilityManager
```gdscript
# Set a setting
AccessibilityManager.set_setting("text_size", 1.3)

# Get a setting
var text_size = AccessibilityManager.get_setting("text_size")

# Apply preset
AccessibilityManager.apply_preset("high_visibility")

# Reset to defaults
AccessibilityManager.reset_to_defaults()
```

---

## 📦 Resource Reference

### Creating Resources

#### Region
```gdscript
var region = RegionResource.new(
    "region_id",
    "Region Name",
    "Description",
    5  # required level
)
region.climate = "temperate"
region.danger_level = 3
```

#### Waypoint
```gdscript
var waypoint = WaypointResource.new(
    "waypoint_id",
    "Waypoint Name",
    "region_id",
    Vector3(x, y, z)
)
waypoint.travel_cost = 50
waypoint.required_quest = "quest_id"
```

#### Faction
```gdscript
var faction = FactionResource.new(
    "faction_id",
    "Faction Name",
    "Description"
)
faction.current_reputation = 100
```

#### Recipe
```gdscript
var recipe = RecipeResource.new(
    "recipe_id",
    "Recipe Name",
    "output_item_id",
    "weapon"  # category
)
recipe.add_ingredient("iron_ore", 3)
recipe.required_level = 5
```

#### Specialization
```gdscript
var spec = SpecializationResource.new(
    "spec_id",
    "Spec Name",
    "Description"
)
spec.attack_bonus = 5
spec.add_ability("ability_id", 3)  # unlock at level 3
```

#### Companion
```gdscript
var companion = CompanionResource.new(
    "companion_id",
    "Companion Name",
    "warrior"  # class
)
companion.level = 5
companion.hire_cost = 500
```

#### Seasonal Event
```gdscript
var event = SeasonalEventResource.new(
    "event_id",
    "Event Name",
    "seasonal"  # type
)
event.start_date = "2026-03-01"
event.end_date = "2026-03-31"
event.bonus_xp_multiplier = 1.5
```

---

## 🎯 Common Patterns

### Quest Integration
```gdscript
# Check faction reputation in quest
func can_start_quest() -> bool:
    return FactionManager.has_reputation("rohirrim", 1000)

# Award faction reputation on quest complete
func on_quest_complete():
    FactionManager.add_reputation("rohirrim", 250)
```

### Regional Quests
```gdscript
# Check if in correct region
func check_region_requirement() -> bool:
    var current = RegionManager.get_current_region()
    return current and current.region_id == "rohan"
```

### Crafting Integration
```gdscript
# UI button click handler
func on_craft_button_pressed(recipe_id: String):
    var check = CraftingManager.can_craft(recipe_id)
    if check["can_craft"]:
        CraftingManager.craft_item(recipe_id)
    else:
        show_error(check["reason"])
```

### Event Bonuses
```gdscript
# Apply event bonuses to rewards
func give_quest_reward(base_xp: int, base_gold: int):
    var multipliers = SeasonalEventManager.get_event_multipliers()
    var final_xp = int(base_xp * multipliers["xp"])
    var final_gold = int(base_gold * multipliers["gold"])
    GameManager.add_experience(final_xp)
    GameManager.add_gold(final_gold)
```

### Difficulty Scaling
```gdscript
# Scale enemy stats
func spawn_enemy(base_health: float, base_damage: float):
    var enemy = Enemy.new()
    enemy.max_health = base_health * DifficultyManager.get_enemy_health_multiplier()
    enemy.damage = base_damage * DifficultyManager.get_enemy_damage_multiplier()
```

---

## 🔌 EventBus Signals

### Phase 5 Signals
```gdscript
# Connect to signals
EventBus.region_discovered.connect(_on_region_discovered)
EventBus.waypoint_discovered.connect(_on_waypoint_discovered)
EventBus.fast_travel_started.connect(_on_fast_travel_started)
EventBus.reputation_changed.connect(_on_reputation_changed)

func _on_region_discovered(region_id: String, region_name: String):
    show_notification("Discovered: " + region_name)
```

### Manager Signals
```gdscript
# Crafting signals
CraftingManager.recipe_discovered.connect(_on_recipe_discovered)
CraftingManager.crafting_completed.connect(_on_crafting_completed)

# Companion signals
CompanionManager.companion_hired.connect(_on_companion_hired)
CompanionManager.loyalty_changed.connect(_on_loyalty_changed)

# Event signals
SeasonalEventManager.event_started.connect(_on_event_started)
DifficultyManager.difficulty_changed.connect(_on_difficulty_changed)
```

---

## 💾 Save/Load Integration

All managers have save/load methods:
```gdscript
# Saving
var save_data = {
    "regions": RegionManager.save_data(),
    "fast_travel": FastTravelManager.save_data(),
    "factions": FactionManager.save_data(),
    "crafting": CraftingManager.save_data(),
    "specialization": SpecializationManager.save_data(),
    "companions": CompanionManager.save_data(),
    "events": SeasonalEventManager.save_data(),
    "difficulty": DifficultyManager.save_data(),
    "accessibility": AccessibilityManager.save_data()
}

# Loading
RegionManager.load_data(data["regions"])
FastTravelManager.load_data(data["fast_travel"])
# ... etc
```

---

## 🐛 Common Issues

### Manager Not Found
- Ensure autoload is registered in project.godot
- Check spelling of manager name

### Recipe Not Crafting
- Verify all ingredients in inventory
- Check crafting skill requirement
- Verify recipe is discovered

### Companion Won't Hire
- Check gold amount
- Verify level requirement
- Check quest prerequisites
- Ensure not at max companions

### Event Not Starting
- Check date range
- Verify event is registered
- Check level requirements

---

## 📚 Further Reading

- **PHASE_5_6_7_IMPLEMENTATION_SUMMARY.md** - Detailed technical docs
- **PHASE_8_9_10_ROADMAP.md** - Future features
- **PHASE_5_7_COMPLETE.md** - Executive summary

---

**Last Updated:** January 2026  
**Version:** v0.6.0  
**Status:** Phase 5-7 Complete
