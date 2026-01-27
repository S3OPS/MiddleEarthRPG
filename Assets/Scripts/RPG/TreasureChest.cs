using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public string itemName = "Ancient Sword";
    public ItemType itemType = ItemType.Weapon;
    public int itemValue = 100;
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
        var bootstrap = FindObjectOfType<RPGBootstrap>();
        if (bootstrap != null)
        {
            bootstrap.OnChestOpened(itemName, itemType, itemValue, goldAmount);
        }

        // Visual feedback
        GetComponent<Renderer>().material.color = new Color(0.8f, 0.8f, 0.3f);
        Debug.Log($"Opened chest! Found {itemName} and {goldAmount} gold!");
    }
}
