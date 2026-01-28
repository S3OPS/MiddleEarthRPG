using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Enhanced save/load system with multiple save slots and auto-save
/// Part of Phase 6 (v2.5) Technical Systems
/// </summary>
public class EnhancedSaveSystem : MonoBehaviour
{
    public static EnhancedSaveSystem Instance { get; private set; }
    
    [Header("Save Settings")]
    public int maxSaveSlots = 5;
    public bool autoSaveEnabled = true;
    public float autoSaveInterval = 300f; // 5 minutes
    
    private string _saveDirectory;
    private float _autoSaveTimer;
    private int _currentSlot = 0;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        _saveDirectory = Path.Combine(Application.persistentDataPath, "Saves");
        
        if (!Directory.Exists(_saveDirectory))
        {
            Directory.CreateDirectory(_saveDirectory);
        }
    }

    private void Update()
    {
        if (autoSaveEnabled)
        {
            _autoSaveTimer += Time.deltaTime;
            if (_autoSaveTimer >= autoSaveInterval)
            {
                AutoSave();
                _autoSaveTimer = 0f;
            }
        }
    }

    #region Save Operations
    
    public bool SaveGame(int slot, string saveName = null)
    {
        try
        {
            if (slot < 0 || slot >= maxSaveSlots)
            {
                Debug.LogError($"Invalid save slot: {slot}");
                return false;
            }
            
            var saveData = CollectSaveData();
            saveData.saveName = saveName ?? $"Save {slot + 1}";
            saveData.slotIndex = slot;
            saveData.saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
            string filePath = GetSaveFilePath(slot);
            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(filePath, json);
            
            _currentSlot = slot;
            Debug.Log($"✅ Game saved to slot {slot}: {saveData.saveName}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game: {e.Message}");
            return false;
        }
    }

    public bool LoadGame(int slot)
    {
        try
        {
            if (slot < 0 || slot >= maxSaveSlots)
            {
                Debug.LogError($"Invalid save slot: {slot}");
                return false;
            }
            
            string filePath = GetSaveFilePath(slot);
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"No save file found in slot {slot}");
                return false;
            }
            
            string json = File.ReadAllText(filePath);
            var saveData = JsonUtility.FromJson<EnhancedSaveData>(json);
            
            if (saveData == null)
            {
                Debug.LogError($"Failed to parse save data from slot {slot}");
                return false;
            }
            
            ApplySaveData(saveData);
            _currentSlot = slot;
            
            Debug.Log($"✅ Game loaded from slot {slot}: {saveData.saveName}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load game: {e.Message}");
            return false;
        }
    }

    public bool DeleteSave(int slot)
    {
        try
        {
            string filePath = GetSaveFilePath(slot);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log($"✅ Deleted save in slot {slot}");
                return true;
            }
            return false;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to delete save: {e.Message}");
            return false;
        }
    }

    private void AutoSave()
    {
        if (SaveGame(_currentSlot, $"AutoSave {_currentSlot + 1}"))
        {
            Debug.Log($"🔄 Auto-saved to slot {_currentSlot}");
        }
    }
    
    #endregion

    #region Save Data Collection
    
    private EnhancedSaveData CollectSaveData()
    {
        var data = new EnhancedSaveData();
        
        // TODO: Collect data from actual game systems
        // This is a template - would need integration with actual game state
        
        data.characterName = "Hero";
        data.level = 5;
        data.gold = 1000;
        data.playTime = Time.time;
        
        // Collect quest data
        if (EnhancedQuestSystem.Instance != null)
        {
            var activeQuests = EnhancedQuestSystem.Instance.GetActiveQuests();
            data.activeQuestIds = new List<string>();
            foreach (var quest in activeQuests)
            {
                data.activeQuestIds.Add(quest.questId);
            }
            data.completedQuestCount = EnhancedQuestSystem.Instance.GetCompletedQuestCount();
        }
        
        // Collect lore data
        if (LoreBookSystem.Instance != null)
        {
            data.discoveredLoreBooks = new List<string>();
            var discoveredBooks = LoreBookSystem.Instance.GetDiscoveredBooks();
            foreach (var book in discoveredBooks)
            {
                data.discoveredLoreBooks.Add(book.bookId);
            }
        }
        
        // Collect NPC relationships
        data.npcRelationships = new Dictionary<string, int>();
        if (DialogueSystem.Instance != null)
        {
            data.npcRelationships["gandalf"] = DialogueSystem.Instance.GetRelationshipLevel("gandalf");
            data.npcRelationships["legolas"] = DialogueSystem.Instance.GetRelationshipLevel("legolas");
        }
        
        return data;
    }

    private void ApplySaveData(EnhancedSaveData data)
    {
        // TODO: Apply data to actual game systems
        Debug.Log($"Loading save: {data.saveName}");
        Debug.Log($"Character: {data.characterName}, Level: {data.level}");
        Debug.Log($"Play Time: {data.playTime:F0} seconds");
        Debug.Log($"Completed Quests: {data.completedQuestCount}");
        Debug.Log($"Discovered Lore: {data.discoveredLoreBooks?.Count ?? 0} books");
    }
    
    #endregion

    #region Save Slot Management
    
    public List<SaveSlotInfo> GetAllSaveSlots()
    {
        var slots = new List<SaveSlotInfo>();
        
        for (int i = 0; i < maxSaveSlots; i++)
        {
            var slotInfo = GetSaveSlotInfo(i);
            slots.Add(slotInfo);
        }
        
        return slots;
    }

    public SaveSlotInfo GetSaveSlotInfo(int slot)
    {
        string filePath = GetSaveFilePath(slot);
        
        if (!File.Exists(filePath))
        {
            return new SaveSlotInfo
            {
                slotIndex = slot,
                isEmpty = true
            };
        }
        
        try
        {
            string json = File.ReadAllText(filePath);
            var data = JsonUtility.FromJson<EnhancedSaveData>(json);
            
            return new SaveSlotInfo
            {
                slotIndex = slot,
                isEmpty = false,
                saveName = data.saveName,
                saveDate = data.saveDate,
                characterName = data.characterName,
                level = data.level,
                playTime = data.playTime
            };
        }
        catch
        {
            return new SaveSlotInfo
            {
                slotIndex = slot,
                isEmpty = true,
                isCorrupted = true
            };
        }
    }

    private string GetSaveFilePath(int slot)
    {
        return Path.Combine(_saveDirectory, $"save_slot_{slot}.json");
    }
    
    #endregion

    #region Quick Actions
    
    public void QuickSave()
    {
        SaveGame(_currentSlot, $"QuickSave");
    }

    public void QuickLoad()
    {
        LoadGame(_currentSlot);
    }
    
    #endregion
}

[Serializable]
public class EnhancedSaveData
{
    public string saveName;
    public int slotIndex;
    public string saveDate;
    public float playTime;
    
    // Character
    public string characterName;
    public int level;
    public float health;
    public float stamina;
    public int gold;
    
    // Progress
    public List<string> activeQuestIds;
    public int completedQuestCount;
    public List<string> discoveredLoreBooks;
    public Dictionary<string, int> npcRelationships;
    
    // Position
    public float posX;
    public float posY;
    public float posZ;
}

[Serializable]
public class SaveSlotInfo
{
    public int slotIndex;
    public bool isEmpty;
    public bool isCorrupted;
    public string saveName;
    public string saveDate;
    public string characterName;
    public int level;
    public float playTime;
}
