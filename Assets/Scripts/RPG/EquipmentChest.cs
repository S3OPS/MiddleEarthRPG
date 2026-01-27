using UnityEngine;

/// <summary>
/// Treasure chest that contains equipment items
/// </summary>
public class EquipmentChest : MonoBehaviour
{
    public Equipment equipment;
    public int goldAmount = 50;
    public bool isOpened = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpened)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        isOpened = true;
        if (RPGBootstrap.Instance != null)
        {
            RPGBootstrap.Instance.OnEquipmentChestOpened(equipment, goldAmount);
        }

        // Visual feedback
        GetComponent<Renderer>().material.color = new Color(0.8f, 0.8f, 0.3f);
        Debug.Log($"Opened chest! Found {equipment.name} and {goldAmount} gold!");
        
        // Effects
        if (EffectsManager.Instance != null)
        {
            EffectsManager.Instance.PlayTreasureEffect(transform.position);
        }
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayTreasureSound();
        }
        
        if (AchievementSystem.Instance != null)
        {
            AchievementSystem.Instance.OnChestOpened();
        }
    }
}
