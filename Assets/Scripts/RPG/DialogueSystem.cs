using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages branching dialogue conversations with NPCs.
/// Supports dialogue choices, NPC relationship tracking, and quest integration.
/// </summary>
public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance { get; private set; }
    
    private Dictionary<string, NPCRelationship> _npcRelationships = new Dictionary<string, NPCRelationship>();
    private Dictionary<string, DialogueTree> _dialogueTrees = new Dictionary<string, DialogueTree>();
    private DialogueTree _currentDialogue;
    private Action<int> _onChoiceSelected;

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
        InitializeDialogueTrees();
    }

    private void InitializeDialogueTrees()
    {
        // Gandalf's dialogue tree
        var gandalfTree = new DialogueTree("gandalf", "Gandalf the Grey");
        gandalfTree.AddNode(new DialogueNode(
            "greeting",
            "Ah, a wanderer! The world grows darker by the day. What brings you to these lands?",
            new List<DialogueChoice>
            {
                new DialogueChoice("I seek adventure and glory.", "adventure_path", 5),
                new DialogueChoice("I'm looking for work. Any quests?", "quest_path", 0),
                new DialogueChoice("Just passing through.", "farewell", -5)
            }
        ));
        gandalfTree.AddNode(new DialogueNode(
            "adventure_path",
            "Excellent! A brave heart is what Middle-earth needs. I have a task that might interest you...",
            new List<DialogueChoice>
            {
                new DialogueChoice("Tell me more.", "quest_offer", 10),
                new DialogueChoice("Maybe another time.", "farewell", 0)
            }
        ));
        gandalfTree.AddNode(new DialogueNode(
            "quest_path",
            "Always to the point, I see. Very well, there ARE troubles brewing in the east...",
            new List<DialogueChoice>
            {
                new DialogueChoice("I'll help. What needs doing?", "quest_offer", 5),
                new DialogueChoice("How much does it pay?", "mercenary", -5)
            }
        ));
        gandalfTree.AddNode(new DialogueNode(
            "mercenary",
            "Gold? Is that all that motivates you? *sighs* Very well, the reward will be substantial.",
            new List<DialogueChoice> { new DialogueChoice("Fine, I'll do it.", "quest_offer", 0) }
        ));
        gandalfTree.AddNode(new DialogueNode(
            "quest_offer",
            "Dark forces gather in the dungeons to the north. Investigate and report back what you find.",
            new List<DialogueChoice> { new DialogueChoice("I'll take care of it.", "quest_accepted", 5) }
        ));
        gandalfTree.AddNode(new DialogueNode(
            "quest_accepted",
            "Good luck, brave adventurer. May the light guide your path.",
            new List<DialogueChoice>()
        ));
        gandalfTree.AddNode(new DialogueNode(
            "farewell",
            "Safe travels, friend. The road goes ever on and on.",
            new List<DialogueChoice>()
        ));
        _dialogueTrees["gandalf"] = gandalfTree;

        // Legolas's dialogue tree
        var legolasTree = new DialogueTree("legolas", "Legolas");
        legolasTree.AddNode(new DialogueNode(
            "greeting",
            "Greetings, traveler. I am Legolas of the Woodland Realm. These lands grow perilous.",
            new List<DialogueChoice>
            {
                new DialogueChoice("What dangers do you speak of?", "danger_info", 5),
                new DialogueChoice("Can you teach me archery?", "training", 10),
                new DialogueChoice("I must be going.", "farewell", 0)
            }
        ));
        legolasTree.AddNode(new DialogueNode(
            "danger_info",
            "Orcs multiply in the shadows. Their numbers grow with each passing day. We must remain vigilant.",
            new List<DialogueChoice>
            {
                new DialogueChoice("I'll help fight them.", "quest_offer", 10),
                new DialogueChoice("That's concerning. Farewell.", "farewell", 0)
            }
        ));
        legolasTree.AddNode(new DialogueNode(
            "training",
            "Archery takes decades to master, but I can share some wisdom with a worthy student.",
            new List<DialogueChoice> { new DialogueChoice("I'm ready to learn.", "quest_offer", 5) }
        ));
        legolasTree.AddNode(new DialogueNode(
            "quest_offer",
            "Prove your skill by defeating 10 enemies. Return when you have done so.",
            new List<DialogueChoice> { new DialogueChoice("Consider it done.", "quest_accepted", 5) }
        ));
        legolasTree.AddNode(new DialogueNode(
            "quest_accepted",
            "May your aim be true and your blade swift.",
            new List<DialogueChoice>()
        ));
        legolasTree.AddNode(new DialogueNode(
            "farewell",
            "May the stars light your path.",
            new List<DialogueChoice>()
        ));
        _dialogueTrees["legolas"] = legolasTree;
    }

    public void StartDialogue(string npcId)
    {
        if (_dialogueTrees.ContainsKey(npcId))
        {
            _currentDialogue = _dialogueTrees[npcId];
            if (!_npcRelationships.ContainsKey(npcId))
            {
                _npcRelationships[npcId] = new NPCRelationship(npcId);
            }
            ShowDialogueNode("greeting");
        }
        else
        {
            Debug.LogWarning($"No dialogue tree found for NPC: {npcId}");
        }
    }

    private void ShowDialogueNode(string nodeId)
    {
        var node = _currentDialogue.GetNode(nodeId);
        if (node != null)
        {
            Debug.Log($"[{_currentDialogue.npcName}]: {node.text}");
            
            // Show in HUD if available
            if (ContentHUD.Instance != null)
            {
                ContentHUD.Instance.ShowDialogue(node, _currentDialogue.npcName);
            }
            
            if (node.choices.Count > 0)
            {
                Debug.Log("Choices:");
                for (int i = 0; i < node.choices.Count; i++)
                {
                    Debug.Log($"{i + 1}. {node.choices[i].text}");
                }
            }
            else
            {
                // End of conversation
                EndDialogue();
            }
        }
    }

    public void SelectChoice(int choiceIndex)
    {
        var node = _currentDialogue.GetNode(_currentDialogue.currentNodeId);
        if (node != null && choiceIndex >= 0 && choiceIndex < node.choices.Count)
        {
            var choice = node.choices[choiceIndex];
            
            // Update relationship
            if (_npcRelationships.ContainsKey(_currentDialogue.npcId))
            {
                _npcRelationships[_currentDialogue.npcId].AdjustRelationship(choice.relationshipDelta);
            }
            
            // Move to next node
            _currentDialogue.currentNodeId = choice.nextNodeId;
            ShowDialogueNode(choice.nextNodeId);
            
            // Trigger quest events if needed
            if (choice.nextNodeId == "quest_accepted")
            {
                TriggerQuestFromDialogue(_currentDialogue.npcId);
            }
        }
    }

    private void TriggerQuestFromDialogue(string npcId)
    {
        // Integrate with quest system
        if (npcId == "gandalf")
        {
            Debug.Log("Quest activated: Investigate the Northern Dungeons");
        }
        else if (npcId == "legolas")
        {
            Debug.Log("Quest activated: Prove Your Combat Skill");
        }
    }

    private void EndDialogue()
    {
        Debug.Log("[Conversation ended]");
        
        // Hide HUD dialogue box
        if (ContentHUD.Instance != null)
        {
            ContentHUD.Instance.HideDialogue();
        }
        
        _currentDialogue = null;
    }

    public int GetRelationshipLevel(string npcId)
    {
        return _npcRelationships.ContainsKey(npcId) ? _npcRelationships[npcId].relationshipPoints : 0;
    }

    public string GetRelationshipStatus(string npcId)
    {
        if (!_npcRelationships.ContainsKey(npcId)) return "Stranger";
        
        int points = _npcRelationships[npcId].relationshipPoints;
        if (points >= 50) return "Trusted Friend";
        if (points >= 25) return "Friend";
        if (points >= 0) return "Acquaintance";
        if (points >= -25) return "Distrustful";
        return "Hostile";
    }
    
    private void OnDestroy()
    {
        // Clear collections to prevent memory leaks
        _npcRelationships.Clear();
        _dialogueTrees.Clear();
        _currentDialogue = null;
        _onChoiceSelected = null;
        
        if (Instance == this)
        {
            Instance = null;
        }
    }
}

[System.Serializable]
public class DialogueTree
{
    public string npcId;
    public string npcName;
    public string currentNodeId = "greeting";
    private Dictionary<string, DialogueNode> _nodes = new Dictionary<string, DialogueNode>();

    public DialogueTree(string id, string name)
    {
        npcId = id;
        npcName = name;
    }

    public void AddNode(DialogueNode node)
    {
        _nodes[node.nodeId] = node;
    }

    public DialogueNode GetNode(string nodeId)
    {
        return _nodes.ContainsKey(nodeId) ? _nodes[nodeId] : null;
    }
}

[System.Serializable]
public class DialogueNode
{
    public string nodeId;
    public string text;
    public List<DialogueChoice> choices;

    public DialogueNode(string id, string dialogueText, List<DialogueChoice> dialogueChoices)
    {
        nodeId = id;
        text = dialogueText;
        choices = dialogueChoices ?? new List<DialogueChoice>();
    }
}

[System.Serializable]
public class DialogueChoice
{
    public string text;
    public string nextNodeId;
    public int relationshipDelta;

    public DialogueChoice(string choiceText, string nextNode, int relationshipChange = 0)
    {
        text = choiceText;
        nextNodeId = nextNode;
        relationshipDelta = relationshipChange;
    }
}

[System.Serializable]
public class NPCRelationship
{
    public string npcId;
    public int relationshipPoints;
    public int conversationCount;

    public NPCRelationship(string id)
    {
        npcId = id;
        relationshipPoints = 0;
        conversationCount = 0;
    }

    public void AdjustRelationship(int delta)
    {
        relationshipPoints = Mathf.Clamp(relationshipPoints + delta, -100, 100);
        conversationCount++;
    }
}
