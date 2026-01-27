# Middle-earth Adventure RPG

An immersive 3D RPG game set in a Lord of the Rings inspired fantasy world. Embark on epic quests, battle fearsome enemies, and explore the legendary lands of Middle-earth. The scene auto-builds itself at runtime for the fastest start.

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
  - SETUP.md — Installation guide
  - GAME_DESIGN.md — Complete game design document
  - PLAYER_EXPERIENCE.md — Player experience walkthrough

## New Features Highlights

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
