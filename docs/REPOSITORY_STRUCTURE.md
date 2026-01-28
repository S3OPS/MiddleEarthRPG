# Middle-earth Adventure RPG - Repository Structure

**Last Updated:** January 2026  
**Version:** 2.0.0 (Enhanced Edition)

---

## Overview

This repository contains a complete, production-ready 3D RPG game built in Unity 2022.3 LTS. The game features a Lord of the Rings inspired fantasy world with comprehensive RPG systems including character progression, active combat, quests, achievements, equipment, and more.

---

## Directory Structure

```
MiddleEarthRPG/
│
├── Assets/
│   ├── Scripts/
│   │   ├── RPG/                    # Core RPG systems (16 scripts)
│   │   │   ├── CharacterStats.cs          # Character progression & stats
│   │   │   ├── CombatSystem.cs            # Active combat with combos & crits
│   │   │   ├── EquipmentSystem.cs         # Equipment & stat bonuses
│   │   │   ├── InventorySystem.cs         # Items & gold management
│   │   │   ├── Quest.cs                   # Quest data structure
│   │   │   ├── QuestManager.cs            # Quest management & tracking
│   │   │   ├── AchievementSystem.cs       # Achievement tracking
│   │   │   ├── AudioManager.cs            # Procedural sound generation
│   │   │   ├── EffectsManager.cs          # Particle effects & visuals
│   │   │   ├── MinimapSystem.cs           # Top-down minimap
│   │   │   ├── Enemy.cs                   # Enemy AI & behavior
│   │   │   ├── NPC.cs                     # NPC interactions
│   │   │   ├── LocationTrigger.cs         # Location discovery
│   │   │   ├── TreasureChest.cs           # Loot chests
│   │   │   ├── EquipmentChest.cs          # Equipment loot
│   │   │   └── RPGSystemsDemo.cs          # Demo/testing script
│   │   │
│   │   ├── Player/                 # Player controls (2 scripts)
│   │   │   ├── PlayerController.cs        # WASD movement, sprint, jump
│   │   │   └── CameraLook.cs              # Mouse look camera
│   │   │
│   │   ├── Config/                 # Configuration (2 scripts)
│   │   │   ├── RPGConfig.cs               # Config data structure
│   │   │   └── ConfigLoader.cs            # JSON config loader
│   │   │
│   │   └── RPGBootstrap.cs         # Main game initialization
│   │
│   └── StreamingAssets/
│       └── rpg_config.json         # Tunable game parameters
│
├── ProjectSettings/                # Unity project configuration
│   ├── ProjectVersion.txt          # Unity version (2022.3 LTS)
│   └── ...                         # Other Unity settings
│
├── docs/                           # Comprehensive documentation
│   ├── ENHANCEMENT_PLAN.md         # Detailed enhancement roadmap
│   ├── REPOSITORY_STRUCTURE.md     # This file
│   ├── GAME_DESIGN.md              # Complete game design document
│   ├── IMPLEMENTATION_SUMMARY.md   # Implementation details
│   ├── PLAYER_EXPERIENCE.md        # Player walkthrough
│   └── SETUP.md                    # Installation & setup guide
│
├── tools/
│   └── install.ps1                 # One-command installation script
│
├── README.md                       # Main project README
├── CHANGELOG.md                    # Version history & changes
├── .gitignore                      # Git ignore rules
└── .editorconfig                   # Code formatting config
```

---

## Core Systems Overview

### 1. RPG Systems (`Assets/Scripts/RPG/`)

The heart of the game, containing all RPG mechanics:

#### Character & Progression
- **CharacterStats.cs** (~200 LOC)
  - Health, Stamina, Strength, Wisdom, Agility
  - Experience and leveling (XP scaling: 1.5x per level)
  - Level-up bonuses: +20 HP, +10 Stamina, +2 all stats
  - Damage, healing, stamina management

#### Combat
- **CombatSystem.cs** (~350 LOC)
  - Left-click: Basic attack (3 unit range)
  - Right-click: AOE special (4 unit radius, 30 stamina cost)
  - Combo system: +20% damage per combo hit
  - Critical hits: 15% base + agility bonus, 2x damage
  - Cooldowns: 0.5s attack, 5s special

- **Enemy.cs** (~300 LOC)
  - Patrol behavior (random wandering)
  - Chase behavior (10 unit detection range)
  - Flee behavior (below 20% health)
  - Attack behavior (5 unit range)
  - Quest progress integration

#### Items & Equipment
- **InventorySystem.cs** (~250 LOC)
  - 5 item types: QuestItem, Weapon, Armor, Potion, Treasure
  - Gold currency system
  - Item stacking and management
  - Add/remove/check operations

- **EquipmentSystem.cs** (~300 LOC)
  - 3 slots: Weapon, Armor, Accessory
  - 5 rarity tiers: Common → Uncommon → Rare → Epic → Legendary
  - Stat bonuses: Attack, Defense, Health, Stamina
  - Auto-equip for empty slots

- **TreasureChest.cs** (~100 LOC)
  - Collision-based opening
  - Gold + item rewards
  - Visual feedback (color change)

- **EquipmentChest.cs** (~150 LOC)
  - Equipment-specific loot
  - Achievement integration
  - Legendary item drops

#### Quests
- **Quest.cs** (~150 LOC)
  - Multi-objective quest structure
  - 4 objective types: Collect, Defeat, Explore, Talk
  - Progress tracking and completion %
  - Reward system (gold, XP, items)

- **QuestManager.cs** (~400 LOC)
  - 7 LOTR-themed quests
  - Quest state management
  - Objective updates from game events
  - Completion and reward distribution

#### Achievements
- **AchievementSystem.cs** (~350 LOC)
  - 12 achievements across categories:
    - Combat: First Blood, Orc Slayer, Legendary Warrior
    - Treasure: Treasure Hunter, Dragon's Hoard
    - Exploration: Explorer, Fellowship Complete, Quest Master
    - Progression: Maximum Power, Combo Master
    - Mastery: Heavy Hitter, Fully Equipped
  - Unlock detection and tracking
  - Audio/visual feedback on unlock

#### Audio & Effects
- **AudioManager.cs** (~400 LOC)
  - Procedural sound generation (sine waves)
  - 10 audio sources (pooling)
  - Sound types: Combat, Progression, Interaction
  - Volume controls (master, music, SFX)

- **EffectsManager.cs** (~350 LOC)
  - Particle effects: Hit, special, level-up, treasure, quest
  - Floating damage numbers
  - Particle physics simulation
  - Visual feedback for all game events

#### UI & Navigation
- **MinimapSystem.cs** (~150 LOC)
  - 200x200 top-down minimap
  - Orthographic camera at 50 units height
  - Real-time player tracking
  - RenderTexture-based rendering

#### World Interaction
- **NPC.cs** (~100 LOC)
  - 3 NPCs: Gandalf, Legolas, Gimli
  - Trigger-based dialogue
  - Quest activation on interaction

- **LocationTrigger.cs** (~100 LOC)
  - 3 locations: The Shire, Plains of Rohan, Lands of Mordor
  - Discovery XP rewards
  - Quest objective completion
  - One-time visit tracking

---

### 2. Player Systems (`Assets/Scripts/Player/`)

First-person player controls:

- **PlayerController.cs**
  - WASD movement
  - Shift sprint (1.8x speed multiplier)
  - Space jump
  - Ground detection
  - Configurable speeds from rpg_config.json

- **CameraLook.cs**
  - Mouse-based camera rotation
  - Configurable sensitivity
  - Vertical angle clamping (-90° to 90°)

---

### 3. Configuration (`Assets/Scripts/Config/`)

JSON-based configuration system:

- **RPGConfig.cs**
  - Data structure for config parameters
  - Character name, starting gold
  - Movement speeds, mouse sensitivity

- **ConfigLoader.cs**
  - Loads rpg_config.json from StreamingAssets
  - Default values fallback
  - Error handling

- **rpg_config.json**
  ```json
  {
    "characterName": "Aragorn",
    "startingGold": 100,
    "moveSpeed": 6.0,
    "sprintMultiplier": 1.8,
    "mouseSensitivity": 2.0
  }
  ```

---

### 4. Bootstrap System

- **RPGBootstrap.cs** (Main entry point)
  - [RuntimeInitializeOnLoadMethod] auto-loading
  - Singleton pattern for performance
  - World generation (terrain, lighting, fog)
  - System initialization order:
    1. Character stats
    2. Inventory
    3. Quest manager
    4. Achievement system
    5. Audio manager
    6. Effects manager
    7. Combat system
    8. Equipment system
    9. Minimap
  - Player spawning and setup
  - HUD rendering (OnGUI)
  - NPC, enemy, treasure, location creation

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
- **docs/SETUP.md** - Installation guide, gameplay tips, troubleshooting
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
1. Open Unity Hub
2. Add project (Unity 2022.3 LTS)
3. Open Main scene
4. Press Play

**Modify game balance:**
- Edit `Assets/StreamingAssets/rpg_config.json`
- Or change constants in respective C# files

**Add documentation:**
- Add markdown files to `docs/` folder
- Link from README.md or GAME_DESIGN.md

**Build for Windows:**
- File → Build Settings
- Platform: Windows
- Build & Run

---

## Troubleshooting

### Common Issues

1. **Unity version mismatch**
   - Solution: Use Unity 2022.3 LTS (specified in ProjectVersion.txt)

2. **Scene appears empty**
   - Normal! World builds at runtime via RPGBootstrap

3. **Config not loading**
   - Check rpg_config.json is in StreamingAssets folder
   - Verify JSON syntax is valid

4. **Performance issues**
   - Check enemy count (17+ can be intensive)
   - Reduce particle counts in EffectsManager
   - Disable minimap if needed

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
