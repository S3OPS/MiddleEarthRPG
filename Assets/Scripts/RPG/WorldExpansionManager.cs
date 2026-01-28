using UnityEngine;
using MiddleEarth.Core;

namespace MiddleEarth.RPG
{
    /// <summary>
    /// Central manager for v2.3 World Expansion features
    /// Coordinates Day/Night Cycle, Weather, Fast Travel, and Dungeon systems
    /// </summary>
    public class WorldExpansionManager : MonoBehaviour
    {
        [Header("System References")]
        [SerializeField] private DayNightCycle dayNightCycle;
        [SerializeField] private WeatherSystem weatherSystem;
        [SerializeField] private FastTravelSystem fastTravelSystem;
        [SerializeField] private DungeonSystem dungeonSystem;

        [Header("Integration Settings")]
        [SerializeField] private bool weatherAffectedByTime = true;
        [SerializeField] private bool nightTimeIncreasesEnemyDifficulty = true;
        [SerializeField] private float nightDifficultyMultiplier = 1.2f;

        // Singleton pattern
        private static WorldExpansionManager instance;
        public static WorldExpansionManager Instance => instance;

        // System accessors
        public DayNightCycle DayNight => dayNightCycle;
        public WeatherSystem Weather => weatherSystem;
        public FastTravelSystem FastTravel => fastTravelSystem;
        public DungeonSystem Dungeons => dungeonSystem;

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
            InitializeSystems();
            SetupSystemIntegration();
            GameLogger.Log("World Expansion Manager initialized - v2.3", LogCategory.General);
        }

        private void InitializeSystems()
        {
            // Find or create systems if not assigned
            if (dayNightCycle == null)
            {
                dayNightCycle = FindObjectOfType<DayNightCycle>();
                if (dayNightCycle == null)
                {
                    GameObject dnc = new GameObject("DayNightCycle");
                    dnc.transform.SetParent(transform);
                    dayNightCycle = dnc.AddComponent<DayNightCycle>();
                }
            }

            if (weatherSystem == null)
            {
                weatherSystem = FindObjectOfType<WeatherSystem>();
                if (weatherSystem == null)
                {
                    GameObject ws = new GameObject("WeatherSystem");
                    ws.transform.SetParent(transform);
                    weatherSystem = ws.AddComponent<WeatherSystem>();
                }
            }

            if (fastTravelSystem == null)
            {
                fastTravelSystem = FindObjectOfType<FastTravelSystem>();
                if (fastTravelSystem == null)
                {
                    GameObject fts = new GameObject("FastTravelSystem");
                    fts.transform.SetParent(transform);
                    fastTravelSystem = fts.AddComponent<FastTravelSystem>();
                }
            }

            if (dungeonSystem == null)
            {
                dungeonSystem = FindObjectOfType<DungeonSystem>();
                if (dungeonSystem == null)
                {
                    GameObject ds = new GameObject("DungeonSystem");
                    ds.transform.SetParent(transform);
                    dungeonSystem = ds.AddComponent<DungeonSystem>();
                }
            }
        }

        private void SetupSystemIntegration()
        {
            // Subscribe to day/night cycle events
            if (dayNightCycle != null)
            {
                dayNightCycle.OnPeriodChanged += HandleTimeOfDayChanged;
            }

            // Subscribe to weather events
            if (weatherSystem != null)
            {
                weatherSystem.OnWeatherChanged += HandleWeatherChanged;
            }

            // Subscribe to dungeon events
            if (dungeonSystem != null)
            {
                dungeonSystem.OnDungeonStarted += HandleDungeonStarted;
                dungeonSystem.OnDungeonCompleted += HandleDungeonCompleted;
            }
        }

        private void HandleTimeOfDayChanged(DayNightCycle.TimeOfDay newPeriod)
        {
            GameLogger.Log($"Time of day changed to: {newPeriod}", LogCategory.General);

            // Adjust weather probabilities based on time
            if (weatherAffectedByTime && weatherSystem != null)
            {
                // Night time slightly increases chance of fog
                if (newPeriod == DayNightCycle.TimeOfDay.Night)
                {
                    // Could adjust weather probabilities here
                }
            }

            // TODO: Integrate with EventBus using IGameEvent wrapper when needed
            // EventBus.Publish(new TimeOfDayChangedEvent(newPeriod));
        }

        private void HandleWeatherChanged(WeatherSystem.WeatherType newWeather)
        {
            GameLogger.Log($"Weather changed to: {newWeather}", LogCategory.General);

            // TODO: Integrate with EventBus using IGameEvent wrapper when needed
            // EventBus.Publish(new WeatherChangedEvent(newWeather));
        }

        private void HandleDungeonStarted(DungeonSystem.Dungeon dungeon)
        {
            GameLogger.Log($"Entered dungeon: {dungeon.dungeonName}", LogCategory.General);

            // Pause day/night cycle in dungeons
            if (dayNightCycle != null)
            {
                dayNightCycle.TimeScale = 0f;
            }

            // Set weather to clear in dungeons
            if (weatherSystem != null)
            {
                weatherSystem.EnableDynamicWeather(false);
                weatherSystem.SetWeather(WeatherSystem.WeatherType.Clear, true);
            }
        }

        private void HandleDungeonCompleted(DungeonSystem.Dungeon dungeon)
        {
            GameLogger.Log($"Completed dungeon: {dungeon.dungeonName}", LogCategory.General);

            // Resume day/night cycle
            if (dayNightCycle != null)
            {
                dayNightCycle.TimeScale = 1f;
            }

            // Re-enable dynamic weather
            if (weatherSystem != null)
            {
                weatherSystem.EnableDynamicWeather(true);
            }
        }

        // Public utility methods
        public float GetEnvironmentalDifficultyModifier()
        {
            float modifier = 1f;

            // Night time increases difficulty
            if (nightTimeIncreasesEnemyDifficulty && dayNightCycle != null)
            {
                if (dayNightCycle.IsNightTime())
                {
                    modifier *= nightDifficultyMultiplier;
                }
            }

            // Weather affects difficulty
            if (weatherSystem != null)
            {
                if (weatherSystem.IsRaining())
                {
                    modifier *= 1.1f; // 10% harder in rain
                }
                if (weatherSystem.IsSnowing())
                {
                    modifier *= 1.15f; // 15% harder in snow
                }
            }

            // Dungeon difficulty
            if (dungeonSystem != null && dungeonSystem.IsInDungeon)
            {
                modifier *= dungeonSystem.GetCurrentDifficultyModifier();
            }

            return modifier;
        }

        public string GetCurrentEnvironmentStatus()
        {
            string status = "";

            if (dayNightCycle != null)
            {
                status += $"Time: {dayNightCycle.GetTimeString()} ({dayNightCycle.CurrentPeriod})\n";
            }

            if (weatherSystem != null)
            {
                status += $"Weather: {weatherSystem.CurrentWeather}\n";
            }

            if (fastTravelSystem != null)
            {
                status += $"Waypoints: {fastTravelSystem.GetDiscoveredCount()}/{fastTravelSystem.GetTotalWaypointCount()}\n";
            }

            if (dungeonSystem != null && dungeonSystem.IsInDungeon)
            {
                status += $"Dungeon: Floor {dungeonSystem.CurrentFloor + 1}/{dungeonSystem.CurrentDungeon.totalFloors}\n";
            }

            return status;
        }

        public void EnableAllSystems(bool enable)
        {
            if (dayNightCycle != null) dayNightCycle.enabled = enable;
            if (weatherSystem != null) weatherSystem.enabled = enable;
            if (fastTravelSystem != null) fastTravelSystem.enabled = enable;
            if (dungeonSystem != null) dungeonSystem.enabled = enable;
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            if (dayNightCycle != null)
            {
                dayNightCycle.OnPeriodChanged -= HandleTimeOfDayChanged;
            }

            if (weatherSystem != null)
            {
                weatherSystem.OnWeatherChanged -= HandleWeatherChanged;
            }

            if (dungeonSystem != null)
            {
                dungeonSystem.OnDungeonStarted -= HandleDungeonStarted;
                dungeonSystem.OnDungeonCompleted -= HandleDungeonCompleted;
            }
        }
    }
}
