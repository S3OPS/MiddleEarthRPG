# Complete Beginner's Guide to Playing Middle-earth Adventure RPG

Welcome! This guide will walk you through every step to get this game running on your computer using Godot Engine. Don't worry if you've never done anything like this before - we'll explain everything.

---

## Table of Contents
1. [What Is This Game?](#what-is-this-game)
2. [What You'll Need](#what-youll-need)
3. [Step 1: Download Godot Engine](#step-1-download-godot-engine)
4. [Step 2: Get the Game Files](#step-2-get-the-game-files)
5. [Step 3: Open the Project in Godot](#step-3-open-the-project-in-godot)
6. [Step 4: Play the Game](#step-4-play-the-game)
7. [Troubleshooting](#troubleshooting)
8. [Game Controls](#game-controls)
9. [Exporting the Game](#exporting-the-game-creating-a-standalone-executable)

---

## What Is This Game?

Middle-earth Adventure RPG is a 3D role-playing game inspired by The Lord of the Rings. You'll explore lands like The Shire, Rohan, and Mordor, fight enemies, complete quests, and meet characters like Gandalf, Legolas, and Gimli.

This is a **Godot project**, which means it was made using a free and open-source game engine called Godot. To play the game, you need to download Godot first (it's completely free, no account needed!).

### 🎮 Why We Chose Godot

**This project was migrated from Unity to Godot!** We chose Godot because:
- ✅ **100% Free** - No subscriptions, no royalties, ever
- ✅ **No Account Required** - Download and start immediately
- ✅ **Lightweight** - Much smaller download (~50 MB vs Unity's 3+ GB)
- ✅ **Open Source** - Community-driven development

👉 **Learn more:** See [ALTERNATIVE_ENGINES.md](ALTERNATIVE_ENGINES.md) for the full story

---

## What You'll Need

Before we start, make sure you have:

| Requirement | Details |
|-------------|---------|
| **Computer** | Windows 10, Windows 11, macOS, or Linux |
| **Internet** | To download Godot (about 80 MB) and game files |
| **Disk Space** | At least 500 MB of free space |
| **Time** | About 5-10 minutes for everything |

---

## Step 1: Download Godot Engine

Godot Engine is a free, lightweight game engine that's super easy to install. The best part? **No account required, no installation wizard, and only about 50 MB to download!**

### For Windows:

1. **Open your web browser** (Chrome, Firefox, Edge, or Safari)
   - Look for the browser icon on your desktop or taskbar and click it
   
2. **Go to the Godot download page**
   - Click in the address bar at the top of your browser
   - Type: `https://godotengine.org/download`
   - Press the **Enter** key on your keyboard

3. **Download Godot 4.3 (Standard version)**
   - Look for the **"Godot Engine - Standard version"** button under "4.3 Stable"
   - Click **"Download 64-bit"** for Windows
   - A file will start downloading (about 80 MB - very small!)
   - Wait for the download to finish (you'll see a progress indicator)

4. **Extract and Run Godot** (No installation needed!)
   - Go to your **Downloads** folder
     - Press **Windows key + E** to open File Explorer
     - Click on **Downloads** in the left sidebar
   - Find the downloaded file (something like `Godot_v4.3-stable_win64.exe.zip`)
   - **Right-click** on it and select **"Extract All..."**
   - Choose where to save it (your Documents folder or Desktop is a good choice)
   - Click **Extract**
   - Open the extracted folder and you'll see `Godot_v4.3-stable_win64.exe`
   - **Double-click** the `.exe` file to run Godot
   - That's it! No installation wizard, no account needed!

**Pro tip:** Create a shortcut to `Godot_v4.3-stable_win64.exe` on your Desktop for easy access later.

### For macOS:

1. **Open Safari** (or your preferred browser)
   - Click the Safari icon in your Dock (the bar at the bottom of your screen)

2. **Go to the Godot download page**
   - Click in the address bar at the top
   - Type: `https://godotengine.org/download`
   - Press **Return** (Enter)

3. **Download Godot 4.3**
   - Look for **"Godot Engine - Standard version"** under "4.3 Stable"
   - Click **"Download Universal"** for macOS
   - Wait for the download to complete (about 80 MB)

4. **Extract and Run Godot**
   - Open **Finder** (the blue smiling face icon in your Dock)
   - Click **Downloads** in the left sidebar
   - Find the file called `Godot_v4.3-stable_macos.universal.zip`
   - **Double-click** it to extract
   - You'll see `Godot.app` appear
   - **Drag Godot.app** to your **Applications** folder (optional but recommended)
   - **Double-click Godot.app** to run it
   - If macOS says it can't verify the developer, go to **System Preferences → Security & Privacy** and click **"Open Anyway"**

### For Linux:

1. **Open your web browser** and go to `https://godotengine.org/download`

2. **Download Godot 4.3**
   - Look for **"Godot Engine - Standard version"**
   - Click the appropriate download for your system (usually 64-bit)

3. **Make it executable and run**
   - Open a terminal
   - Navigate to your Downloads folder: `cd ~/Downloads`
   - Make executable: `chmod +x Godot_v4.3-stable_linux.x86_64`
   - Run it: `./Godot_v4.3-stable_linux.x86_64`
   - Optionally, move it to a permanent location like `/usr/local/bin` or create a desktop shortcut

**Note:** Some Linux distributions may have Godot in their package managers (`apt`, `dnf`, `pacman`), but make sure you get **version 4.3 or higher** for this project.

---

## Step 2: Get the Game Files

You need to download the game project files on your computer.

### Option A: Download from GitHub (Recommended)

1. **Go to the project page**
   - Open your web browser
   - Go to the GitHub page where this project is hosted
   
2. **Download the files**
   - Look for a green button that says **"Code"**
   - Click it, then click **"Download ZIP"**
   - Wait for the download to complete

3. **Extract the files**
   
   **Windows:**
   - Go to your Downloads folder
   - Find the ZIP file (it will have the project name)
   - Right-click on it
   - Click **"Extract All..."**
   - Choose where to save it (your Documents folder is a good choice)
   - Click **Extract**
   
   **macOS:**
   - Go to your Downloads folder in Finder
   - Double-click the ZIP file
   - It will automatically extract to a folder

### Option B: If Someone Gave You a Folder

If someone gave you the project files on a USB drive or shared folder:
1. Copy the entire folder to a location on your computer (like Documents)
2. Make sure the folder contains a file named `project.godot` and folders like `scenes` and `scripts`

---

## Step 3: Open the Project in Godot

Now we'll open the game in Godot!

1. **Launch Godot Engine**
   - **Windows:** Double-click the `Godot_v4.3-stable_win64.exe` file you extracted earlier
   - **macOS:** Open `Godot.app` from your Applications folder (or wherever you saved it)
   - **Linux:** Run the Godot executable you downloaded

2. **Import the project** (first time only)
   - When Godot opens, you'll see the **Project Manager** window
   - Click the **"Import"** button on the right side
   - A file browser will open
   - Navigate to where you extracted the game files
   - Find and select the **`project.godot`** file (this is in the main project folder)
   - Click **"Open"**
   - The project will appear in your project list with the name "Middle-earth Adventure RPG"

3. **Open the project**
   - In the Project Manager, find "Middle-earth Adventure RPG" (or MiddleEarthRPG)
   - Click on it once to select it
   - Click the **"Edit"** button on the right side
   - The Godot Editor will open - this may take 10-30 seconds the first time

4. **The Godot Editor opens**
   - You'll see an interface with several panels (Scene, Inspector, FileSystem, etc.)
   - Don't worry! You only need to know a couple of things to play the game

---

## Step 4: Play the Game

You're almost there! Here's how to run the game:

1. **Make sure the Main scene is open** (if not already)
   - Look at the bottom panel labeled **"FileSystem"**
   - Find the folder called **`scenes`**
   - Double-click on **`main.tscn`** (or just **`main`**)
   - The scene will load in the editor
   - **Note:** The scene might look empty or minimal in the editor - this is normal! The game world is created when you run it.

2. **Press Play!**
   - Look at the **top-right corner** of the Godot Editor
   - You'll see a **Play button** (▶️ triangle icon) - or press **F5** on your keyboard
   - Click it!
   - The game will launch in a new window
   - **After pressing Play**, you'll see the full Middle-earth world with terrain, characters, NPCs, and enemies

3. **To stop playing**
   - Press **F8** or click the **Stop button** (⏹️ square icon) in the Godot Editor
   - Or simply close the game window

**Pro tip:** The game runs in a separate window from the editor, so you can Alt+Tab (or Cmd+Tab on Mac) between the game and the editor!

---

## Troubleshooting

### The game window shows an empty or minimal scene

**What it looks like:** When you press F5 (Play), the game window shows just a skybox or very little content.

**Understanding how this project works:**
The main.tscn scene may look **minimal in the editor**. This project uses a "runtime bootstrap" system - the entire game world is created automatically when you run the game. This is normal and by design!

**In the Godot Editor:** The Scene panel might show minimal objects - this is expected!

**After pressing F5 (Play):** You should see the full Middle-earth world with:
- Terrain (The Shire, Rohan, Mordor)
- Characters (Gandalf, Legolas, Gimli)
- Enemies (Orcs, Dark Servants)
- Player with camera

**If you see a different scene:**

**What to do:**
1. In the Godot Editor, look at the bottom panel called **"FileSystem"**
2. Navigate to the `scenes` folder
3. Double-click on **`main.tscn`**
4. Now press **F5** (or click the Play button ▶️)

If pressing Play doesn't show any content:
1. Check the **Output** panel at the bottom (View → Output) for any red error messages
2. Make sure all scripts loaded successfully (no red errors in the Output panel)
3. Try: Project → Reload Current Project (this reloads all scripts)

### "Godot version mismatch" or similar warning

**What it means:** The project was created with a slightly different Godot version.

**What to do:** 
- Godot is usually very good at backwards/forwards compatibility within major versions
- If you're using Godot 4.3+, the project should work fine
- If you see errors, try downloading Godot 4.3 specifically from the Godot website

### Missing scripts or errors on startup

**What it means:** Some files didn't load correctly.

**What to do:**
1. Close Godot
2. Delete the `.godot` folder inside the project folder (this is the import cache)
3. Open the project again in Godot
4. Wait for it to reimport (this may take a minute)

### The game runs slowly

**What to do:**
- In Godot, go to **Project → Project Settings → Rendering**
- Under "Quality", try lowering some settings
- Make sure your graphics drivers are up to date
- Try lowering the resolution in the project settings

### Godot won't open or crashes

**Windows:** 
- Make sure you extracted the ZIP file (don't run Godot from inside the ZIP)
- Try running the Godot executable as administrator (right-click → Run as administrator)
- Check if your antivirus is blocking it

**macOS:** 
- Check your security settings: System Preferences → Security & Privacy
- Click "Open Anyway" if Godot is blocked
- If it says "damaged and can't be opened", run this in Terminal:
  ```
  xattr -cr /path/to/Godot.app
  ```

**Linux:**
- Make sure the file is executable: `chmod +x Godot_v4.3-stable_linux.x86_64`
- Check if you need to install dependencies (usually not needed for the official build)

### "Cannot open project at path" or "Project file is corrupted"

**What to do:**
1. Make sure you're selecting the `project.godot` file (not a folder)
2. Make sure you fully extracted the ZIP file (not running from inside the ZIP)
3. Re-download the project from GitHub
4. Check that the download wasn't corrupted (try downloading again)

### Script errors or "Parse Error"

**What it means:** Godot found problems in the GDScript code.

**What to do:**
1. Check the **Output** panel (View → Output) for error details
2. Most common cause: The project was made with Godot 4.x and you're using an older version
   - Make sure you downloaded **Godot 4.3** or higher (not Godot 3.x)
3. If you see red errors mentioning specific scripts, the files might be corrupted
   - Re-download the project from GitHub

### "Not enough disk space"

**What to do:**
- Delete files you don't need from your computer
- Empty your Recycle Bin (Windows) or Trash (macOS)
- Godot itself is only 50-80 MB, so you don't need much space
- The project is around 200-300 MB

### Game runs but I see pink/magenta textures

**What it means:** Some textures or materials didn't load correctly.

**What to do:**
1. Close Godot
2. Delete the `.godot` folder in the project directory
3. Reopen the project and let Godot reimport all assets
4. If specific textures are still pink, check the Output panel for import errors

### Import errors or "Cannot import" messages

**What to do:**
1. Usually these appear during first import - just wait for Godot to finish
2. If the project opens but you see warnings, check the Output panel
3. Common fixes:
   - Close Godot, delete `.godot` folder, reopen
   - Make sure all project files were extracted properly
   - Re-download the project if files are missing

### I can't find the play button or it's grayed out

**What to do:**
- Make sure you have a scene open (like `main.tscn`)
- The Play button (▶️) is in the top-right corner
- If you don't have a main scene set, Godot will ask you to select one
- Choose `scenes/main.tscn` as the main scene
- You can also set it via Project → Project Settings → Application → Run → Main Scene

### Keyboard/mouse input not working in game

**What to do:**
- Make sure the game window has focus (click on it)
- Try Alt+Tab to switch to the game window
- On some systems, you might need to click inside the game window first
- Check that no other programs are capturing input (like overlays or recording software)

---

## Game Controls

Once the game is running, here's how to play:

| Action | Control |
|--------|---------|
| **Move** | W, A, S, D keys |
| **Look around** | Move your mouse |
| **Sprint** | Hold Shift while moving |
| **Jump** | Spacebar |
| **Attack** | Left mouse button |
| **Special ability** | Right mouse button |
| **Settings Menu** | ESC key |

### Coming Soon

These features are planned for future releases:

| Feature | Planned Control |
|---------|----------------|
| **Quest Journal** | J key |
| **Character Sheet** | C key |
| **Inventory** | I key |
| **World Map** | M key |
| **Interact with NPCs** | E key or walk into them |

---

## Exporting the Game (Creating a Standalone Executable)

So far, we've been playing the game by pressing F5 *inside* the Godot Editor. But what if you want to create a standalone game that you (or your friends) can play **without** having Godot installed? That's called "exporting" the game!

### Simple Overview

Exporting in Godot is much simpler than in some other engines:

1. **Project → Export** - Opens the export window
2. **Add export preset** - Choose your platform (Windows, Linux, macOS)
3. **Export Project** - Choose where to save and click Export

That's the basic idea! Now let's go through it step-by-step.

---

### Step-by-Step Export Guide

#### Step 1: Install Export Templates (One-time setup)

The first time you export, Godot needs to download "export templates" (about 500 MB).

1. In Godot Editor, click **Project** in the top menu
2. Click **Export...**
3. A window will open - if templates are missing, you'll see a message
4. Click **"Manage Export Templates"**
5. Click **"Download and Install"**
6. Wait for the download to complete (about 500 MB, takes 5-10 minutes)
7. Close the templates window

You only need to do this once! The templates work for all your Godot 4.3 projects.

---

#### Step 2: Add an Export Preset

1. In the Export window, click **"Add..."** at the top
2. Choose your platform:
   - **Windows Desktop** - For Windows PC (.exe file)
   - **Linux/X11** - For Linux PC
   - **macOS** - For Mac (requires a Mac to build properly)
3. A preset will appear in the left sidebar

**Pro tip:** You can add multiple presets (Windows, Linux, Mac) to export for all platforms!

---

#### Step 3: Configure Export Settings (Optional)

For most users, the default settings work fine. But here are a few options you might want to change:

1. Click on your preset (e.g., "Windows Desktop") in the left sidebar
2. Look for these settings:
   - **Texture Format** - Leave as default (usually "BPTC")
   - **Export With Debug** - Uncheck this for a final release (makes the game smaller and faster)
   - **Runnable (Executable)** - Make sure this is checked so you get an .exe file

Most settings can stay at default. Don't worry about getting everything perfect!

---

#### Step 4: Export the Game

1. Make sure your preset is selected (highlighted) in the left sidebar
2. Click the **"Export Project"** button at the bottom
3. Choose where to save your exported game:
   - Pick a folder (like Desktop or create a new folder called "MiddleEarthExport")
   - Give your game a name (like `MiddleEarthAdventure.exe` for Windows)
4. Click **"Save"**
5. **Wait** - Godot is creating the game files (usually takes 1-5 minutes)
6. When it's done, you'll have a standalone executable!

---

### What You Get After Exporting

After export completes, you'll have:

**On Windows:**
- `MiddleEarthAdventure.exe` - The game executable
- `MiddleEarthAdventure.pck` - Game data file (must stay in same folder as .exe)

**On Linux:**
- `MiddleEarthAdventure.x86_64` - The game executable (or similar name)
- `MiddleEarthAdventure.pck` - Game data file

**On macOS:**
- `MiddleEarthAdventure.app` - The complete game application
- Or separate executable + .pck file depending on settings

**Important:** Keep the `.exe` and `.pck` files together! Both are needed.

**You can share these files** with friends, and they can play your game without having Godot installed!

---

### Common Export Issues

**"No export template found"**
- You need to download export templates (see Step 1 above)
- Go to Project → Export → Manage Export Templates → Download and Install

**Export button is grayed out**
- Make sure you added an export preset first (Step 2)
- Make sure export templates are installed

**Exported game won't run**
- Make sure both the `.exe` and `.pck` files are in the same folder
- On Linux, make sure the file is executable: `chmod +x MiddleEarthAdventure.x86_64`
- Check if antivirus is blocking the file

**Exported game is huge (multiple GB)**
- This is normal for 3D games with lots of assets
- To reduce size: In export settings, try enabling "Optimize for File Size"
- Make sure "Export With Debug" is unchecked in the export preset

**Game runs but shows errors**
- Check the console output (run from terminal/command prompt to see errors)
- Common issue: Missing resources (make sure all scenes and scripts are included)
- Try exporting with "Export With Debug" checked to get more error info

**"PCK file not found" error**
- The `.pck` file must be in the same folder as the executable
- The `.pck` filename must match the executable name (or be named the same with .pck extension)

---

### Advanced: Export for Multiple Platforms

Want to share your game with friends on different operating systems? You can export for multiple platforms!

1. Add presets for Windows, Linux, and macOS (see Step 2)
2. Export each one to a different folder:
   - `Builds/Windows/MiddleEarthAdventure.exe`
   - `Builds/Linux/MiddleEarthAdventure.x86_64`
   - `Builds/macOS/MiddleEarthAdventure.app`
3. Share the appropriate folder with friends based on their OS

**Note:** macOS exports ideally should be built on a Mac for proper code signing and packaging.

---

## What's Next?

Now that you're playing, here are some tips for beginners:

1. **Start in The Shire** (the green area to the southwest) - it's the easiest area
2. **Talk to NPCs** - walk into characters like Gandalf to interact
3. **Open treasure chests** - walk into them to get items and gold
4. **Complete quests** - press J to see your active quests
5. **Level up** - defeat enemies to gain experience and become stronger

---

## Need More Help?

- **README.md** - Overview of all features
- **docs/SETUP.md** - Quick setup reference
- **docs/PLAYER_EXPERIENCE.md** - Detailed gameplay guide
- **docs/GAME_DESIGN.md** - Full game design document

---

Congratulations! 🎉 You've installed and are now playing Middle-earth Adventure RPG!

Enjoy your journey through Middle-earth!
