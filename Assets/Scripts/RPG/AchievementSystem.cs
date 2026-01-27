using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Achievement system for tracking player accomplishments
/// </summary>
[System.Serializable]
public class Achievement
{
    public string id;
    public string name;
    public string description;
    public bool isUnlocked;
    public System.DateTime unlockedTime;
    
    public Achievement(string id, string name, string description)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.isUnlocked = false;
    }
    
    public void Unlock()
    {
        if (!isUnlocked)
        {
            isUnlocked = true;
            unlockedTime = System.DateTime.Now;
            Debug.Log($"🏆 Achievement Unlocked: {name}!");
            
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayQuestCompleteSound();
            }
        }
    }
}

public class AchievementSystem : MonoBehaviour
{
    public static AchievementSystem Instance { get; private set; }
    
    private List<Achievement> _achievements = new List<Achievement>();
    private int _enemiesDefeated;
    private int _questsCompleted;
    private int _chestsOpened;
    private int _locationsDiscovered;
    private int _totalDamageDealt;
    private int _maxComboReached;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        InitializeAchievements();
    }
    
    private void InitializeAchievements()
    {
        _achievements.Add(new Achievement("first_blood", "First Blood", "Defeat your first enemy"));
        _achievements.Add(new Achievement("orc_slayer", "Orc Slayer", "Defeat 10 enemies"));
        _achievements.Add(new Achievement("legendary_warrior", "Legendary Warrior", "Defeat 50 enemies"));
        
        _achievements.Add(new Achievement("treasure_hunter", "Treasure Hunter", "Open 5 treasure chests"));
        _achievements.Add(new Achievement("hoarder", "Dragon's Hoard", "Collect 1000 gold"));
        
        _achievements.Add(new Achievement("explorer", "Explorer", "Discover all major locations"));
        _achievements.Add(new Achievement("fellowship", "Fellowship Complete", "Complete the Fellowship quest"));
        _achievements.Add(new Achievement("quest_master", "Quest Master", "Complete all quests"));
        
        _achievements.Add(new Achievement("max_level", "Maximum Power", "Reach level 10"));
        _achievements.Add(new Achievement("combo_master", "Combo Master", "Achieve a 10-hit combo"));
        
        _achievements.Add(new Achievement("heavy_hitter", "Heavy Hitter", "Deal 10,000 total damage"));
        _achievements.Add(new Achievement("fully_equipped", "Fully Equipped", "Equip legendary items in all slots"));
    }
    
    public void OnEnemyDefeated(int damage)
    {
        _enemiesDefeated++;
        _totalDamageDealt += damage;
        
        CheckAchievement("first_blood", _enemiesDefeated >= 1);
        CheckAchievement("orc_slayer", _enemiesDefeated >= 10);
        CheckAchievement("legendary_warrior", _enemiesDefeated >= 50);
        CheckAchievement("heavy_hitter", _totalDamageDealt >= 10000);
    }
    
    public void OnQuestCompleted(string questId)
    {
        _questsCompleted++;
        
        if (questId == "fellowship")
        {
            CheckAchievement("fellowship", true);
        }
        
        CheckAchievement("quest_master", _questsCompleted >= 4);
    }
    
    public void OnChestOpened()
    {
        _chestsOpened++;
        CheckAchievement("treasure_hunter", _chestsOpened >= 5);
    }
    
    public void OnLocationDiscovered()
    {
        _locationsDiscovered++;
        CheckAchievement("explorer", _locationsDiscovered >= 3);
    }
    
    public void OnGoldChanged(int totalGold)
    {
        CheckAchievement("hoarder", totalGold >= 1000);
    }
    
    public void OnLevelUp(int level)
    {
        CheckAchievement("max_level", level >= 10);
    }
    
    public void OnComboAchieved(int comboCount)
    {
        if (comboCount > _maxComboReached)
        {
            _maxComboReached = comboCount;
            CheckAchievement("combo_master", _maxComboReached >= 10);
        }
    }
    
    public void OnFullyEquipped(bool hasAllLegendary)
    {
        CheckAchievement("fully_equipped", hasAllLegendary);
    }
    
    private void CheckAchievement(string achievementId, bool condition)
    {
        if (condition)
        {
            Achievement achievement = _achievements.Find(a => a.id == achievementId);
            if (achievement != null && !achievement.isUnlocked)
            {
                achievement.Unlock();
            }
        }
    }
    
    public List<Achievement> GetAllAchievements()
    {
        return new List<Achievement>(_achievements);
    }
    
    public int GetUnlockedCount()
    {
        return _achievements.FindAll(a => a.isUnlocked).Count;
    }
    
    public int GetTotalCount()
    {
        return _achievements.Count;
    }
    
    public float GetCompletionPercentage()
    {
        return (float)GetUnlockedCount() / GetTotalCount() * 100f;
    }
}
