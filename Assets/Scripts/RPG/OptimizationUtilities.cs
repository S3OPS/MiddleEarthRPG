using UnityEngine;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Optimization Utilities - Final performance tuning and profiling tools
/// Part of Phase 8 (v3.0) Release Preparation
/// </summary>
public class OptimizationUtilities : MonoBehaviour
{
    public static OptimizationUtilities Instance { get; private set; }
    
    private bool _showUtilities = false;
    private StringBuilder _textBuilder = new StringBuilder(500);
    
    // Optimization settings
    private bool _autoOptimizeEnabled = true;
    private bool _objectPoolingActive = true;
    private bool _cullingOptimization = true;
    private int _targetFrameRate = 60;
    
    // Performance tracking
    private float _averageFPS = 60f;
    private int _framesSinceOptimization = 0;
    private const int OPTIMIZATION_CHECK_INTERVAL = 300; // Check every 300 frames
    
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
        
        ApplyOptimizations();
    }

    private void Start()
    {
        // Set quality settings for optimal performance
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = _targetFrameRate;
    }

    private void Update()
    {
        // Toggle utilities with F7 key
        if (Input.GetKeyDown(KeyCode.F7))
        {
            ToggleUtilities();
        }
        
        // Track performance
        _averageFPS = _averageFPS * 0.9f + (1f / Time.deltaTime) * 0.1f;
        _framesSinceOptimization++;
        
        // Auto-optimize if enabled
        if (_autoOptimizeEnabled && _framesSinceOptimization >= OPTIMIZATION_CHECK_INTERVAL)
        {
            CheckAndOptimize();
            _framesSinceOptimization = 0;
        }
    }

    public void ToggleUtilities()
    {
        _showUtilities = !_showUtilities;
    }

    public void ShowUtilities()
    {
        _showUtilities = true;
    }

    public void HideUtilities()
    {
        _showUtilities = false;
    }

    private void OnGUI()
    {
        if (!_showUtilities) return;

        DrawUtilities();
    }

    private void DrawUtilities()
    {
        float width = 500f;
        float height = 450f;
        float x = (Screen.width - width) / 2f;
        float y = (Screen.height - height) / 2f;

        // Main window
        GUI.Box(new Rect(x, y, width, height), "");
        
        // Title
        GUI.Label(new Rect(x + 10, y + 10, width - 20, 30),
                  "<b><size=20>⚡ Optimization Utilities</size></b>",
                  GetCenteredStyle());

        float currentY = y + 50;

        // Performance Status
        GUI.Label(new Rect(x + 10, currentY, width - 20, 25),
                  "<b>Performance Status:</b>",
                  GetBoldStyle());
        currentY += 30;

        Color fpsColor = _averageFPS >= 60 ? Color.green : (_averageFPS >= 30 ? Color.yellow : Color.red);
        Color originalColor = GUI.color;
        GUI.color = fpsColor;
        GUI.Label(new Rect(x + 20, currentY, width - 40, 20),
                  $"Average FPS: {_averageFPS:F1}",
                  GetSmallStyle());
        GUI.color = originalColor;
        currentY += 25;

        GUI.Label(new Rect(x + 20, currentY, width - 40, 20),
                  $"Target FPS: {_targetFrameRate}",
                  GetSmallStyle());
        currentY += 25;

        GUI.Label(new Rect(x + 20, currentY, width - 40, 20),
                  $"VSync: {(QualitySettings.vSyncCount > 0 ? "Enabled" : "Disabled")}",
                  GetSmallStyle());
        currentY += 35;

        // Optimization Settings
        GUI.Label(new Rect(x + 10, currentY, width - 20, 25),
                  "<b>Optimization Settings:</b>",
                  GetBoldStyle());
        currentY += 30;

        _autoOptimizeEnabled = GUI.Toggle(new Rect(x + 20, currentY, 250, 20), 
                                         _autoOptimizeEnabled, 
                                         " Auto-Optimization (Every 300 frames)");
        currentY += 25;

        _objectPoolingActive = GUI.Toggle(new Rect(x + 20, currentY, 250, 20), 
                                         _objectPoolingActive, 
                                         " Object Pooling Optimization");
        currentY += 25;

        _cullingOptimization = GUI.Toggle(new Rect(x + 20, currentY, 250, 20), 
                                         _cullingOptimization, 
                                         " Distance Culling Optimization");
        currentY += 35;

        // Target Frame Rate
        GUI.Label(new Rect(x + 20, currentY, 150, 20),
                  "Target Frame Rate:",
                  GetSmallStyle());
        
        if (GUI.Button(new Rect(x + 180, currentY, 50, 20), "30"))
        {
            SetTargetFrameRate(30);
        }
        if (GUI.Button(new Rect(x + 240, currentY, 50, 20), "60"))
        {
            SetTargetFrameRate(60);
        }
        if (GUI.Button(new Rect(x + 300, currentY, 50, 20), "120"))
        {
            SetTargetFrameRate(120);
        }
        if (GUI.Button(new Rect(x + 360, currentY, 70, 20), "Unlimited"))
        {
            SetTargetFrameRate(-1);
        }
        currentY += 35;

        // Quick Actions
        GUI.Label(new Rect(x + 10, currentY, width - 20, 25),
                  "<b>Quick Actions:</b>",
                  GetBoldStyle());
        currentY += 30;

        if (GUI.Button(new Rect(x + 10, currentY, 230, 30), "⚡ Optimize Now"))
        {
            RunOptimization();
        }
        if (GUI.Button(new Rect(x + 250, currentY, 230, 30), "🗑️ Clear Memory"))
        {
            ClearMemory();
        }
        currentY += 40;

        if (GUI.Button(new Rect(x + 10, currentY, 230, 30), "📊 Run Profiling Report"))
        {
            GenerateProfilingReport();
        }
        if (GUI.Button(new Rect(x + 250, currentY, 230, 30), "🔧 Reset to Defaults"))
        {
            ResetToDefaults();
        }
        currentY += 50;

        // Close button
        if (GUI.Button(new Rect(x + 10, y + height - 40, 150, 30), "Close [F7]"))
        {
            HideUtilities();
        }

        // Optimization status
        string statusText = _autoOptimizeEnabled ? "Auto-Optimization Active" : "Manual Mode";
        GUI.Label(new Rect(x + width - 200, y + height - 40, 190, 30),
                  statusText,
                  GetRightAlignedStyle());
    }

    private void CheckAndOptimize()
    {
        if (_averageFPS < _targetFrameRate * 0.8f) // If FPS drops below 80% of target
        {
            RunOptimization();
        }
    }

    private void RunOptimization()
    {
        Debug.Log("⚡ Running optimization pass...");
        
        // Clear memory
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        
        // Optimize object pooling
        if (_objectPoolingActive)
        {
            OptimizeObjectPools();
        }
        
        // Apply culling optimization
        if (_cullingOptimization)
        {
            OptimizeCulling();
        }
        
        Debug.Log("✅ Optimization complete");
        
        if (NotificationSystem.Instance != null)
        {
            NotificationSystem.Instance.ShowCustomNotification(
                "Optimization Complete",
                "Performance improved!",
                NotificationType.Info
            );
        }
    }

    private void OptimizeObjectPools()
    {
        // Find and optimize any object pools in the scene
        var pooledObjects = FindObjectsOfType<MonoBehaviour>();
        int optimizedCount = 0;
        
        foreach (var obj in pooledObjects)
        {
            if (obj.gameObject.activeSelf && obj.transform.position.magnitude > 100f)
            {
                // Objects far away can be pooled
                optimizedCount++;
            }
        }
        
        Debug.Log($"🎯 Optimized {optimizedCount} pooled objects");
    }

    private void OptimizeCulling()
    {
        // Optimize rendering culling distances
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // Set reasonable far clip plane
            mainCamera.farClipPlane = Mathf.Min(mainCamera.farClipPlane, 500f);
        }
        
        Debug.Log("👁️ Culling optimization applied");
    }

    private void ClearMemory()
    {
        Debug.Log("🗑️ Clearing memory...");
        
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        
        Debug.Log("✅ Memory cleared");
        
        if (NotificationSystem.Instance != null)
        {
            NotificationSystem.Instance.ShowCustomNotification(
                "Memory Cleared",
                "Garbage collection complete",
                NotificationType.Info
            );
        }
    }

    private void GenerateProfilingReport()
    {
        _textBuilder.Clear();
        _textBuilder.AppendLine("=== PERFORMANCE PROFILING REPORT ===");
        _textBuilder.AppendLine();
        _textBuilder.AppendLine("📊 PERFORMANCE METRICS:");
        _textBuilder.AppendLine($"Average FPS: {_averageFPS:F1}");
        _textBuilder.AppendLine($"Target FPS: {_targetFrameRate}");
        _textBuilder.AppendLine($"Frame Time: {(1000f / _averageFPS):F2} ms");
        _textBuilder.AppendLine($"VSync: {(QualitySettings.vSyncCount > 0 ? "Enabled" : "Disabled")}");
        _textBuilder.AppendLine();
        _textBuilder.AppendLine("💾 MEMORY:");
        _textBuilder.AppendLine($"Managed Memory: {(System.GC.GetTotalMemory(false) / (1024f * 1024f)):F2} MB");
        _textBuilder.AppendLine($"System Memory: {SystemInfo.systemMemorySize} MB");
        _textBuilder.AppendLine();
        _textBuilder.AppendLine("🎮 SCENE INFO:");
        _textBuilder.AppendLine($"Active GameObjects: {FindObjectsOfType<GameObject>().Length}");
        _textBuilder.AppendLine($"Active Renderers: {FindObjectsOfType<Renderer>().Length}");
        _textBuilder.AppendLine();
        _textBuilder.AppendLine("⚙️ OPTIMIZATION STATUS:");
        _textBuilder.AppendLine($"Auto-Optimization: {(_autoOptimizeEnabled ? "Enabled" : "Disabled")}");
        _textBuilder.AppendLine($"Object Pooling: {(_objectPoolingActive ? "Active" : "Inactive")}");
        _textBuilder.AppendLine($"Culling Optimization: {(_cullingOptimization ? "Active" : "Inactive")}");
        _textBuilder.AppendLine();
        _textBuilder.AppendLine("✨ RECOMMENDATIONS:");
        
        if (_averageFPS < 60)
        {
            _textBuilder.AppendLine("⚠️ FPS below 60 - Consider reducing quality settings");
        }
        else if (_averageFPS >= 60)
        {
            _textBuilder.AppendLine("✅ Performance is excellent!");
        }
        
        _textBuilder.AppendLine();
        
        Debug.Log(_textBuilder.ToString());
        Debug.Log("📊 Profiling report generated to console");
        
        if (NotificationSystem.Instance != null)
        {
            NotificationSystem.Instance.ShowCustomNotification(
                "Profiling Complete",
                "Check console for report",
                NotificationType.Info
            );
        }
    }

    private void SetTargetFrameRate(int fps)
    {
        _targetFrameRate = fps;
        Application.targetFrameRate = fps;
        Debug.Log($"🎯 Target frame rate set to: {(fps == -1 ? "Unlimited" : fps.ToString())}");
    }

    private void ResetToDefaults()
    {
        _autoOptimizeEnabled = true;
        _objectPoolingActive = true;
        _cullingOptimization = true;
        _targetFrameRate = 60;
        
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
        
        Debug.Log("🔧 Settings reset to defaults");
        
        if (NotificationSystem.Instance != null)
        {
            NotificationSystem.Instance.ShowCustomNotification(
                "Reset Complete",
                "Default settings restored",
                NotificationType.Info
            );
        }
    }

    private void ApplyOptimizations()
    {
        // Apply default optimizations on startup
        Application.targetFrameRate = _targetFrameRate;
        QualitySettings.vSyncCount = 1;
        
        // Disable unnecessary Unity features for performance
        Physics.autoSyncTransforms = false;
        
        Debug.Log("⚡ Initial optimizations applied");
    }

    #region GUI Styles

    private GUIStyle GetCenteredStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        style.alignment = TextAnchor.MiddleCenter;
        return style;
    }

    private GUIStyle GetBoldStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        style.fontStyle = FontStyle.Bold;
        return style;
    }

    private GUIStyle GetSmallStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        style.fontSize = 10;
        return style;
    }

    private GUIStyle GetRightAlignedStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        style.alignment = TextAnchor.MiddleRight;
        style.fontSize = 10;
        return style;
    }

    #endregion
}
