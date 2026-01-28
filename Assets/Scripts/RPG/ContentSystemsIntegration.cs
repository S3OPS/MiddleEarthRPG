using UnityEngine;

/// <summary>
/// Integrates all Phase 5 (v2.4) Content & Narrative systems
/// Provides demo functionality and system coordination
/// </summary>
public class ContentSystemsIntegration : MonoBehaviour
{
    public static ContentSystemsIntegration Instance { get; private set; }
    
    [Header("System References")]
    public DialogueSystem dialogueSystem;
    public BossEncounterSystem bossSystem;
    public LoreBookSystem loreSystem;
    public EnhancedQuestSystem questSystem;
    public ContentHUD contentHUD;
    
    [Header("Demo Mode")]
    public bool enableDemoContent = true;
    public bool autoDiscoverLore = false;
    public bool spawnTestBoss = false;
    
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

    private void Start()
    {
        InitializeSystems();
        
        if (enableDemoContent)
        {
            SetupDemoContent();
        }
    }

    private void InitializeSystems()
    {
        // Ensure all systems are initialized
        if (dialogueSystem == null && DialogueSystem.Instance != null)
        {
            dialogueSystem = DialogueSystem.Instance;
        }
        
        if (bossSystem == null && BossEncounterSystem.Instance != null)
        {
            bossSystem = BossEncounterSystem.Instance;
        }
        
        if (loreSystem == null && LoreBookSystem.Instance != null)
        {
            loreSystem = LoreBookSystem.Instance;
        }
        
        if (questSystem == null && EnhancedQuestSystem.Instance != null)
        {
            questSystem = EnhancedQuestSystem.Instance;
        }
        
        if (contentHUD == null && ContentHUD.Instance != null)
        {
            contentHUD = ContentHUD.Instance;
        }
        
        Debug.Log("✅ Content Systems Integration initialized");
        LogSystemStatus();
    }

    private void SetupDemoContent()
    {
        Debug.Log("🎮 Setting up demo content...");
        
        // Auto-discover some lore books for testing
        if (autoDiscoverLore && loreSystem != null)
        {
            DiscoverSampleLore();
        }
        
        // Activate sample quests
        if (questSystem != null)
        {
            ActivateSampleQuests();
        }
        
        // Spawn test boss if enabled
        if (spawnTestBoss && bossSystem != null)
        {
            SpawnTestBoss();
        }
    }

    private void DiscoverSampleLore()
    {
        // Discover a few lore books for testing
        loreSystem.DiscoverBook("shire_001");
        loreSystem.DiscoverBook("rohan_001");
        loreSystem.DiscoverBook("character_001");
        Debug.Log($"📖 Discovered {loreSystem.GetDiscoveredCount()} lore books");
    }

    private void ActivateSampleQuests()
    {
        // Activate a few demo quests
        questSystem.ActivateQuest("shadow_forest");
        questSystem.ActivateQuest("night_watch");
        Debug.Log($"📜 Activated sample quests");
    }

    private void SpawnTestBoss()
    {
        // Spawn a test boss at origin
        var testBoss = bossSystem.SpawnBoss("cave_troll", Vector3.zero);
        if (testBoss != null && contentHUD != null)
        {
            contentHUD.ShowBossInfo(testBoss);
            Debug.Log("⚔️ Test boss spawned!");
        }
    }

    private void LogSystemStatus()
    {
        Debug.Log("=== Content Systems Status ===");
        Debug.Log($"Dialogue System: {(dialogueSystem != null ? "✓" : "✗")}");
        Debug.Log($"Boss System: {(bossSystem != null ? "✓" : "✗")}");
        Debug.Log($"Lore System: {(loreSystem != null ? "✓" : "✗")}");
        Debug.Log($"Quest System: {(questSystem != null ? "✓" : "✗")}");
        Debug.Log($"Content HUD: {(contentHUD != null ? "✓" : "✗")}");
        
        if (loreSystem != null)
        {
            Debug.Log($"Total Lore Books: {loreSystem.GetTotalBooks()}");
        }
        
        if (questSystem != null)
        {
            Debug.Log($"Active Quests: {questSystem.GetActiveQuests().Count}");
        }
        
        Debug.Log("=============================");
    }

    #region Public API for Game Integration
    
    /// <summary>
    /// Trigger an NPC dialogue interaction
    /// </summary>
    public void StartNPCDialogue(string npcId, string npcName)
    {
        if (dialogueSystem != null)
        {
            dialogueSystem.StartDialogue(npcId);
        }
    }

    /// <summary>
    /// Discover a lore book at a location
    /// </summary>
    public void DiscoverLoreBook(string bookId)
    {
        if (loreSystem != null && loreSystem.DiscoverBook(bookId))
        {
            var book = loreSystem.GetBook(bookId);
            if (book != null && contentHUD != null)
            {
                contentHUD.ShowLoreBook(book);
            }
        }
    }

    /// <summary>
    /// Spawn a boss encounter
    /// </summary>
    public BossEncounter SpawnBoss(string bossId, Vector3 position)
    {
        if (bossSystem != null)
        {
            var boss = bossSystem.SpawnBoss(bossId, position);
            if (boss != null && contentHUD != null)
            {
                contentHUD.ShowBossInfo(boss);
            }
            return boss;
        }
        return null;
    }

    /// <summary>
    /// Activate a quest by ID
    /// </summary>
    public void ActivateQuest(string questId)
    {
        if (questSystem != null)
        {
            questSystem.ActivateQuest(questId);
        }
    }

    /// <summary>
    /// Progress a quest to the next stage
    /// </summary>
    public void ProgressQuest(string questId)
    {
        if (questSystem != null)
        {
            questSystem.AdvanceQuestStage(questId);
        }
    }

    /// <summary>
    /// Select a branching quest path
    /// </summary>
    public void SelectQuestBranch(string questId, string branchId)
    {
        if (questSystem != null)
        {
            questSystem.SelectQuestBranch(questId, branchId);
        }
    }

    /// <summary>
    /// Get relationship status with an NPC
    /// </summary>
    public string GetNPCRelationship(string npcId)
    {
        if (dialogueSystem != null)
        {
            return dialogueSystem.GetRelationshipStatus(npcId);
        }
        return "Unknown";
    }
    
    #endregion

    #region Debug Commands
    
    private void Update()
    {
        // Debug hotkeys (only in editor)
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TestDialogueSystem();
        }
        
        if (Input.GetKeyDown(KeyCode.F2))
        {
            TestLoreSystem();
        }
        
        if (Input.GetKeyDown(KeyCode.F3))
        {
            TestBossSystem();
        }
        
        if (Input.GetKeyDown(KeyCode.F4))
        {
            TestQuestSystem();
        }
        #endif
    }

    private void TestDialogueSystem()
    {
        Debug.Log("🧪 Testing Dialogue System...");
        if (dialogueSystem != null)
        {
            dialogueSystem.StartDialogue("gandalf");
        }
    }

    private void TestLoreSystem()
    {
        Debug.Log("🧪 Testing Lore System...");
        DiscoverLoreBook("shire_001");
    }

    private void TestBossSystem()
    {
        Debug.Log("🧪 Testing Boss System...");
        SpawnBoss("cave_troll", new Vector3(0, 1, 5));
    }

    private void TestQuestSystem()
    {
        Debug.Log("🧪 Testing Quest System...");
        ActivateQuest("shadow_forest");
    }
    
    #endregion
}
