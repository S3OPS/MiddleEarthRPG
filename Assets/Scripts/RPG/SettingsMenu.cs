using UnityEngine;
using System;

/// <summary>
/// Settings menu system for graphics, audio, and controls configuration
/// Part of Phase 6 (v2.5) Technical Systems
/// </summary>
public class SettingsMenu : MonoBehaviour
{
    public static SettingsMenu Instance { get; private set; }
    
    [Header("Graphics Settings")]
    public int targetFrameRate = 60;
    public bool vSyncEnabled = true;
    public QualityLevel qualityLevel = QualityLevel.High;
    public bool fullscreen = true;
    
    [Header("Audio Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 0.8f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    
    [Header("Gameplay Settings")]
    public float mouseSensitivity = 1f;
    public bool invertYAxis = false;
    public float cameraDistance = 5f;
    
    private GameSettings _settings;
    private bool _showSettingsMenu = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        LoadSettings();
        ApplySettings();
    }

    private void Update()
    {
        // Toggle settings menu with ESC or Settings key
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.O))
        {
            ToggleSettingsMenu();
        }
    }

    private void OnGUI()
    {
        if (_showSettingsMenu)
        {
            DrawSettingsMenu();
        }
    }

    #region Settings Menu UI
    
    public void ToggleSettingsMenu()
    {
        _showSettingsMenu = !_showSettingsMenu;
        Time.timeScale = _showSettingsMenu ? 0f : 1f; // Pause game when menu open
    }

    private void DrawSettingsMenu()
    {
        float menuWidth = 500f;
        float menuHeight = 600f;
        float menuX = (Screen.width - menuWidth) / 2f;
        float menuY = (Screen.height - menuHeight) / 2f;
        
        GUI.Box(new Rect(menuX, menuY, menuWidth, menuHeight), "");
        
        // Title
        GUI.Label(new Rect(menuX + 10, menuY + 10, menuWidth - 20, 30),
                  "<b><size=22>Settings</size></b>",
                  new GUIStyle(GUI.skin.label) { richText = true, alignment = TextAnchor.UpperCenter });
        
        float yPos = menuY + 50;
        
        // Graphics Section
        GUI.Label(new Rect(menuX + 20, yPos, menuWidth - 40, 25),
                  "<b>Graphics</b>",
                  new GUIStyle(GUI.skin.label) { richText = true });
        yPos += 30;
        
        GUI.Label(new Rect(menuX + 20, yPos, 150, 25), "Quality:");
        qualityLevel = (QualityLevel)GUI.SelectionGrid(
            new Rect(menuX + 180, yPos, menuWidth - 200, 25),
            (int)qualityLevel,
            new string[] { "Low", "Medium", "High" },
            3
        );
        yPos += 30;
        
        fullscreen = GUI.Toggle(new Rect(menuX + 20, yPos, 200, 25), fullscreen, "Fullscreen");
        yPos += 30;
        
        vSyncEnabled = GUI.Toggle(new Rect(menuX + 20, yPos, 200, 25), vSyncEnabled, "V-Sync");
        yPos += 30;
        
        GUI.Label(new Rect(menuX + 20, yPos, 150, 25), $"Target FPS: {targetFrameRate}");
        targetFrameRate = (int)GUI.HorizontalSlider(new Rect(menuX + 180, yPos + 5, menuWidth - 200, 20), targetFrameRate, 30, 144);
        yPos += 40;
        
        // Audio Section
        GUI.Label(new Rect(menuX + 20, yPos, menuWidth - 40, 25),
                  "<b>Audio</b>",
                  new GUIStyle(GUI.skin.label) { richText = true });
        yPos += 30;
        
        GUI.Label(new Rect(menuX + 20, yPos, 150, 25), $"Master: {(masterVolume * 100):F0}%");
        masterVolume = GUI.HorizontalSlider(new Rect(menuX + 180, yPos + 5, menuWidth - 200, 20), masterVolume, 0f, 1f);
        yPos += 30;
        
        GUI.Label(new Rect(menuX + 20, yPos, 150, 25), $"Music: {(musicVolume * 100):F0}%");
        musicVolume = GUI.HorizontalSlider(new Rect(menuX + 180, yPos + 5, menuWidth - 200, 20), musicVolume, 0f, 1f);
        yPos += 30;
        
        GUI.Label(new Rect(menuX + 20, yPos, 150, 25), $"SFX: {(sfxVolume * 100):F0}%");
        sfxVolume = GUI.HorizontalSlider(new Rect(menuX + 180, yPos + 5, menuWidth - 200, 20), sfxVolume, 0f, 1f);
        yPos += 40;
        
        // Controls Section
        GUI.Label(new Rect(menuX + 20, yPos, menuWidth - 40, 25),
                  "<b>Controls</b>",
                  new GUIStyle(GUI.skin.label) { richText = true });
        yPos += 30;
        
        GUI.Label(new Rect(menuX + 20, yPos, 150, 25), $"Mouse Sensitivity: {mouseSensitivity:F1}");
        mouseSensitivity = GUI.HorizontalSlider(new Rect(menuX + 180, yPos + 5, menuWidth - 200, 20), mouseSensitivity, 0.1f, 3f);
        yPos += 30;
        
        invertYAxis = GUI.Toggle(new Rect(menuX + 20, yPos, 200, 25), invertYAxis, "Invert Y Axis");
        yPos += 30;
        
        GUI.Label(new Rect(menuX + 20, yPos, 150, 25), $"Camera Distance: {cameraDistance:F1}");
        cameraDistance = GUI.HorizontalSlider(new Rect(menuX + 180, yPos + 5, menuWidth - 200, 20), cameraDistance, 2f, 10f);
        yPos += 40;
        
        // Buttons
        if (GUI.Button(new Rect(menuX + 20, menuY + menuHeight - 90, (menuWidth - 50) / 3, 30), "Apply"))
        {
            ApplySettings();
            SaveSettings();
        }
        
        if (GUI.Button(new Rect(menuX + 30 + (menuWidth - 50) / 3, menuY + menuHeight - 90, (menuWidth - 50) / 3, 30), "Reset"))
        {
            ResetToDefaults();
        }
        
        if (GUI.Button(new Rect(menuX + 40 + 2 * (menuWidth - 50) / 3, menuY + menuHeight - 90, (menuWidth - 50) / 3, 30), "Close"))
        {
            ToggleSettingsMenu();
        }
        
        // Help text
        GUI.Label(new Rect(menuX + 20, menuY + menuHeight - 50, menuWidth - 40, 40),
                  "<i>Press ESC or O to open/close settings menu</i>",
                  new GUIStyle(GUI.skin.label) { richText = true, alignment = TextAnchor.MiddleCenter, fontSize = 10 });
    }
    
    #endregion

    #region Settings Application
    
    private void ApplySettings()
    {
        // Graphics
        Application.targetFrameRate = targetFrameRate;
        QualitySettings.vSyncCount = vSyncEnabled ? 1 : 0;
        QualitySettings.SetQualityLevel((int)qualityLevel);
        Screen.fullScreen = fullscreen;
        
        // Audio
        AudioListener.volume = masterVolume;
        if (AudioManager.Instance != null)
        {
            // Would set specific volumes on AudioManager
        }
        
        Debug.Log("✅ Settings applied");
    }

    private void SaveSettings()
    {
        _settings = new GameSettings
        {
            // Graphics
            targetFrameRate = this.targetFrameRate,
            vSyncEnabled = this.vSyncEnabled,
            qualityLevel = (int)this.qualityLevel,
            fullscreen = this.fullscreen,
            
            // Audio
            masterVolume = this.masterVolume,
            musicVolume = this.musicVolume,
            sfxVolume = this.sfxVolume,
            
            // Controls
            mouseSensitivity = this.mouseSensitivity,
            invertYAxis = this.invertYAxis,
            cameraDistance = this.cameraDistance
        };
        
        string json = JsonUtility.ToJson(_settings, true);
        PlayerPrefs.SetString("GameSettings", json);
        PlayerPrefs.Save();
        
        Debug.Log("✅ Settings saved");
    }

    private void LoadSettings()
    {
        if (!PlayerPrefs.HasKey("GameSettings"))
        {
            ResetToDefaults();
            return;
        }
        
        try
        {
            string json = PlayerPrefs.GetString("GameSettings");
            _settings = JsonUtility.FromJson<GameSettings>(json);
            
            // Graphics
            targetFrameRate = _settings.targetFrameRate;
            vSyncEnabled = _settings.vSyncEnabled;
            qualityLevel = (QualityLevel)_settings.qualityLevel;
            fullscreen = _settings.fullscreen;
            
            // Audio
            masterVolume = _settings.masterVolume;
            musicVolume = _settings.musicVolume;
            sfxVolume = _settings.sfxVolume;
            
            // Controls
            mouseSensitivity = _settings.mouseSensitivity;
            invertYAxis = _settings.invertYAxis;
            cameraDistance = _settings.cameraDistance;
            
            Debug.Log("✅ Settings loaded");
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to load settings: {e.Message}");
            ResetToDefaults();
        }
    }

    private void ResetToDefaults()
    {
        // Graphics
        targetFrameRate = 60;
        vSyncEnabled = true;
        qualityLevel = QualityLevel.High;
        fullscreen = true;
        
        // Audio
        masterVolume = 1f;
        musicVolume = 0.8f;
        sfxVolume = 1f;
        
        // Controls
        mouseSensitivity = 1f;
        invertYAxis = false;
        cameraDistance = 5f;
        
        ApplySettings();
        Debug.Log("✅ Settings reset to defaults");
    }
    
    #endregion

    #region Public API
    
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        ApplySettings();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        ApplySettings();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        ApplySettings();
    }

    public void SetQuality(QualityLevel level)
    {
        qualityLevel = level;
        ApplySettings();
    }

    public void SetFullscreen(bool enabled)
    {
        fullscreen = enabled;
        ApplySettings();
    }
    
    #endregion
}

[Serializable]
public class GameSettings
{
    // Graphics
    public int targetFrameRate;
    public bool vSyncEnabled;
    public int qualityLevel;
    public bool fullscreen;
    
    // Audio
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
    
    // Controls
    public float mouseSensitivity;
    public bool invertYAxis;
    public float cameraDistance;
}

public enum QualityLevel
{
    Low = 0,
    Medium = 1,
    High = 2
}
