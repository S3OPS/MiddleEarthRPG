using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName = "Gandalf";
    public string npcId = "gandalf"; // ID for dialogue system
    public string greeting = "You shall not pass!";
    public string questId = "";
    public bool canGiveQuest = true;
    public bool hasSpokenTo = false;
    public bool hasDialogueTree = true; // Can use dialogue system

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractWithPlayer();
        }
    }

    public void InteractWithPlayer()
    {
        hasSpokenTo = true;
        
        // Use new dialogue system if available
        if (hasDialogueTree && DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.StartDialogue(npcId);
        }
        else
        {
            // Fallback to simple greeting
            Debug.Log($"{npcName}: {greeting}");
        }

        if (RPGBootstrap.Instance != null)
        {
            RPGBootstrap.Instance.OnNPCInteraction(npcName, questId);
        }
    }

    public int GetRelationshipLevel()
    {
        if (DialogueSystem.Instance != null)
        {
            return DialogueSystem.Instance.GetRelationshipLevel(npcId);
        }
        return 0;
    }

    public string GetRelationshipStatus()
    {
        if (DialogueSystem.Instance != null)
        {
            return DialogueSystem.Instance.GetRelationshipStatus(npcId);
        }
        return "Stranger";
    }
}
