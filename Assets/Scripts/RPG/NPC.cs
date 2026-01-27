using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName = "Gandalf";
    public string greeting = "You shall not pass!";
    public string questId = "";
    public bool canGiveQuest = true;
    public bool hasSpokenTo = false;

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
        Debug.Log($"{npcName}: {greeting}");

        var bootstrap = FindObjectOfType<RPGBootstrap>();
        if (bootstrap != null)
        {
            bootstrap.OnNPCInteraction(npcName, questId);
        }
    }
}
