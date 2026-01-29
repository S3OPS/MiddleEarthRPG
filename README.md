# Middle-earth Adventure RPG

An immersive 3D RPG game set in a Lord of the Rings inspired fantasy world. Embark on epic quests, battle fearsome enemies, and explore the legendary lands of Middle-earth.

**🎮 Built with Godot Engine** — This project has been migrated from Unity to Godot 4.x for a truly free and open-source experience.

## 🆕 New to This Game? Start Here!

**Never used Godot before?** No problem! We have a complete step-by-step guide:

👉 **[Getting Started Guide](docs/GETTING_STARTED.md)** — Complete installation, setup, and gameplay guide

## 📚 Why Godot?

We've successfully migrated this project from Unity to Godot Engine. Learn more about our journey:

👉 **[Migration Story](docs/ALTERNATIVE_ENGINES.md)** — Why we chose Godot and how the migration went

---

## 🚀 Quick Start

### Requirements
- **Godot 4.3 or later** (free download from https://godotengine.org/)
- No additional dependencies or accounts needed!

### Running the Game
1. Download and install Godot 4.3+ from https://godotengine.org/
2. Clone or download this repository
3. Open Godot and click "Import"
4. Navigate to the project folder and select `project.godot`
5. Click "Import & Edit"
6. Press **F5** or click "Run Project" to start the game

**Complete installation guide:** See [docs/GETTING_STARTED.md](docs/GETTING_STARTED.md)

## 🎮 Controls
- **WASD**: Move your character
- **Mouse**: Look around
- **Shift**: Sprint (drains stamina)
- **Space**: Jump
- **Left Mouse Button**: Attack
- **Right Mouse Button**: Special ability (AOE attack, costs 30 stamina)
- **I**: Toggle inventory (coming soon)
- **C**: Toggle character sheet (coming soon)
- **J**: Toggle quest journal (coming soon)
- **M**: Toggle map (coming soon)
- **ESC**: Toggle mouse capture / Pause menu

## 🎯 Game Features

### ✅ Currently Implemented (Phase 1 Complete)
- **Player Movement**: WASD controls with mouse camera
- **Sprint System**: Hold Shift to sprint (drains stamina)
- **Jump Mechanics**: Space to jump
- **Combat System**: Basic attack and special AOE attack
- **Character Stats**: Health, stamina, experience, and level progression
- **Level-Up System**: Automatic character progression
- **Save/Load System**: 5 save slots with auto-save
- **Event System**: Signal-based communication for game events
- **Game Constants**: All balance values from original design

### 🎯 Coming Soon (Phase 3 - In Progress)
- **Quest System**: Epic quests with branching paths and objectives ✅
- **Inventory System**: Collect and manage items and equipment ✅
- **Equipment System**: Legendary weapons and armor with stat bonuses ✅
- **Dialogue System**: Branching conversations with NPCs ✅
- **Complete UI**: Quest journal ✅, Inventory panel ✅, Dialogue panel ✅
- Loot drops from enemies (in progress)
- NPC characters (in progress)
- Treasure chests (in progress)

### 🎯 Coming Soon (Phases 3-4)
- **Enemy AI**: Smart pathfinding enemies with combat behaviors ✅ (Phase 2)
- **Complete HUD**: Health, stamina, XP bars ✅ (Phase 2)
- **Day/Night Cycle**: Dynamic world lighting and time-based events
- **Weather System**: Rain, snow, fog affecting gameplay
- **Procedural Dungeons**: Multi-floor dungeons with bosses and treasures
- **Boss Encounters**: Unique boss battles with special mechanics
- **Achievement System**: Unlock achievements for completing challenges

### 🌍 Planned World Features
- Iconic locations: The Shire, Plains of Rohan, Lands of Mordor
- NPCs: Meet Gandalf, Legolas, Gimli, and other legendary characters
- Fast travel system between discovered waypoints
- Treasure chests and loot scattered throughout the world

## 🏗️ Project Structure

```
MiddleEarthRPG/
├── project.godot           # Godot project configuration
├── scenes/
│   ├── main.tscn          # Main game scene
│   ├── player/            # Player character scenes
│   ├── enemies/           # Enemy types (coming soon)
│   ├── world/             # World elements
│   ├── ui/                # UI screens (coming soon)
│   └── systems/           # Game systems
├── scripts/
│   ├── autoload/          # Singleton scripts (GameManager, EventBus, SaveManager)
│   ├── resources/         # Custom resources (CharacterStats)
│   └── utilities/         # Utilities (Constants)
├── assets/                # Game assets (models, textures, audio)
├── docs/                  # Documentation
└── [Legacy Unity Files]   # Original Unity implementation kept for reference
    ├── Assets/            # Unity C# scripts (reference only)
    ├── ProjectSettings/   # Unity settings (reference only)
    └── tools/             # Unity build tools (deprecated)
```

## 📖 Documentation

- **[GETTING_STARTED.md](docs/GETTING_STARTED.md)** — Complete installation, setup, and gameplay guide
- **[GAME_DESIGN.md](docs/GAME_DESIGN.md)** — Complete game design document
- **[ALTERNATIVE_ENGINES.md](docs/ALTERNATIVE_ENGINES.md)** — Story of our migration from Unity to Godot
- **[REPOSITORY_STRUCTURE.md](docs/REPOSITORY_STRUCTURE.md)** — Codebase navigation guide

### Legacy Unity Documentation (Archived)
The following documents describe the original Unity implementation, kept for reference:
- **[THE_ONE_RING.md](docs/THE_ONE_RING.md)** — Unity version roadmap and status (archived)
- **[CODE_AUDIT.md](docs/CODE_AUDIT.md)** — Unity code quality audit (archived)
- **[ENHANCEMENT_PLAN.md](docs/ENHANCEMENT_PLAN.md)** — Unity enhancement roadmap (archived)

## 🔄 Migration Status

This project was originally built in Unity and has been successfully migrated to Godot 4.x. The migration is following an 8-week roadmap:

- ✅ **Phase 1 (Weeks 1-2): Foundation** — Complete
  - Godot project structure, player movement, basic combat, character stats
- ✅ **Phase 2 (Weeks 3-4): Core Systems** — Complete
  - Enemy AI with state machine, combat system, HUD, navigation
- 🎯 **Phase 3 (Weeks 5-6): Advanced Features** — In Progress (65% complete)
  - Quest system ✅, Dialogue system ✅, Inventory system ✅, Equipment system ✅
  - Sample data ✅, UI panels (scripts complete, scenes pending)
- 📅 **Phase 4 (Weeks 7-8): Content & Polish** — Planned
  - Dungeons, bosses, quests, UI polish, performance optimization

**Current Version:** Godot Alpha v0.3 (Phase 3 in progress)  
**Original Unity Version:** v3.1 (archived in legacy files)

## 🤝 Contributing

We welcome contributions! Whether you're:
- Adding new features to the Godot implementation
- Improving documentation
- Reporting bugs or suggesting enhancements
- Creating assets or content

Please feel free to open issues or submit pull requests.

## 📜 License

This project is open source. The original Unity implementation and all assets are available in the repository for reference and learning purposes.

## 🎮 About Godot Engine

Godot is a free and open-source game engine released under the MIT license. It provides a huge set of common tools, so you can focus on making your game without reinventing the wheel. Learn more at https://godotengine.org/

---

**Note:** The `Assets/` folder and Unity-specific files are kept in the repository as reference material for the migration. The active development is now in the Godot project files (`project.godot`, `scenes/`, `scripts/`).
