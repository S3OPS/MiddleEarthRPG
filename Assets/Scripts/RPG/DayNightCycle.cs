using UnityEngine;
using MiddleEarth.Core;

namespace MiddleEarth.RPG
{
    /// <summary>
    /// Day/Night cycle system with time-based events and lighting
    /// Part of v2.3 World Expansion - Priority 2 feature
    /// </summary>
    public class DayNightCycle : MonoBehaviour
    {
        [Header("Time Settings")]
        [SerializeField] private float dayLengthInMinutes = 20f; // Real-time minutes for full day
        [SerializeField] private float currentTimeOfDay = 0.5f; // 0 = midnight, 0.5 = noon, 1 = midnight
        [SerializeField] private float timeScale = 1f; // Speed multiplier

        [Header("Lighting")]
        [SerializeField] private Light directionalLight; // Sun/Moon light
        [SerializeField] private Gradient lightColorGradient; // Color changes throughout day
        [SerializeField] private AnimationCurve lightIntensityCurve; // Intensity changes throughout day

        [Header("Sky")]
        [SerializeField] private Material skyboxMaterial;
        [SerializeField] private Color nightSkyColor = new Color(0.1f, 0.1f, 0.2f);
        [SerializeField] private Color daySkyColor = new Color(0.5f, 0.7f, 1f);

        [Header("Events")]
        [SerializeField] private bool enableTimeBasedEvents = true;

        // Time of day thresholds
        private const float DAWN_TIME = 0.25f;    // 6 AM
        private const float NOON_TIME = 0.5f;     // 12 PM
        private const float DUSK_TIME = 0.75f;    // 6 PM
        private const float MIDNIGHT_TIME = 0f;   // 12 AM

        // Current time period
        private TimeOfDay currentPeriod = TimeOfDay.Day;
        private TimeOfDay previousPeriod = TimeOfDay.Day;

        public enum TimeOfDay
        {
            Night,
            Dawn,
            Day,
            Dusk
        }

        // Events for other systems to subscribe to
        public delegate void TimeChanged(float normalizedTime);
        public delegate void PeriodChanged(TimeOfDay newPeriod);
        
        public event TimeChanged OnTimeChanged;
        public event PeriodChanged OnPeriodChanged;

        // Public accessors
        public float CurrentTimeOfDay => currentTimeOfDay;
        public TimeOfDay CurrentPeriod => currentPeriod;
        public float TimeScale { get => timeScale; set => timeScale = Mathf.Max(0f, value); }

        private void Start()
        {
            InitializeSystem();
            GameLogger.Log("Day/Night Cycle initialized - v2.3 World Expansion", LogCategory.General);
        }

        private void Update()
        {
            UpdateTimeOfDay();
            UpdateLighting();
            UpdateTimePeriod();
        }

        private void InitializeSystem()
        {
            // Find directional light if not assigned
            if (directionalLight == null)
            {
                GameObject lightObj = GameObject.Find("Directional Light");
                if (lightObj != null)
                {
                    directionalLight = lightObj.GetComponent<Light>();
                }
            }

            // Create default gradient if not set
            if (lightColorGradient == null)
            {
                lightColorGradient = new Gradient();
                GradientColorKey[] colorKeys = new GradientColorKey[4];
                colorKeys[0] = new GradientColorKey(new Color(0.2f, 0.3f, 0.5f), 0f);    // Night
                colorKeys[1] = new GradientColorKey(new Color(1f, 0.7f, 0.5f), 0.25f);   // Dawn
                colorKeys[2] = new GradientColorKey(new Color(1f, 1f, 1f), 0.5f);        // Day
                colorKeys[3] = new GradientColorKey(new Color(1f, 0.5f, 0.3f), 0.75f);   // Dusk

                GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
                alphaKeys[0] = new GradientAlphaKey(1f, 0f);
                alphaKeys[1] = new GradientAlphaKey(1f, 1f);

                lightColorGradient.SetKeys(colorKeys, alphaKeys);
            }

            // Create default intensity curve if not set
            if (lightIntensityCurve == null || lightIntensityCurve.length == 0)
            {
                lightIntensityCurve = new AnimationCurve();
                lightIntensityCurve.AddKey(0f, 0.1f);    // Night - dim
                lightIntensityCurve.AddKey(0.25f, 0.7f); // Dawn - getting brighter
                lightIntensityCurve.AddKey(0.5f, 1.0f);  // Day - full brightness
                lightIntensityCurve.AddKey(0.75f, 0.6f); // Dusk - dimming
                lightIntensityCurve.AddKey(1f, 0.1f);    // Night - dim
            }
        }

        private void UpdateTimeOfDay()
        {
            // Progress time
            float dayProgress = (timeScale * Time.deltaTime) / (dayLengthInMinutes * 60f);
            currentTimeOfDay += dayProgress;

            // Wrap around at 1.0 (full day cycle)
            if (currentTimeOfDay >= 1f)
            {
                currentTimeOfDay -= 1f;
            }

            // Notify listeners
            OnTimeChanged?.Invoke(currentTimeOfDay);
        }

        private void UpdateLighting()
        {
            if (directionalLight == null) return;

            // Update light color
            directionalLight.color = lightColorGradient.Evaluate(currentTimeOfDay);

            // Update light intensity
            directionalLight.intensity = lightIntensityCurve.Evaluate(currentTimeOfDay);

            // Rotate light to simulate sun movement
            float sunAngle = currentTimeOfDay * 360f - 90f; // -90 to start at horizon
            directionalLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

            // Update skybox if available
            if (skyboxMaterial != null && skyboxMaterial.HasProperty("_Tint"))
            {
                Color skyColor = Color.Lerp(nightSkyColor, daySkyColor, lightIntensityCurve.Evaluate(currentTimeOfDay));
                skyboxMaterial.SetColor("_Tint", skyColor);
            }
        }

        private void UpdateTimePeriod()
        {
            previousPeriod = currentPeriod;

            // Determine current time period
            if (currentTimeOfDay < DAWN_TIME || currentTimeOfDay >= (DUSK_TIME + (1f - DUSK_TIME) / 2f))
            {
                currentPeriod = TimeOfDay.Night;
            }
            else if (currentTimeOfDay < (DAWN_TIME + (NOON_TIME - DAWN_TIME) / 2f))
            {
                currentPeriod = TimeOfDay.Dawn;
            }
            else if (currentTimeOfDay < (NOON_TIME + (DUSK_TIME - NOON_TIME) / 2f))
            {
                currentPeriod = TimeOfDay.Day;
            }
            else
            {
                currentPeriod = TimeOfDay.Dusk;
            }

            // Notify if period changed
            if (currentPeriod != previousPeriod)
            {
                OnPeriodChanged?.Invoke(currentPeriod);
                GameLogger.Log($"Time period changed to: {currentPeriod}", LogCategory.General);

                if (enableTimeBasedEvents)
                {
                    TriggerPeriodEvents(currentPeriod);
                }
            }
        }

        private void TriggerPeriodEvents(TimeOfDay period)
        {
            // Trigger events based on time of day
            // Other systems can subscribe to OnPeriodChanged for custom behavior
            switch (period)
            {
                case TimeOfDay.Dawn:
                    // Could trigger morning NPC routines, enemy respawns, etc.
                    break;
                case TimeOfDay.Day:
                    // Full daylight activities
                    break;
                case TimeOfDay.Dusk:
                    // Evening activities, shops closing, etc.
                    break;
                case TimeOfDay.Night:
                    // Night activities, different enemy spawns, etc.
                    break;
            }
        }

        // Public methods for external control
        public void SetTimeOfDay(float normalizedTime)
        {
            currentTimeOfDay = Mathf.Clamp01(normalizedTime);
        }

        public void SetTimeOfDay(int hour, int minute = 0)
        {
            float totalHours = hour + (minute / 60f);
            currentTimeOfDay = (totalHours / 24f) % 1f;
        }

        public void GetCurrentTime(out int hour, out int minute)
        {
            float totalHours = currentTimeOfDay * 24f;
            hour = Mathf.FloorToInt(totalHours);
            minute = Mathf.FloorToInt((totalHours - hour) * 60f);
        }

        public string GetTimeString()
        {
            GetCurrentTime(out int hour, out int minute);
            return $"{hour:00}:{minute:00}";
        }

        public bool IsNightTime()
        {
            return currentPeriod == TimeOfDay.Night;
        }

        public bool IsDayTime()
        {
            return currentPeriod == TimeOfDay.Day;
        }
    }
}
