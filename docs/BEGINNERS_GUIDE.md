# Complete Beginner's Guide to Installing Middle-earth Adventure RPG

Welcome! This guide will walk you through every step to get this game running on your computer. Don't worry if you've never done anything like this before - we'll explain everything.

---

## Table of Contents
1. [What Is This Game?](#what-is-this-game)
2. [What You'll Need](#what-youll-need)
3. [Step 1: Download and Install Unity Hub](#step-1-download-and-install-unity-hub)
4. [Step 2: Install Unity Editor](#step-2-install-unity-editor)
5. [Step 3: Get the Game Files](#step-3-get-the-game-files)
6. [Step 4: Open the Project in Unity](#step-4-open-the-project-in-unity)
7. [Step 5: Play the Game](#step-5-play-the-game)
8. [Troubleshooting](#troubleshooting)
9. [Game Controls](#game-controls)

---

## What Is This Game?

Middle-earth Adventure RPG is a 3D role-playing game inspired by The Lord of the Rings. You'll explore lands like The Shire, Rohan, and Mordor, fight enemies, complete quests, and meet characters like Gandalf, Legolas, and Gimli.

This is a **Unity project**, which means it was made using a program called Unity. To play the game, you need to install Unity first.

---

## What You'll Need

Before we start, make sure you have:

| Requirement | Details |
|-------------|---------|
| **Computer** | Windows 10, Windows 11, macOS, or Linux |
| **Internet** | To download the software (about 3-5 GB total) |
| **Disk Space** | At least 10 GB of free space |
| **Time** | About 30-60 minutes for everything to download and install |

---

## Step 1: Download and Install Unity Hub

Unity Hub is a free program that helps you manage Unity projects and editors.

### For Windows:

1. **Open your web browser** (Chrome, Firefox, Edge, or Safari)
   - Look for the browser icon on your desktop or taskbar and click it
   
2. **Go to the Unity download page**
   - Click in the address bar at the top of your browser
   - Type: `https://unity.com/download`
   - Press the **Enter** key on your keyboard

3. **Download Unity Hub**
   - Look for a button that says **"Download for Windows"** and click it
   - A file will start downloading (usually goes to your "Downloads" folder)
   - Wait for the download to finish (you'll see a progress indicator)

4. **Install Unity Hub**
   - Find the downloaded file called `UnityHubSetup.exe`
     - Press **Windows key + E** to open File Explorer
     - Click on **Downloads** in the left sidebar
     - Look for `UnityHubSetup.exe`
   - Double-click the file to run it
   - If a window asks "Do you want to allow this app to make changes?" click **Yes**
   - Click **I Agree** when you see the license agreement
   - Click **Install**
   - Wait for the installation to complete
   - Click **Finish**

### For macOS:

1. **Open Safari** (or your preferred browser)
   - Click the Safari icon in your Dock (the bar at the bottom of your screen)

2. **Go to the Unity download page**
   - Click in the address bar at the top
   - Type: `https://unity.com/download`
   - Press **Return** (Enter)

3. **Download Unity Hub**
   - Click **"Download for Mac"**
   - Wait for the download to complete

4. **Install Unity Hub**
   - Open **Finder** (the blue smiling face icon in your Dock)
   - Click **Downloads** in the left sidebar
   - Find the file called `UnityHubSetup.dmg` and double-click it
   - A window will open - drag the **Unity Hub** icon to the **Applications** folder
   - Wait for the copy to complete
   - You can now close the window and eject the disk image

### For Linux:

1. **Open your web browser** and go to `https://unity.com/download`

2. **Download the Linux version** (AppImage format)

3. **Make it executable**
   - Open a terminal
   - Navigate to your Downloads folder: `cd ~/Downloads`
   - Make executable: `chmod +x UnityHub.AppImage`
   - Run it: `./UnityHub.AppImage`

---

## Step 2: Install Unity Editor

The Unity Editor is the actual program that runs the game. You need a specific version: **2022.3 LTS**.

### Open Unity Hub:

**Windows:**
- Click the **Start button** (Windows icon in the bottom-left corner)
- Type `Unity Hub`
- Click on **Unity Hub** when it appears

**macOS:**
- Click the **Launchpad** icon in your Dock (grid of squares icon)
- Click on **Unity Hub**

**Linux:**
- Run the AppImage you downloaded, or find Unity Hub in your applications menu

### Create a Unity Account (if you don't have one):

1. Unity Hub will ask you to sign in
2. Click **"Create account"** if you don't have one
3. Fill in your email address and create a password
4. Check your email for a verification link and click it
5. Go back to Unity Hub and sign in with your new account

### Get a Free License:

1. After signing in, Unity Hub may ask about a license
2. Choose **"Get a free personal license"**
3. Click **Agree and get personal edition license**

### Install Unity 2022.3 LTS:

1. In Unity Hub, click **"Installs"** in the left sidebar
2. Click the **"Install Editor"** button (blue button)
3. Find **Unity 2022.3.0f1 LTS** (this is the specific version the project uses)
   - If you can't find exactly 2022.3.0f1, any 2022.3.x version should work
   - LTS means "Long Term Support" - it's a stable version
4. Click **Install** next to it
5. On the next screen, you can leave the default options selected
6. Click **Continue** or **Install**
7. **Wait** - this download is large (about 2-3 GB) and may take 15-30 minutes depending on your internet speed

---

## Step 3: Get the Game Files

You need to download or have the game project files on your computer.

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
2. Make sure the folder contains folders named `Assets`, `ProjectSettings`, and `tools`

---

## Step 4: Open the Project in Unity

Now we'll connect everything together!

1. **Open Unity Hub** (if it's not already open)

2. **Add the project**
   - Click **"Projects"** in the left sidebar
   - Click the **"Add"** button (it might say "Add project from disk" or just show an "Add" arrow)
   - Navigate to where you saved/extracted the game files
   - Select the main project folder (the one containing `Assets`, `ProjectSettings`, etc.)
   - Click **"Add Project"** or **"Open"**

3. **Wait for Unity to process**
   - The first time you open a project, Unity needs to import all the files
   - This can take 5-15 minutes
   - You'll see a loading bar - just wait for it to finish

4. **The Unity Editor opens**
   - You'll see a complex interface with many panels
   - Don't worry! You only need to know a few things to play the game

---

## Step 5: Play the Game

Almost there! Just a couple more clicks:

1. **Open the Main scene** (if not already open)
   - In the Unity Editor, look at the bottom panel called **"Project"**
   - Navigate to `Assets/Scenes` (double-click folders to open them)
   - Double-click on **"Main"** (or **"Main.unity"**)

2. **Press Play!**
   - Look at the top-center of the Unity Editor
   - You'll see a **Play button** (▶️ triangle icon)
   - Click it!
   - The game will start in the center panel called "Game"

3. **To stop playing**
   - Click the Play button again (the same button you used to start)

---

## Troubleshooting

### "Unity version mismatch" or similar warning

**What it means:** The project wants a slightly different Unity version.

**What to do:** 
- Usually you can just click "Continue" or "Open" and it will work fine
- If it doesn't work, go back to Unity Hub → Installs and install version 2022.3.0f1 specifically

### "Missing scripts" or pink/purple objects

**What it means:** Some files didn't load correctly.

**What to do:**
1. Close Unity
2. Delete the `Library` folder inside the project folder
3. Open the project again in Unity Hub
4. Wait for it to reimport (this may take a while)

### The game runs slowly

**What to do:**
- In Unity, go to **Edit → Project Settings → Quality**
- Choose a lower quality level
- Or press F7 in-game to access optimization utilities

### Unity Hub won't open

**Windows:** Try running it as administrator:
- Right-click on Unity Hub
- Select "Run as administrator"

**macOS:** Check your security settings:
- Go to System Preferences → Security & Privacy
- Click "Open Anyway" if Unity Hub is blocked

### "Not enough disk space"

**What to do:**
- Delete files you don't need from your computer
- Empty your Recycle Bin (Windows) or Trash (macOS)
- Try installing Unity on a different drive if you have one

### "All compiler errors must be fixed before you can enter play mode"

This is the most common issue for new users! It means Unity found problems in the code that must be resolved before you can play.

**Most likely cause:** You're using a different Unity version than the project requires.

**Step-by-step fix:**

1. **Check which Unity version you have installed**
   - Open Unity Hub
   - Click **Installs** in the left sidebar
   - Note the version number (e.g., 2022.3.5f1)

2. **Install the correct version (2022.3.0f1)**
   - This project was made with Unity **2022.3.0f1**
   - In Unity Hub → Installs → Click **Install Editor**
   - Look for **2022.3.0f1** in the list and install it
   - If you can't find exactly 2022.3.0f1, any **2022.3.x** version should work

3. **Re-open the project with the correct version**
   - In Unity Hub → Projects
   - Click the dropdown arrow next to the project's Unity version
   - Select **2022.3.0f1** (or your 2022.3.x version)
   - Wait for Unity to reimport the project

**If you still get errors after using the correct Unity version:**

1. **Clear the Library folder** (this forces Unity to rebuild everything)
   - Close Unity completely
   - Open the project folder in File Explorer (Windows) or Finder (macOS)
   - Delete the folder named `Library`
   - Open the project again in Unity Hub
   - Wait for Unity to reimport all files (this may take 5-15 minutes)

2. **Check for missing packages**
   - In Unity, go to **Window → Package Manager**
   - Look for any packages with warning icons
   - Click **Update** or **Install** for any missing packages

3. **Refresh the project**
   - In Unity, go to **Assets → Reimport All**
   - Wait for the reimport to complete

**Still having problems?**
- Make sure you downloaded the complete project (all folders: Assets, ProjectSettings, etc.)
- Try downloading the project again from GitHub using "Download ZIP"
- If on Windows, avoid special characters in the project path (like #, %, &, etc.)

### I see errors in red text in Unity

**What to do:**
- Click on the red text at the bottom of Unity to see the error details
- Often you can ignore warnings (yellow text) and still play the game
- If it says "Script error" or "NullReferenceException", try closing and reopening the project

### "The layout could not be fully loaded"

**What it looks like:**
```
The layout "C:/Users/.../Unity/Editor-5.x/Preferences/Layouts/current/default-2022.dwlt" 
could not be fully loaded, this can happen when the layout contains EditorWindows 
not available in this project.
```

**What it means:** Your Unity Editor had a saved window layout from a previous project that uses different windows than this project. This is just a warning - it won't prevent you from playing the game.

**What to do:**
- This warning is **harmless** and you can usually **ignore it**
- Unity will automatically use a default layout instead
- To get rid of the warning permanently:
  1. In Unity, go to **Window → Layouts → Default**
  2. This resets your editor layout to the standard arrangement
  3. The warning won't appear next time you open the project

### "error CS0006: Metadata file could not be found" (com.unity.collab-proxy)

**What it looks like:**
```
error CS0006: Metadata file 'Library/PackageCache/com.unity.collab-proxy@.../Lib/Editor/PlasticSCM/log4netPlastic.dll' could not be found
error CS0006: Metadata file 'Library/PackageCache/com.unity.collab-proxy@.../Unity.Plastic.Antlr3.Runtime.dll' could not be found
error CS0006: Metadata file 'Library/PackageCache/com.unity.collab-proxy@.../Unity.Plastic.Newtonsoft.Json.dll' could not be found
```

**What it means:** The Unity Collaborate/Plastic SCM package files are corrupted or incomplete. This is a known Unity issue that can happen when the package cache becomes corrupted.

**Step-by-step fix:**

1. **Close Unity completely**
   - Make sure the Unity Editor is fully closed

2. **Delete the Library folder**
   - Open the project folder in File Explorer (Windows) or Finder (macOS)
   - Find and delete the folder named `Library`
   - This is safe to delete - Unity will rebuild it

3. **Delete the package cache** (if the above doesn't work)
   - Close Unity completely
   - Navigate to your global Unity package cache (note: these are hidden folders):
     - **Windows:** 
       1. Press **Windows key + R** to open the Run dialog
       2. Type `%LOCALAPPDATA%\Unity\cache\packages` and press Enter
       3. This opens the packages folder directly
     - **macOS:** 
       1. Open **Finder**
       2. Click **Go** in the menu bar, then hold **Option** key and click **Library**
       3. Navigate to `Unity/cache/packages`
       4. Full path: `/Users/YourUsername/Library/Unity/cache/packages` (replace "YourUsername" with your actual username)
   - Delete the `com.unity.collab-proxy` folder (or delete the entire `packages` folder to be thorough)
   - Open your project again in Unity Hub

4. **If errors persist, manually refresh packages**
   - In Unity, go to **Window → Package Manager**
   - Find **Version Control** (formerly Collaborate) in the list
   - Click the **menu button with three vertical dots** next to the package and select **Remove**
   - After removal, restart Unity
   - If you need Version Control, you can reinstall it from Package Manager

5. **Alternative: Update the package**
   - Open your project's `Packages/manifest.json` file
   - Find the line with `"com.unity.collab-proxy"`
   - Update the version number to a newer version (check Unity Package Manager for latest)
   - Save the file and let Unity reimport

**Note:** The `com.unity.collab-proxy` package is for Unity's cloud collaboration features. If you're not using cloud collaboration, removing this package won't affect your ability to play the game.

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
| **Interact** | Walk into NPCs, chests, or locations |
| **Quest Journal** | J key |
| **Character Sheet** | C key |
| **World Map** | M key |
| **Settings Menu** | ESC or O key |

### Advanced Controls (Optional)

These are developer and testing tools - you don't need these to play the game:

| Action | Control |
|--------|---------|
| **Performance Monitor** | F3 key |
| **Release Checklist** | F4 key |
| **Feedback System** | F5 key |
| **Screenshot Tool** | F6 key (F12 for quick capture) |
| **Optimization Utils** | F7 key |
| **Beta Manager** | F8 key |
| **Release Manager** | F9 key |
| **Support Tools** | F10 key |

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
