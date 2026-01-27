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
}
