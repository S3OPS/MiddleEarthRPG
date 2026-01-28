using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Public Beta Manager - Coordinates beta testing process for v3.0
/// Manages beta tester registration, test scenarios, and progress tracking
/// </summary>
public class PublicBetaManager : MonoBehaviour
{
    private static PublicBetaManager instance;
    public static PublicBetaManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("PublicBetaManager");
                instance = go.AddComponent<PublicBetaManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    [System.Serializable]
    public class BetaTester
    {
        public string testerId;
        public string name;
        public DateTime registrationDate;
        public int testsCompleted;
        public int bugsReported;
        public int feedbackSubmitted;
        public float playtimeHours;
    }

    [System.Serializable]
    public class TestScenario
    {
        public string scenarioId;
        public string name;
        public string description;
        public List<string> objectives;
        public bool completed;
        public DateTime completedDate;
    }

    private List<BetaTester> betaTesters = new List<BetaTester>();
    private List<TestScenario> testScenarios = new List<TestScenario>();
    private bool showBetaUI = false;
    private Vector2 scrollPosition = Vector2.zero;
    private int selectedTab = 0; // 0 = Testers, 1 = Scenarios, 2 = Analytics
    private string newTesterName = "";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeTestScenarios();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Toggle beta manager UI with F8 key
        if (Input.GetKeyDown(KeyCode.F8))
        {
            showBetaUI = !showBetaUI;
        }
    }

    void InitializeTestScenarios()
    {
        testScenarios = new List<TestScenario>
        {
            new TestScenario
            {
                scenarioId = "BETA_001",
                name = "Core Gameplay Loop",
                description = "Test the basic gameplay: movement, combat, quests",
                objectives = new List<string>
                {
                    "Complete tutorial quest",
                    "Engage in 5 combats",
                    "Level up character",
                    "Complete 1 side quest"
                },
                completed = false
            },
            new TestScenario
            {
                scenarioId = "BETA_002",
                name = "World Exploration",
                description = "Explore different regions and test fast travel",
                objectives = new List<string>
                {
                    "Discover 5 waypoints",
                    "Use fast travel system",
                    "Experience day/night cycle",
                    "Witness 3 weather changes"
                },
                completed = false
            },
            new TestScenario
            {
                scenarioId = "BETA_003",
                name = "Content Systems",
                description = "Test dialogue, bosses, and lore",
                objectives = new List<string>
                {
                    "Complete 3 dialogues with different NPCs",
                    "Defeat 1 boss encounter",
                    "Read 5 lore books",
                    "Complete 2 enhanced quests"
                },
                completed = false
            },
            new TestScenario
            {
                scenarioId = "BETA_004",
                name = "UI/UX Testing",
                description = "Test all UI systems and menus",
                objectives = new List<string>
                {
                    "Open and use Quest Journal (J)",
                    "Open and use Character Sheet (C)",
                    "Open and use World Map (M)",
                    "Configure settings menu (O)",
                    "Save and load game"
                },
                completed = false
            },
            new TestScenario
            {
                scenarioId = "BETA_005",
                name = "Performance & Stability",
                description = "Monitor performance and report issues",
                objectives = new List<string>
                {
                    "Check FPS in combat (F3)",
                    "Play for 30+ minutes continuously",
                    "Test all systems without crashes",
                    "Report any performance issues"
                },
                completed = false
            }
        };
    }

    public void RegisterBetaTester(string name)
    {
        BetaTester tester = new BetaTester
        {
            testerId = "TESTER_" + (betaTesters.Count + 1).ToString("D3"),
            name = name,
            registrationDate = DateTime.Now,
            testsCompleted = 0,
            bugsReported = 0,
            feedbackSubmitted = 0,
            playtimeHours = 0
        };
        betaTesters.Add(tester);
        
        if (NotificationSystem.Instance != null)
        {
            NotificationSystem.Instance.ShowNotification(
                $"Beta Tester Registered: {name}",
                NotificationSystem.NotificationType.Info
            );
        }
    }

    public void CompleteTestScenario(string scenarioId)
    {
        TestScenario scenario = testScenarios.Find(s => s.scenarioId == scenarioId);
        if (scenario != null && !scenario.completed)
        {
            scenario.completed = true;
            scenario.completedDate = DateTime.Now;
            
            if (NotificationSystem.Instance != null)
            {
                NotificationSystem.Instance.ShowNotification(
                    $"Test Scenario Complete: {scenario.name}",
                    NotificationSystem.NotificationType.Achievement
                );
            }
        }
    }

    void OnGUI()
    {
        if (!showBetaUI) return;

        // Beta Manager Window
        float windowWidth = 700f;
        float windowHeight = 500f;
        GUILayout.BeginArea(new Rect(Screen.width / 2 - windowWidth / 2, Screen.height / 2 - windowHeight / 2, windowWidth, windowHeight));
        
        GUILayout.BeginVertical("box");
        
        // Header
        GUIStyle headerStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
        GUILayout.Label("🚀 Public Beta Manager v3.0", headerStyle);
        GUILayout.Space(10);

        // Tabs
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Beta Testers", GUILayout.Height(30)))
            selectedTab = 0;
        if (GUILayout.Button("Test Scenarios", GUILayout.Height(30)))
            selectedTab = 1;
        if (GUILayout.Button("Analytics", GUILayout.Height(30)))
            selectedTab = 2;
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        // Content area
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(350));
        
        switch (selectedTab)
        {
            case 0:
                DrawBetaTestersTab();
                break;
            case 1:
                DrawTestScenariosTab();
                break;
            case 2:
                DrawAnalyticsTab();
                break;
        }
        
        GUILayout.EndScrollView();
        
        // Footer
        GUILayout.Space(10);
        GUILayout.Label("Press F8 to toggle this window | Press ESC to close", GUI.skin.box);
        
        if (GUILayout.Button("Close", GUILayout.Height(30)))
        {
            showBetaUI = false;
        }
        
        GUILayout.EndVertical();
        GUILayout.EndArea();

        // ESC to close
        if (Input.GetKeyDown(KeyCode.Escape) && showBetaUI)
        {
            showBetaUI = false;
        }
    }

    void DrawBetaTestersTab()
    {
        GUILayout.Label($"📊 Registered Beta Testers: {betaTesters.Count}", GUI.skin.box);
        GUILayout.Space(10);

        // Registration form
        GUILayout.BeginVertical("box");
        GUILayout.Label("Register New Beta Tester:");
        GUILayout.BeginHorizontal();
        GUILayout.Label("Name:", GUILayout.Width(50));
        newTesterName = GUILayout.TextField(newTesterName, GUILayout.Width(200));
        if (GUILayout.Button("Register", GUILayout.Width(100)))
        {
            if (!string.IsNullOrEmpty(newTesterName))
            {
                RegisterBetaTester(newTesterName);
                newTesterName = "";
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.Space(10);

        // Testers list
        foreach (var tester in betaTesters)
        {
            GUILayout.BeginVertical("box");
            GUILayout.Label($"🎮 {tester.name} ({tester.testerId})");
            GUILayout.Label($"   Registered: {tester.registrationDate:MMM dd, yyyy}");
            GUILayout.Label($"   Tests: {tester.testsCompleted} | Bugs: {tester.bugsReported} | Feedback: {tester.feedbackSubmitted}");
            GUILayout.Label($"   Playtime: {tester.playtimeHours:F1} hours");
            GUILayout.EndVertical();
            GUILayout.Space(5);
        }
    }

    void DrawTestScenariosTab()
    {
        int completedScenarios = testScenarios.FindAll(s => s.completed).Count;
        GUILayout.Label($"🎯 Test Scenarios: {completedScenarios}/{testScenarios.Count} Complete", GUI.skin.box);
        GUILayout.Space(10);

        foreach (var scenario in testScenarios)
        {
            GUILayout.BeginVertical("box");
            
            string status = scenario.completed ? "✅" : "🎯";
            GUILayout.Label($"{status} {scenario.name} ({scenario.scenarioId})");
            GUILayout.Label($"   {scenario.description}");
            
            GUILayout.Label("   Objectives:");
            foreach (var objective in scenario.objectives)
            {
                GUILayout.Label($"      • {objective}");
            }
            
            if (scenario.completed)
            {
                GUILayout.Label($"   Completed: {scenario.completedDate:MMM dd, yyyy}");
            }
            else
            {
                if (GUILayout.Button("Mark as Complete", GUILayout.Width(150)))
                {
                    CompleteTestScenario(scenario.scenarioId);
                }
            }
            
            GUILayout.EndVertical();
            GUILayout.Space(5);
        }
    }

    void DrawAnalyticsTab()
    {
        GUILayout.Label("📈 Beta Testing Analytics", GUI.skin.box);
        GUILayout.Space(10);

        // Overall statistics
        GUILayout.BeginVertical("box");
        GUILayout.Label("Overall Statistics:");
        GUILayout.Label($"   Total Beta Testers: {betaTesters.Count}");
        
        int totalBugs = 0;
        int totalFeedback = 0;
        float totalPlaytime = 0;
        foreach (var tester in betaTesters)
        {
            totalBugs += tester.bugsReported;
            totalFeedback += tester.feedbackSubmitted;
            totalPlaytime += tester.playtimeHours;
        }
        
        GUILayout.Label($"   Total Bugs Reported: {totalBugs}");
        GUILayout.Label($"   Total Feedback Submitted: {totalFeedback}");
        GUILayout.Label($"   Total Playtime: {totalPlaytime:F1} hours");
        GUILayout.Label($"   Average Playtime per Tester: {(betaTesters.Count > 0 ? totalPlaytime / betaTesters.Count : 0):F1} hours");
        GUILayout.EndVertical();
        GUILayout.Space(10);

        // Scenario completion
        GUILayout.BeginVertical("box");
        GUILayout.Label("Test Scenario Progress:");
        int completed = testScenarios.FindAll(s => s.completed).Count;
        float completionRate = testScenarios.Count > 0 ? (float)completed / testScenarios.Count * 100f : 0f;
        GUILayout.Label($"   Completed: {completed}/{testScenarios.Count} ({completionRate:F1}%)");
        
        // Progress bar
        Rect progressRect = GUILayoutUtility.GetRect(18, 20);
        GUI.Box(progressRect, "");
        Rect fillRect = new Rect(progressRect.x + 2, progressRect.y + 2, (progressRect.width - 4) * completionRate / 100f, progressRect.height - 4);
        GUI.Box(fillRect, "", GetProgressBarStyle(completionRate));
        GUILayout.EndVertical();
        GUILayout.Space(10);

        // Beta status
        GUILayout.BeginVertical("box");
        GUILayout.Label("Beta Phase Status:");
        GUILayout.Label("   Current Phase: Public Beta Testing");
        GUILayout.Label("   Start Date: January 28, 2026");
        GUILayout.Label("   Status: 🟢 Active");
        GUILayout.EndVertical();
    }

    GUIStyle GetProgressBarStyle(float percentage)
    {
        GUIStyle style = new GUIStyle(GUI.skin.box);
        if (percentage >= 75f)
            style.normal.background = MakeTex(2, 2, new Color(0.2f, 0.8f, 0.2f));
        else if (percentage >= 50f)
            style.normal.background = MakeTex(2, 2, new Color(0.8f, 0.8f, 0.2f));
        else
            style.normal.background = MakeTex(2, 2, new Color(0.8f, 0.4f, 0.2f));
        return style;
    }

    Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}
