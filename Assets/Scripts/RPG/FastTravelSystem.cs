using UnityEngine;
using System.Collections.Generic;
using MiddleEarth.Core;
using static MiddleEarth.Core.GameLogger;

namespace MiddleEarth.RPG
{
    /// <summary>
    /// Fast travel waypoint system for quick movement between discovered locations
    /// Part of v2.3 World Expansion - Priority 2 feature
    /// </summary>
    public class FastTravelSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool requireDiscovery = true; // Must visit waypoint before using
        [SerializeField] private float travelDelay = 2f; // Delay before teleporting (for UI/animation)
        [SerializeField] private bool requireCombatEnd = true; // Can't travel during combat

        private Dictionary<string, Waypoint> waypoints = new Dictionary<string, Waypoint>();
        private HashSet<string> discoveredWaypoints = new HashSet<string>();
        private bool isTraveling = false;

        [System.Serializable]
        public class Waypoint
        {
            public string waypointId;
            public string displayName;
            public string description;
            public Vector3 position;
            public bool discovered;
            public string region; // e.g., "The Shire", "Rohan", "Mordor"

            public Waypoint(string id, string name, Vector3 pos, string desc = "", string regionName = "")
            {
                waypointId = id;
                displayName = name;
                position = pos;
                description = desc;
                discovered = false;
                region = regionName;
            }
        }

        // Events
        public delegate void WaypointDiscovered(Waypoint waypoint);
        public delegate void TravelStarted(Waypoint destination);
        public delegate void TravelCompleted(Waypoint destination);

        public event WaypointDiscovered OnWaypointDiscovered;
        public event TravelStarted OnTravelStarted;
        public event TravelCompleted OnTravelCompleted;

        // Singleton pattern for easy access
        private static FastTravelSystem instance;
        public static FastTravelSystem Instance => instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            InitializeFastTravelSystem();
            GameLogger.Log("Fast Travel System initialized - v2.3 World Expansion", LogCategory.General);
        }

        private void InitializeFastTravelSystem()
        {
            // Register default waypoints for Middle-earth locations
            RegisterDefaultWaypoints();
        }

        private void RegisterDefaultWaypoints()
        {
            // The Shire
            RegisterWaypoint("shire_hobbiton", "Hobbiton", new Vector3(0, 0, 0), 
                "The peaceful village of the Hobbits", "The Shire");
            RegisterWaypoint("shire_forest", "Old Forest", new Vector3(20, 0, 15), 
                "The ancient and mysterious forest", "The Shire");

            // Rohan
            RegisterWaypoint("rohan_edoras", "Edoras", new Vector3(50, 0, 30), 
                "The capital city of Rohan", "Rohan");
            RegisterWaypoint("rohan_plains", "Plains of Rohan", new Vector3(60, 0, 20), 
                "The vast grasslands of the horse-lords", "Rohan");

            // Mordor
            RegisterWaypoint("mordor_gate", "Black Gate", new Vector3(80, 0, 50), 
                "The entrance to the dark land", "Mordor");
            RegisterWaypoint("mordor_lands", "Lands of Mordor", new Vector3(90, 0, 60), 
                "The desolate realm of darkness", "Mordor");

            GameLogger.Log($"Registered {waypoints.Count} waypoints", LogCategory.General);
        }

        public void RegisterWaypoint(string id, string name, Vector3 position, string description = "", string region = "")
        {
            if (!waypoints.ContainsKey(id))
            {
                Waypoint waypoint = new Waypoint(id, name, position, description, region);
                waypoints[id] = waypoint;
            }
        }

        public void DiscoverWaypoint(string waypointId)
        {
            if (waypoints.TryGetValue(waypointId, out Waypoint waypoint))
            {
                if (!waypoint.discovered)
                {
                    waypoint.discovered = true;
                    discoveredWaypoints.Add(waypointId);
                    OnWaypointDiscovered?.Invoke(waypoint);
                    GameLogger.Log($"Discovered waypoint: {waypoint.displayName}", LogCategory.General);
                }
            }
        }

        public void DiscoverNearbyWaypoint(Vector3 playerPosition, float discoveryRadius = 5f)
        {
            foreach (var kvp in waypoints)
            {
                Waypoint waypoint = kvp.Value;
                if (!waypoint.discovered)
                {
                    float distanceSqr = (waypoint.position - playerPosition).sqrMagnitude;
                    if (distanceSqr <= discoveryRadius * discoveryRadius)
                    {
                        DiscoverWaypoint(kvp.Key);
                    }
                }
            }
        }

        public bool CanTravelTo(string waypointId)
        {
            if (isTraveling) return false;

            if (!waypoints.ContainsKey(waypointId)) return false;

            Waypoint waypoint = waypoints[waypointId];

            // Check if discovery is required
            if (requireDiscovery && !waypoint.discovered) return false;

            // Check if in combat (if required)
            if (requireCombatEnd && IsPlayerInCombat())
            {
                GameLogger.Log("Cannot fast travel during combat", LogCategory.General, LogLevel.Warning);
                return false;
            }

            return true;
        }

        public void TravelTo(string waypointId, GameObject player)
        {
            if (!CanTravelTo(waypointId))
            {
                GameLogger.Log($"Cannot travel to waypoint: {waypointId}", LogCategory.General, LogLevel.Warning);
                return;
            }

            Waypoint destination = waypoints[waypointId];
            StartTravel(destination, player);
        }

        private void StartTravel(Waypoint destination, GameObject player)
        {
            if (player == null) return;

            isTraveling = true;
            OnTravelStarted?.Invoke(destination);
            GameLogger.Log($"Traveling to: {destination.displayName}", LogCategory.General);

            // Start coroutine for delayed travel
            StartCoroutine(TravelCoroutine(destination, player));
        }

        private System.Collections.IEnumerator TravelCoroutine(Waypoint destination, GameObject player)
        {
            // Wait for travel delay (time for UI/animation)
            yield return new WaitForSeconds(travelDelay);

            // Teleport player
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
                player.transform.position = destination.position;
                controller.enabled = true;
            }
            else
            {
                player.transform.position = destination.position;
            }

            isTraveling = false;
            OnTravelCompleted?.Invoke(destination);
            GameLogger.Log($"Arrived at: {destination.displayName}", LogCategory.General);
        }

        private bool IsPlayerInCombat()
        {
            // Check if player is in combat
            // This would need to integrate with the combat system
            CombatSystem combat = FindObjectOfType<CombatSystem>();
            if (combat != null)
            {
                // Could check combat state here
                return false; // Placeholder
            }
            return false;
        }

        // Public query methods
        public List<Waypoint> GetDiscoveredWaypoints()
        {
            List<Waypoint> discovered = new List<Waypoint>();
            foreach (string id in discoveredWaypoints)
            {
                if (waypoints.TryGetValue(id, out Waypoint waypoint))
                {
                    discovered.Add(waypoint);
                }
            }
            return discovered;
        }

        public List<Waypoint> GetAllWaypoints()
        {
            return new List<Waypoint>(waypoints.Values);
        }

        public Waypoint GetWaypoint(string waypointId)
        {
            waypoints.TryGetValue(waypointId, out Waypoint waypoint);
            return waypoint;
        }

        public bool IsWaypointDiscovered(string waypointId)
        {
            return discoveredWaypoints.Contains(waypointId);
        }

        public int GetDiscoveredCount()
        {
            return discoveredWaypoints.Count;
        }

        public int GetTotalWaypointCount()
        {
            return waypoints.Count;
        }

        public List<Waypoint> GetWaypointsByRegion(string region)
        {
            List<Waypoint> regionWaypoints = new List<Waypoint>();
            foreach (var waypoint in waypoints.Values)
            {
                if (waypoint.region == region)
                {
                    regionWaypoints.Add(waypoint);
                }
            }
            return regionWaypoints;
        }
    }
}
