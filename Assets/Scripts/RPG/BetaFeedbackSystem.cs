using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;

/// <summary>
/// Beta Feedback System - In-game feedback and bug reporting tool
/// Part of Phase 8 (v3.0) Release Preparation
/// </summary>
public class BetaFeedbackSystem : MonoBehaviour
{
    public static BetaFeedbackSystem Instance { get; private set; }
    
    private bool _showFeedbackForm = false;
    private StringBuilder _textBuilder = new StringBuilder(500);
    
    // Feedback data
    private string _feedbackText = "";
    private FeedbackType _selectedType = FeedbackType.Suggestion;
    private int _rating = 3;
    private List<FeedbackReport> _feedbackHistory = new List<FeedbackReport>();
    
    private Vector2 _scrollPosition;
    private bool _showHistory = false;
    
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
    }

    private void Update()
    {
        // Toggle feedback form with F5 key
        if (Input.GetKeyDown(KeyCode.F5))
        {
            ToggleFeedbackForm();
        }
    }

    public void ToggleFeedbackForm()
    {
        _showFeedbackForm = !_showFeedbackForm;
        if (_showFeedbackForm)
        {
            _showHistory = false;
        }
    }

    public void ShowFeedbackForm()
    {
        _showFeedbackForm = true;
        _showHistory = false;
    }

    public void HideFeedbackForm()
    {
        _showFeedbackForm = false;
    }

    private void OnGUI()
    {
        if (!_showFeedbackForm) return;

        if (_showHistory)
        {
            DrawFeedbackHistory();
        }
        else
        {
            DrawFeedbackForm();
        }
    }

    private void DrawFeedbackForm()
    {
        float width = 500f;
        float height = 450f;
        float x = (Screen.width - width) / 2f;
        float y = (Screen.height - height) / 2f;

        // Main window
        GUI.Box(new Rect(x, y, width, height), "");
        
        // Title
        GUI.Label(new Rect(x + 10, y + 10, width - 20, 30),
                  "<b><size=20>📝 Beta Feedback</size></b>",
                  GetCenteredStyle());

        float currentY = y + 50;

        // Feedback Type
        GUI.Label(new Rect(x + 10, currentY, 150, 25),
                  "Feedback Type:",
                  GetBoldStyle());

        string[] typeNames = System.Enum.GetNames(typeof(FeedbackType));
        _selectedType = (FeedbackType)GUI.SelectionGrid(
            new Rect(x + 160, currentY, width - 170, 25),
            (int)_selectedType,
            typeNames,
            typeNames.Length
        );
        currentY += 35;

        // Rating
        GUI.Label(new Rect(x + 10, currentY, 150, 25),
                  "Overall Rating:",
                  GetBoldStyle());

        for (int i = 1; i <= 5; i++)
        {
            string star = i <= _rating ? "⭐" : "☆";
            if (GUI.Button(new Rect(x + 160 + (i - 1) * 35, currentY, 30, 25), star))
            {
                _rating = i;
            }
        }
        currentY += 35;

        // Feedback Text Area
        GUI.Label(new Rect(x + 10, currentY, width - 20, 25),
                  "Your Feedback:",
                  GetBoldStyle());
        currentY += 25;

        _feedbackText = GUI.TextArea(new Rect(x + 10, currentY, width - 20, 150), _feedbackText, 1000);
        currentY += 160;

        // System Info
        GUI.Label(new Rect(x + 10, currentY, width - 20, 15),
                  $"<i>System: Unity {Application.unityVersion} | {SystemInfo.operatingSystem}</i>",
                  GetTinyStyle());
        currentY += 20;

        // Buttons
        if (GUI.Button(new Rect(x + 10, currentY, 150, 30), "Submit Feedback"))
        {
            SubmitFeedback();
        }

        if (GUI.Button(new Rect(x + 170, currentY, 150, 30), "View History"))
        {
            _showHistory = true;
        }

        if (GUI.Button(new Rect(x + 330, currentY, 150, 30), "Close [F5]"))
        {
            HideFeedbackForm();
        }

        currentY += 35;

        // Feedback count
        GUI.Label(new Rect(x + 10, currentY, width - 20, 20),
                  $"Total Feedback Submitted: {_feedbackHistory.Count}",
                  GetSmallStyle());
    }

    private void DrawFeedbackHistory()
    {
        float width = 600f;
        float height = 500f;
        float x = (Screen.width - width) / 2f;
        float y = (Screen.height - height) / 2f;

        // Main window
        GUI.Box(new Rect(x, y, width, height), "");
        
        // Title
        GUI.Label(new Rect(x + 10, y + 10, width - 20, 30),
                  "<b><size=20>📋 Feedback History</size></b>",
                  GetCenteredStyle());

        // Back button
        if (GUI.Button(new Rect(x + 10, y + 50, 100, 25), "← Back"))
        {
            _showHistory = false;
        }

        float currentY = y + 85;

        if (_feedbackHistory.Count == 0)
        {
            GUI.Label(new Rect(x + 10, currentY, width - 20, 40),
                      "<i>No feedback submitted yet. Press 'Submit Feedback' to add your first report!</i>",
                      GetCenteredStyle());
        }
        else
        {
            // Scroll view for feedback items
            Rect scrollViewRect = new Rect(x + 10, currentY, width - 20, height - currentY - 50);
            Rect scrollContentRect = new Rect(0, 0, width - 40, _feedbackHistory.Count * 120f);
            
            _scrollPosition = GUI.BeginScrollView(scrollViewRect, _scrollPosition, scrollContentRect);
            
            float itemY = 10;
            for (int i = _feedbackHistory.Count - 1; i >= 0; i--)
            {
                DrawFeedbackItem(10, itemY, width - 50, _feedbackHistory[i]);
                itemY += 120f;
            }
            
            GUI.EndScrollView();
        }

        // Close button
        if (GUI.Button(new Rect(x + 10, y + height - 40, 150, 30), "Close [F5]"))
        {
            HideFeedbackForm();
        }

        // Export button
        if (GUI.Button(new Rect(x + width - 160, y + height - 40, 150, 30), "Export All"))
        {
            ExportFeedback();
        }
    }

    private void DrawFeedbackItem(float x, float y, float width, FeedbackReport report)
    {
        GUI.Box(new Rect(x, y, width, 110), "");

        // Header with type and rating
        string typeIcon = GetFeedbackTypeIcon(report.Type);
        string stars = new string('⭐', report.Rating);
        
        GUI.Label(new Rect(x + 5, y + 5, width - 10, 20),
                  $"{typeIcon} <b>{report.Type}</b> - {stars}",
                  GetRichTextStyle());

        // Timestamp
        GUI.Label(new Rect(x + 5, y + 25, width - 10, 15),
                  report.Timestamp,
                  GetTinyStyle());

        // Feedback text (truncated)
        string truncatedText = report.FeedbackText.Length > 150 
            ? report.FeedbackText.Substring(0, 150) + "..." 
            : report.FeedbackText;
        
        GUI.Label(new Rect(x + 5, y + 45, width - 10, 60),
                  truncatedText,
                  GetSmallStyle());
    }

    private void SubmitFeedback()
    {
        if (string.IsNullOrWhiteSpace(_feedbackText))
        {
            Debug.LogWarning("Feedback text is empty");
            return;
        }

        var report = new FeedbackReport
        {
            Type = _selectedType,
            Rating = _rating,
            FeedbackText = _feedbackText,
            Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            UnityVersion = Application.unityVersion,
            Platform = SystemInfo.operatingSystem
        };

        _feedbackHistory.Add(report);

        // Log to console
        Debug.Log($"📝 Feedback Submitted: [{_selectedType}] Rating: {_rating}/5\n{_feedbackText}");

        // Show notification
        if (NotificationSystem.Instance != null)
        {
            NotificationSystem.Instance.ShowCustomNotification(
                "Feedback Submitted",
                "Thank you for your feedback!",
                NotificationType.Info
            );
        }

        // Clear form
        _feedbackText = "";
        _rating = 3;
    }

    private void ExportFeedback()
    {
        _textBuilder.Clear();
        _textBuilder.AppendLine("=== BETA FEEDBACK REPORT ===");
        _textBuilder.AppendLine($"Generated: {DateTime.Now}");
        _textBuilder.AppendLine($"Total Reports: {_feedbackHistory.Count}");
        _textBuilder.AppendLine();

        foreach (var report in _feedbackHistory)
        {
            _textBuilder.AppendLine($"--- {report.Type} ---");
            _textBuilder.AppendLine($"Rating: {report.Rating}/5");
            _textBuilder.AppendLine($"Date: {report.Timestamp}");
            _textBuilder.AppendLine($"Platform: {report.Platform}");
            _textBuilder.AppendLine($"Feedback: {report.FeedbackText}");
            _textBuilder.AppendLine();
        }

        Debug.Log(_textBuilder.ToString());
        Debug.Log("📋 Feedback exported to console");
    }

    private string GetFeedbackTypeIcon(FeedbackType type)
    {
        switch (type)
        {
            case FeedbackType.Bug: return "🐛";
            case FeedbackType.Suggestion: return "💡";
            case FeedbackType.Praise: return "👍";
            default: return "📝";
        }
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
        style.wordWrap = true;
        return style;
    }

    private GUIStyle GetTinyStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        style.fontSize = 8;
        return style;
    }

    #endregion
}

public class FeedbackReport
{
    public FeedbackType Type { get; set; }
    public int Rating { get; set; }
    public string FeedbackText { get; set; }
    public string Timestamp { get; set; }
    public string UnityVersion { get; set; }
    public string Platform { get; set; }
}

public enum FeedbackType
{
    Bug,
    Suggestion,
    Praise
}
