using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// HUD system for displaying Phase 5 content (dialogues, lore, boss info, quests)
/// Provides visual feedback for the new narrative systems
/// </summary>
public class ContentHUD : MonoBehaviour
{
    public static ContentHUD Instance { get; private set; }
    
    private StringBuilder _hudBuilder = new StringBuilder(500);
    private bool _showDialogue = false;
    private bool _showLoreBook = false;
    private bool _showBossInfo = false;
    private bool _showQuestJournal = false;
    
    // Display state
    private DialogueNode _currentDialogueNode;
    private string _currentNPCName;
    private LoreBook _currentBook;
    private BossEncounter _currentBoss;
    private List<EnhancedQuest> _activeQuests = new List<EnhancedQuest>();
    
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

    private void OnGUI()
    {
        if (_showDialogue && _currentDialogueNode != null)
        {
            DrawDialogueBox();
        }
        
        if (_showLoreBook && _currentBook != null)
        {
            DrawLoreBook();
        }
        
        if (_showBossInfo && _currentBoss != null)
        {
            DrawBossHealthBar();
        }
        
        if (_showQuestJournal)
        {
            DrawQuestJournal();
        }
        
        // Always show help text
        DrawHelpText();
    }

    #region Dialogue UI
    public void ShowDialogue(DialogueNode node, string npcName)
    {
        _currentDialogueNode = node;
        _currentNPCName = npcName;
        _showDialogue = true;
    }

    public void HideDialogue()
    {
        _showDialogue = false;
        _currentDialogueNode = null;
    }

    private void DrawDialogueBox()
    {
        // Background box
        float boxWidth = 600f;
        float boxHeight = 250f;
        float boxX = (Screen.width - boxWidth) / 2f;
        float boxY = Screen.height - boxHeight - 50f;
        
        GUI.Box(new Rect(boxX, boxY, boxWidth, boxHeight), "");
        
        // NPC Name
        GUI.Label(new Rect(boxX + 10, boxY + 5, boxWidth - 20, 25), 
                  $"<b><size=16>{_currentNPCName}</size></b>",
                  new GUIStyle(GUI.skin.label) { richText = true });
        
        // Dialogue text
        GUI.Label(new Rect(boxX + 10, boxY + 35, boxWidth - 20, 100),
                  _currentDialogueNode.text,
                  new GUIStyle(GUI.skin.label) { wordWrap = true });
        
        // Choices
        if (_currentDialogueNode.choices != null && _currentDialogueNode.choices.Count > 0)
        {
            float choiceY = boxY + 145;
            for (int i = 0; i < _currentDialogueNode.choices.Count; i++)
            {
                if (GUI.Button(new Rect(boxX + 10, choiceY + (i * 30), boxWidth - 20, 25),
                              $"{i + 1}. {_currentDialogueNode.choices[i].text}"))
                {
                    if (DialogueSystem.Instance != null)
                    {
                        DialogueSystem.Instance.SelectChoice(i);
                    }
                }
            }
        }
        else
        {
            // End of conversation
            if (GUI.Button(new Rect(boxX + 10, boxY + 145, boxWidth - 20, 30), "Close [ESC]"))
            {
                HideDialogue();
            }
        }
    }
    #endregion

    #region Lore Book UI
    public void ShowLoreBook(LoreBook book)
    {
        _currentBook = book;
        _showLoreBook = true;
    }

    public void HideLoreBook()
    {
        _showLoreBook = false;
        _currentBook = null;
    }

    private void DrawLoreBook()
    {
        // Book background
        float boxWidth = 700f;
        float boxHeight = 500f;
        float boxX = (Screen.width - boxWidth) / 2f;
        float boxY = (Screen.height - boxHeight) / 2f;
        
        GUI.Box(new Rect(boxX, boxY, boxWidth, boxHeight), "");
        
        // Book title
        GUI.Label(new Rect(boxX + 10, boxY + 10, boxWidth - 20, 30),
                  $"<b><size=18>{_currentBook.title}</size></b>",
                  new GUIStyle(GUI.skin.label) { richText = true, alignment = TextAnchor.UpperCenter });
        
        // Category and location
        GUI.Label(new Rect(boxX + 10, boxY + 45, boxWidth - 20, 20),
                  $"<i>Category: {_currentBook.category} | Found in: {_currentBook.location}</i>",
                  new GUIStyle(GUI.skin.label) { richText = true, alignment = TextAnchor.UpperCenter });
        
        // Content (scrollable would be better, but keeping it simple)
        GUI.Label(new Rect(boxX + 20, boxY + 75, boxWidth - 40, boxHeight - 130),
                  _currentBook.content,
                  new GUIStyle(GUI.skin.label) { wordWrap = true });
        
        // Close button
        if (GUI.Button(new Rect(boxX + 10, boxY + boxHeight - 40, boxWidth - 20, 30), "Close [B]"))
        {
            HideLoreBook();
        }
    }
    #endregion

    #region Boss UI
    public void ShowBossInfo(BossEncounter boss)
    {
        _currentBoss = boss;
        _showBossInfo = true;
    }

    public void HideBossInfo()
    {
        _showBossInfo = false;
        _currentBoss = null;
    }

    private void DrawBossHealthBar()
    {
        if (_currentBoss == null) return;
        
        var bossData = _currentBoss.GetBossData();
        int currentHealth = _currentBoss.GetCurrentHealth();
        float healthPercent = (float)currentHealth / bossData.maxHealth;
        
        // Boss name and title
        float boxWidth = 500f;
        float boxHeight = 80f;
        float boxX = (Screen.width - boxWidth) / 2f;
        float boxY = 50f;
        
        GUI.Box(new Rect(boxX, boxY, boxWidth, boxHeight), "");
        
        // Boss name
        GUI.Label(new Rect(boxX + 10, boxY + 5, boxWidth - 20, 25),
                  $"<b><size=18><color=red>⚔ {bossData.bossName} ⚔</color></size></b>",
                  new GUIStyle(GUI.skin.label) { richText = true, alignment = TextAnchor.UpperCenter });
        
        // Health bar background
        GUI.Box(new Rect(boxX + 10, boxY + 35, boxWidth - 20, 30), "");
        
        // Health bar fill
        Color healthColor = healthPercent > 0.5f ? Color.green : (healthPercent > 0.25f ? Color.yellow : Color.red);
        GUI.color = healthColor;
        GUI.Box(new Rect(boxX + 10, boxY + 35, (boxWidth - 20) * healthPercent, 30), "");
        GUI.color = Color.white;
        
        // Health text
        GUI.Label(new Rect(boxX + 10, boxY + 35, boxWidth - 20, 30),
                  $"{currentHealth} / {bossData.maxHealth}",
                  new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
        
        // Phase indicator
        var currentPhase = _currentBoss.GetCurrentPhase();
        if (currentPhase != null && currentPhase.isActive)
        {
            GUI.Label(new Rect(boxX + 10, boxY + 67, boxWidth - 20, 15),
                      $"<color=orange>Phase: {currentPhase.phaseName}</color>",
                      new GUIStyle(GUI.skin.label) { richText = true, alignment = TextAnchor.UpperCenter, fontSize = 10 });
        }
    }
    #endregion

    #region Quest Journal UI
    public void ToggleQuestJournal()
    {
        _showQuestJournal = !_showQuestJournal;
        if (_showQuestJournal && EnhancedQuestSystem.Instance != null)
        {
            _activeQuests = EnhancedQuestSystem.Instance.GetActiveQuests();
        }
    }

    private void DrawQuestJournal()
    {
        float boxWidth = 600f;
        float boxHeight = 400f;
        float boxX = (Screen.width - boxWidth) / 2f;
        float boxY = (Screen.height - boxHeight) / 2f;
        
        GUI.Box(new Rect(boxX, boxY, boxWidth, boxHeight), "");
        
        // Title
        GUI.Label(new Rect(boxX + 10, boxY + 10, boxWidth - 20, 30),
                  "<b><size=20>Quest Journal</size></b>",
                  new GUIStyle(GUI.skin.label) { richText = true, alignment = TextAnchor.UpperCenter });
        
        // Quest list
        float questY = boxY + 50;
        if (_activeQuests.Count > 0)
        {
            foreach (var quest in _activeQuests)
            {
                var currentStage = quest.GetCurrentStage();
                string questText = $"<b>{quest.questName}</b> [{quest.questType}]\n" +
                                 $"Stage: {currentStage?.stageName ?? "Unknown"}\n" +
                                 $"Progress: {quest.currentStageIndex + 1}/{quest.stages.Count}";
                
                GUI.Label(new Rect(boxX + 20, questY, boxWidth - 40, 60),
                         questText,
                         new GUIStyle(GUI.skin.label) { richText = true, wordWrap = true });
                
                questY += 70;
                
                if (questY > boxY + boxHeight - 100) break; // Don't overflow
            }
        }
        else
        {
            GUI.Label(new Rect(boxX + 20, questY, boxWidth - 40, 30),
                     "<i>No active quests</i>",
                     new GUIStyle(GUI.skin.label) { richText = true });
        }
        
        // Close button
        if (GUI.Button(new Rect(boxX + 10, boxY + boxHeight - 40, boxWidth - 20, 30), "Close [J]"))
        {
            _showQuestJournal = false;
        }
        
        // Stats
        if (EnhancedQuestSystem.Instance != null)
        {
            int completed = EnhancedQuestSystem.Instance.GetCompletedQuestCount();
            GUI.Label(new Rect(boxX + 10, boxY + boxHeight - 70, boxWidth - 20, 20),
                     $"Completed Quests: {completed}",
                     new GUIStyle(GUI.skin.label) { fontSize = 10 });
        }
    }
    #endregion

    #region Help Text
    private void DrawHelpText()
    {
        _hudBuilder.Clear();
        _hudBuilder.AppendLine("<b>Content Controls:</b>");
        _hudBuilder.AppendLine("[J] Quest Journal");
        _hudBuilder.AppendLine("[L] Lore Collection");
        _hudBuilder.AppendLine("[ESC] Close Dialogue");
        
        if (LoreBookSystem.Instance != null)
        {
            int discovered = LoreBookSystem.Instance.GetDiscoveredCount();
            int total = LoreBookSystem.Instance.GetTotalBooks();
            _hudBuilder.AppendLine($"Lore: {discovered}/{total} ({LoreBookSystem.Instance.GetCompletionPercentage():F0}%)");
        }
        
        if (DialogueSystem.Instance != null && _currentNPCName != null)
        {
            string status = DialogueSystem.Instance.GetRelationshipStatus(_currentNPCName);
            _hudBuilder.AppendLine($"Reputation: {status}");
        }
        
        GUI.Label(new Rect(10, Screen.height - 120, 250, 120),
                 _hudBuilder.ToString(),
                 new GUIStyle(GUI.skin.label) { richText = true, fontSize = 10 });
    }
    #endregion

    #region Input Handling
    private void Update()
    {
        // Quest Journal toggle
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleQuestJournal();
        }
        
        // Lore collection toggle
        if (Input.GetKeyDown(KeyCode.L))
        {
            ShowLoreCollection();
        }
        
        // Close dialogue
        if (Input.GetKeyDown(KeyCode.Escape) && _showDialogue)
        {
            HideDialogue();
        }
        
        // Close lore book
        if (Input.GetKeyDown(KeyCode.B) && _showLoreBook)
        {
            HideLoreBook();
        }
    }

    private void ShowLoreCollection()
    {
        if (LoreBookSystem.Instance == null) return;
        
        var discoveredBooks = LoreBookSystem.Instance.GetDiscoveredBooks();
        if (discoveredBooks.Count > 0)
        {
            // Show the first discovered book as a demo
            ShowLoreBook(discoveredBooks[0]);
        }
        else
        {
            Debug.Log("No lore books discovered yet.");
        }
    }
    #endregion
}
