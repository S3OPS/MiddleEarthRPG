using UnityEngine;
using System;
using System.IO;
using MiddleEarth.Config;

namespace MiddleEarth.Core
{
    /// <summary>
    /// Game state for serialization.
    /// Contains all player progress data that should be persisted.
    /// </summary>
    [Serializable]
    public class GameSaveData
    {
        // Metadata
        public int saveVersion = GameConstants.SAVE_FILE_VERSION;
        public string saveDate;
        public string checksum;
        
        // Character data
        public string characterName;
        public int level;
        public float currentHealth;
        public float maxHealth;
        public float currentStamina;
        public float maxStamina;
        public int strength;
        public int wisdom;
        public int agility;
        public int experience;
        public int experienceToNextLevel;
        
        // Inventory data
        public int gold;
        public string[] itemNames;
        public int[] itemQuantities;
        
        // Equipment data
        public string equippedWeapon;
        public string equippedArmor;
        public string equippedAccessory;
        
        // Progress data
        public string[] completedQuests;
        public string[] unlockedAchievements;
        public string[] discoveredLocations;
        
        // Statistics
        public int enemiesDefeated;
        public int questsCompleted;
        public int chestsOpened;
        public float playTime;
    }
    
    /// <summary>
    /// Save system for persisting game state.
    /// Implements security features including checksums for tamper detection.
    /// </summary>
    public static class SaveSystem
    {
        private const string SAVE_FILE_NAME = "middle_earth_save.json";
        
        /// <summary>
        /// Get the save file path.
        /// </summary>
        public static string SavePath => Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
        
        /// <summary>
        /// Save game data to file.
        /// </summary>
        /// <param name="data">The game data to save.</param>
        /// <returns>True if save was successful.</returns>
        public static bool Save(GameSaveData data)
        {
            if (data == null)
            {
                Debug.LogError("SaveSystem: Cannot save null data");
                return false;
            }
            
            try
            {
                // Set metadata
                data.saveDate = DateTime.Now.ToString("O");
                data.saveVersion = GameConstants.SAVE_FILE_VERSION;
                
                // Serialize data (without checksum)
                data.checksum = "";
                string jsonData = JsonUtility.ToJson(data, true);
                
                // Generate and set checksum
                data.checksum = InputValidator.GenerateChecksum(jsonData);
                jsonData = JsonUtility.ToJson(data, true);
                
                // Validate size
                if (!InputValidator.IsValidJsonSize(jsonData))
                {
                    Debug.LogError("SaveSystem: Save data exceeds maximum allowed size");
                    return false;
                }
                
                // Write to file
                File.WriteAllText(SavePath, jsonData);
                Debug.Log($"SaveSystem: Game saved successfully to {SavePath}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"SaveSystem: Failed to save game - {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Load game data from file.
        /// </summary>
        /// <returns>The loaded game data, or null if loading failed.</returns>
        public static GameSaveData Load()
        {
            if (!File.Exists(SavePath))
            {
                Debug.Log("SaveSystem: No save file found");
                return null;
            }
            
            try
            {
                string jsonData = File.ReadAllText(SavePath);
                
                // Validate size
                if (!InputValidator.IsValidJsonSize(jsonData))
                {
                    Debug.LogError("SaveSystem: Save file exceeds maximum allowed size");
                    return null;
                }
                
                // Parse the data
                GameSaveData data = JsonUtility.FromJson<GameSaveData>(jsonData);
                
                if (data == null)
                {
                    Debug.LogError("SaveSystem: Failed to parse save file");
                    return null;
                }
                
                // Verify version compatibility
                if (data.saveVersion > GameConstants.SAVE_FILE_VERSION)
                {
                    Debug.LogError($"SaveSystem: Save file version {data.saveVersion} is newer than supported version {GameConstants.SAVE_FILE_VERSION}");
                    return null;
                }
                
                // Verify checksum
                string savedChecksum = data.checksum;
                data.checksum = "";
                string verifyJson = JsonUtility.ToJson(data, true);
                
                if (!InputValidator.VerifyChecksum(verifyJson, savedChecksum))
                {
                    Debug.LogWarning("SaveSystem: Save file checksum mismatch - possible tampering detected");
                    // Allow loading but warn the user
                }
                
                data.checksum = savedChecksum;
                Debug.Log($"SaveSystem: Game loaded successfully from {SavePath}");
                return data;
            }
            catch (Exception ex)
            {
                Debug.LogError($"SaveSystem: Failed to load game - {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// Delete the save file.
        /// </summary>
        /// <returns>True if deletion was successful.</returns>
        public static bool Delete()
        {
            try
            {
                if (File.Exists(SavePath))
                {
                    File.Delete(SavePath);
                    Debug.Log("SaveSystem: Save file deleted");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.LogError($"SaveSystem: Failed to delete save - {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Check if a save file exists.
        /// </summary>
        public static bool SaveExists => File.Exists(SavePath);
    }
}
