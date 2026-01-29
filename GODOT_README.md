# Middle-earth Adventure RPG - Godot Edition

## 🎮 About

This is the Godot 4.x implementation of Middle-earth Adventure RPG, ported from the Unity version. An immersive 3D RPG game set in a Lord of the Rings inspired fantasy world.

## 🚀 Current Implementation Status

### ✅ Phase 1: Foundation (COMPLETE)
- [x] Godot project structure created
- [x] Autoload singletons (GameManager, EventBus, SaveManager)
- [x] Constants.gd with all game balance values from Unity
- [x] Player character with movement, camera, and combat
- [x] Basic terrain and environment scene
- [x] CharacterStats resource system

### 🎯 Next Steps: Phase 2 - Core Systems
- [ ] Enemy AI with NavigationAgent3D
- [ ] Enhanced combat system with visual effects
- [ ] Inventory system
- [ ] Quest system
- [ ] UI (HUD, inventory, character sheet)

## 🎮 Controls

- **WASD**: Move character
- **Mouse**: Look around
- **Shift**: Sprint (drains stamina)
- **Space**: Jump
- **Left Click**: Attack
- **Right Click**: Special Attack (AOE, costs 30 stamina)
- **I**: Toggle inventory (coming soon)
- **C**: Toggle character sheet (coming soon)
- **J**: Toggle quest journal (coming soon)
- **M**: Toggle map (coming soon)
- **ESC**: Toggle mouse capture

## 🏗️ Project Structure

```
MiddleEarthRPG/
├── project.godot           # Godot project file
├── scenes/
│   ├── main.tscn          # Main game scene
│   ├── player/            # Player character
│   ├── enemies/           # Enemy types
│   ├── world/             # World elements
│   ├── ui/                # UI screens
│   └── systems/           # Game systems
├── scripts/
│   ├── autoload/          # Singleton scripts
│   │   ├── game_manager.gd
│   │   ├── event_bus.gd
│   │   └── save_manager.gd
│   ├── resources/         # Custom resources
│   │   └── character_stats.gd
│   └── utilities/
│       └── constants.gd   # Game balance values
├── assets/                # Game assets
└── data/                  # Configuration files
```

## 🔧 Development

### Requirements
- Godot 4.3 or later
- No additional dependencies needed

### Running the Game
1. Open the project in Godot 4.x
2. Press F5 or click "Run Project"
3. The game will start in the main scene

### Reference Implementation
All game balance values, mechanics, and features are based on the Unity implementation in the parent directory. See `Assets/Scripts/` for the original C# code.

## 📊 Game Features

### Implemented
- ✅ Player movement with WASD + mouse camera
- ✅ Sprint system with stamina drain
- ✅ Jump mechanics
- ✅ Basic combat (attack + special AOE attack)
- ✅ Character stats (health, stamina, XP, level)
- ✅ Level-up progression system
- ✅ Save/load system (5 slots)
- ✅ Event bus for signal-based communication
- ✅ Game balance constants from Unity version

### Coming Soon (Phases 2-4)
- 🎯 Enemy AI with pathfinding
- 🎯 Inventory and equipment systems
- 🎯 Quest system with branching paths
- 🎯 Dialogue system
- 🎯 Complete UI (HUD, menus, journal)
- 🎯 Day/night cycle
- 🎯 Weather system
- 🎯 Procedural dungeons
- 🎯 Boss encounters

## 📝 Notes

This is an active port in progress. Features are being implemented following the 8-week roadmap outlined in `docs/ALTERNATIVE_ENGINES.md`.

The Godot implementation aims to maintain feature parity with the Unity version while taking advantage of Godot's simpler architecture, built-in features, and more intuitive patterns.
