using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Post-Launch Support System - Manages bug tracking, hotfixes, and updates post-release
/// Coordinates ongoing support and maintenance for v3.0+
/// </summary>
public class PostLaunchSupportSystem : MonoBehaviour
{
    private static PostLaunchSupportSystem instance;
    public static PostLaunchSupportSystem Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("PostLaunchSupportSystem");
                instance = go.AddComponent<PostLaunchSupportSystem>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    [System.Serializable]
    public class BugReport
    {
        public string bugId;
        public string title;
        public string description;
        public string severity; // Critical, High, Medium, Low
        public string status; // Open, In Progress, Fixed, Closed
        public DateTime reportedDate;
        public DateTime? fixedDate;
        public string reporter;
        public string assignee;
    }

    [System.Serializable]
    public class HotfixRelease
    {
        public string version;
        public DateTime releaseDate;
        public List<string> bugsFixed;
        public string notes;
        public bool deployed;
    }

    [System.Serializable]
    public class SupportMetrics
    {
        public int totalBugsReported;
        public int bugsFixed;
        public int bugsOpen;
        public float averageFixTime; // in days
        public int hotfixesReleased;
        public DateTime lastHotfix;
    }

    private List<BugReport> bugReports = new List<BugReport>();
    private List<HotfixRelease> hotfixes = new List<HotfixRelease>();
    private SupportMetrics metrics = new SupportMetrics();
    private bool showSupportUI = false;
    private Vector2 scrollPosition = Vector2.zero;
    private int selectedTab = 0; // 0 = Bug Reports, 1 = Hotfixes, 2 = Metrics
    
    // New bug form fields
    private string newBugTitle = "";
    private string newBugDescription = "";
    private int newBugSeverity = 2; // 0=Critical, 1=High, 2=Medium, 3=Low
    private string[] severityOptions = { "Critical", "High", "Medium", "Low" };

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSampleData();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Toggle support UI with F10 key
        if (Input.GetKeyDown(KeyCode.F10))
        {
            showSupportUI = !showSupportUI;
        }
    }

    void InitializeSampleData()
    {
        // Initialize with some sample post-launch tracking
        metrics = new SupportMetrics
        {
            totalBugsReported = 0,
            bugsFixed = 0,
            bugsOpen = 0,
            averageFixTime = 0f,
            hotfixesReleased = 0,
            lastHotfix = DateTime.Now
        };

        // Sample hotfix (placeholder for future releases)
        hotfixes.Add(new HotfixRelease
        {
            version = "v3.0.1",
            releaseDate = DateTime.Now.AddDays(7),
            bugsFixed = new List<string>(),
            notes = "Planned hotfix - awaiting bug reports from public beta",
            deployed = false
        });
    }

    public void ReportBug(string title, string description, string severity)
    {
        BugReport bug = new BugReport
        {
            bugId = "BUG_" + (bugReports.Count + 1).ToString("D4"),
            title = title,
            description = description,
            severity = severity,
            status = "Open",
            reportedDate = DateTime.Now,
            fixedDate = null,
            reporter = "Player",
            assignee = "Unassigned"
        };

        bugReports.Add(bug);
        metrics.totalBugsReported++;
        metrics.bugsOpen++;
        UpdateMetrics();

        if (NotificationSystem.Instance != null)
        {
            NotificationSystem.Instance.ShowNotification(
                $"Bug Reported: {bug.bugId}",
                NotificationSystem.NotificationType.Info
            );
        }

        Debug.Log($"Bug reported: {bug.bugId} - {title} ({severity})");
    }

    public void MarkBugFixed(string bugId)
    {
        BugReport bug = bugReports.Find(b => b.bugId == bugId);
        if (bug != null && bug.status != "Fixed" && bug.status != "Closed")
        {
            bug.status = "Fixed";
            bug.fixedDate = DateTime.Now;
            metrics.bugsFixed++;
            metrics.bugsOpen--;
            UpdateMetrics();

            if (NotificationSystem.Instance != null)
            {
                NotificationSystem.Instance.ShowNotification(
                    $"Bug Fixed: {bugId}",
                    NotificationSystem.NotificationType.Achievement
                );
            }
        }
    }

    public void CreateHotfix(string version, List<string> bugsFixed, string notes)
    {
        HotfixRelease hotfix = new HotfixRelease
        {
            version = version,
            releaseDate = DateTime.Now,
            bugsFixed = bugsFixed,
            notes = notes,
            deployed = false
        };

        hotfixes.Add(hotfix);
        
        if (NotificationSystem.Instance != null)
        {
            NotificationSystem.Instance.ShowNotification(
                $"Hotfix {version} Created",
                NotificationSystem.NotificationType.Info
            );
        }
    }

    public void DeployHotfix(string version)
    {
        HotfixRelease hotfix = hotfixes.Find(h => h.version == version);
        if (hotfix != null && !hotfix.deployed)
        {
            hotfix.deployed = true;
            metrics.hotfixesReleased++;
            metrics.lastHotfix = DateTime.Now;
            UpdateMetrics();

            if (NotificationSystem.Instance != null)
            {
                NotificationSystem.Instance.ShowNotification(
                    $"Hotfix {version} Deployed!",
                    NotificationSystem.NotificationType.Achievement
                );
            }
        }
    }

    void UpdateMetrics()
    {
        // Calculate average fix time
        List<BugReport> fixedBugs = bugReports.FindAll(b => b.fixedDate.HasValue);
        if (fixedBugs.Count > 0)
        {
            float totalDays = 0f;
            foreach (var bug in fixedBugs)
            {
                TimeSpan fixTime = bug.fixedDate.Value - bug.reportedDate;
                totalDays += (float)fixTime.TotalDays;
            }
            metrics.averageFixTime = totalDays / fixedBugs.Count;
        }
    }

    void OnGUI()
    {
        if (!showSupportUI) return;

        // Post-Launch Support Window
        float windowWidth = 750f;
        float windowHeight = 550f;
        GUILayout.BeginArea(new Rect(Screen.width / 2 - windowWidth / 2, Screen.height / 2 - windowHeight / 2, windowWidth, windowHeight));
        
        GUILayout.BeginVertical("box");
        
        // Header
        GUIStyle headerStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
        GUILayout.Label("🛠️ Post-Launch Support System", headerStyle);
        GUILayout.Space(10);

        // Status banner
        GUIStyle statusStyle = new GUIStyle(GUI.skin.box)
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter
        };
        GUILayout.Label($"Open Bugs: {metrics.bugsOpen} | Fixed: {metrics.bugsFixed} | Hotfixes: {metrics.hotfixesReleased}", statusStyle);
        GUILayout.Space(10);

        // Tabs
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Bug Reports", GUILayout.Height(30)))
            selectedTab = 0;
        if (GUILayout.Button("Hotfixes", GUILayout.Height(30)))
            selectedTab = 1;
        if (GUILayout.Button("Metrics", GUILayout.Height(30)))
            selectedTab = 2;
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        // Content area
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(370));
        
        switch (selectedTab)
        {
            case 0:
                DrawBugReportsTab();
                break;
            case 1:
                DrawHotfixesTab();
                break;
            case 2:
                DrawMetricsTab();
                break;
        }
        
        GUILayout.EndScrollView();
        
        // Footer
        GUILayout.Space(10);
        GUILayout.Label("Press F10 to toggle this window | Press ESC to close", GUI.skin.box);
        
        if (GUILayout.Button("Close", GUILayout.Height(30)))
        {
            showSupportUI = false;
        }
        
        GUILayout.EndVertical();
        GUILayout.EndArea();

        // ESC to close
        if (Input.GetKeyDown(KeyCode.Escape) && showSupportUI)
        {
            showSupportUI = false;
        }
    }

    void DrawBugReportsTab()
    {
        GUILayout.Label($"🐛 Bug Reports: {bugReports.Count} Total", GUI.skin.box);
        GUILayout.Space(10);

        // New bug report form
        GUILayout.BeginVertical("box");
        GUILayout.Label("Report New Bug:", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold });
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Title:", GUILayout.Width(60));
        newBugTitle = GUILayout.TextField(newBugTitle, GUILayout.Width(300));
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Severity:", GUILayout.Width(60));
        newBugSeverity = GUILayout.SelectionGrid(newBugSeverity, severityOptions, 4, GUILayout.Width(400));
        GUILayout.EndHorizontal();
        
        GUILayout.Label("Description:");
        newBugDescription = GUILayout.TextArea(newBugDescription, GUILayout.Height(60), GUILayout.Width(650));
        
        if (GUILayout.Button("Submit Bug Report", GUILayout.Height(25)))
        {
            if (!string.IsNullOrEmpty(newBugTitle) && !string.IsNullOrEmpty(newBugDescription))
            {
                ReportBug(newBugTitle, newBugDescription, severityOptions[newBugSeverity]);
                newBugTitle = "";
                newBugDescription = "";
                newBugSeverity = 2;
            }
        }
        GUILayout.EndVertical();
        GUILayout.Space(10);

        // Bug reports list
        if (bugReports.Count == 0)
        {
            GUILayout.Label("No bugs reported yet. System is stable! 🎉", GUI.skin.box);
        }
        else
        {
            foreach (var bug in bugReports)
            {
                GUILayout.BeginVertical("box");
                
                string statusIcon = bug.status == "Fixed" ? "✅" : bug.status == "In Progress" ? "🔄" : "🐛";
                Color originalColor = GUI.backgroundColor;
                
                // Color-code by severity
                if (bug.severity == "Critical")
                    GUI.backgroundColor = new Color(1f, 0.3f, 0.3f);
                else if (bug.severity == "High")
                    GUI.backgroundColor = new Color(1f, 0.7f, 0.3f);
                else if (bug.severity == "Medium")
                    GUI.backgroundColor = new Color(1f, 1f, 0.5f);
                
                GUILayout.Label($"{statusIcon} {bug.bugId}: {bug.title} [{bug.severity}]");
                GUI.backgroundColor = originalColor;
                
                GUILayout.Label($"   Status: {bug.status} | Reported: {bug.reportedDate:MMM dd, yyyy}");
                GUILayout.Label($"   {bug.description}");
                
                if (bug.status == "Fixed" && bug.fixedDate.HasValue)
                {
                    TimeSpan fixTime = bug.fixedDate.Value - bug.reportedDate;
                    GUILayout.Label($"   Fixed: {bug.fixedDate.Value:MMM dd, yyyy} (in {fixTime.TotalDays:F1} days)");
                }
                else if (bug.status == "Open")
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Mark as In Progress", GUILayout.Width(150)))
                    {
                        bug.status = "In Progress";
                    }
                    if (GUILayout.Button("Mark as Fixed", GUILayout.Width(120)))
                    {
                        MarkBugFixed(bug.bugId);
                    }
                    GUILayout.EndHorizontal();
                }
                
                GUILayout.EndVertical();
                GUILayout.Space(5);
            }
        }
    }

    void DrawHotfixesTab()
    {
        GUILayout.Label($"🔧 Hotfix Releases: {hotfixes.Count} Total", GUI.skin.box);
        GUILayout.Space(10);

        foreach (var hotfix in hotfixes)
        {
            GUILayout.BeginVertical("box");
            
            string status = hotfix.deployed ? "✅ Deployed" : "🎯 Planned";
            GUILayout.Label($"{status} {hotfix.version}");
            GUILayout.Label($"   Release Date: {hotfix.releaseDate:MMM dd, yyyy}");
            GUILayout.Label($"   Bugs Fixed: {hotfix.bugsFixed.Count}");
            
            if (hotfix.bugsFixed.Count > 0)
            {
                GUILayout.Label("   Fixed Issues:");
                foreach (var bugId in hotfix.bugsFixed)
                {
                    GUILayout.Label($"      • {bugId}");
                }
            }
            
            if (!string.IsNullOrEmpty(hotfix.notes))
            {
                GUILayout.Label($"   Notes: {hotfix.notes}");
            }
            
            if (!hotfix.deployed)
            {
                if (GUILayout.Button("Deploy Hotfix", GUILayout.Width(120)))
                {
                    DeployHotfix(hotfix.version);
                }
            }
            
            GUILayout.EndVertical();
            GUILayout.Space(5);
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Create New Hotfix", GUILayout.Height(30)))
        {
            string nextVersion = $"v3.0.{hotfixes.Count + 1}";
            List<string> fixedBugIds = bugReports.FindAll(b => b.status == "Fixed" && !IsInHotfix(b.bugId))
                .ConvertAll(b => b.bugId);
            CreateHotfix(nextVersion, fixedBugIds, "Automated hotfix creation");
        }
    }

    bool IsInHotfix(string bugId)
    {
        foreach (var hotfix in hotfixes)
        {
            if (hotfix.bugsFixed.Contains(bugId))
                return true;
        }
        return false;
    }

    void DrawMetricsTab()
    {
        GUILayout.Label("📊 Support Metrics & Analytics", GUI.skin.box);
        GUILayout.Space(10);

        // Overall metrics
        GUILayout.BeginVertical("box");
        GUILayout.Label("Bug Tracking Metrics:", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold });
        GUILayout.Label($"   Total Bugs Reported: {metrics.totalBugsReported}");
        GUILayout.Label($"   Bugs Fixed: {metrics.bugsFixed}");
        GUILayout.Label($"   Bugs Open: {metrics.bugsOpen}");
        
        if (metrics.totalBugsReported > 0)
        {
            float fixRate = (float)metrics.bugsFixed / metrics.totalBugsReported * 100f;
            GUILayout.Label($"   Fix Rate: {fixRate:F1}%");
        }
        
        if (metrics.averageFixTime > 0)
        {
            GUILayout.Label($"   Average Fix Time: {metrics.averageFixTime:F1} days");
        }
        GUILayout.EndVertical();
        GUILayout.Space(10);

        // Hotfix metrics
        GUILayout.BeginVertical("box");
        GUILayout.Label("Hotfix Metrics:", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold });
        GUILayout.Label($"   Total Hotfixes: {hotfixes.Count}");
        GUILayout.Label($"   Deployed: {metrics.hotfixesReleased}");
        GUILayout.Label($"   Pending: {hotfixes.Count - metrics.hotfixesReleased}");
        
        if (metrics.hotfixesReleased > 0)
        {
            GUILayout.Label($"   Last Hotfix: {metrics.lastHotfix:MMM dd, yyyy}");
        }
        GUILayout.EndVertical();
        GUILayout.Space(10);

        // Severity breakdown
        GUILayout.BeginVertical("box");
        GUILayout.Label("Bug Severity Breakdown:", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold });
        
        int critical = bugReports.FindAll(b => b.severity == "Critical" && b.status == "Open").Count;
        int high = bugReports.FindAll(b => b.severity == "High" && b.status == "Open").Count;
        int medium = bugReports.FindAll(b => b.severity == "Medium" && b.status == "Open").Count;
        int low = bugReports.FindAll(b => b.severity == "Low" && b.status == "Open").Count;
        
        GUILayout.Label($"   Critical: {critical}");
        GUILayout.Label($"   High: {high}");
        GUILayout.Label($"   Medium: {medium}");
        GUILayout.Label($"   Low: {low}");
        GUILayout.EndVertical();
        GUILayout.Space(10);

        // Health status
        GUILayout.BeginVertical("box");
        GUILayout.Label("System Health:", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold });
        
        string healthStatus;
        if (critical > 0)
            healthStatus = "🔴 Critical - Immediate attention required";
        else if (high > 0)
            healthStatus = "🟡 Attention needed - High priority bugs open";
        else if (metrics.bugsOpen > 5)
            healthStatus = "🟡 Monitoring - Multiple bugs open";
        else
            healthStatus = "🟢 Healthy - System stable";
        
        GUILayout.Label($"   Status: {healthStatus}");
        GUILayout.EndVertical();
    }
}
