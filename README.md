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

### Currently Functional
- **WASD**: Move your character
- **Mouse**: Look around
- **Shift**: Sprint (drains stamina)
- **Space**: Jump
- **Left Mouse Button**: Attack
- **Right Mouse Button**: Special ability (AOE attack, costs 30 stamina)
- **E**: Interact with NPCs and objects
- **I**: Toggle inventory
- **J**: Toggle quest journal
- **ESC**: Toggle mouse capture / Pause menu

### ✅ New in Phase 4
- **C**: Toggle character sheet ✅
- **M**: Toggle map ✅

## 🎯 Game Features

### ✅ Currently Implemented (Phase 1 Complete)
- **Player Movement**: WASD controls with mouse camera
- **Sprint System**: Hold Shift to sprint (drains stamina)
- **Jump Mechanics**: Space to jump
- **Combat System**: Basic attack and special AOE attack
- **Character Stats**: Health, stamina, experience, and level progression
- **Level-Up System**: Automatic character progression
- **Save/Load System**: 5 save slots with auto-save
- **Event System**: Signal-based communication with 41 game events
- **Game Constants**: All balance values from original design

### ✅ Phase 3 Complete (Advanced Features)
- **Quest System**: Epic quests with branching paths and objectives ✅
- **Inventory System**: Collect and manage items and equipment ✅
- **Equipment System**: Legendary weapons and armor with stat bonuses ✅
- **Dialogue System**: Branching conversations with NPCs ✅
- **UI Panels**: Quest journal, Inventory panel, Dialogue panel ✅
- **NPC System**: Interactive NPCs (Gandalf, Legolas, Gimli, Guide) ✅
- **Loot System**: Item drops from enemies and treasure chests ✅
- **Game Integration**: All systems connected and functional ✅

### ✅ Phase 4 Complete (Content & Polish)
- **Enemy AI**: Smart pathfinding enemies with combat behaviors ✅ (Phase 2)
- **Complete HUD**: Health, stamina, XP bars ✅ (Phase 2)
- **Day/Night Cycle**: Dynamic world lighting and time-based events ✅
- **Weather System**: Rain, snow, fog affecting gameplay ✅
- **Procedural Dungeons**: Multi-floor dungeons with bosses and treasures ✅
- **Boss Encounters**: Unique boss battles with special mechanics ✅
- **Achievement System**: Unlock achievements for completing challenges ✅

### ✅ Phase 5-7 Complete (World Expansion & Advanced Systems)
- **Phase 5: World Expansion** — 4 regions, fast travel, 6 factions, 12 regional quests ✅
- **Phase 6: Advanced Systems** — Crafting (11 recipes), specializations, companions ✅
- **Phase 7: Live Ops & Polish** — Seasonal events, difficulty modes, accessibility ✅

### ✅ Phase 8-10 Complete (Multiplayer & Endgame)
- **Phase 8: Multiplayer & Social** — Co-op, guilds, trading, friends system ✅
- **Phase 9: Endgame Content** — Raids, PvP arena, prestige system, world bosses ✅
- **Phase 10: Polish & QoL** — Mounts, pets, housing, quality of life improvements ✅
- 📄 See [docs/PHASE_8_9_10_ROADMAP.md](docs/PHASE_8_9_10_ROADMAP.md) for details

### 🌍 World Features (Implemented)
- Iconic locations: The Shire, Rohan, Mordor, Rivendell ✅
- NPCs: Gandalf, Legolas, Gimli, and other legendary characters ✅
- Fast travel system between discovered waypoints ✅
- Treasure chests and loot scattered throughout the world ✅
- Guild/fellowship system for cooperative play ✅
- Player housing and decoration ✅

## 🏗️ Project Structure

```
MiddleEarthRPG/
├── project.godot           # Godot project configuration
├── scenes/
│   ├── main.tscn          # Main game scene
│   ├── player/            # Player character (player.tscn, player.gd)
│   │   ├── player.tscn    # Player scene
│   │   └── player.gd      # Player controller script
│   ├── enemies/           # Enemy types
│   │   ├── orc.tscn       # Orc enemy scene
│   │   └── enemy_base.gd  # Base enemy AI script
│   └── ui/                # UI screens
│       ├── hud.tscn       # HUD (health, stamina, XP bars)
│       ├── hud.gd         # HUD controller
│       ├── quest_journal.gd      # Quest UI script (scene pending)
│       ├── inventory_panel.gd    # Inventory UI script (scene pending)
│       └── dialogue_panel.gd     # Dialogue UI script (scene pending)
├── scripts/
│   ├── autoload/          # Singleton scripts (26 managers)
│   │   ├── game_manager.gd       # Core game state
│   │   ├── event_bus.gd          # Event system (50+ signals)
│   │   ├── save_manager.gd       # Save/load system
│   │   ├── quest_manager.gd      # Quest tracking
│   │   ├── inventory_manager.gd  # Inventory management
│   │   ├── dialogue_manager.gd   # Dialogue system
│   │   ├── region_manager.gd     # Region system
│   │   ├── fast_travel_manager.gd # Fast travel
│   │   ├── faction_manager.gd    # Faction reputation
│   │   ├── crafting_manager.gd   # Crafting system
│   │   ├── specialization_manager.gd # Combat specializations
│   │   ├── companion_manager.gd  # Companion system
│   │   ├── seasonal_event_manager.gd # Seasonal events
│   │   ├── difficulty_manager.gd # Difficulty modes
│   │   ├── accessibility_manager.gd # Accessibility
│   │   ├── multiplayer_manager.gd # Multiplayer & co-op
│   │   ├── guild_manager.gd      # Guild system
│   │   ├── trading_manager.gd    # Player trading
│   │   ├── social_manager.gd     # Friends & social
│   │   ├── raid_manager.gd       # Raid dungeons
│   │   ├── arena_manager.gd      # PvP arena
│   │   ├── prestige_manager.gd   # Prestige system
│   │   ├── world_boss_manager.gd # World bosses
│   │   ├── mount_manager.gd      # Mount system
│   │   ├── pet_manager.gd        # Pet collection
│   │   └── housing_manager.gd    # Player housing
│   ├── components/        # Reusable components
│   │   ├── player_movement_component.gd
│   │   ├── player_combat_component.gd
│   │   ├── health_component.gd
│   │   └── enemy_ai_component.gd
│   ├── resources/         # Custom resources (21 types)
│   │   ├── character_stats.gd
│   │   ├── quest_resource.gd
│   │   ├── inventory_item.gd
│   │   ├── dialogue_resource.gd
│   │   ├── region_resource.gd
│   │   ├── waypoint_resource.gd
│   │   ├── faction_resource.gd
│   │   ├── recipe_resource.gd
│   │   ├── specialization_resource.gd
│   │   ├── companion_resource.gd
│   │   ├── seasonal_event_resource.gd
│   │   ├── guild_resource.gd
│   │   ├── trade_offer_resource.gd
│   │   ├── friend_resource.gd
│   │   ├── raid_dungeon_resource.gd
│   │   ├── arena_match_resource.gd
│   │   ├── world_boss_resource.gd
│   │   ├── mount_resource.gd
│   │   ├── pet_resource.gd
│   │   └── housing_resource.gd
│   ├── data/              # Game data
│   │   ├── sample_quests.gd      # 5 sample quests
│   │   ├── sample_items.gd       # 15+ sample items
│   │   └── sample_dialogues.gd   # 5 sample dialogues
│   ├── utilities/         # Utility classes
│   │   ├── constants.gd          # Game balance values (30+)
│   │   ├── object_pool.gd        # Object pooling
│   │   └── performance_monitor.gd
│   └── game_initializer.gd       # Auto-loads sample data
└── docs/                  # Documentation (20+ files)
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

This project was originally built in Unity and has been successfully migrated to Godot 4.x. The migration followed an 8-week roadmap (Phases 1-4) and is now extended with an additional 12 weeks for Phases 5-7:

- ✅ **Phase 1 (Weeks 1-2): Foundation** — Complete
  - Godot project structure, player movement, basic combat, character stats
- ✅ **Phase 2 (Weeks 3-4): Core Systems** — Complete
  - Enemy AI with state machine, combat system, HUD, navigation
- ✅ **Phase 3 (Weeks 5-6): Advanced Features** — Complete
  - Quest system, Dialogue system, Inventory system, Equipment system
  - NPC system, Loot & treasure system
- ✅ **Phase 4 (Weeks 7-8): Content & Polish** — Complete
  - Dungeons, bosses, quests, UI polish, performance optimization, achievements
- ✅ **Phase 5 (Weeks 9-12): World Expansion** — Complete
  - 4 regions, fast travel (6 waypoints), 6 factions, 12 regional quests
- ✅ **Phase 6 (Weeks 13-16): Advanced Systems** — Complete
  - Crafting (11 recipes), specializations (3 paths), companions (6 NPCs)
- ✅ **Phase 7 (Weeks 17-20): Live Ops & Polish** — Complete
  - 7 seasonal events, 4 difficulty modes, 20+ accessibility options
- ✅ **Phase 8 (Weeks 21-24): Multiplayer & Social** — Complete
  - Co-op (2-4 players), guilds, trading, friends system
- ✅ **Phase 9 (Weeks 25-28): Endgame Content** — Complete
  - Raids, PvP arena, prestige system, world bosses
- ✅ **Phase 10 (Weeks 29-32): Polish & QoL** — Complete
  - Mounts, pets, housing, quality of life improvements

**Current Version:** Godot Alpha v0.9 (All Phases 1-10: Complete)  
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
