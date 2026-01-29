# Middle-earth Adventure RPG - Repository Structure

## ⚠️ DOCUMENT STATUS

**This document describes the current Godot implementation.**

**Current Version:** Godot 4.3+ (Active Development)  
**Legacy Version:** Unity v2.0.0 (Fully migrated to Godot)

---

## Overview

This repository contains a 3D RPG game inspired by Lord of the Rings, featuring character progression, active combat, quests, achievements, and equipment systems.

**Current State:** The project has been migrated from Unity to Godot Engine for a truly free and open-source experience.

---

## Active Godot Project Structure

```
MiddleEarthRPG/
│
├── project.godot              # Godot project configuration
├── icon.svg                   # Project icon
│
├── scenes/                    # Godot scenes (.tscn files)
│   ├── main.tscn             # Main game scene
│   ├── player/               # Player character scenes
│   ├── world/                # World elements
│   ├── ui/                   # UI scenes (planned)
│   └── systems/              # Game system scenes
│
├── scripts/                   # GDScript and C# scripts
│   ├── autoload/             # Singleton autoload scripts
│   │   ├── game_manager.gd  # Core game manager
│   │   ├── event_bus.gd     # Global event system
│   │   └── save_manager.gd  # Save/load system
│   │
│   ├── resources/            # Custom Godot resources
│   │   └── character_stats.gd
│   │
│   └── utilities/
│       └── constants.gd      # Game constants and balance values
│
├── assets/                    # Game assets (models, textures, audio)
│
├── docs/                      # Documentation
│   ├── GETTING_STARTED.md    # Complete setup and installation guide
│   ├── ALTERNATIVE_ENGINES.md # Migration story
│   ├── GAME_DESIGN.md        # Game design document
│   ├── REPOSITORY_STRUCTURE.md # This file
│   └── [Additional documentation...]
```

---

## Core Systems

The game has been fully migrated to Godot Engine. For detailed information about the current Godot implementation, see:

- **scripts/autoload/** - Core managers and systems (GameManager, EventBus, SaveManager, etc.)
- **scripts/resources/** - Custom resource definitions (CharacterStats, QuestResource, etc.)
- **scripts/components/** - Reusable components for game entities
- **scripts/data/** - Sample game data (quests, items, dialogues)
- **scripts/utilities/** - Utility classes (Constants, ObjectPool, etc.)
- **scenes/** - Godot scene files for the game world, player, UI, enemies, etc.

For a complete guide to the Godot implementation, see the main [README.md](../README.md) and [GETTING_STARTED.md](GETTING_STARTED.md).

---

## Legacy Unity Implementation

The original Unity implementation has been fully migrated to Godot. Historical documentation about the Unity version can be found in:
- [THE_ONE_RING.md](THE_ONE_RING.md) - Unity version roadmap and status (archived)
- [CODE_AUDIT.md](CODE_AUDIT.md) - Unity code quality audit (archived)
- [ENHANCEMENT_PLAN.md](ENHANCEMENT_PLAN.md) - Unity enhancement roadmap (archived)

---

## Game Content

### Quests (7 Total)

1. **The Shire's Burden** (Easy)
   - Gather 3 Lembas Bread
   - Explore the Old Forest
   - Rewards: 100 Gold, 150 XP

2. **Fellowship of the Ring** (Easy)
   - Speak with Gandalf, Legolas, Gimli
   - Rewards: 150 Gold, 200 XP

3. **Riders of Rohan** (Medium)
   - Defeat 5 Orc Scouts
   - Survey Plains of Rohan
   - Rewards: 200 Gold, 300 XP

4. **The Path to Mordor** (Hard)
   - Enter Lands of Mordor
   - Defeat 10 servants of darkness
   - Find Ring of Power
   - Rewards: 500 Gold, 1000 XP

5. **Master of Arms** (Medium)
   - Equip legendary weapon
   - Achieve 10-hit combo
   - Defeat 15 enemies
   - Rewards: 300 Gold, 500 XP

6. **Treasure Seeker** (Easy)
   - Open 5 treasure chests
   - Collect 500 gold
   - Rewards: 250 Gold, 400 XP

7. **Legend of Middle-earth** (Very Hard)
   - Reach level 10
   - Discover all locations
   - Defeat 25 enemies
   - Rewards: 1000 Gold, 2000 XP

### Locations (3 Regions)

- **The Shire** (Southwest, Green terrain)
  - Peaceful starting area
  - Treasure chests with quest items
  
- **Plains of Rohan** (East, Golden terrain)
  - Mid-game combat zone
  - 5 Orc Scouts patrolling

- **Lands of Mordor** (North, Dark terrain)
  - End-game challenge area
  - 12+ Dark Servants
  - Legendary equipment

### NPCs (3 Characters)

- **Gandalf the Grey** - Quest giver, Fellowship member
- **Legolas** - Elven archer, Fellowship member
- **Gimli** - Dwarven warrior, Fellowship member

### Enemies (17+ Total)

- **Orc Scouts** (5) - 50 HP, 10 DMG, Plains of Rohan
- **Dark Servants** (12+) - 50 HP, 10 DMG, Lands of Mordor

### Equipment (Legendary Items)

- **Andúril** - +25 ATK, +20 HP (Legendary Weapon)
- **Mithril Coat** - +30 DEF, +50 HP (Legendary Armor)
- **Elven Blade** - +15 ATK, +10 STA (Rare Weapon)
- **Elven Cloak** - +15 DEF, +20 STA (Epic Armor)
- **Ring of Power** - +20 ATK, +20 DEF, +100 HP, +50 STA (Legendary Accessory)

---

## Documentation Files

### Player-Facing Documentation

- **README.md** - Main overview, features, controls, quick start
- **docs/GETTING_STARTED.md** - Complete installation, setup, and gameplay guide
- **docs/PLAYER_EXPERIENCE.md** - Visual walkthrough of gameplay

### Developer Documentation

- **docs/GAME_DESIGN.md** - Complete technical design document
- **docs/IMPLEMENTATION_SUMMARY.md** - What was built, statistics, features
- **docs/REPOSITORY_STRUCTURE.md** - This file (codebase navigation)
- **docs/ENHANCEMENT_PLAN.md** - Future roadmap with 50+ enhancements

### History

- **CHANGELOG.md** - Version history, v1.0 → v2.0 changes

---

## Architecture Patterns

### Singleton Pattern

Used for all manager classes (RPGBootstrap, QuestManager, AchievementSystem, AudioManager, etc.):

```csharp
public class ExampleManager
{
    private static ExampleManager instance;
    public static ExampleManager Instance
    {
        get
        {
            if (instance == null)
                instance = new ExampleManager();
            return instance;
        }
    }
}
```

### Auto-Initialization

Main bootstrap uses Unity's RuntimeInitializeOnLoadMethod:

```csharp
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
public static void Initialize()
{
    if (Instance != null) return; // Singleton check
    Instance = new RPGBootstrap();
    Instance.CreateWorld();
}
```

### Component-Based

GameObjects with attached scripts for behaviors:
- Player → PlayerController + CameraLook
- Enemies → Enemy component
- NPCs → NPC component
- Chests → TreasureChest / EquipmentChest component

---

## Code Statistics

- **Total C# Scripts:** 23
- **Total Lines of Code:** ~3,500 (excluding comments/blanks)
- **Core RPG Systems:** 16 scripts, ~3,050 LOC
- **Player Systems:** 2 scripts, ~150 LOC
- **Config System:** 2 scripts, ~100 LOC
- **Bootstrap:** 1 script, ~600 LOC

### Code Quality Metrics

- ✅ **Zero TODO comments** - All planned features implemented
- ✅ **Consistent naming** - Clear, descriptive names throughout
- ✅ **Single responsibility** - Each class has one clear purpose
- ✅ **Proper null checks** - Safe code with bounds validation
- ✅ **Documentation** - XML comments where needed

---

## Build Information

### Requirements

- **Unity Version:** 2022.3 LTS (specified in ProjectSettings/ProjectVersion.txt)
- **Platform:** Windows 10/11 (primary target)
- **.NET Version:** .NET Standard 2.1
- **Scripting Backend:** Mono

### Build Process

1. Open project in Unity Hub (2022.3 LTS)
2. Open Main scene (auto-builds at runtime)
3. File → Build Settings → Add Open Scenes
4. Build & Run

### Runtime Behavior

- Scene is empty on load
- RPGBootstrap executes via [RuntimeInitializeOnLoadMethod]
- Entire world builds procedurally:
  - Terrain generation
  - Lighting setup
  - NPC placement
  - Enemy spawning
  - Treasure chest creation
  - Player spawn and initialization
- No Unity scene editing required!

---

## Configuration Files

### Unity Project Settings

- **ProjectSettings/ProjectVersion.txt** - Unity version lock
- **ProjectSettings/InputManager.asset** - Input configuration
- **ProjectSettings/QualitySettings.asset** - Graphics quality presets

### Code Formatting

- **.editorconfig** - Consistent code formatting across editors
  - Indentation: 4 spaces
  - Line endings: CRLF (Windows)
  - Charset: UTF-8

### Version Control

- **.gitignore** - Excludes Unity temp files, builds, user settings

---

## Quick Reference

### Adding New Features

| Feature | Files to Modify | Systems to Update |
|---------|----------------|-------------------|
| **New Quest** | QuestManager.cs | CreateLOTRQuests() method |
| **New NPC** | RPGBootstrap.cs | SetupWorld() → CreateNPC() |
| **New Enemy** | RPGBootstrap.cs | SetupWorld() → CreateEnemy() |
| **New Location** | RPGBootstrap.cs | SetupWorld() → CreateLocationTrigger() |
| **New Item** | InventorySystem.cs | ItemType enum, treasure chest rewards |
| **New Equipment** | EquipmentSystem.cs | Item creation in chests |
| **New Achievement** | AchievementSystem.cs | InitializeAchievements() |
| **New Sound** | AudioManager.cs | PlaySound() switch cases |

### Common Tasks

**Run the game:**
1. Install Godot 4.3+ from https://godotengine.org/
2. Open Godot and import the project
3. Select `project.godot`
4. Press F5 or click "Run Project"

**Modify game balance:**
- Edit `scripts/utilities/constants.gd`
- All balance values are defined in this single file

**Add documentation:**
- Add markdown files to `docs/` folder
- Link from README.md or GAME_DESIGN.md

**Build for Windows:**
- Project → Export
- Add export preset for Windows
- Export project

---

## Troubleshooting

### Common Issues

1. **Godot version issues**
   - Solution: Use Godot 4.3 or later

2. **Scene not loading**
   - Check that scenes/ directory is intact
   - Verify project.godot file exists

3. **Performance issues**
   - Check enemy count in scene
   - Adjust quality settings in Godot project settings
   - Use the performance monitor in-game

---

## Future Development

See **docs/ENHANCEMENT_PLAN.md** for comprehensive roadmap including:
- 50+ enhancement ideas
- Priority framework (Quick Wins → Strategic Investments)
- 6-phase implementation roadmap
- Technical recommendations
- Risk assessment

### Immediate Next Steps

1. Quest Journal UI
2. Character Sheet / Equipment Screen
3. Merchant System
4. Save/Load System
5. Skill Tree System

---

## Additional Resources

- **Unity Documentation:** https://docs.unity3d.com/2022.3/Documentation/Manual/
- **C# Documentation:** https://learn.microsoft.com/en-us/dotnet/csharp/
- **Game Design Patterns:** https://gameprogrammingpatterns.com/

---

**Document Maintained By:** Development Team  
**Last Updated:** January 2026  
**Repository:** https://github.com/S3OPS/MiddleEarthRPG
