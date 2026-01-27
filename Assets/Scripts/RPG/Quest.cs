using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questId;
    public string questName;
    public string description;
    public List<QuestObjective> objectives;
    public int goldReward;
    public int experienceReward;
    public List<Item> itemRewards;
    public bool isCompleted;
    public bool isActive;

    public Quest(string id, string name, string desc, int goldReward = 50, int expReward = 100)
    {
        questId = id;
        questName = name;
        description = desc;
        objectives = new List<QuestObjective>();
        this.goldReward = goldReward;
        this.experienceReward = expReward;
        itemRewards = new List<Item>();
        isCompleted = false;
        isActive = false;
    }

    public void AddObjective(QuestObjective objective)
    {
        objectives.Add(objective);
    }

    public void UpdateProgress(string objectiveId, int progress)
    {
        var objective = objectives.Find(o => o.objectiveId == objectiveId);
        if (objective != null)
        {
            objective.currentProgress = Mathf.Min(objective.requiredProgress, objective.currentProgress + progress);
            objective.isCompleted = objective.currentProgress >= objective.requiredProgress;
        }
        CheckCompletion();
    }

    public void CheckCompletion()
    {
        if (!isCompleted && objectives.TrueForAll(o => o.isCompleted))
        {
            isCompleted = true;
        }
    }

    public float GetCompletionPercentage()
    {
        if (objectives.Count == 0) return 0f;
        int completedObjectives = objectives.FindAll(o => o.isCompleted).Count;
        return (float)completedObjectives / objectives.Count * 100f;
    }
}

[System.Serializable]
public class QuestObjective
{
    public string objectiveId;
    public string description;
    public ObjectiveType type;
    public int requiredProgress;
    public int currentProgress;
    public bool isCompleted;

    public QuestObjective(string id, string desc, ObjectiveType type, int required)
    {
        objectiveId = id;
        description = desc;
        this.type = type;
        requiredProgress = required;
        currentProgress = 0;
        isCompleted = false;
    }
}

public enum ObjectiveType
{
    CollectItem,
    DefeatEnemy,
    ExploreLocation,
    TalkToNPC
}
