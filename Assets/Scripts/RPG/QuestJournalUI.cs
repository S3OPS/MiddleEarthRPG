using UnityEngine;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Quest Journal UI - Visual quest tracking interface
/// Part of Phase 7 (v2.6) UI/UX Polish
/// </summary>
public class QuestJournalUI : MonoBehaviour
{
    public static QuestJournalUI Instance { get; private set; }
    
    private bool _showJournal = false;
    private QuestManager _questManager;
    private EnhancedQuestSystem _enhancedQuestSystem;
    private Vector2 _scrollPosition;
    private int _selectedTab = 0; // 0 = Active, 1 = Completed, 2 = All
    private StringBuilder _textBuilder = new StringBuilder(1000);
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Find quest managers
        _questManager = FindObjectOfType<QuestManager>();
        _enhancedQuestSystem = EnhancedQuestSystem.Instance;
    }

    private void Update()
    {
        // Toggle journal with J key
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleJournal();
        }
    }

    public void ToggleJournal()
    {
        _showJournal = !_showJournal;
    }

    public void ShowJournal()
    {
        _showJournal = true;
    }

    public void HideJournal()
    {
        _showJournal = false;
    }

    private void OnGUI()
    {
        if (!_showJournal) return;

        DrawQuestJournal();
    }

    private void DrawQuestJournal()
    {
        float width = 700f;
        float height = 500f;
        float x = (Screen.width - width) / 2f;
        float y = (Screen.height - height) / 2f;

        // Main journal window
        GUI.Box(new Rect(x, y, width, height), "");
        
        // Title
        GUI.Label(new Rect(x + 10, y + 10, width - 20, 30),
                  "<b><size=20>📜 Quest Journal</size></b>",
                  GetCenteredStyle());

        // Tabs
        DrawTabs(x, y + 50, width);

        // Quest list area
        Rect scrollViewRect = new Rect(x + 10, y + 90, width - 20, height - 140);
        Rect scrollContentRect = new Rect(0, 0, width - 40, CalculateContentHeight());
        
        _scrollPosition = GUI.BeginScrollView(scrollViewRect, _scrollPosition, scrollContentRect);
        
        DrawQuestList(10, 10, width - 40);
        
        GUI.EndScrollView();

        // Bottom buttons
        if (GUI.Button(new Rect(x + 10, y + height - 40, 150, 30), "Close [J]"))
        {
            HideJournal();
        }

        // Quest count
        int activeCount = GetActiveQuestCount();
        int completedCount = GetCompletedQuestCount();
        GUI.Label(new Rect(x + width - 200, y + height - 40, 190, 30),
                  $"Active: {activeCount} | Completed: {completedCount}",
                  GetRightAlignedStyle());
    }

    private void DrawTabs(float x, float y, float width)
    {
        float tabWidth = width / 3f - 20f;
        
        // Active Quests tab
        if (GUI.Button(new Rect(x + 10, y, tabWidth, 30), "Active Quests"))
        {
            _selectedTab = 0;
            _scrollPosition = Vector2.zero;
        }
        
        // Completed Quests tab
        if (GUI.Button(new Rect(x + 20 + tabWidth, y, tabWidth, 30), "Completed"))
        {
            _selectedTab = 1;
            _scrollPosition = Vector2.zero;
        }
        
        // All Quests tab
        if (GUI.Button(new Rect(x + 30 + tabWidth * 2, y, tabWidth, 30), "All Quests"))
        {
            _selectedTab = 2;
            _scrollPosition = Vector2.zero;
        }

        // Highlight selected tab
        float selectedX = x + 10 + (_selectedTab * (tabWidth + 10));
        GUI.Box(new Rect(selectedX, y - 2, tabWidth, 34), "");
    }

    private void DrawQuestList(float x, float y, float width)
    {
        float currentY = y;
        
        // Draw enhanced quests if available
        if (_enhancedQuestSystem != null)
        {
            var quests = _enhancedQuestSystem.GetActiveQuests();
            
            foreach (var quest in quests)
            {
                if (!ShouldShowQuest(quest)) continue;
                
                currentY = DrawEnhancedQuest(quest, x, currentY, width);
            }
        }
        
        // Draw standard quests if available
        if (_questManager != null)
        {
            var quests = _selectedTab == 1 ? new List<Quest>() : _questManager.GetActiveQuests();
            
            foreach (var quest in quests)
            {
                if (!ShouldShowStandardQuest(quest)) continue;
                
                currentY = DrawStandardQuest(quest, x, currentY, width);
            }
        }

        if (currentY == y)
        {
            GUI.Label(new Rect(x, y, width, 40),
                     "<i>No quests to display</i>",
                     GetCenteredStyle());
        }
    }

    private bool ShouldShowQuest(EnhancedQuest quest)
    {
        if (_selectedTab == 0) return quest.isActive && !quest.IsComplete();
        if (_selectedTab == 1) return quest.IsComplete();
        return true;
    }

    private bool ShouldShowStandardQuest(Quest quest)
    {
        if (_selectedTab == 0) return quest.isActive && !quest.isCompleted;
        if (_selectedTab == 1) return quest.isCompleted;
        return true;
    }

    private float DrawEnhancedQuest(EnhancedQuest quest, float x, float y, float width)
    {
        float boxHeight = 100f;
        
        // Quest box
        GUI.Box(new Rect(x, y, width, boxHeight), "");
        
        // Quest name and type
        _textBuilder.Clear();
        _textBuilder.Append("<b><size=14>");
        _textBuilder.Append(quest.questName);
        _textBuilder.Append("</size></b> [");
        _textBuilder.Append(quest.questType.ToString());
        _textBuilder.Append("]");
        
        GUI.Label(new Rect(x + 10, y + 5, width - 20, 25),
                  _textBuilder.ToString(),
                  GetRichTextStyle());

        // Quest description
        GUI.Label(new Rect(x + 10, y + 30, width - 20, 35),
                  quest.description,
                  GetSmallStyle());

        // Current stage
        var currentStage = quest.GetCurrentStage();
        if (currentStage != null)
        {
            _textBuilder.Clear();
            _textBuilder.Append("Current: ");
            _textBuilder.Append(currentStage.stageName);
            _textBuilder.Append(" (Stage ");
            _textBuilder.Append(quest.currentStageIndex + 1);
            _textBuilder.Append("/");
            _textBuilder.Append(quest.stages.Count);
            _textBuilder.Append(")");
            
            GUI.Label(new Rect(x + 10, y + 70, width - 20, 20),
                      _textBuilder.ToString(),
                      GetSmallStyle());
        }

        return y + boxHeight + 10;
    }

    private float DrawStandardQuest(Quest quest, float x, float y, float width)
    {
        float boxHeight = 90f;
        
        // Quest box
        GUI.Box(new Rect(x, y, width, boxHeight), "");
        
        // Quest name
        GUI.Label(new Rect(x + 10, y + 5, width - 20, 25),
                  $"<b><size=14>{quest.questName}</size></b>",
                  GetRichTextStyle());

        // Quest description
        GUI.Label(new Rect(x + 10, y + 30, width - 20, 30),
                  quest.description,
                  GetSmallStyle());

        // Progress
        float progress = quest.GetCompletionPercentage();
        GUI.Label(new Rect(x + 10, y + 65, width - 20, 20),
                  $"Progress: {progress:F0}% ({quest.objectives.FindAll(o => o.isCompleted).Count}/{quest.objectives.Count} objectives)",
                  GetSmallStyle());

        return y + boxHeight + 10;
    }

    private float CalculateContentHeight()
    {
        int questCount = 0;
        
        if (_enhancedQuestSystem != null)
        {
            var quests = _enhancedQuestSystem.GetActiveQuests();
            foreach (var quest in quests)
            {
                if (ShouldShowQuest(quest)) questCount++;
            }
        }
        
        if (_questManager != null)
        {
            var quests = _selectedTab == 1 ? new List<Quest>() : _questManager.GetActiveQuests();
            foreach (var quest in quests)
            {
                if (ShouldShowStandardQuest(quest)) questCount++;
            }
        }

        return questCount > 0 ? questCount * 110f : 100f;
    }

    private int GetActiveQuestCount()
    {
        int count = 0;
        
        if (_enhancedQuestSystem != null)
        {
            count += _enhancedQuestSystem.GetActiveQuests().Count;
        }
        
        if (_questManager != null)
        {
            count += _questManager.GetActiveQuests().Count;
        }
        
        return count;
    }

    private int GetCompletedQuestCount()
    {
        int count = 0;
        
        if (_enhancedQuestSystem != null)
        {
            count += _enhancedQuestSystem.GetCompletedQuestCount();
        }
        
        // Note: Standard QuestManager doesn't track completed count
        
        return count;
    }

    private GUIStyle GetCenteredStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        style.alignment = TextAnchor.MiddleCenter;
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

    private GUIStyle GetRichTextStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        style.wordWrap = true;
        return style;
    }

    private GUIStyle GetSmallStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        style.wordWrap = true;
        style.fontSize = 10;
        return style;
    }
}
