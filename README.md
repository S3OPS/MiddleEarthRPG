# Lunar Crust Prototype

Playable 3D moon-mining factory automation prototype inspired by **The Crust**. The scene auto-builds itself at runtime for the fastest start.

## One-command local install (Windows PowerShell)
```powershell
powershell -ExecutionPolicy Bypass -File .\tools\install.ps1 -AutoRun
```

## Controls
- **WASD**: Move
- **Mouse**: Look
- **Shift**: Sprint
- **Space**: Jump

## Prototype Loop
Extractor → Conveyor → Refinery → Ingots (watch the HUD counters update).

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

## Folder Map
- `Assets/Scripts` — Runtime gameplay code
- `Assets/Scenes/Main.unity` — Minimal scene (runtime bootstrap)
- `Assets/StreamingAssets/config.json` — Tunable prototype config
- `tools/install.ps1` — One-command setup
- `docs/SETUP.md` — Dummy-proof guide
