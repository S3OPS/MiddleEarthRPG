using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enhanced quest system with branching paths, choices, and dynamic content.
/// Extends the basic quest system to support complex narratives.
/// </summary>
public class EnhancedQuestSystem : MonoBehaviour
{
    public static EnhancedQuestSystem Instance { get; private set; }
    
    private Dictionary<string, EnhancedQuest> _enhancedQuests = new Dictionary<string, EnhancedQuest>();
    private HashSet<string> _completedQuests = new HashSet<string>();
    private HashSet<string> _activeQuests = new HashSet<string>();
    
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
    }

    private void Start()
    {
        InitializeEnhancedQuests();
    }

    private void InitializeEnhancedQuests()
    {
        // Quest 1: The Shadow in the Forest (Branching Quest)
        var quest1 = new EnhancedQuest(
            "shadow_forest",
            "The Shadow in the Forest",
            "Dark creatures have been spotted in the Old Forest. Investigate and decide how to handle the threat.",
            QuestType.Main,
            300,
            500
        );
        quest1.AddStage(new QuestStage(
            "investigate",
            "Investigate the Old Forest",
            "Travel to the Old Forest and discover what lurks within."
        ));
        quest1.AddStage(new QuestStage(
            "choice_approach",
            "Choose Your Approach",
            "You've found a camp of orcs. Do you attack, sneak past, or try to negotiate?"
        ));
        // Branching based on player choice
        quest1.AddBranch("attack", new QuestBranch(
            "attack",
            "Attack the Orcs",
            "Engage in combat",
            new List<string> { "defeat_orcs" }
        ));
        quest1.AddBranch("sneak", new QuestBranch(
            "sneak",
            "Sneak Past",
            "Avoid detection",
            new List<string> { "reach_leader" }
        ));
        quest1.AddBranch("negotiate", new QuestBranch(
            "negotiate",
            "Attempt Negotiation",
            "Talk to the orc leader",
            new List<string> { "talk_leader" }
        ));
        _enhancedQuests[quest1.questId] = quest1;

        // Quest 2: The Lost Heirloom (Time-Sensitive Quest)
        var quest2 = new EnhancedQuest(
            "lost_heirloom",
            "The Lost Heirloom",
            "A noble family has lost a precious heirloom. Find it before sunset or it may be lost forever.",
            QuestType.Timed,
            200,
            300
        );
        quest2.timeLimit = 600f; // 10 minutes
        quest2.AddStage(new QuestStage(
            "search_shire",
            "Search the Shire",
            "Look for clues about the missing heirloom in the Shire."
        ));
        quest2.AddStage(new QuestStage(
            "follow_trail",
            "Follow the Trail",
            "Track the thief to their hideout."
        ));
        quest2.AddStage(new QuestStage(
            "recover_heirloom",
            "Recover the Heirloom",
            "Retrieve the stolen item before time runs out."
        ));
        _enhancedQuests[quest2.questId] = quest2;

        // Quest 3: The Weathered Wanderer (Weather-Dependent Quest)
        var quest3 = new EnhancedQuest(
            "weathered_wanderer",
            "The Weathered Wanderer",
            "An old traveler seeks rare herbs that only bloom during rainfall. Help them gather ingredients.",
            QuestType.Environmental,
            150,
            250
        );
        quest3.requiredWeather = "Rain";
        quest3.AddStage(new QuestStage(
            "find_traveler",
            "Find the Traveler",
            "Locate the old wanderer in Rohan."
        ));
        quest3.AddStage(new QuestStage(
            "gather_herbs",
            "Gather Rainbloom Herbs",
            "Collect 5 Rainbloom herbs during rainfall (0/5)."
        ));
        quest3.AddStage(new QuestStage(
            "return_herbs",
            "Return to the Traveler",
            "Bring the herbs back to the wanderer."
        ));
        _enhancedQuests[quest3.questId] = quest3;

        // Quest 4: Secrets of the Deep (Dungeon Quest)
        var quest4 = new EnhancedQuest(
            "secrets_deep",
            "Secrets of the Deep",
            "Ancient texts speak of a powerful artifact hidden in the deepest dungeon level.",
            QuestType.Dungeon,
            500,
            1000
        );
        quest4.AddStage(new QuestStage(
            "enter_dungeon",
            "Enter the Ancient Dungeon",
            "Find and enter a dungeon in Mordor."
        ));
        quest4.AddStage(new QuestStage(
            "reach_depth",
            "Descend to Floor 5",
            "Navigate to the 5th floor of the dungeon."
        ));
        quest4.AddStage(new QuestStage(
            "defeat_boss",
            "Defeat the Guardian",
            "Overcome the boss protecting the artifact."
        ));
        quest4.AddStage(new QuestStage(
            "claim_artifact",
            "Claim the Artifact",
            "Retrieve the legendary item."
        ));
        _enhancedQuests[quest4.questId] = quest4;

        // Quest 5: Night Watch (Time-of-Day Quest)
        var quest5 = new EnhancedQuest(
            "night_watch",
            "Night Watch",
            "Monsters grow bolder at night. Patrol the roads and protect travelers during the dark hours.",
            QuestType.Environmental,
            250,
            400
        );
        quest5.requiredTimeOfDay = "Night";
        quest5.AddStage(new QuestStage(
            "wait_nightfall",
            "Wait for Nightfall",
            "Begin patrol when night falls."
        ));
        quest5.AddStage(new QuestStage(
            "patrol_roads",
            "Patrol the Roads",
            "Defeat 8 enemies during nighttime (0/8)."
        ));
        quest5.AddStage(new QuestStage(
            "report_success",
            "Report to the Captain",
            "Return to the guard captain at dawn."
        ));
        _enhancedQuests[quest5.questId] = quest5;

        // Quest 6: The Fellowship Reunited (Multi-Stage Epic)
        var quest6 = new EnhancedQuest(
            "fellowship_reunited",
            "The Fellowship Reunited",
            "Gather the scattered members of the Fellowship for one final mission.",
            QuestType.Main,
            1000,
            2000
        );
        quest6.AddStage(new QuestStage(
            "find_gandalf",
            "Seek Gandalf",
            "Find Gandalf in the Shire."
        ));
        quest6.AddStage(new QuestStage(
            "find_legolas",
            "Seek Legolas",
            "Find Legolas in Rohan."
        ));
        quest6.AddStage(new QuestStage(
            "find_gimli",
            "Seek Gimli",
            "Find Gimli near a dungeon entrance."
        ));
        quest6.AddStage(new QuestStage(
            "council_meeting",
            "Attend the Council",
            "Gather at Rivendell for the war council."
        ));
        quest6.AddStage(new QuestStage(
            "final_battle",
            "The Final Battle",
            "Face the darkness together."
        ));
        _enhancedQuests[quest6.questId] = quest6;

        // Quest 7: Treasure of the Ancients (Hidden Quest)
        var quest7 = new EnhancedQuest(
            "treasure_ancients",
            "Treasure of the Ancients",
            "Rumors speak of a hidden treasure that can only be found by those who discover all the ancient lore.",
            QuestType.Secret,
            800,
            1500
        );
        quest7.isHidden = true; // Only reveals after collecting 10+ lore books
        quest7.AddStage(new QuestStage(
            "collect_lore",
            "Gather Ancient Knowledge",
            "Discover at least 10 lore books (0/10)."
        ));
        quest7.AddStage(new QuestStage(
            "decipher_clues",
            "Decipher the Clues",
            "Piece together the location from the collected texts."
        ));
        quest7.AddStage(new QuestStage(
            "find_treasure",
            "Claim the Treasure",
            "Locate and retrieve the legendary treasure."
        ));
        _enhancedQuests[quest7.questId] = quest7;

        // Quest 8: The Healer's Request (Reputation Quest)
        var quest8 = new EnhancedQuest(
            "healers_request",
            "The Healer's Request",
            "A healer needs rare ingredients, but will only trust those with a good reputation.",
            QuestType.Reputation,
            300,
            500
        );
        quest8.requiredReputation = 50; // Need friendly reputation with NPCs
        quest8.AddStage(new QuestStage(
            "earn_trust",
            "Build Reputation",
            "Complete other quests to earn the healer's trust."
        ));
        quest8.AddStage(new QuestStage(
            "accept_quest",
            "Accept the Mission",
            "The healer will now give you their request."
        ));
        quest8.AddStage(new QuestStage(
            "gather_ingredients",
            "Gather Rare Herbs",
            "Collect 3 rare healing herbs from dangerous locations (0/3)."
        ));
        quest8.AddStage(new QuestStage(
            "deliver_herbs",
            "Deliver the Ingredients",
            "Return the herbs to the healer."
        ));
        _enhancedQuests[quest8.questId] = quest8;
    }

    public void ActivateQuest(string questId)
    {
        if (_enhancedQuests.ContainsKey(questId) && !_activeQuests.Contains(questId))
        {
            var quest = _enhancedQuests[questId];
            
            // Check prerequisites
            if (quest.isHidden && !CheckQuestPrerequisites(quest))
            {
                Debug.Log("Quest prerequisites not met.");
                return;
            }
            
            _activeQuests.Add(questId);
            quest.isActive = true;
            quest.startTime = Time.time;
            
            Debug.Log($"📜 NEW QUEST: {quest.questName}");
            Debug.Log($"Type: {quest.questType}");
            Debug.Log($"Description: {quest.description}");
            
            if (quest.questType == QuestType.Timed)
            {
                Debug.Log($"⏰ Time Limit: {quest.timeLimit} seconds");
            }
        }
    }

    private bool CheckQuestPrerequisites(EnhancedQuest quest)
    {
        // Check reputation requirement
        if (quest.requiredReputation > 0)
        {
            // Would integrate with NPC relationship system
            return true; // Simplified for now
        }
        
        // Check lore collection requirement for hidden quests
        if (quest.isHidden && quest.questId == "treasure_ancients")
        {
            if (LoreBookSystem.Instance != null)
            {
                return LoreBookSystem.Instance.GetDiscoveredCount() >= 10;
            }
        }
        
        return true;
    }

    public void AdvanceQuestStage(string questId)
    {
        if (_enhancedQuests.ContainsKey(questId) && _activeQuests.Contains(questId))
        {
            var quest = _enhancedQuests[questId];
            quest.AdvanceStage();
            
            if (quest.IsComplete())
            {
                CompleteQuest(questId);
            }
        }
    }

    public void SelectQuestBranch(string questId, string branchId)
    {
        if (_enhancedQuests.ContainsKey(questId) && _activeQuests.Contains(questId))
        {
            var quest = _enhancedQuests[questId];
            if (quest.branches.ContainsKey(branchId))
            {
                quest.selectedBranch = branchId;
                Debug.Log($"Quest Branch Selected: {quest.branches[branchId].branchName}");
            }
        }
    }

    private void CompleteQuest(string questId)
    {
        var quest = _enhancedQuests[questId];
        _completedQuests.Add(questId);
        _activeQuests.Remove(questId);
        
        Debug.Log($"✅ QUEST COMPLETE: {quest.questName}");
        Debug.Log($"Rewards: {quest.goldReward} gold, {quest.experienceReward} XP");
        
        // Trigger achievement
        if (AchievementSystem.Instance != null)
        {
            AchievementSystem.Instance.OnQuestCompleted(questId);
        }
    }

    public List<EnhancedQuest> GetActiveQuests()
    {
        var active = new List<EnhancedQuest>();
        foreach (var questId in _activeQuests)
        {
            if (_enhancedQuests.ContainsKey(questId))
            {
                active.Add(_enhancedQuests[questId]);
            }
        }
        return active;
    }

    public EnhancedQuest GetQuest(string questId)
    {
        return _enhancedQuests.ContainsKey(questId) ? _enhancedQuests[questId] : null;
    }

    public int GetCompletedQuestCount()
    {
        return _completedQuests.Count;
    }
}

[System.Serializable]
public class EnhancedQuest
{
    public string questId;
    public string questName;
    public string description;
    public QuestType questType;
    public int goldReward;
    public int experienceReward;
    public bool isActive;
    public bool isHidden;
    
    // Branching support
    public Dictionary<string, QuestBranch> branches = new Dictionary<string, QuestBranch>();
    public string selectedBranch;
    
    // Multi-stage support
    public List<QuestStage> stages = new List<QuestStage>();
    public int currentStageIndex = 0;
    
    // Time/environment requirements
    public float timeLimit;
    public float startTime;
    public string requiredWeather;
    public string requiredTimeOfDay;
    public int requiredReputation;

    public EnhancedQuest(string id, string name, string desc, QuestType type, int gold, int exp)
    {
        questId = id;
        questName = name;
        description = desc;
        questType = type;
        goldReward = gold;
        experienceReward = exp;
        isActive = false;
        isHidden = false;
    }

    public void AddStage(QuestStage stage)
    {
        stages.Add(stage);
    }

    public void AddBranch(string branchId, QuestBranch branch)
    {
        branches[branchId] = branch;
    }

    public void AdvanceStage()
    {
        if (currentStageIndex < stages.Count - 1)
        {
            currentStageIndex++;
            Debug.Log($"Quest Stage: {stages[currentStageIndex].stageName}");
        }
    }

    public bool IsComplete()
    {
        return currentStageIndex >= stages.Count - 1;
    }

    public QuestStage GetCurrentStage()
    {
        return currentStageIndex < stages.Count ? stages[currentStageIndex] : null;
    }
}

[System.Serializable]
public class QuestStage
{
    public string stageId;
    public string stageName;
    public string description;
    public bool isComplete;

    public QuestStage(string id, string name, string desc)
    {
        stageId = id;
        stageName = name;
        description = desc;
        isComplete = false;
    }
}

[System.Serializable]
public class QuestBranch
{
    public string branchId;
    public string branchName;
    public string description;
    public List<string> requiredObjectives;

    public QuestBranch(string id, string name, string desc, List<string> objectives)
    {
        branchId = id;
        branchName = name;
        description = desc;
        requiredObjectives = objectives ?? new List<string>();
    }
}

public enum QuestType
{
    Main,           // Main story quests
    Side,           // Optional side quests
    Timed,          // Time-limited quests
    Environmental,  // Weather/time-of-day dependent
    Dungeon,        // Dungeon-specific quests
    Secret,         // Hidden quests
    Reputation,     // Requires reputation with NPCs
    Daily           // Repeatable daily quests
}
