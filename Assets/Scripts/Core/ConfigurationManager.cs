using UnityEngine;
using MiddleEarth.Config;

namespace MiddleEarth.Core
{
    /// <summary>
    /// Configuration manager for validating and managing game settings.
    /// Ensures all configuration values are within safe bounds.
    /// </summary>
    public class ConfigurationManager : MonoBehaviour
    {
        private static ConfigurationManager _instance;
        public static ConfigurationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("ConfigurationManager");
                    _instance = go.AddComponent<ConfigurationManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }
        
        [Header("Validation Settings")]
        [SerializeField] private bool _validateOnStart = true;
        [SerializeField] private bool _logValidationResults = true;
        
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            
            if (_validateOnStart)
            {
                ValidateAllConstants();
            }
        }
        
        /// <summary>
        /// Validate all game constants are within acceptable ranges.
        /// </summary>
        public bool ValidateAllConstants()
        {
            bool allValid = true;
            
            // Combat validation
            allValid &= ValidateFloat(GameConstants.CRITICAL_HIT_BASE_CHANCE, 0f, 1f, "CRITICAL_HIT_BASE_CHANCE");
            allValid &= ValidateFloat(GameConstants.CRITICAL_HIT_DAMAGE_MULTIPLIER, 1f, 10f, "CRITICAL_HIT_DAMAGE_MULTIPLIER");
            allValid &= ValidateFloat(GameConstants.STRENGTH_DAMAGE_MULTIPLIER, 0f, 100f, "STRENGTH_DAMAGE_MULTIPLIER");
            allValid &= ValidateFloat(GameConstants.COMBO_DAMAGE_BONUS, 0f, 2f, "COMBO_DAMAGE_BONUS");
            allValid &= ValidateFloat(GameConstants.SPECIAL_ABILITY_STAMINA_COST, 0f, 1000f, "SPECIAL_ABILITY_STAMINA_COST");
            allValid &= ValidateFloat(GameConstants.ATTACK_COOLDOWN, 0.1f, 10f, "ATTACK_COOLDOWN");
            allValid &= ValidateFloat(GameConstants.SPECIAL_COOLDOWN, 0.1f, 60f, "SPECIAL_COOLDOWN");
            allValid &= ValidateFloat(GameConstants.ATTACK_RANGE, 0.1f, 100f, "ATTACK_RANGE");
            allValid &= ValidateFloat(GameConstants.SPECIAL_AOE_RADIUS, 0.1f, 100f, "SPECIAL_AOE_RADIUS");
            
            // Enemy AI validation
            allValid &= ValidateFloat(GameConstants.ENEMY_FLEE_HEALTH_PERCENT, 0f, 1f, "ENEMY_FLEE_HEALTH_PERCENT");
            allValid &= ValidateFloat(GameConstants.ENEMY_MOVE_SPEED, 0.1f, 20f, "ENEMY_MOVE_SPEED");
            allValid &= ValidateFloat(GameConstants.ENEMY_WANDER_DISTANCE, 0.1f, 100f, "ENEMY_WANDER_DISTANCE");
            allValid &= ValidateFloat(GameConstants.ENEMY_DETECTION_RANGE, 0.1f, 100f, "ENEMY_DETECTION_RANGE");
            allValid &= ValidateFloat(GameConstants.ENEMY_ATTACK_RANGE, 0.1f, 100f, "ENEMY_ATTACK_RANGE");
            allValid &= ValidateFloat(GameConstants.ENEMY_ATTACK_COOLDOWN, 0.1f, 10f, "ENEMY_ATTACK_COOLDOWN");
            allValid &= ValidateFloat(GameConstants.ENEMY_WANDER_INTERVAL, 0.1f, 60f, "ENEMY_WANDER_INTERVAL");
            
            // Particle validation
            allValid &= ValidateInt(GameConstants.NORMAL_HIT_PARTICLE_COUNT, 1, 100, "NORMAL_HIT_PARTICLE_COUNT");
            allValid &= ValidateInt(GameConstants.HIT_PARTICLE_COUNT, 1, 100, "HIT_PARTICLE_COUNT");
            allValid &= ValidateInt(GameConstants.SPECIAL_PARTICLE_COUNT, 1, 200, "SPECIAL_PARTICLE_COUNT");
            allValid &= ValidateInt(GameConstants.LEVELUP_PARTICLE_COUNT, 1, 100, "LEVELUP_PARTICLE_COUNT");
            allValid &= ValidateInt(GameConstants.TREASURE_PARTICLE_COUNT, 1, 100, "TREASURE_PARTICLE_COUNT");
            allValid &= ValidateInt(GameConstants.QUEST_PARTICLE_COUNT, 1, 100, "QUEST_PARTICLE_COUNT");
            
            // Progression validation
            allValid &= ValidateInt(GameConstants.BASE_XP_REQUIREMENT, 1, 10000, "BASE_XP_REQUIREMENT");
            allValid &= ValidateFloat(GameConstants.XP_SCALING_FACTOR, 1f, 5f, "XP_SCALING_FACTOR");
            allValid &= ValidateInt(GameConstants.LEVELUP_HEALTH_BONUS, 1, 1000, "LEVELUP_HEALTH_BONUS");
            allValid &= ValidateInt(GameConstants.LEVELUP_STAMINA_BONUS, 1, 1000, "LEVELUP_STAMINA_BONUS");
            allValid &= ValidateInt(GameConstants.LEVELUP_STAT_INCREASE, 1, 100, "LEVELUP_STAT_INCREASE");
            
            // Pool validation
            allValid &= ValidateInt(GameConstants.PARTICLE_POOL_INITIAL_SIZE, 10, 1000, "PARTICLE_POOL_INITIAL_SIZE");
            allValid &= ValidateInt(GameConstants.PARTICLE_POOL_MAX_SIZE, 10, 10000, "PARTICLE_POOL_MAX_SIZE");
            allValid &= ValidateInt(GameConstants.AUDIO_SOURCE_POOL_SIZE, 1, 100, "AUDIO_SOURCE_POOL_SIZE");
            
            // Economy validation
            allValid &= ValidateInt(GameConstants.STARTING_GOLD, 0, 1000000, "STARTING_GOLD");
            allValid &= ValidateInt(GameConstants.ENEMY_GOLD_REWARD, 0, 10000, "ENEMY_GOLD_REWARD");
            allValid &= ValidateInt(GameConstants.ENEMY_XP_REWARD, 0, 10000, "ENEMY_XP_REWARD");
            allValid &= ValidateInt(GameConstants.LOCATION_DISCOVERY_XP, 0, 10000, "LOCATION_DISCOVERY_XP");
            allValid &= ValidateInt(GameConstants.TREASURE_CHEST_GOLD, 0, 100000, "TREASURE_CHEST_GOLD");
            
            // Save system validation
            allValid &= ValidateInt(GameConstants.SAVE_FILE_VERSION, 1, 100, "SAVE_FILE_VERSION");
            allValid &= ValidateInt(GameConstants.MAX_SAVE_FILE_SIZE, 1024, 10485760, "MAX_SAVE_FILE_SIZE");
            
            if (_logValidationResults)
            {
                if (allValid)
                {
                    GameLogger.Info("All game constants validated successfully!", GameLogger.LogCategory.General);
                }
                else
                {
                    GameLogger.Warning("Some game constants failed validation checks", GameLogger.LogCategory.General);
                }
            }
            
            return allValid;
        }
        
        private bool ValidateFloat(float value, float min, float max, string constantName)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
            {
                if (_logValidationResults)
                {
                    GameLogger.Error($"Constant '{constantName}' has invalid value: {value}", GameLogger.LogCategory.General);
                }
                return false;
            }
            
            if (value < min || value > max)
            {
                if (_logValidationResults)
                {
                    GameLogger.Warning($"Constant '{constantName}' ({value}) is outside recommended range [{min}, {max}]", GameLogger.LogCategory.General);
                }
                return false;
            }
            
            return true;
        }
        
        private bool ValidateInt(int value, int min, int max, string constantName)
        {
            if (value < min || value > max)
            {
                if (_logValidationResults)
                {
                    GameLogger.Warning($"Constant '{constantName}' ({value}) is outside recommended range [{min}, {max}]", GameLogger.LogCategory.General);
                }
                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// Get a summary of current configuration.
        /// </summary>
        public string GetConfigurationSummary()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("=== Game Configuration Summary ===");
            sb.AppendLine($"Attack Range: {GameConstants.ATTACK_RANGE}");
            sb.AppendLine($"Critical Hit Chance: {GameConstants.CRITICAL_HIT_BASE_CHANCE * 100}%");
            sb.AppendLine($"Enemy Detection Range: {GameConstants.ENEMY_DETECTION_RANGE}");
            sb.AppendLine($"Particle Pool Size: {GameConstants.PARTICLE_POOL_INITIAL_SIZE}-{GameConstants.PARTICLE_POOL_MAX_SIZE}");
            sb.AppendLine($"XP Scaling: {GameConstants.XP_SCALING_FACTOR}x");
            return sb.ToString();
        }
    }
}
