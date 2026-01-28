using UnityEngine;
using MiddleEarth.Core;
using MiddleEarth.Config;

namespace MiddleEarth.RPG
{
    /// <summary>
    /// Dynamic weather system with rain, snow, and fog
    /// Part of v2.3 World Expansion - Priority 2 feature
    /// </summary>
    public class WeatherSystem : MonoBehaviour
    {
        [Header("Weather Settings")]
        [SerializeField] private bool enableDynamicWeather = true;
        [SerializeField] private float weatherChangeInterval = 300f; // Seconds between weather changes
        [SerializeField] private float transitionDuration = 30f; // Seconds to transition between weather states

        [Header("Particle Systems")]
        [SerializeField] private ParticleSystem rainParticles;
        [SerializeField] private ParticleSystem snowParticles;
        [SerializeField] private ParticleSystem fogParticles;

        [Header("Audio")]
        [SerializeField] private AudioSource weatherAudioSource;

        [Header("Gameplay Effects")]
        [SerializeField] private bool affectPlayerMovement = true;
        [SerializeField] private float rainMovementModifier = 0.95f; // 5% slower in rain
        [SerializeField] private float snowMovementModifier = 0.85f; // 15% slower in snow
        [SerializeField] private float fogVisibilityRange = 50f; // Reduced visibility in fog

        private WeatherType currentWeather = WeatherType.Clear;
        private WeatherType targetWeather = WeatherType.Clear;
        private float weatherTimer = 0f;
        private float transitionProgress = 1f; // 0 = old weather, 1 = new weather

        public enum WeatherType
        {
            Clear,
            Rain,
            Snow,
            Fog,
            Storm
        }

        // Events
        public delegate void WeatherChanged(WeatherType newWeather);
        public event WeatherChanged OnWeatherChanged;

        // Public accessors
        public WeatherType CurrentWeather => currentWeather;
        public float GetMovementModifier() => GetCurrentMovementModifier();

        private void Start()
        {
            InitializeWeatherSystem();
            GameLogger.Log("Weather System initialized - v2.3 World Expansion", LogCategory.General);
        }

        private void Update()
        {
            if (enableDynamicWeather)
            {
                UpdateWeatherTimer();
            }

            UpdateWeatherTransition();
        }

        private void InitializeWeatherSystem()
        {
            // Initialize with clear weather
            currentWeather = WeatherType.Clear;
            targetWeather = WeatherType.Clear;
            weatherTimer = weatherChangeInterval;

            // Ensure all particle systems start disabled
            if (rainParticles != null) rainParticles.Stop();
            if (snowParticles != null) snowParticles.Stop();
            if (fogParticles != null) fogParticles.Stop();
        }

        private void UpdateWeatherTimer()
        {
            weatherTimer -= Time.deltaTime;

            if (weatherTimer <= 0f)
            {
                // Time to change weather
                ChangeWeather();
                weatherTimer = weatherChangeInterval;
            }
        }

        private void ChangeWeather()
        {
            // Select new random weather
            WeatherType newWeather = SelectRandomWeather();

            if (newWeather != currentWeather)
            {
                targetWeather = newWeather;
                transitionProgress = 0f;
                GameLogger.Log($"Weather changing from {currentWeather} to {targetWeather}", LogCategory.General);
            }
        }

        private WeatherType SelectRandomWeather()
        {
            // Weight probabilities: Clear (40%), Rain (30%), Fog (15%), Snow (10%), Storm (5%)
            float roll = Random.value;

            if (roll < 0.40f) return WeatherType.Clear;
            else if (roll < 0.70f) return WeatherType.Rain;
            else if (roll < 0.85f) return WeatherType.Fog;
            else if (roll < 0.95f) return WeatherType.Snow;
            else return WeatherType.Storm;
        }

        private void UpdateWeatherTransition()
        {
            if (transitionProgress < 1f)
            {
                transitionProgress += Time.deltaTime / transitionDuration;
                transitionProgress = Mathf.Clamp01(transitionProgress);

                // Update particle systems based on transition
                UpdateWeatherEffects();

                // If transition complete, update current weather
                if (transitionProgress >= 1f)
                {
                    currentWeather = targetWeather;
                    OnWeatherChanged?.Invoke(currentWeather);
                    GameLogger.Log($"Weather changed to: {currentWeather}", LogCategory.General);
                }
            }
        }

        private void UpdateWeatherEffects()
        {
            // Update rain
            if (rainParticles != null)
            {
                bool shouldBeActive = targetWeather == WeatherType.Rain || targetWeather == WeatherType.Storm;
                UpdateParticleSystem(rainParticles, shouldBeActive, targetWeather == currentWeather ? 1f : transitionProgress);
            }

            // Update snow
            if (snowParticles != null)
            {
                bool shouldBeActive = targetWeather == WeatherType.Snow;
                UpdateParticleSystem(snowParticles, shouldBeActive, targetWeather == currentWeather ? 1f : transitionProgress);
            }

            // Update fog
            if (fogParticles != null)
            {
                bool shouldBeActive = targetWeather == WeatherType.Fog;
                UpdateParticleSystem(fogParticles, shouldBeActive, targetWeather == currentWeather ? 1f : transitionProgress);
            }

            // Update audio
            UpdateWeatherAudio();
        }

        private void UpdateParticleSystem(ParticleSystem ps, bool shouldBeActive, float intensity)
        {
            if (shouldBeActive)
            {
                if (!ps.isPlaying)
                {
                    ps.Play();
                }

                // Adjust emission rate based on intensity
                var emission = ps.emission;
                emission.rateOverTimeMultiplier = intensity;
            }
            else
            {
                if (ps.isPlaying)
                {
                    ps.Stop();
                }
            }
        }

        private void UpdateWeatherAudio()
        {
            if (weatherAudioSource == null) return;

            // Adjust audio volume based on weather intensity
            float targetVolume = 0f;

            switch (currentWeather)
            {
                case WeatherType.Rain:
                    targetVolume = 0.3f;
                    break;
                case WeatherType.Storm:
                    targetVolume = 0.6f;
                    break;
                case WeatherType.Snow:
                    targetVolume = 0.15f;
                    break;
                case WeatherType.Fog:
                    targetVolume = 0.1f;
                    break;
            }

            weatherAudioSource.volume = Mathf.Lerp(weatherAudioSource.volume, targetVolume, Time.deltaTime);
        }

        private float GetCurrentMovementModifier()
        {
            if (!affectPlayerMovement) return 1f;

            switch (currentWeather)
            {
                case WeatherType.Rain:
                case WeatherType.Storm:
                    return rainMovementModifier;
                case WeatherType.Snow:
                    return snowMovementModifier;
                default:
                    return 1f;
            }
        }

        // Public methods
        public void SetWeather(WeatherType weather, bool immediate = false)
        {
            targetWeather = weather;
            
            if (immediate)
            {
                currentWeather = weather;
                transitionProgress = 1f;
                UpdateWeatherEffects();
                OnWeatherChanged?.Invoke(currentWeather);
            }
            else
            {
                transitionProgress = 0f;
            }
        }

        public void EnableDynamicWeather(bool enable)
        {
            enableDynamicWeather = enable;
        }

        public float GetVisibilityRange()
        {
            if (currentWeather == WeatherType.Fog)
            {
                return fogVisibilityRange;
            }
            return float.MaxValue;
        }

        public bool IsRaining()
        {
            return currentWeather == WeatherType.Rain || currentWeather == WeatherType.Storm;
        }

        public bool IsSnowing()
        {
            return currentWeather == WeatherType.Snow;
        }
    }
}
