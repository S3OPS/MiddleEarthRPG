using UnityEngine;

namespace MiddleEarth.Config
{
    /// <summary>
    /// Centralized game balance constants.
    /// Modify these values to tune gameplay without changing code.
    /// All magic numbers from the codebase are defined here for maintainability.
    /// </summary>
    public static class GameConstants
    {
        // ========================================
        // COMBAT SETTINGS
        // ========================================
        
        /// <summary>Base chance for critical hit (15%)</summary>
        public const float CRITICAL_HIT_BASE_CHANCE = 0.15f;
        
        /// <summary>Damage multiplier for critical hits (2x)</summary>
        public const float CRITICAL_HIT_DAMAGE_MULTIPLIER = 2f;
        
        /// <summary>Strength stat contribution to base damage per point</summary>
        public const float STRENGTH_DAMAGE_MULTIPLIER = 2f;
        
        /// <summary>Combo damage increase per hit (20%)</summary>
        public const float COMBO_DAMAGE_BONUS = 0.2f;
        
        /// <summary>Stamina cost for special ability</summary>
        public const float SPECIAL_ABILITY_STAMINA_COST = 30f;
        
        /// <summary>Attack cooldown in seconds</summary>
        public const float ATTACK_COOLDOWN = 0.5f;
        
        /// <summary>Special ability cooldown in seconds</summary>
        public const float SPECIAL_COOLDOWN = 5f;
        
        /// <summary>Attack range in units (raycast distance)</summary>
        public const float ATTACK_RANGE = 3f;
        
        /// <summary>Special ability AOE radius in units</summary>
        public const float SPECIAL_AOE_RADIUS = 4f;
        
        
        // ========================================
        // ENEMY AI SETTINGS
        // ========================================
        
        /// <summary>Health percentage when enemy starts fleeing (20%)</summary>
        public const float ENEMY_FLEE_HEALTH_PERCENT = 0.2f;
        
        /// <summary>Enemy movement speed in units per second</summary>
        public const float ENEMY_MOVE_SPEED = 2f;
        
        /// <summary>Enemy patrol wander distance in units</summary>
        public const float ENEMY_WANDER_DISTANCE = 5f;
        
        /// <summary>Enemy player detection range in units</summary>
        public const float ENEMY_DETECTION_RANGE = 10f;
        
        /// <summary>Enemy attack range in units</summary>
        public const float ENEMY_ATTACK_RANGE = 5f;
        
        /// <summary>Enemy attack cooldown in seconds</summary>
        public const float ENEMY_ATTACK_COOLDOWN = 2f;
        
        /// <summary>Enemy wander interval in seconds</summary>
        public const float ENEMY_WANDER_INTERVAL = 3f;
        
        
        // ========================================
        // PARTICLE VFX SETTINGS
        // ========================================
        
        /// <summary>Particle count for normal (non-critical) hit effects</summary>
        public const int NORMAL_HIT_PARTICLE_COUNT = 8;
        
        /// <summary>Particle count for critical hit effects</summary>
        public const int HIT_PARTICLE_COUNT = 15;
        
        /// <summary>Particle count for special ability effects</summary>
        public const int SPECIAL_PARTICLE_COUNT = 30;
        
        /// <summary>Particle count for level-up effect</summary>
        public const int LEVELUP_PARTICLE_COUNT = 20;
        
        /// <summary>Particle count for treasure effect</summary>
        public const int TREASURE_PARTICLE_COUNT = 12;
        
        /// <summary>Particle count for quest complete effect</summary>
        public const int QUEST_PARTICLE_COUNT = 8;
        
        /// <summary>Particle lifetime for normal hit effects in seconds</summary>
        public const float PARTICLE_LIFETIME = 0.5f;
        
        /// <summary>Particle lifetime for special ability effects in seconds</summary>
        public const float SPECIAL_PARTICLE_LIFETIME = 0.8f;
        
        /// <summary>Particle lifetime for level-up effects in seconds</summary>
        public const float LEVELUP_PARTICLE_LIFETIME = 1.5f;
        
        /// <summary>Particle lifetime for treasure effects in seconds</summary>
        public const float TREASURE_PARTICLE_LIFETIME = 0.6f;
        
        /// <summary>Particle lifetime for quest complete effects in seconds</summary>
        public const float QUEST_PARTICLE_LIFETIME = 1f;
        
        /// <summary>Particle size scale</summary>
        public const float PARTICLE_SIZE = 0.1f;
        
        /// <summary>Particle velocity multiplier</summary>
        public const float PARTICLE_VELOCITY = 2f;
        
        
        // ========================================
        // PROGRESSION SETTINGS
        // ========================================
        
        /// <summary>Base XP requirement for level 2</summary>
        public const int BASE_XP_REQUIREMENT = 100;
        
        /// <summary>XP scaling factor per level (1.5x exponential growth)</summary>
        public const float XP_SCALING_FACTOR = 1.5f;
        
        /// <summary>Health bonus per level up</summary>
        public const int LEVELUP_HEALTH_BONUS = 20;
        
        /// <summary>Stamina bonus per level up</summary>
        public const int LEVELUP_STAMINA_BONUS = 10;
        
        /// <summary>Stat increase per level (applies to all stats)</summary>
        public const int LEVELUP_STAT_INCREASE = 2;
        
        
        // ========================================
        // UI SETTINGS
        // ========================================
        
        /// <summary>Damage number display duration in seconds</summary>
        public const float DAMAGE_NUMBER_DURATION = 1f;
        
        /// <summary>Damage number rise speed (units per second)</summary>
        public const float DAMAGE_NUMBER_RISE_SPEED = 1f;
        
        /// <summary>Minimap size (width and height in pixels)</summary>
        public const int MINIMAP_SIZE = 200;
        
        /// <summary>Minimap camera height above player</summary>
        public const float MINIMAP_HEIGHT = 50f;
        
        /// <summary>HUD StringBuilder initial capacity</summary>
        public const int HUD_STRING_CAPACITY = 500;
        
        
        // ========================================
        // WORLD GENERATION SETTINGS
        // ========================================
        
        /// <summary>Terrain size (both X and Z dimensions)</summary>
        public const float TERRAIN_SIZE = 100f;
        
        /// <summary>Default terrain height</summary>
        public const float TERRAIN_HEIGHT = 0f;
        
        /// <summary>Fog density</summary>
        public const float FOG_DENSITY = 0.02f;
        
        
        // ========================================
        // OBJECT POOLING SETTINGS
        // ========================================
        
        /// <summary>Initial particle pool size</summary>
        public const int PARTICLE_POOL_INITIAL_SIZE = 100;
        
        /// <summary>Maximum particle pool size before reuse</summary>
        public const int PARTICLE_POOL_MAX_SIZE = 200;
        
        /// <summary>Audio source pool size</summary>
        public const int AUDIO_SOURCE_POOL_SIZE = 10;
        
        
        // ========================================
        // PERFORMANCE OPTIMIZATION CONSTANTS
        // ========================================
        
        /// <summary>Squared distance check for patrol reach (eliminates sqrt)</summary>
        public const float PATROL_REACH_DISTANCE_SQR = 1f; // 1 unit squared
    }
}
