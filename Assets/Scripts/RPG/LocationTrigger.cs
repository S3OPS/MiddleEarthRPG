using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    public string locationName = "The Shire";
    public string questId = "";
    public string objectiveId = "";
    public bool hasVisited = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasVisited)
        {
            hasVisited = true;
            Debug.Log($"Discovered: {locationName}");

            if (RPGBootstrap.Instance != null && !string.IsNullOrEmpty(questId) && !string.IsNullOrEmpty(objectiveId))
            {
                RPGBootstrap.Instance.OnLocationDiscovered(locationName, questId, objectiveId);
            }
            
            // Effects
            if (EffectsManager.Instance != null)
            {
                EffectsManager.Instance.PlayQuestCompleteEffect(transform.position);
            }
            
            if (AchievementSystem.Instance != null)
            {
                AchievementSystem.Instance.OnLocationDiscovered();
            }
        }
    }
}
