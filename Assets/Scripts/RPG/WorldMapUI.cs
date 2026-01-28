using UnityEngine;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// World Map UI - Interactive map showing discovered locations and waypoints
/// Part of Phase 7 (v2.6) UI/UX Polish
/// </summary>
public class WorldMapUI : MonoBehaviour
{
    public static WorldMapUI Instance { get; private set; }
    
    private bool _showMap = false;
    private FastTravelSystem _fastTravelSystem;
    private StringBuilder _textBuilder = new StringBuilder(500);
    private Vector2 _scrollPosition;
    
    // Map regions
    private Dictionary<string, Vector2> _regionPositions = new Dictionary<string, Vector2>
    {
        { "The Shire", new Vector2(0.3f, 0.7f) },
        { "Rohan", new Vector2(0.5f, 0.5f) },
        { "Mordor", new Vector2(0.8f, 0.3f) }
    };
    
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
        _fastTravelSystem = FastTravelSystem.Instance;
    }

    private void Update()
    {
        // Toggle map with M key
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
    }

    public void ToggleMap()
    {
        _showMap = !_showMap;
    }

    public void ShowMap()
    {
        _showMap = true;
    }

    public void HideMap()
    {
        _showMap = false;
    }

    private void OnGUI()
    {
        if (!_showMap) return;

        DrawWorldMap();
    }

    private void DrawWorldMap()
    {
        float width = 700f;
        float height = 550f;
        float x = (Screen.width - width) / 2f;
        float y = (Screen.height - height) / 2f;

        // Main window
        GUI.Box(new Rect(x, y, width, height), "");
        
        // Title
        GUI.Label(new Rect(x + 10, y + 10, width - 20, 30),
                  "<b><size=20>🗺️ Map of Middle-earth</size></b>",
                  GetCenteredStyle());

        // Map area
        float mapWidth = width - 20;
        float mapHeight = 350f;
        Rect mapRect = new Rect(x + 10, y + 50, mapWidth, mapHeight);
        
        GUI.Box(mapRect, "");
        DrawMapContents(mapRect);

        // Legend
        DrawLegend(x + 10, y + 410, width - 20);

        // Close button
        if (GUI.Button(new Rect(x + 10, y + height - 40, 150, 30), "Close [M]"))
        {
            HideMap();
        }

        // Discovered count
        int discovered = GetDiscoveredWaypointCount();
        int total = GetTotalWaypointCount();
        GUI.Label(new Rect(x + width - 200, y + height - 40, 190, 30),
                  $"Discovered: {discovered}/{total}",
                  GetRightAlignedStyle());
    }

    private void DrawMapContents(Rect mapRect)
    {
        // Draw background "map"
        GUI.Box(new Rect(mapRect.x + 5, mapRect.y + 5, mapRect.width - 10, mapRect.height - 10), "");

        if (_fastTravelSystem == null) return;

        // Draw regions and waypoints
        foreach (var region in _regionPositions)
        {
            string regionName = region.Key;
            Vector2 position = region.Value;
            
            float regionX = mapRect.x + (position.x * mapRect.width);
            float regionY = mapRect.y + (position.y * mapRect.height);

            // Draw region label
            GUI.Label(new Rect(regionX - 50, regionY - 60, 100, 20),
                      $"<b>{regionName}</b>",
                      GetCenteredStyle());

            // Draw waypoints in this region
            DrawRegionWaypoints(regionName, regionX, regionY, mapRect);
        }

        // Draw player position indicator
        DrawPlayerPosition(mapRect);
    }

    private void DrawRegionWaypoints(string regionName, float centerX, float centerY, Rect mapRect)
    {
        if (_fastTravelSystem == null) return;

        var waypoints = _fastTravelSystem.GetWaypointsInRegion(regionName);
        if (waypoints == null || waypoints.Count == 0) return;

        float angle = 0f;
        float radius = 40f;
        float angleStep = 360f / waypoints.Count;

        foreach (var waypoint in waypoints)
        {
            float offsetX = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float offsetY = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            
            float wpX = centerX + offsetX;
            float wpY = centerY + offsetY;

            // Draw waypoint marker
            if (waypoint.isDiscovered)
            {
                // Discovered waypoint - green
                GUI.color = Color.green;
                GUI.Box(new Rect(wpX - 5, wpY - 5, 10, 10), "");
                GUI.color = Color.white;

                // Waypoint name on hover
                Rect hoverRect = new Rect(wpX - 30, wpY - 30, 60, 60);
                if (hoverRect.Contains(Event.current.mousePosition))
                {
                    GUI.Label(new Rect(wpX - 40, wpY + 10, 80, 20),
                              waypoint.waypointName,
                              GetSmallCenteredStyle());

                    // Click to fast travel
                    if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                    {
                        if (_fastTravelSystem.CanFastTravel())
                        {
                            _fastTravelSystem.TravelToWaypoint(waypoint.waypointId);
                            HideMap();
                        }
                    }
                }
            }
            else
            {
                // Undiscovered waypoint - gray
                GUI.color = Color.gray;
                GUI.Box(new Rect(wpX - 4, wpY - 4, 8, 8), "");
                GUI.color = Color.white;
            }

            angle += angleStep;
        }
    }

    private void DrawPlayerPosition(Rect mapRect)
    {
        // Draw player marker in center (simplified - would use actual position)
        float playerX = mapRect.x + mapRect.width / 2f;
        float playerY = mapRect.y + mapRect.height / 2f;

        GUI.color = Color.yellow;
        GUI.Box(new Rect(playerX - 6, playerY - 6, 12, 12), "");
        GUI.color = Color.white;

        GUI.Label(new Rect(playerX - 20, playerY - 25, 40, 20),
                  "YOU",
                  GetSmallCenteredStyle());
    }

    private void DrawLegend(float x, float y, float width)
    {
        GUI.Label(new Rect(x, y, width, 20),
                  "<b>Legend:</b>",
                  GetSmallStyle());

        float currentX = x + 70;

        // Green box
        GUI.color = Color.green;
        GUI.Box(new Rect(currentX, y + 2, 10, 10), "");
        GUI.color = Color.white;
        GUI.Label(new Rect(currentX + 15, y, 100, 20),
                  "Discovered",
                  GetSmallStyle());
        currentX += 120;

        // Gray box
        GUI.color = Color.gray;
        GUI.Box(new Rect(currentX, y + 2, 8, 8), "");
        GUI.color = Color.white;
        GUI.Label(new Rect(currentX + 15, y, 100, 20),
                  "Unknown",
                  GetSmallStyle());
        currentX += 100;

        // Yellow box
        GUI.color = Color.yellow;
        GUI.Box(new Rect(currentX, y + 2, 12, 12), "");
        GUI.color = Color.white;
        GUI.Label(new Rect(currentX + 15, y, 100, 20),
                  "Your Location",
                  GetSmallStyle());
    }

    private int GetDiscoveredWaypointCount()
    {
        if (_fastTravelSystem == null) return 0;
        return _fastTravelSystem.GetDiscoveredWaypoints().Count;
    }

    private int GetTotalWaypointCount()
    {
        if (_fastTravelSystem == null) return 0;
        int count = 0;
        foreach (var region in _regionPositions.Keys)
        {
            var waypoints = _fastTravelSystem.GetWaypointsInRegion(region);
            if (waypoints != null) count += waypoints.Count;
        }
        return count;
    }

    private GUIStyle GetCenteredStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        style.alignment = TextAnchor.MiddleCenter;
        return style;
    }

    private GUIStyle GetSmallCenteredStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        style.alignment = TextAnchor.MiddleCenter;
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

    private GUIStyle GetSmallStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        style.fontSize = 10;
        return style;
    }
}
