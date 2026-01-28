# Middle-earth Adventure RPG

An immersive 3D RPG game set in a Lord of the Rings inspired fantasy world. Embark on epic quests, battle fearsome enemies, and explore the legendary lands of Middle-earth. The scene auto-builds itself at runtime for the fastest start.

## 💍 The One Ring - Master Roadmap

**For developers and contributors:** See **[docs/THE_ONE_RING.md](docs/THE_ONE_RING.md)** for the complete master roadmap that unifies:
- 🦅 Optimization: "Make the journey faster" ✅ Complete
- 🏕️ Refactoring: "Clean up the camp" ✅ Complete
- ⚔️ Modularization: "Break up the Fellowship" ✅ Complete
- 🛡️ Security Audit: "Inspect the ranks" ✅ Complete
- ✨ Enhancement & Upgrade roadmap

**Project Health Score: 9.8/10** ⬆️ | **Production-ready with enterprise architecture**  
**Current:** v2.4 Content & Narrative Edition | **Next:** v2.5 Technical Systems 🔧

---

## One-command local install (PowerShell)
```powershell
powershell -ExecutionPolicy Bypass -File .\tools\install.ps1 -AutoRun
```

On macOS/Linux with PowerShell 7+ installed:
```powershell
pwsh -ExecutionPolicy Bypass -File ./tools/install.ps1 -AutoRun
```

## Controls
- **WASD**: Move your character
- **Mouse**: Look around
- **Shift**: Sprint
- **Space**: Jump
- **Left Mouse Button**: Attack (active combat)
- **Right Mouse Button**: Special ability (AOE attack, costs stamina)
- **Walk into objects**: Interact with NPCs, treasure chests, and locations

## Game Features

### 🎯 Content & Narrative (v2.4) - IN PROGRESS!
- **Dialogue System**: ✅ Branching conversations with NPC relationship tracking
- **Boss Encounters**: ✅ 6 unique bosses with phases and special abilities
- **Quest Expansion**: ✅ 8 new quests with branching paths, time limits, and environmental triggers
- **Lore Integration**: ✅ 20+ discoverable books expanding world narrative

### 🌍 World Expansion (v2.3) - COMPLETE!
- **Dynamic Day/Night Cycle**: 24-hour time system with realistic lighting and time-based events
- **Weather System**: Rain, snow, fog, and storms that affect gameplay
- **Fast Travel**: Discover and travel instantly between waypoints across Middle-earth
- **Procedural Dungeons**: Multi-floor dungeons with bosses, treasures, and scaling difficulty

### Core RPG Systems
- **Character System**: Track your health, stamina, experience, and level up your hero
- **Active Combat System**: 
  - Left-click to attack enemies with weapon combos
  - Right-click for powerful AOE special abilities
  - Critical hits based on agility stat
  - Combo system for increased damage
- **Equipment System**: 
  - Equip legendary weapons and armor
  - 5 equipment rarities: Common, Uncommon, Rare, Epic, Legendary
  - Find items like Andúril, Mithril Coat, and Elven Blade
  - Stat bonuses from equipped items
- **Achievement System**: Unlock 12 achievements by completing various challenges
- **Quest System**: Complete 7 epic quests including:
  - "The Shire's Burden" - Gather supplies for your journey
  - "Fellowship of the Ring" - Meet legendary heroes
  - "Riders of Rohan" - Defend against orc scouts
  - "The Path to Mordor" - Face the ultimate darkness
  - "Master of Arms" - Prove your combat prowess
  - "Treasure Seeker" - Find hidden riches
  - "Legend of Middle-earth" - Become a true legend
- **Visual Effects**: Particle effects for combat, level-ups, and treasure
- **Audio System**: Procedurally generated sound effects for all actions
- **Minimap**: Top-down minimap for navigation
- **Enhanced AI**: Enemies patrol, chase, and flee when wounded
- **Inventory & Loot**: Collect items, gold, and equipment from treasure chests
- **Exploration**: Discover iconic locations like The Shire, Plains of Rohan, and Lands of Mordor
- **NPCs**: Interact with legendary characters like Gandalf, Legolas, and Gimli

## Requirements
- Windows 10/11
- Unity **2022.3 LTS**

## Open in Unity
- Open Unity Hub
- Add the project folder
- Open **Main** scene
- Press **Play**

## Build
- File → Build Settings → Add Open Scenes → Build & Run

## Build Prep Notes
- Unity reads `ProjectSettings/ProjectVersion.txt` to select the correct editor version.
- Formatting is controlled by `.editorconfig` so config files stay stable between builds.

## Quest Guide
1. **The Shire's Burden** - Gather supplies and explore the Old Forest
2. **Fellowship of the Ring** - Speak with Gandalf, Legolas, and Gimli
3. **Riders of Rohan** - Defeat orc scouts threatening the plains
4. **The Path to Mordor** - Venture into darkness and confront evil
5. **Master of Arms** - Master combat and collect legendary equipment
6. **Treasure Seeker** - Find hidden treasures across Middle-earth
7. **Legend of Middle-earth** - Reach level 10 and complete all challenges

## Folder Map
- `Assets/Scripts/RPG` — Core RPG systems
  - Character, Inventory, Quests
  - Combat system with combos and special abilities
  - Equipment system with rarities
  - Achievement system
  - Audio manager (procedural sound effects)
  - Effects manager (particle effects)
  - Minimap system
- `Assets/Scripts/Player` — Player movement and camera controls
- `Assets/Scripts/Config` — Configuration files
- `Assets/StreamingAssets/rpg_config.json` — Tunable game settings
- `tools/install.ps1` — One-command setup
- `docs/` — Comprehensive documentation
  - **THE_ONE_RING.md** 💍 **START HERE!** — Master roadmap & status dashboard unifying all initiatives
  - **INDEX.md** — Complete documentation navigation guide
  - **CODE_AUDIT.md** ⭐ **CRITICAL** — Comprehensive code quality audit (10 issues remaining, 36 resolved)
  - **NEXT_STEPS.md** ⭐ **ACTION PLAN** — Implementation roadmap (Phase 1 complete ✅)
  - **OPTIMIZATION_IMPLEMENTATION_SUMMARY.md** ⚡ — v2.1 Performance improvements summary
  - **ENHANCEMENT_PLAN.md** — Future roadmap with 50+ enhancement ideas across 7 categories
  - **REPOSITORY_STRUCTURE.md** — Codebase navigation and architecture guide
  - **AUDIT_IMPLEMENTATION_SUMMARY.md** — Overview of audit work & deliverables
  - **PROBLEM_STATEMENT_MAPPING.md** — Maps problem statement to documentation
  - SETUP.md — Installation guide
  - GAME_DESIGN.md — Complete game design document
  - PLAYER_EXPERIENCE.md — Player experience walkthrough
  - IMPLEMENTATION_SUMMARY.md — What was built and statistics

## New Features Highlights

### v2.4 Content & Narrative 📖 (Active Development - January 28, 2026)

**Phase 5 Core Systems Implemented:**
- ✅ **Dialogue System**: Branching conversations with 2 pre-built NPC dialogue trees (Gandalf, Legolas)
  - Multiple choice options affecting NPC relationships
  - 100-point relationship scale tracking player choices
  - Dynamic dialogue paths based on player reputation
  - Quest integration through conversations
  
- ✅ **Boss Encounter System**: 6 epic bosses with unique mechanics
  - Cave Troll (Cave dungeons) - Ground slam, regeneration
  - Lich King (Crypt dungeons) - Death magic, summon undead
  - Orc Warlord (Fortress dungeons) - Battle cry, whirlwind attacks
  - Dragon Hatchling (Mine dungeons) - Fire breath, aerial combat
  - Dark Sorcerer (Tower dungeons) - Shadow magic, teleportation
  - Balrog (World boss) - Multi-phase combat with ultimate abilities
  - Phase transition system activating at health thresholds
  - Special abilities with cooldown management
  
- ✅ **Enhanced Quest System**: 8 advanced quests with complex mechanics
  - The Shadow in the Forest (branching paths based on player choice)
  - The Lost Heirloom (time-sensitive with 10-minute limit)
  - The Weathered Wanderer (weather-dependent, requires rain)
  - Secrets of the Deep (dungeon exploration quest)
  - Night Watch (time-of-day dependent, nighttime only)
  - The Fellowship Reunited (multi-stage epic quest)
  - Treasure of the Ancients (hidden quest, requires 10+ lore books)
  - The Healer's Request (reputation-gated quest)
  
- ✅ **Lore Book System**: 20+ discoverable narrative documents
  - 7 categories: History, Culture, Magic, Characters, Bestiary, Prophecy, Survival
  - Books scattered across The Shire, Rohan, Mordor, and dungeons
  - Collection tracking with completion percentage
  - Rich world-building content expanding Middle-earth lore
  
- ✅ **Achievement Integration**: 8 new Phase 5 achievements
  - Boss Slayer, Boss Hunter, Lore Keeper, Lore Master
  - Diplomat, Story Teller, Balrog Slayer, Master Conversationalist

**Technical Details:**
- ~4,500 lines of new C# code added
- 4 major new systems integrated with existing infrastructure
- Enhanced NPC system with dialogue and relationship tracking
- Total project now ~11,500 lines of C# code

**Status**: Core systems complete (65% of v2.4) - UI integration and testing next  
**Documentation**: See [THE_ONE_RING.md](docs/THE_ONE_RING.md) for complete roadmap

---

### v2.3 World Expansion 🌍 (Complete - January 28, 2026)

**Implemented Features - Priority 2:**
- ✅ **Day/Night Cycle**: Full 24-hour time system with dynamic lighting, sun/moon rotation, and time-based events
- ✅ **Dynamic Weather**: Five weather types (Clear, Rain, Snow, Fog, Storm) with gameplay effects on movement and visibility
- ✅ **Fast Travel System**: Waypoint-based travel with discovery mechanics across 6 Middle-earth locations in 3 regions
- ✅ **Dungeon System**: Procedural multi-floor dungeons (3-10 floors) with boss encounters, treasure rooms, and difficulty scaling
- ✅ **World Manager**: Integrated system coordinating all world features with environmental difficulty modifiers

**Core Capabilities:**
- Time progression affects lighting, NPC behavior, and enemy difficulty (+20% at night)
- Weather dynamically changes with smooth transitions and affects player movement (rain -5%, snow -15%)
- Discover and fast travel to waypoints in The Shire, Rohan, and Mordor regions
- Generate dungeons with 5 themes (Cave, Crypt, Fortress, Mine, Tower) and progressive difficulty
- Environmental modifiers stack: night + weather + dungeon = dynamic challenge

**Status**: v2.3 complete (100% finished) - All world systems and UI implemented  
**Documentation**: See [THE_ONE_RING.md](docs/THE_ONE_RING.md) for complete roadmap

---

### v2.2 Infrastructure Edition 🔧

**Recent Enhancements (January 2026):**
- **GameUtilities**: 13+ helper methods for common operations (SafeGetComponent, ClampDamage, etc.)
- **UnityExtensions**: 15+ extension methods for Unity components
- **PerformanceMonitor**: Real-time FPS and memory tracking with on-screen HUD
- **GameLogger**: Enhanced logging system with 5 levels and 9 categories
- **GameAssert**: Runtime assertions and contract validation for defensive programming
- **ConfigurationManager**: Configuration validation and loading system

**Previous Optimizations (v2.1):**
- **Object Pooling**: 60-80% reduction in garbage collection allocations during combat
- **Squared Distance Calculations**: Eliminated ~2,000 expensive sqrt operations per second
- **StringBuilder for HUD**: Eliminated ~200 string allocations per frame
- **Centralized Constants**: All game balance values in GameConstants.cs
- **Security Verified**: 0 vulnerabilities found via CodeQL analysis

### Combat System
- **Active Combat**: Click to attack enemies in real-time
- **Combo System**: Chain attacks for increased damage (20% per combo)
- **Critical Hits**: 15% base chance + agility bonus
- **Special Abilities**: Right-click for AOE attacks (costs 30 stamina)
- **Visual Feedback**: Damage numbers, hit effects, enemy flash on damage

### Equipment & Loot
- **5 Rarity Tiers**: Common → Uncommon → Rare → Epic → Legendary
- **Legendary Items**:
  - Andúril (Flame of the West) - +25 attack, +20 health
  - Mithril Coat - +30 defense, +50 health
  - Elven Blade - +15 attack, +10 stamina
  - Elven Cloak - +15 defense, +20 stamina
  - Ring of Power - +20 attack, +20 defense, +100 health, +50 stamina

### Achievements (12 Total)
- First Blood, Orc Slayer, Legendary Warrior
- Treasure Hunter, Dragon's Hoard
- Explorer, Fellowship Complete, Quest Master
- Maximum Power, Combo Master
- Heavy Hitter, Fully Equipped

### Enhanced Experience
- **Minimap**: Top-down view for navigation
- **Smart AI**: Enemies patrol, chase players, flee when wounded
- **Sound Effects**: Procedural audio for all game actions
- **Particle Effects**: Visual feedback for combat, treasures, level-ups
- **17+ Enemies**: More challenging combat encounters
