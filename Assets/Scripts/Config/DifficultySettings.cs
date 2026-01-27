using UnityEngine;

namespace MiddleEarth.Config
{
    /// <summary>
    /// Difficulty level enumeration.
    /// </summary>
    public enum DifficultyLevel
    {
        Easy,
        Normal,
        Hard,
        Legendary
    }
    
    /// <summary>
    /// Difficulty modifiers for game balance.
    /// Implements the "Clean up the Camp" principle - centralized configuration.
    /// </summary>
    [System.Serializable]
    public class DifficultyModifiers
    {
        public float playerDamageMultiplier = 1f;
        public float playerHealthMultiplier = 1f;
        public float enemyDamageMultiplier = 1f;
        public float enemyHealthMultiplier = 1f;
        public float experienceMultiplier = 1f;
        public float goldMultiplier = 1f;
        public float staminaCostMultiplier = 1f;
        public float criticalHitBonus = 0f;
    }
    
    /// <summary>
    /// Difficulty settings manager for game balance.
    /// Provides preset difficulty configurations and runtime adjustment.
    /// </summary>
    public static class DifficultySettings
    {
        private static DifficultyLevel _currentDifficulty = DifficultyLevel.Normal;
        private static DifficultyModifiers _currentModifiers;
        
        /// <summary>
        /// Get the current difficulty level.
        /// </summary>
        public static DifficultyLevel CurrentDifficulty => _currentDifficulty;
        
        /// <summary>
        /// Get the current difficulty modifiers.
        /// </summary>
        public static DifficultyModifiers Modifiers => _currentModifiers ?? GetModifiersForDifficulty(_currentDifficulty);
        
        /// <summary>
        /// Set the difficulty level.
        /// </summary>
        /// <param name="difficulty">The difficulty level to set.</param>
        public static void SetDifficulty(DifficultyLevel difficulty)
        {
            _currentDifficulty = difficulty;
            _currentModifiers = GetModifiersForDifficulty(difficulty);
            Debug.Log($"Difficulty set to: {difficulty}");
        }
        
        /// <summary>
        /// Get modifiers for a specific difficulty level.
        /// </summary>
        /// <param name="difficulty">The difficulty level.</param>
        /// <returns>The modifiers for that difficulty.</returns>
        public static DifficultyModifiers GetModifiersForDifficulty(DifficultyLevel difficulty)
        {
            switch (difficulty)
            {
                case DifficultyLevel.Easy:
                    return new DifficultyModifiers
                    {
                        playerDamageMultiplier = 1.5f,
                        playerHealthMultiplier = 1.5f,
                        enemyDamageMultiplier = 0.5f,
                        enemyHealthMultiplier = 0.75f,
                        experienceMultiplier = 1.25f,
                        goldMultiplier = 1.25f,
                        staminaCostMultiplier = 0.75f,
                        criticalHitBonus = 0.1f
                    };
                    
                case DifficultyLevel.Normal:
                    return new DifficultyModifiers
                    {
                        playerDamageMultiplier = 1f,
                        playerHealthMultiplier = 1f,
                        enemyDamageMultiplier = 1f,
                        enemyHealthMultiplier = 1f,
                        experienceMultiplier = 1f,
                        goldMultiplier = 1f,
                        staminaCostMultiplier = 1f,
                        criticalHitBonus = 0f
                    };
                    
                case DifficultyLevel.Hard:
                    return new DifficultyModifiers
                    {
                        playerDamageMultiplier = 0.8f,
                        playerHealthMultiplier = 0.8f,
                        enemyDamageMultiplier = 1.5f,
                        enemyHealthMultiplier = 1.5f,
                        experienceMultiplier = 1.5f,
                        goldMultiplier = 0.75f,
                        staminaCostMultiplier = 1.25f,
                        criticalHitBonus = -0.05f
                    };
                    
                case DifficultyLevel.Legendary:
                    return new DifficultyModifiers
                    {
                        playerDamageMultiplier = 0.6f,
                        playerHealthMultiplier = 0.6f,
                        enemyDamageMultiplier = 2f,
                        enemyHealthMultiplier = 2f,
                        experienceMultiplier = 2f,
                        goldMultiplier = 0.5f,
                        staminaCostMultiplier = 1.5f,
                        criticalHitBonus = -0.1f
                    };
                    
                default:
                    return new DifficultyModifiers();
            }
        }
        
        /// <summary>
        /// Apply difficulty modifier to player damage.
        /// </summary>
        /// <param name="baseDamage">The base damage value.</param>
        /// <returns>The modified damage value.</returns>
        public static float ApplyPlayerDamage(float baseDamage)
        {
            return baseDamage * Modifiers.playerDamageMultiplier;
        }
        
        /// <summary>
        /// Apply difficulty modifier to enemy damage.
        /// </summary>
        /// <param name="baseDamage">The base damage value.</param>
        /// <returns>The modified damage value.</returns>
        public static float ApplyEnemyDamage(float baseDamage)
        {
            return baseDamage * Modifiers.enemyDamageMultiplier;
        }
        
        /// <summary>
        /// Apply difficulty modifier to experience gain.
        /// </summary>
        /// <param name="baseXP">The base XP value.</param>
        /// <returns>The modified XP value.</returns>
        public static int ApplyExperience(int baseXP)
        {
            return Mathf.RoundToInt(baseXP * Modifiers.experienceMultiplier);
        }
        
        /// <summary>
        /// Apply difficulty modifier to gold gain.
        /// </summary>
        /// <param name="baseGold">The base gold value.</param>
        /// <returns>The modified gold value.</returns>
        public static int ApplyGold(int baseGold)
        {
            return Mathf.RoundToInt(baseGold * Modifiers.goldMultiplier);
        }
        
        /// <summary>
        /// Get the modified critical hit chance.
        /// </summary>
        /// <param name="baseCritChance">The base critical hit chance.</param>
        /// <returns>The modified critical hit chance.</returns>
        public static float GetCriticalHitChance(float baseCritChance)
        {
            return Mathf.Clamp01(baseCritChance + Modifiers.criticalHitBonus);
        }
    }
}
