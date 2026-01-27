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

            var bootstrap = FindObjectOfType<RPGBootstrap>();
            if (bootstrap != null && !string.IsNullOrEmpty(questId) && !string.IsNullOrEmpty(objectiveId))
            {
                bootstrap.OnLocationDiscovered(locationName, questId, objectiveId);
            }
        }
    }
}
