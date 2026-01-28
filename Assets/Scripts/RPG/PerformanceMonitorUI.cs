using UnityEngine;
using System.Text;
using System.Collections.Generic;

/// <summary>
/// Performance Monitor UI - Real-time performance metrics display
/// Part of Phase 8 (v3.0) Release Preparation
/// </summary>
public class PerformanceMonitorUI : MonoBehaviour
{
    public static PerformanceMonitorUI Instance { get; private set; }
    
    private bool _showMonitor = false;
    private StringBuilder _textBuilder = new StringBuilder(1000);
    
    // Performance metrics
    private float _fps;
    private float _frameTime;
    private int _drawCalls;
    private float _memoryUsage;
    private int _activeObjects;
    
    // FPS tracking
    private Queue<float> _fpsHistory = new Queue<float>();
    private const int FPS_HISTORY_SIZE = 60;
    private float _fpsUpdateTimer = 0f;
    private float _fpsUpdateInterval = 0.1f;
    
    // System info
    private string _gpuName;
    private string _cpuName;
    private int _systemMemory;
    
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
        
        // Get system info
        _gpuName = SystemInfo.graphicsDeviceName;
        _cpuName = SystemInfo.processorType;
        _systemMemory = SystemInfo.systemMemorySize;
    }

    private void Update()
    {
        // Toggle monitor with F3 key
        if (Input.GetKeyDown(KeyCode.F3))
        {
            ToggleMonitor();
        }
        
        UpdateMetrics();
    }

    private void UpdateMetrics()
    {
        // Update FPS
        _fpsUpdateTimer += Time.deltaTime;
        if (_fpsUpdateTimer >= _fpsUpdateInterval)
        {
            _fps = 1f / Time.deltaTime;
            _frameTime = Time.deltaTime * 1000f;
            
            _fpsHistory.Enqueue(_fps);
            if (_fpsHistory.Count > FPS_HISTORY_SIZE)
            {
                _fpsHistory.Dequeue();
            }
            
            _fpsUpdateTimer = 0f;
        }
        
        // Update memory usage
        _memoryUsage = (float)System.GC.GetTotalMemory(false) / (1024f * 1024f);
        
        // Count active GameObjects
        _activeObjects = FindObjectsOfType<GameObject>().Length;
    }

    public void ToggleMonitor()
    {
        _showMonitor = !_showMonitor;
    }

    public void ShowMonitor()
    {
        _showMonitor = true;
    }

    public void HideMonitor()
    {
        _showMonitor = false;
    }

    private void OnGUI()
    {
        if (!_showMonitor) return;

        DrawPerformanceMonitor();
    }

    private void DrawPerformanceMonitor()
    {
        float width = 400f;
        float height = 400f;
        float x = 10f;
        float y = 10f;

        // Main window
        GUI.Box(new Rect(x, y, width, height), "");
        
        // Title
        GUI.Label(new Rect(x + 10, y + 10, width - 20, 30),
                  "<b><size=18>⚡ Performance Monitor</size></b>",
                  GetCenteredStyle());

        float currentY = y + 45;
        float lineHeight = 20f;

        // FPS and Frame Time
        _textBuilder.Clear();
        _textBuilder.Append("<b>Performance:</b>");
        GUI.Label(new Rect(x + 10, currentY, width - 20, lineHeight),
                  _textBuilder.ToString(),
                  GetRichTextStyle());
        currentY += lineHeight + 5;

        DrawMetricLine(x + 20, currentY, width - 30, "FPS", $"{_fps:F1}", GetFPSColor(_fps));
        currentY += lineHeight;
        
        DrawMetricLine(x + 20, currentY, width - 30, "Frame Time", $"{_frameTime:F2} ms", Color.white);
        currentY += lineHeight;

        // FPS Graph
        DrawFPSGraph(x + 10, currentY, width - 20, 60f);
        currentY += 70;

        // Memory
        _textBuilder.Clear();
        _textBuilder.Append("<b>Memory:</b>");
        GUI.Label(new Rect(x + 10, currentY, width - 20, lineHeight),
                  _textBuilder.ToString(),
                  GetRichTextStyle());
        currentY += lineHeight + 5;

        DrawMetricLine(x + 20, currentY, width - 30, "Managed Memory", $"{_memoryUsage:F1} MB", Color.white);
        currentY += lineHeight;
        
        DrawMetricLine(x + 20, currentY, width - 30, "Active Objects", _activeObjects.ToString(), Color.white);
        currentY += lineHeight + 10;

        // System Info
        _textBuilder.Clear();
        _textBuilder.Append("<b>System Info:</b>");
        GUI.Label(new Rect(x + 10, currentY, width - 20, lineHeight),
                  _textBuilder.ToString(),
                  GetRichTextStyle());
        currentY += lineHeight + 5;

        GUI.Label(new Rect(x + 20, currentY, width - 30, lineHeight),
                  $"GPU: {TruncateString(_gpuName, 35)}",
                  GetSmallStyle());
        currentY += lineHeight;

        GUI.Label(new Rect(x + 20, currentY, width - 30, lineHeight),
                  $"CPU: {TruncateString(_cpuName, 35)}",
                  GetSmallStyle());
        currentY += lineHeight;

        GUI.Label(new Rect(x + 20, currentY, width - 30, lineHeight),
                  $"System RAM: {_systemMemory} MB",
                  GetSmallStyle());
        currentY += lineHeight + 10;

        // Close button
        if (GUI.Button(new Rect(x + 10, y + height - 40, width - 20, 30), "Close [F3]"))
        {
            HideMonitor();
        }
    }

    private void DrawMetricLine(float x, float y, float width, string label, string value, Color valueColor)
    {
        GUI.Label(new Rect(x, y, width * 0.6f, 20),
                  label + ":",
                  GetSmallStyle());
        
        Color originalColor = GUI.color;
        GUI.color = valueColor;
        GUI.Label(new Rect(x + width * 0.6f, y, width * 0.4f, 20),
                  value,
                  GetBoldSmallStyle());
        GUI.color = originalColor;
    }

    private void DrawFPSGraph(float x, float y, float width, float height)
    {
        // Draw graph background
        GUI.Box(new Rect(x, y, width, height), "");

        if (_fpsHistory.Count < 2) return;

        // Draw FPS line graph
        float[] fpsArray = _fpsHistory.ToArray();
        float maxFPS = 120f;
        float minFPS = 0f;

        Vector3 prevPoint = Vector3.zero;
        bool first = true;

        for (int i = 0; i < fpsArray.Length; i++)
        {
            float normalizedX = (float)i / FPS_HISTORY_SIZE;
            float normalizedY = Mathf.Clamp01((fpsArray[i] - minFPS) / (maxFPS - minFPS));
            
            float pointX = x + normalizedX * width;
            float pointY = y + height - (normalizedY * height);

            if (!first)
            {
                // Draw line from previous point
                DrawLine(new Vector2(prevPoint.x, prevPoint.y), 
                        new Vector2(pointX, pointY), 
                        GetFPSColor(fpsArray[i]));
            }

            prevPoint = new Vector3(pointX, pointY, 0);
            first = false;
        }

        // Draw reference lines
        GUI.color = new Color(1, 1, 1, 0.2f);
        float fps60Y = y + height - ((60f / maxFPS) * height);
        DrawLine(new Vector2(x, fps60Y), new Vector2(x + width, fps60Y), Color.yellow);
        GUI.color = Color.white;

        // Labels
        GUI.Label(new Rect(x + 5, y + 5, 100, 15), "FPS History", GetTinyStyle());
    }

    private void DrawLine(Vector2 start, Vector2 end, Color color)
    {
        // Simple line drawing using GUI boxes
        float distance = Vector2.Distance(start, end);
        float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
        
        Color originalColor = GUI.color;
        GUI.color = color;
        
        // Draw a thin box as a line
        GUIUtility.RotateAroundPivot(angle, start);
        GUI.Box(new Rect(start.x, start.y - 1, distance, 2), "");
        GUIUtility.RotateAroundPivot(-angle, start);
        
        GUI.color = originalColor;
    }

    private Color GetFPSColor(float fps)
    {
        if (fps >= 60f) return Color.green;
        if (fps >= 30f) return Color.yellow;
        return Color.red;
    }

    private string TruncateString(string str, int maxLength)
    {
        if (str.Length <= maxLength) return str;
        return str.Substring(0, maxLength - 3) + "...";
    }

    #region GUI Styles

    private GUIStyle GetCenteredStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        style.alignment = TextAnchor.MiddleCenter;
        return style;
    }

    private GUIStyle GetRichTextStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        return style;
    }

    private GUIStyle GetSmallStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        style.fontSize = 10;
        return style;
    }

    private GUIStyle GetTinyStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        style.fontSize = 8;
        return style;
    }

    private GUIStyle GetBoldSmallStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        style.fontSize = 10;
        style.fontStyle = FontStyle.Bold;
        return style;
    }

    #endregion
}
