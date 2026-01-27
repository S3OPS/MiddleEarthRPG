using UnityEngine;
using System;

namespace MiddleEarth.Core
{
    /// <summary>
    /// Game statistics tracking system.
    /// Tracks player progress and achievements for analytics and display.
    /// </summary>
    public class GameStatistics : MonoBehaviour
    {
        public static GameStatistics Instance { get; private set; }
        
        // Combat statistics
        public int TotalEnemiesDefeated { get; private set; }
        public int TotalDamageDealt { get; private set; }
        public int TotalDamageTaken { get; private set; }
        public int TotalCriticalHits { get; private set; }
        public int HighestCombo { get; private set; }
        public int TotalAttacks { get; private set; }
        public int TotalSpecialAbilitiesUsed { get; private set; }
        
        // Exploration statistics
        public int LocationsDiscovered { get; private set; }
        public int ChestsOpened { get; private set; }
        public int NPCsSpokenTo { get; private set; }
        
        // Progression statistics
        public int QuestsCompleted { get; private set; }
        public int AchievementsUnlocked { get; private set; }
        public int HighestLevel { get; private set; }
        public int TotalGoldEarned { get; private set; }
        public int TotalExperienceEarned { get; private set; }
        
        // Session statistics
        public float TotalPlayTime { get; private set; }
        public DateTime SessionStartTime { get; private set; }
        public int Deaths { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            SessionStartTime = DateTime.Now;
        }
        
        private void Update()
        {
            TotalPlayTime += Time.deltaTime;
        }
        
        // Combat tracking methods
        public void RecordEnemyDefeated()
        {
            TotalEnemiesDefeated++;
        }
        
        public void RecordDamageDealt(int damage, bool isCritical)
        {
            TotalDamageDealt += damage;
            TotalAttacks++;
            if (isCritical)
            {
                TotalCriticalHits++;
            }
        }
        
        public void RecordDamageTaken(int damage)
        {
            TotalDamageTaken += damage;
        }
        
        public void RecordCombo(int comboCount)
        {
            if (comboCount > HighestCombo)
            {
                HighestCombo = comboCount;
            }
        }
        
        public void RecordSpecialAbilityUsed()
        {
            TotalSpecialAbilitiesUsed++;
        }
        
        // Exploration tracking methods
        public void RecordLocationDiscovered()
        {
            LocationsDiscovered++;
        }
        
        public void RecordChestOpened()
        {
            ChestsOpened++;
        }
        
        public void RecordNPCInteraction()
        {
            NPCsSpokenTo++;
        }
        
        // Progression tracking methods
        public void RecordQuestCompleted()
        {
            QuestsCompleted++;
        }
        
        public void RecordAchievementUnlocked()
        {
            AchievementsUnlocked++;
        }
        
        public void RecordLevelUp(int newLevel)
        {
            if (newLevel > HighestLevel)
            {
                HighestLevel = newLevel;
            }
        }
        
        public void RecordGoldEarned(int gold)
        {
            TotalGoldEarned += gold;
        }
        
        public void RecordExperienceEarned(int xp)
        {
            TotalExperienceEarned += xp;
        }
        
        // Session tracking
        public void RecordDeath()
        {
            Deaths++;
        }
        
        /// <summary>
        /// Get a summary string of current statistics.
        /// </summary>
        public string GetStatisticsSummary()
        {
            TimeSpan playTime = TimeSpan.FromSeconds(TotalPlayTime);
            float hitRate = TotalAttacks > 0 ? (float)TotalCriticalHits / TotalAttacks * 100 : 0;
            
            return $@"=== GAME STATISTICS ===

COMBAT:
  Enemies Defeated: {TotalEnemiesDefeated}
  Total Damage Dealt: {TotalDamageDealt:N0}
  Total Damage Taken: {TotalDamageTaken:N0}
  Critical Hits: {TotalCriticalHits} ({hitRate:F1}% crit rate)
  Highest Combo: {HighestCombo}
  Special Abilities Used: {TotalSpecialAbilitiesUsed}

EXPLORATION:
  Locations Discovered: {LocationsDiscovered}
  Chests Opened: {ChestsOpened}
  NPCs Spoken To: {NPCsSpokenTo}

PROGRESSION:
  Highest Level: {HighestLevel}
  Quests Completed: {QuestsCompleted}
  Achievements Unlocked: {AchievementsUnlocked}
  Total Gold Earned: {TotalGoldEarned:N0}
  Total XP Earned: {TotalExperienceEarned:N0}

SESSION:
  Play Time: {playTime.Hours}h {playTime.Minutes}m {playTime.Seconds}s
  Deaths: {Deaths}
";
        }
        
        /// <summary>
        /// Reset all statistics (for new game).
        /// </summary>
        public void ResetStatistics()
        {
            TotalEnemiesDefeated = 0;
            TotalDamageDealt = 0;
            TotalDamageTaken = 0;
            TotalCriticalHits = 0;
            HighestCombo = 0;
            TotalAttacks = 0;
            TotalSpecialAbilitiesUsed = 0;
            LocationsDiscovered = 0;
            ChestsOpened = 0;
            NPCsSpokenTo = 0;
            QuestsCompleted = 0;
            AchievementsUnlocked = 0;
            HighestLevel = 1;
            TotalGoldEarned = 0;
            TotalExperienceEarned = 0;
            TotalPlayTime = 0;
            Deaths = 0;
            SessionStartTime = DateTime.Now;
        }
    }
}
