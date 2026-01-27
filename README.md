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
- **Walk into objects**: Interact with NPCs, treasure chests, and locations

## Game Features
- **Character System**: Track your health, stamina, experience, and level up your hero
- **Quest System**: Complete epic quests inspired by LOTR including "The Shire's Burden," "Riders of Rohan," and "The Path to Mordor"
- **Inventory & Loot**: Collect items, gold, and equipment from treasure chests
- **Combat**: Battle orcs and dark servants across Middle-earth
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

## Folder Map
- `Assets/Scripts/RPG` — Core RPG systems (Character, Inventory, Quests)
- `Assets/Scripts/Player` — Player movement and camera controls
- `Assets/Scripts/Config` — Configuration files
- `Assets/StreamingAssets/rpg_config.json` — Tunable game settings
- `tools/install.ps1` — One-command setup
- `docs/SETUP.md` — Dummy-proof guide
