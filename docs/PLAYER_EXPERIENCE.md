# Middle-earth Adventure RPG - Player Experience

## What You'll See When Playing

### On Game Start
```
═══════════════════════════════════════════════════════════
│  === MIDDLE-EARTH ADVENTURE ===                          │
│                                                           │
│  Aragorn - Level 1                                       │
│  Health: 100/100  Stamina: 100/100                       │
│  XP: 0/100  Gold: 100                                    │
│                                                           │
│  Active Quests:                                          │
│    • The Shire's Burden (0%)                             │
│      [ ] Gather 3 Lembas Bread (0/3)                     │
│      [ ] Explore the Old Forest (0/1)                    │
│    • Fellowship of the Ring (0%)                         │
│      [ ] Speak with Gandalf the Grey (0/1)               │
│      [ ] Speak with Legolas (0/1)                        │
│      [ ] Speak with Gimli (0/1)                          │
│                                                           │
│  Controls: WASD Move | Mouse Look | Shift Sprint | Space│
│  Interact: Walk into NPCs, Chests, and Locations         │
═══════════════════════════════════════════════════════════
```

### World View (First-Person Perspective)

```
        ┌─────────────────────────────────────┐
        │           🌤️ Warm Sunlight          │
        │                                     │
        │     🏔️ Distant Mountains 🏔️         │
        │                                     │
        │   🧙 Gandalf                        │
        │   (White figure)                    │
        │        ↓                            │
        │   "Welcome, traveler!"              │
        │                                     │
        │  🏹 Legolas      💎 Chest           │
        │  (Green)         (Golden)           │
        │                                     │
        │        YOU (Player Camera)          │
        │         ▼                           │
        │    🟢 Green plains ahead            │
        │    (The Shire)                      │
        │                                     │
        │  Fog fading into distance...        │
        └─────────────────────────────────────┘
```

### After Discovering The Shire

```
Console Log:
> Discovered: The Shire
> Gained 25 XP!

HUD Updates:
  • The Shire's Burden (50%)
    [ ] Gather 3 Lembas Bread (0/3)
    [✓] Explore the Old Forest (1/1)  ← COMPLETED!
    
  XP: 25/100  ← Updated!
```

### After Opening a Treasure Chest

```
Console Log:
> Opened chest! Found Lembas Bread and 30 gold!

HUD Updates:
  • The Shire's Burden (100%)  ← QUEST COMPLETE!
    [✓] Gather 3 Lembas Bread (3/3)
    [✓] Explore the Old Forest (1/1)
    
  Gold: 230 ← +30 from chest + 100 quest reward
  XP: 175/100 ← +150 quest reward
  
> LEVEL UP! You are now Level 2!
  Health: 120/120
  Stamina: 110/110
  Strength: 12
  Wisdom: 12
  Agility: 12
```

### Combat Encounter

```
Scene:
        YOU
         ↓
    [Looking at]
         ↓
    ⚔️ Orc Scout (Red text)
    HP: 50/50

Console Log:
> Orc Scout attacks the player for 10 damage!
> Orc Scout has been defeated!
> Gained 50 XP!
> Gained 25 Gold!

HUD Updates:
  • Riders of Rohan (20%)
    [✓] Defeat 5 Orc Scouts (1/5)  ← Progress!
```

### Talking to Gandalf

```
Walking towards Gandalf...
[Player enters trigger zone]

Console Log:
> Gandalf the Grey: Welcome, traveler! Dark times are upon us.

HUD Updates:
  • Fellowship of the Ring (33%)
    [✓] Speak with Gandalf the Grey (1/1)  ← DONE!
    [ ] Speak with Legolas (0/1)
    [ ] Speak with Gimli (0/1)
```

### Entering Mordor

```
[Crossing into dark region]

Visual Changes:
- Terrain becomes dark grey/black
- Fog becomes thicker
- Lighting becomes dimmer
- Enemies visible in distance

Console Log:
> Discovered: Lands of Mordor
> Gained 25 XP!

HUD Updates:
  • The Path to Mordor (33%)
    [✓] Enter the Lands of Mordor (1/1)  ← Complete!
    [ ] Defeat servants of darkness (0/10)
    [ ] Find the Ring of Power (0/1)
```

### Full Quest Completion

```
[After completing all objectives for a quest]

Console Log:
> Quest Complete: Fellowship of the Ring!
> Gained 200 XP!
> Gained 150 Gold!

HUD Updates:
  Active Quests:
  • The Path to Mordor (33%)
    [✓] Enter the Lands of Mordor (1/1)
    [ ] Defeat servants of darkness (0/10)
    [ ] Find the Ring of Power (0/1)
    
  [Fellowship of the Ring removed - COMPLETED]
```

### Final Stats (After Playing)

```
═══════════════════════════════════════════════════════════
│  === MIDDLE-EARTH ADVENTURE ===                          │
│                                                           │
│  Aragorn - Level 5  ⬆️ (Leveled up multiple times!)      │
│  Health: 180/180  Stamina: 140/140                       │
│  XP: 450/608  Gold: 1,125  💰                            │
│                                                           │
│  Strength: 18  Wisdom: 18  Agility: 18                   │
│                                                           │
│  Active Quests:                                          │
│    • The Path to Mordor (66%)                            │
│      [✓] Enter the Lands of Mordor (1/1)                 │
│      [✓] Defeat servants of darkness (10/10)             │
│      [ ] Find the Ring of Power (0/1)                    │
│                                                           │
│  Completed Quests: 3/4  ✅                                │
│  Enemies Defeated: 15  ⚔️                                 │
│  Chests Opened: 3  💎                                    │
│  Locations Discovered: 3  🗺️                             │
═══════════════════════════════════════════════════════════
```

## Key Visual Elements

### Color Coding
- **The Shire**: Vibrant green terrain
- **Rohan**: Golden yellow plains
- **Mordor**: Dark grey/black lands
- **NPCs**: Color-coded (White=Gandalf, Green=Legolas, Brown=Gimli)
- **Enemies**: Dark green/grey with red name labels
- **Chests**: Golden/brown wooden boxes

### Labels (Floating Text)
- Location names float above regions
- NPC names in yellow
- Enemy names in red
- Quest markers and objectives

### Atmospheric Effects
- Warm golden sunlight
- Distance fog for immersion
- Color gradients across regions
- Clear sky with directional lighting

## Player Progression Journey

1. **Start** → Spawn in center, see HUD with 2 active quests
2. **Explore** → Walk to The Shire (green area), discover location
3. **Loot** → Find treasure chests, collect Lembas Bread
4. **Quest Complete** → The Shire's Burden finishes, gain rewards
5. **Meet NPCs** → Talk to Gandalf, Legolas, Gimli
6. **Combat** → Battle Orc Scouts in Rohan
7. **Level Up** → Gain XP, increase stats
8. **Endgame** → Enter Mordor, face final challenges
9. **Victory** → Complete all quests, become legendary hero!

## Immersion Features

- Real-time quest tracking
- Instant feedback on all actions
- Clear progression indicators
- Intuitive interaction (just walk into things)
- No complex menus or controls
- Focus on exploration and discovery
