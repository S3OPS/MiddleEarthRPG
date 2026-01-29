# Getting Started with Middle-earth Adventure RPG

Welcome! This guide will walk you through everything you need to download, install, configure, and play Middle-earth Adventure RPG using Godot Engine.

---

## Table of Contents

1. [What Is This Game?](#what-is-this-game)
2. [System Requirements](#system-requirements)
3. [Installation](#installation)
4. [Configuration](#configuration)
5. [Running the Game](#running-the-game)
6. [Game Controls](#game-controls)
7. [Exporting the Game](#exporting-the-game)
8. [Troubleshooting](#troubleshooting)

---

## What Is This Game?

Middle-earth Adventure RPG is a 3D role-playing game inspired by The Lord of the Rings. You'll explore lands like The Shire, Rohan, and Mordor, fight enemies, complete quests, and meet characters like Gandalf, Legolas, and Gimli.

This game is built with **Godot Engine 4.3+**, a free and open-source game engine.

### Why Godot?

We chose Godot because:
- ✅ **100% Free** - No subscriptions, no royalties, ever
- ✅ **No Account Required** - Download and start immediately
- ✅ **Lightweight** - Small download (~50 MB vs Unity's 3+ GB)
- ✅ **Open Source** - Community-driven development

👉 **Learn more:** See [ALTERNATIVE_ENGINES.md](ALTERNATIVE_ENGINES.md) for the full migration story

---

## System Requirements

| Requirement | Details |
|-------------|---------|
| **Operating System** | Windows 10/11, macOS 10.13+, or modern Linux |
| **Internet** | Required to download Godot (~80 MB) and game files |
| **Disk Space** | At least 500 MB free |
| **Graphics** | Any GPU with OpenGL 3.3 or Vulkan support |
| **Time** | About 5-10 minutes for setup |

---

## Installation

Follow these steps in order to set up the game.

### Step 1: Download Godot Engine

Godot Engine is a free game engine with no installation required - it runs as a standalone executable.

#### On Windows:

1. **Open your web browser** and go to: `https://godotengine.org/download`

2. **Download Godot 4.3 (Standard version)**
   - Look for "Godot Engine - Standard version" under "4.3 Stable"
   - Click "Download 64-bit" for Windows
   - Wait for download (~80 MB)

3. **Extract and run Godot**
   - Press **Windows key + E** to open File Explorer
   - Go to your **Downloads** folder
   - Find the file (e.g., `Godot_v4.3-stable_win64.exe.zip`)
   - **Right-click** → **"Extract All..."**
   - Choose where to save (Desktop or Documents recommended)
   - Click **Extract**
   - Open the extracted folder
   - **Double-click** `Godot_v4.3-stable_win64.exe` to run

**Pro tip:** Create a Desktop shortcut to the `.exe` file for easy access later.

#### On macOS:

1. **Open your web browser** and go to: `https://godotengine.org/download`

2. **Download Godot 4.3**
   - Look for "Godot Engine - Standard version" under "4.3 Stable"
   - Click "Download Universal" for macOS
   - Wait for download (~80 MB)

3. **Extract and run**
   - Open **Finder** → **Downloads**
   - Find `Godot_v4.3-stable_macos.universal.zip`
   - **Double-click** to extract
   - You'll see `Godot.app`
   - **Drag Godot.app** to **Applications** folder (optional but recommended)
   - **Double-click Godot.app** to run
   - If macOS blocks it: Go to **System Preferences → Security & Privacy** → click **"Open Anyway"**

#### On Linux:

1. **Open your web browser** and go to: `https://godotengine.org/download`

2. **Download Godot 4.3**
   - Click the appropriate download for your system (usually 64-bit)

3. **Make executable and run**
   ```bash
   cd ~/Downloads
   chmod +x Godot_v4.3-stable_linux.x86_64
   ./Godot_v4.3-stable_linux.x86_64
   ```
   - Optionally, move to `/usr/local/bin` or create a desktop shortcut

**Note:** Some distributions have Godot in package managers, but ensure you get **version 4.3 or higher**.

---

### Step 2: Get the Game Files

Download the game project files to your computer.

#### Option A: Download from GitHub

1. **Go to the project repository**
   - Open your browser
   - Navigate to: `https://github.com/S3OPS/MiddleEarthRPG`

2. **Download the files**
   - Click the green **"Code"** button
   - Click **"Download ZIP"**
   - Wait for download to complete

3. **Extract the files**

   **On Windows:**
   - Go to Downloads folder
   - Find the ZIP file
   - **Right-click** → **"Extract All..."**
   - Choose destination (Documents recommended)
   - Click **Extract**

   **On macOS:**
   - Go to Downloads folder in Finder
   - **Double-click** the ZIP file
   - It will extract automatically

   **On Linux:**
   ```bash
   cd ~/Downloads
   unzip MiddleEarthRPG-main.zip -d ~/Documents/
   ```

#### Option B: Clone with Git

If you have Git installed:

```bash
cd ~/Documents
git clone https://github.com/S3OPS/MiddleEarthRPG.git
```

---

### Step 3: Open the Project in Godot

1. **Launch Godot**
   - **Windows:** Double-click `Godot_v4.3-stable_win64.exe`
   - **macOS:** Open `Godot.app` from Applications
   - **Linux:** Run the Godot executable

2. **Import the project** (first time only)
   - The **Project Manager** window opens
   - Click the **"Import"** button
   - Navigate to where you extracted the game files
   - Find and select the **`project.godot`** file
   - Click **"Open"**
   - The project appears in your project list as "Middle-earth Adventure RPG"

3. **Open the project**
   - Click on "Middle-earth Adventure RPG" in the list
   - Click **"Edit"**
   - The Godot Editor opens (may take 10-30 seconds first time)

---

## Configuration

### Godot Editor Settings (Optional)

The game works with default settings, but you can customize the editor:

1. **Editor → Editor Settings**
   - **Interface → Theme**: Choose between Light/Dark themes
   - **Interface → Editor Scale**: Adjust for high-DPI displays
   - **Text Editor**: Configure font size and color scheme

### Game Settings (Optional)

You can modify game balance and settings by editing the constants file:

1. In Godot's **FileSystem** panel (bottom-left), navigate to:
   ```
   scripts/utilities/constants.gd
   ```

2. **Double-click** to open in the script editor

3. Modify values like:
   - Player stats (health, stamina, speed)
   - Combat parameters (damage, cooldowns)
   - XP requirements and progression

4. **Save** (Ctrl+S or Cmd+S)

### Graphics Quality (Optional)

To adjust performance:

1. **Project → Project Settings**
2. Navigate to **Rendering → Quality**
3. Adjust settings:
   - **MSAA 3D**: Anti-aliasing quality (higher = smoother but slower)
   - **Screen Space AA**: Additional smoothing
   - **Shadow Quality**: Shadow detail level

---

## Running the Game

### Playing from the Editor

1. **Ensure the main scene is open**
   - Look at the **FileSystem** panel (bottom-left)
   - Navigate to `scenes/` folder
   - **Double-click** `main.tscn`

2. **Press Play**
   - Click the **Play button** (▶️) in the top-right corner
   - Or press **F5** on your keyboard
   - The game launches in a new window

3. **To stop playing**
   - Press **F8** or click **Stop** (⏹️) in the editor
   - Or close the game window

**Important:** The scene may look minimal in the editor - this is normal! The full game world is created at runtime when you press Play.

---

## Game Controls

### Basic Controls

| Action | Control |
|--------|---------|
| **Move** | W, A, S, D keys |
| **Look around** | Move your mouse |
| **Sprint** | Hold Shift while moving |
| **Jump** | Spacebar |
| **Attack** | Left mouse button |
| **Special ability** | Right mouse button (AOE attack) |
| **Pause/Menu** | ESC key |

### Coming Soon

These features are planned for future releases:

| Feature | Planned Control |
|---------|----------------|
| **Quest Journal** | J key |
| **Character Sheet** | C key |
| **Inventory** | I key |
| **World Map** | M key |
| **Interact with NPCs** | E key |

### Tips

- **Sprint conservatively** - Stamina regenerates slowly
- **Special attacks** are powerful but cost 30 stamina
- **Level up** by defeating enemies and completing quests
- **Save frequently** - Auto-save is enabled
- Walk into NPCs to interact with them
- Walk into treasure chests to open them

---

## Exporting the Game

Want to create a standalone executable that runs without Godot? Here's how to export the game.

### One-Time Setup: Install Export Templates

The first time you export, Godot needs to download export templates (~500 MB):

1. In Godot Editor: **Project → Export...**
2. If templates are missing, click **"Manage Export Templates"**
3. Click **"Download and Install"**
4. Wait for download (5-10 minutes)
5. Close the templates window

### Export Process

1. **Open Export Window**
   - **Project → Export...**

2. **Add Export Preset**
   - Click **"Add..."**
   - Choose your platform:
     - **Windows Desktop** - For Windows PC (.exe)
     - **Linux/X11** - For Linux
     - **macOS** - For Mac

3. **Configure Settings** (Optional)
   - Click your preset in the left sidebar
   - Key settings:
     - **Export With Debug**: Uncheck for final release
     - **Runnable**: Ensure checked

4. **Export**
   - Click **"Export Project"**
   - Choose save location
   - Name your file (e.g., `MiddleEarthAdventure.exe`)
   - Click **"Save"**
   - Wait 1-5 minutes

### What You Get

After export:

**On Windows:**
- `MiddleEarthAdventure.exe` - Game executable
- `MiddleEarthAdventure.pck` - Game data file

**On Linux:**
- `MiddleEarthAdventure.x86_64` - Game executable
- `MiddleEarthAdventure.pck` - Game data file

**On macOS:**
- `MiddleEarthAdventure.app` - Complete application

**Important:** Keep `.exe` and `.pck` files together! Both are needed.

You can share these files with others, and they can play without having Godot installed.

---

## Troubleshooting

### Installation Issues

#### "Cannot download Godot" or slow download
- **Solution:** Try a different mirror from the download page
- Use the direct GitHub releases page: `https://github.com/godotengine/godot/releases`

#### "Windows protected your PC" warning
- **Solution:** Click "More info" → "Run anyway"
- Godot is safe but not signed with a Windows certificate

#### macOS: "Godot.app is damaged and can't be opened"
- **Solution:** Run in Terminal:
  ```bash
  xattr -cr /path/to/Godot.app
  ```

#### Linux: "Permission denied"
- **Solution:** Make file executable:
  ```bash
  chmod +x Godot_v4.3-stable_linux.x86_64
  ```

---

### Project Issues

#### "Cannot open project" or "Project file is corrupted"
- Ensure you selected the `project.godot` file
- Make sure ZIP file was fully extracted
- Re-download if file may be corrupted

#### "Godot version mismatch" warning
- Godot 4.3+ is required
- Download the correct version from godotengine.org
- Compatibility within Godot 4.x is usually good

#### Missing scripts or errors on startup
1. Close Godot
2. Delete the `.godot` folder in project directory (import cache)
3. Reopen project in Godot
4. Wait for reimport (may take 1 minute)

---

### Runtime Issues

#### Scene appears empty or minimal in editor
- **This is normal!** The main scene looks minimal in the editor
- The full world is created at runtime when you press Play (F5)
- You should see terrain, NPCs, and enemies after starting the game

#### Game window shows no content after pressing Play
1. Check **Output** panel (View → Output) for errors
2. Ensure `main.tscn` is set as main scene:
   - **Project → Project Settings → Application → Run → Main Scene**
3. Try: **Project → Reload Current Project**

#### Game runs slowly
- Lower graphics settings: **Project → Project Settings → Rendering → Quality**
- Update your graphics drivers
- Close other applications
- Reduce window resolution

#### Keyboard/mouse not working in game
- Click inside the game window to give it focus
- Try Alt+Tab to switch to game window
- Check that no overlays are capturing input

#### Pink/magenta textures
1. Close Godot
2. Delete `.godot` folder in project directory
3. Reopen project and wait for reimport

---

### Export Issues

#### "No export template found"
- Install export templates: **Project → Export → Manage Export Templates**

#### Export button grayed out
- Add an export preset first (click "Add...")
- Ensure templates are installed

#### Exported game won't run
- Keep `.exe` and `.pck` files together
- On Linux, make file executable: `chmod +x filename`
- Check antivirus isn't blocking the file
- Try exporting with "Export With Debug" enabled to see errors

#### Exported game is very large
- Normal for 3D games with assets
- Uncheck "Export With Debug" in preset settings
- Enable "Optimize for File Size" in export settings

---

### Getting Help

If you continue to experience issues:

1. **Check the Output panel** in Godot (View → Output) for error messages
2. **Review documentation:**
   - [GAME_DESIGN.md](GAME_DESIGN.md) - Technical specifications
   - [REPOSITORY_STRUCTURE.md](REPOSITORY_STRUCTURE.md) - Code organization
   - [ALTERNATIVE_ENGINES.md](ALTERNATIVE_ENGINES.md) - Migration information
3. **Open an issue** on GitHub with:
   - Your operating system
   - Godot version
   - Error messages from Output panel
   - Steps to reproduce the problem

---

## What's Next?

Once you're playing:

1. **Start in The Shire** (green area to southwest) - easiest starting area
2. **Talk to NPCs** - Walk into Gandalf, Legolas, or Gimli to interact
3. **Open treasure chests** - Walk into them to collect items and gold
4. **Complete quests** - Earn XP and level up
5. **Explore locations** - Discover The Shire, Plains of Rohan, Lands of Mordor
6. **Battle enemies** - Fight Orc Scouts and Dark Servants

### Available Quests

- **The Shire's Burden** - Collect Lembas Bread, explore Old Forest
- **Fellowship of the Ring** - Talk to Gandalf, Legolas, and Gimli
- **Riders of Rohan** - Defeat Orc Scouts in the Plains of Rohan
- **The Path to Mordor** - Enter Mordor and face the ultimate challenge

---

## Additional Resources

- **[README.md](../README.md)** - Project overview and features
- **[GAME_DESIGN.md](GAME_DESIGN.md)** - Complete game design document
- **[ALTERNATIVE_ENGINES.md](ALTERNATIVE_ENGINES.md)** - Why we chose Godot
- **[REPOSITORY_STRUCTURE.md](REPOSITORY_STRUCTURE.md)** - Codebase guide for developers

---

**Congratulations!** 🎉 You're all set to play Middle-earth Adventure RPG!

Enjoy your journey through Middle-earth!

---

**Last Updated:** January 29, 2026  
**Godot Version:** 4.3+  
**Game Version:** Alpha v0.2  
**Document Version:** 1.1
