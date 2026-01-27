# LUNAR CRUST — Dummy-Proof Setup (Windows)

## One Command Install
Open PowerShell inside the repo and run:

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\install.ps1 -AutoRun
```

On macOS/Linux with PowerShell 7+ installed:
```powershell
pwsh -ExecutionPolicy Bypass -File ./tools/install.ps1 -AutoRun
```

## What It Does
- Creates a local config in `%LOCALAPPDATA%\LunarCrust`
- Copies the prototype config
- Creates a launcher script
- Launches Unity Hub (if installed)

## Run Later
```powershell
powershell -ExecutionPolicy Bypass -File "$env:LOCALAPPDATA\LunarCrust\run.ps1"
```

## Open in Unity
1. Open Unity Hub
2. Add the project folder
3. Open the project
4. Press **Play**

## Controls
- **WASD**: Move
- **Mouse**: Look
- **Shift**: Sprint
- **Space**: Jump

## Prototype Loop
Extractor → Conveyor → Refinery → Ingots (HUD updates live)
