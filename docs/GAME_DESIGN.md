# Middle-earth Adventure RPG - Game Design Document

## Overview
An immersive 3D action-RPG set in a fantasy world inspired by J.R.R. Tolkien's Middle-earth. Players embark on epic quests, battle fearsome enemies, and explore legendary locations while building their character's power and collecting legendary items.

## Core Systems

### 1. Character System
**CharacterStats.cs** - Complete RPG character progression
- **Attributes**: Health, Stamina, Strength, Wisdom, Agility
- **Progression**: Experience points and leveling system
- **Level Up Benefits**: 
  - +20 Max Health
  - +10 Max Stamina
  - +2 to all attributes
  - Healing to full on level up
- **Scaling**: Experience requirements increase by 1.5x per level

### 2. Inventory System
**InventorySystem.cs** - Comprehensive item management
- **Item Types**: Quest Items, Weapons, Armor, Potions, Treasure
- **Features**:
  - Automatic item stacking
  - Gold currency system
  - Item add/remove/check operations
  - Detailed item descriptions and values

### 3. Quest System
**Quest.cs & QuestManager.cs** - Rich quest framework
- **Quest Structure**:
  - Multiple objectives per quest
  - Progress tracking for each objective
  - Completion percentage calculation
  - Rewards: Gold, Experience, Items

- **Objective Types**:
  - Collect Items
  - Defeat Enemies
  - Explore Locations
  - Talk to NPCs

### 4. Featured Quests

#### The Shire's Burden
*Difficulty: Easy*
- Gather 3 Lembas Bread
- Explore the Old Forest
- Rewards: 100 Gold, 150 XP

#### Fellowship of the Ring
*Difficulty: Easy*
- Speak with Gandalf the Grey
- Speak with Legolas
- Speak with Gimli
- Rewards: 150 Gold, 200 XP

#### Riders of Rohan
*Difficulty: Medium*
- Defeat 5 Orc Scouts
- Survey the Plains of Rohan
- Rewards: 200 Gold, 300 XP

#### The Path to Mordor
*Difficulty: Hard*
- Enter the Lands of Mordor
- Defeat 10 servants of darkness
- Find the Ring of Power
- Rewards: 500 Gold, 1000 XP

#### Master of Arms
*Difficulty: Medium*
- Equip a legendary weapon
- Achieve a 10-hit combo
- Defeat 15 enemies total
- Rewards: 300 Gold, 500 XP

#### Treasure Seeker
*Difficulty: Easy*
- Open 5 treasure chests
- Collect 500 gold
- Rewards: 250 Gold, 400 XP

#### Legend of Middle-earth
*Difficulty: Very Hard*
- Reach level 10
- Discover all locations
- Defeat 25 enemies
- Rewards: 1000 Gold, 2000 XP

## Enhanced Systems (v2.0)

### 5. Combat System
**CombatSystem.cs** - Active mouse-based combat
- **Attack Types**:
  - Basic Attack (Left-Click): Raycast-based targeting, 3 unit range
  - Special Ability (Right-Click): AOE attack in 4-unit radius, costs 30 stamina
- **Combo System**: 
  - Builds with consecutive hits
  - +20% damage per combo level
  - 1.5 second combo window
- **Critical Hits**:
  - 15% base chance
  - +1% per agility point
  - Deals 2x damage
- **Damage Calculation**: Base damage + (Strength × 2) + Combo multiplier
- **Cooldowns**: 0.5s attack cooldown, 5s special ability cooldown

### 6. Equipment System
**EquipmentSystem.cs** - Equippable gear with stats
- **Equipment Slots**: Weapon, Armor, Accessory
- **Rarity Tiers**: 
  - Common (white)
  - Uncommon (green)
  - Rare (blue)
  - Epic (purple)
  - Legendary (orange)
- **Stat Bonuses**: Attack, Defense, Health, Stamina
- **Legendary Items**:
  - Andúril: +25 ATK, +20 HP (Legendary Weapon)
  - Mithril Coat: +30 DEF, +50 HP (Legendary Armor)
  - Elven Blade: +15 ATK, +10 STA (Rare Weapon)
  - Elven Cloak: +15 DEF, +20 STA (Epic Armor)
  - Ring of Power: +20 ATK, +20 DEF, +100 HP, +50 STA (Legendary Accessory)

### 7. Achievement System
**AchievementSystem.cs** - Track player accomplishments
- **12 Total Achievements**:
  - Combat: First Blood, Orc Slayer, Legendary Warrior
  - Treasure: Treasure Hunter, Dragon's Hoard
  - Exploration: Explorer, Fellowship Complete, Quest Master
  - Progression: Maximum Power, Combo Master
  - Mastery: Heavy Hitter, Fully Equipped
- **Features**:
  - Automatic unlock detection
  - Visual and audio feedback
  - Progress tracking
  - Completion percentage

### 8. Audio System
**AudioManager.cs** - Procedural sound effects
- **Procedural Generation**: Sounds created at runtime using sine waves
- **Sound Types**:
  - Combat: Swing, Hit, Critical, Special Ability
  - Progression: Level Up, Quest Complete
  - Interaction: Treasure Open, Footstep, Enemy Death
- **Audio Pooling**: 10 audio sources for overlapping sounds
- **Volume Control**: Master, Music, and SFX volumes

### 9. Visual Effects System
**EffectsManager.cs** - Particle effects and feedback
- **Particle Types**:
  - Hit Effects: 8 particles (normal), 15 particles (critical)
  - Special Ability: 20 blue/purple magical particles
  - Level Up: 30 golden ascending particles
  - Treasure: 12 sparkle particles
  - Quest Complete: 25 green burst particles
- **Floating Numbers**: Damage/healing numbers with fade animation
- **Particle Physics**: Gravity, air resistance, fade-out

### 10. Minimap System
**MinimapSystem.cs** - Navigation aid
- **Display**: 200x200 pixel minimap in top-right corner
- **Camera**: Orthographic top-down view at 50 units height
- **Features**: Real-time rendering, follows player, adjustable zoom
- **UI**: Dark background, labeled "MAP"

## World Design

### Locations

#### The Shire (Southwest)
- **Theme**: Peaceful, pastoral homeland
- **Terrain**: Green rolling hills
- **Purpose**: Starting area, tutorial quests
- **Inhabitants**: Peaceful hobbits
- **Features**: Treasure chests, quest items

#### Plains of Rohan (East)
- **Theme**: Golden grasslands, horse lords
- **Terrain**: Open plains
- **Purpose**: Mid-game combat zone
- **Enemies**: Orc Scouts
- **Features**: NPC fellowship members

#### Lands of Mordor (North)
- **Theme**: Dark, foreboding wasteland
- **Terrain**: Barren, shadowy
- **Purpose**: End-game challenge area
- **Enemies**: Dark Servants
- **Features**: Powerful artifacts, final quests

#### Central Middle-earth
- **Theme**: Neutral meeting ground
- **Terrain**: Mixed grassland
- **Purpose**: Hub area with NPCs
- **Features**: Safe zone, quest givers

### NPCs

#### Gandalf the Grey
- **Location**: Western area near The Shire
- **Role**: Quest giver, Fellowship member
- **Quote**: "Welcome, traveler! Dark times are upon us."

#### Legolas
- **Location**: Eastern edge near Rohan
- **Role**: Fellowship member, combat expert
- **Quote**: "The orcs are gathering strength."

#### Gimli
- **Location**: Central-east area
- **Role**: Fellowship member, warrior
- **Quote**: "And my axe!"

### Enemies

#### Orc Scouts
- **Health**: 50 HP
- **Damage**: 10
- **Behavior**: Patrols, attacks on sight
- **Location**: Plains of Rohan
- **Rewards**: 25 Gold, 50 XP

#### Dark Servants
- **Health**: 50 HP (tougher variants possible)
- **Damage**: 10
- **Behavior**: Guards Mordor, aggressive
- **Location**: Lands of Mordor
- **Rewards**: 25 Gold, 50 XP

### Interactive Objects

#### Treasure Chests
Multiple chests scattered across Middle-earth:
- **Lembas Bread** - Quest item in The Shire
- **Elven Sword** - Weapon near Rohan
- **Ancient Artifact** - High-value item in Mordor
- **Rewards**: Items + 30 Gold per chest

## Game Atmosphere

### Visual Design
- **Lighting**: Warm, magical sunlight (warm yellow tones)
- **Fog**: Atmospheric distance fog for depth
- **Color Palette**:
  - Shire: Vibrant greens and natural tones
  - Rohan: Golden yellows and earth tones
  - Mordor: Dark greys and shadowy blacks
  - NPCs: Distinguished by color (White for Gandalf, Green for Legolas, Brown for Gimli)

### Audio Cues (Design Intent)
- Combat feedback on enemy hits
- Quest completion fanfares
- Location discovery sounds
- Treasure chest opening effects

## Technical Architecture

### Bootstrap System
**RPGBootstrap.cs** - Main game initialization
- Auto-loads on scene start
- Constructs entire world procedurally
- No Unity scene editing required
- Initializes all systems in correct order

### Configuration
**rpg_config.json** - Tunable game parameters
```json
{
  "characterName": "Aragorn",
  "startingGold": 100,
  "moveSpeed": 6.0,
  "sprintMultiplier": 1.8,
  "mouseSensitivity": 2.0
}
```

### Player Controller
- First-person perspective
- WASD movement
- Mouse look
- Sprint (Shift)
- Jump (Space)
- Trigger-based interaction

## Gameplay Loop

1. **Start**: Spawn in central Middle-earth
2. **Receive Quests**: Active quests displayed in HUD
3. **Explore**: Navigate to different regions
4. **Interact**: 
   - Talk to NPCs (walk into them)
   - Open chests (walk into them)
   - Discover locations (walk into trigger zones)
5. **Combat**: Engage enemies automatically when in range
6. **Progress**: Complete objectives, earn rewards
7. **Level Up**: Gain power, unlock harder quests
8. **Complete**: Finish all quests for full victory

## HUD Information

Real-time display shows:
- Character name and level
- Health and Stamina bars
- Current XP and progress to next level
- Gold count
- Active quests list
- Quest objectives with progress (e.g., 2/3)
- Completion percentage per quest
- Control instructions

## Extensibility

The system is designed for easy expansion:
- Add new quests in QuestManager.CreateLOTRQuests()
- Add new locations in RPGBootstrap.SetupWorld()
- Add new NPCs with CreateNPC()
- Add new enemies with CreateEnemy()
- Add new items in any chest or reward
- Modify config.json for balance tweaking

## Future Enhancement Opportunities

The game has been significantly enhanced with the following systems now implemented:

### ✅ Implemented Enhancements

1. **Combat System**: ✅ Active combat with mouse-based attacks, combos, critical hits, and special abilities
2. **Equipment System**: ✅ Equippable weapons and armor with stat bonuses and rarity tiers
3. **Achievement System**: ✅ 12 achievements tracking player accomplishments
4. **Sound Effects**: ✅ Procedural audio feedback for all game actions
5. **Particle Effects**: ✅ Visual effects for combat, treasures, and level-ups
6. **Enhanced AI**: ✅ Enemy patrol, chase, and flee behaviors
7. **Minimap**: ✅ Top-down navigation map
8. **More Content**: ✅ 7 quests (was 4), 17+ enemies (was 5), 5 equipment chests

### 🚧 Future Opportunities

1. **Save/Load**: Persistent game state across sessions
2. **Dialogue System**: Branching conversations with NPCs
3. **Day/Night Cycle**: Dynamic lighting and time of day
4. **Weather System**: Rain, fog, varying atmospheric conditions
5. **Crafting**: Combine items to create new equipment
6. **Mounts**: Horse riding for faster travel across Middle-earth
7. **Skill Tree**: Choose abilities and specializations
8. **Boss Battles**: Unique enemies with special mechanics
9. **Trading**: Buy and sell items with NPCs
10. **Pause Menu**: Settings and options screen

## Credits

Inspired by:
- J.R.R. Tolkien's Middle-earth
- Classic RPG mechanics
- The Crust (original prototype framework)

Built with Unity 2022.3 LTS
