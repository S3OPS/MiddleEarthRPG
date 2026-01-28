using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private List<Quest> _allQuests = new List<Quest>();
    private List<Quest> _activeQuests = new List<Quest>();
    private CharacterStats _playerStats;
    private InventorySystem _inventory;

    public void Initialize(CharacterStats playerStats, InventorySystem inventory)
    {
        _playerStats = playerStats;
        _inventory = inventory;
        CreateLOTRQuests();
    }

    private void CreateLOTRQuests()
    {
        // Quest 1: The Shire's Burden
        var quest1 = new Quest(
            "shire_burden",
            "The Shire's Burden",
            "Old Bilbo has asked you to deliver a mysterious ring to Rivendell. But first, gather supplies for the journey.",
            100,
            150
        );
        quest1.AddObjective(new QuestObjective("gather_lembas", "Gather 3 Lembas Bread from the Shire", ObjectiveType.CollectItem, 3));
        quest1.AddObjective(new QuestObjective("explore_shire", "Explore the Old Forest", ObjectiveType.ExploreLocation, 1));
        _allQuests.Add(quest1);

        // Quest 2: Riders of Rohan
        var quest2 = new Quest(
            "rohan_riders",
            "Riders of Rohan",
            "The forces of darkness gather. Help Rohan prepare for battle by defeating orc scouts.",
            200,
            300
        );
        quest2.AddObjective(new QuestObjective("defeat_orcs", "Defeat 5 Orc Scouts", ObjectiveType.DefeatEnemy, 5));
        quest2.AddObjective(new QuestObjective("explore_rohan", "Survey the Plains of Rohan", ObjectiveType.ExploreLocation, 1));
        _allQuests.Add(quest2);

        // Quest 3: The Path to Mordor
        var quest3 = new Quest(
            "path_mordor",
            "The Path to Mordor",
            "The final quest approaches. Venture into the dark lands and confront the Shadow.",
            500,
            1000
        );
        quest3.AddObjective(new QuestObjective("explore_mordor", "Enter the Lands of Mordor", ObjectiveType.ExploreLocation, 1));
        quest3.AddObjective(new QuestObjective("defeat_darkness", "Defeat servants of darkness", ObjectiveType.DefeatEnemy, 10));
        quest3.AddObjective(new QuestObjective("find_ring", "Find the Ring of Power", ObjectiveType.CollectItem, 1));
        _allQuests.Add(quest3);

        // Quest 4: Fellowship of the Ring
        var quest4 = new Quest(
            "fellowship",
            "Fellowship of the Ring",
            "Speak with the members of the Fellowship scattered across Middle-earth.",
            150,
            200
        );
        quest4.AddObjective(new QuestObjective("talk_gandalf", "Speak with Gandalf the Grey", ObjectiveType.TalkToNPC, 1));
        quest4.AddObjective(new QuestObjective("talk_legolas", "Speak with Legolas", ObjectiveType.TalkToNPC, 1));
        quest4.AddObjective(new QuestObjective("talk_gimli", "Speak with Gimli", ObjectiveType.TalkToNPC, 1));
        _allQuests.Add(quest4);
        
        // Quest 5: Master of Arms
        var quest5 = new Quest(
            "master_arms",
            "Master of Arms",
            "Prove your worth as a warrior by mastering combat and collecting legendary equipment.",
            300,
            500
        );
        quest5.AddObjective(new QuestObjective("equip_weapon", "Equip a legendary weapon", ObjectiveType.CollectItem, 1));
        quest5.AddObjective(new QuestObjective("combo_10", "Achieve a 10-hit combo", ObjectiveType.DefeatEnemy, 1));
        quest5.AddObjective(new QuestObjective("defeat_15", "Defeat 15 enemies total", ObjectiveType.DefeatEnemy, 15));
        _allQuests.Add(quest5);
        
        // Quest 6: Treasure Seeker
        var quest6 = new Quest(
            "treasure_seeker",
            "Treasure Seeker",
            "The lands are filled with hidden treasures. Find them all and grow rich beyond measure.",
            250,
            400
        );
        quest6.AddObjective(new QuestObjective("find_chests", "Open 5 treasure chests", ObjectiveType.CollectItem, 5));
        quest6.AddObjective(new QuestObjective("collect_gold", "Collect 500 gold", ObjectiveType.CollectItem, 1));
        _allQuests.Add(quest6);
        
        // Quest 7: Legend of Middle-earth
        var quest7 = new Quest(
            "legend",
            "Legend of Middle-earth",
            "Become a true legend by reaching the pinnacle of power and completing all challenges.",
            1000,
            2000
        );
        quest7.AddObjective(new QuestObjective("reach_level_10", "Reach level 10", ObjectiveType.ExploreLocation, 1));
        quest7.AddObjective(new QuestObjective("all_locations", "Discover all locations", ObjectiveType.ExploreLocation, 3));
        quest7.AddObjective(new QuestObjective("defeat_25", "Defeat 25 enemies", ObjectiveType.DefeatEnemy, 25));
        _allQuests.Add(quest7);
    }

    public void ActivateQuest(string questId)
    {
        var quest = _allQuests.Find(q => q.questId == questId);
        if (quest != null && !quest.isActive)
        {
            quest.isActive = true;
            _activeQuests.Add(quest);
        }
    }

    public void UpdateQuestProgress(string questId, string objectiveId, int progress = 1)
    {
        var quest = _activeQuests.Find(q => q.questId == questId);
        if (quest != null && !quest.isCompleted)
        {
            quest.UpdateProgress(objectiveId, progress);
            if (quest.isCompleted)
            {
                CompleteQuest(quest);
            }
        }
    }

    private void CompleteQuest(Quest quest)
    {
        _playerStats.GainExperience(quest.experienceReward);
        _inventory.AddGold(quest.goldReward);
        
        foreach (var item in quest.itemRewards)
        {
            _inventory.AddItem(item);
        }
        
        Debug.Log($"✅ Quest Complete: {quest.questName}!");
        
        // Effects and achievements
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayQuestCompleteSound();
        }
        
        if (AchievementSystem.Instance != null)
        {
            AchievementSystem.Instance.OnQuestCompleted(quest.questId);
        }
    }

    public List<Quest> GetActiveQuests()
    {
        return new List<Quest>(_activeQuests);
    }

    public List<Quest> GetAllQuests()
    {
        return new List<Quest>(_allQuests);
    }

    public Quest GetQuest(string questId)
    {
        return _allQuests.Find(q => q.questId == questId);
    }
    
    private void OnDestroy()
    {
        // Clear collections to prevent memory leaks
        _allQuests.Clear();
        _activeQuests.Clear();
        _playerStats = null;
        _inventory = null;
    }
}
