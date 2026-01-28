using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Notification System - Pop-up notifications for quests, achievements, and loot
/// Part of Phase 7 (v2.6) UI/UX Polish
/// </summary>
public class NotificationSystem : MonoBehaviour
{
    public static NotificationSystem Instance { get; private set; }
    
    private Queue<Notification> _notificationQueue = new Queue<Notification>();
    private Notification _currentNotification;
    private float _notificationTimer;
    private float _notificationDuration = 5f;
    private float _fadeInDuration = 0.3f;
    private float _fadeOutDuration = 0.3f;
    private float _alpha = 0f;
    
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
        if (_currentNotification != null)
        {
            UpdateCurrentNotification();
        }
        else if (_notificationQueue.Count > 0)
        {
            ShowNextNotification();
        }
    }

    private void UpdateCurrentNotification()
    {
        _notificationTimer += Time.deltaTime;

        // Fade in
        if (_notificationTimer < _fadeInDuration)
        {
            _alpha = _notificationTimer / _fadeInDuration;
        }
        // Full display
        else if (_notificationTimer < _notificationDuration - _fadeOutDuration)
        {
            _alpha = 1f;
        }
        // Fade out
        else if (_notificationTimer < _notificationDuration)
        {
            _alpha = (_notificationDuration - _notificationTimer) / _fadeOutDuration;
        }
        // Done
        else
        {
            _currentNotification = null;
            _alpha = 0f;
        }
    }

    private void ShowNextNotification()
    {
        _currentNotification = _notificationQueue.Dequeue();
        _notificationTimer = 0f;
        _alpha = 0f;
    }

    private void OnGUI()
    {
        if (_currentNotification == null || _alpha <= 0f) return;

        DrawNotification(_currentNotification);
    }

    private void DrawNotification(Notification notification)
    {
        float width = 350f;
        float height = 80f;
        float x = Screen.width - width - 20;
        float y = 80f;

        // Apply alpha for fade in/out
        Color originalColor = GUI.color;
        GUI.color = new Color(1f, 1f, 1f, _alpha);

        // Background box
        GUI.Box(new Rect(x, y, width, height), "");

        // Icon and title
        string icon = GetNotificationIcon(notification.type);
        GUI.Label(new Rect(x + 10, y + 10, 50, 30),
                  $"<size=24>{icon}</size>",
                  GetRichTextStyle());

        GUI.Label(new Rect(x + 60, y + 10, width - 70, 25),
                  $"<b>{notification.title}</b>",
                  GetRichTextStyle());

        // Message
        GUI.Label(new Rect(x + 60, y + 35, width - 70, 35),
                  notification.message,
                  GetSmallStyle());

        // Restore color
        GUI.color = originalColor;
    }

    #region Public API

    public void ShowQuestNotification(string questName, string message)
    {
        var notification = new Notification
        {
            type = NotificationType.Quest,
            title = questName,
            message = message
        };
        _notificationQueue.Enqueue(notification);
    }

    public void ShowAchievementNotification(string achievementName)
    {
        var notification = new Notification
        {
            type = NotificationType.Achievement,
            title = "Achievement Unlocked!",
            message = achievementName
        };
        _notificationQueue.Enqueue(notification);
    }

    public void ShowLootNotification(string itemName, int quantity = 1)
    {
        var notification = new Notification
        {
            type = NotificationType.Loot,
            title = "Item Acquired",
            message = quantity > 1 ? $"{itemName} x{quantity}" : itemName
        };
        _notificationQueue.Enqueue(notification);
    }

    public void ShowLevelUpNotification(int newLevel)
    {
        var notification = new Notification
        {
            type = NotificationType.LevelUp,
            title = "Level Up!",
            message = $"You are now level {newLevel}"
        };
        _notificationQueue.Enqueue(notification);
    }

    public void ShowLocationNotification(string locationName)
    {
        var notification = new Notification
        {
            type = NotificationType.Location,
            title = "Location Discovered",
            message = locationName
        };
        _notificationQueue.Enqueue(notification);
    }

    public void ShowCustomNotification(string title, string message, NotificationType type = NotificationType.Info)
    {
        var notification = new Notification
        {
            type = type,
            title = title,
            message = message
        };
        _notificationQueue.Enqueue(notification);
    }

    #endregion

    private string GetNotificationIcon(NotificationType type)
    {
        switch (type)
        {
            case NotificationType.Quest: return "📜";
            case NotificationType.Achievement: return "🏆";
            case NotificationType.Loot: return "💎";
            case NotificationType.LevelUp: return "⭐";
            case NotificationType.Location: return "🗺️";
            case NotificationType.Warning: return "⚠️";
            case NotificationType.Info: return "ℹ️";
            default: return "📢";
        }
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
        style.wordWrap = true;
        return style;
    }
}

public class Notification
{
    public NotificationType type;
    public string title;
    public string message;
}

public enum NotificationType
{
    Quest,
    Achievement,
    Loot,
    LevelUp,
    Location,
    Warning,
    Info
}
