using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public string description;
    public ItemType type;
    public int value;
    public int quantity;

    public Item(string name, string description, ItemType type, int value, int quantity = 1)
    {
        this.name = name;
        this.description = description;
        this.type = type;
        this.value = value;
        this.quantity = quantity;
    }
}

public enum ItemType
{
    QuestItem,
    Weapon,
    Armor,
    Potion,
    Treasure
}

public class InventorySystem
{
    private List<Item> _items = new List<Item>();
    public int gold = 100;

    public void AddItem(Item item)
    {
        var existing = _items.Find(i => i.name == item.name);
        if (existing != null)
        {
            existing.quantity += item.quantity;
        }
        else
        {
            _items.Add(item);
        }
    }

    public bool RemoveItem(string itemName, int quantity = 1)
    {
        var item = _items.Find(i => i.name == itemName);
        if (item != null && item.quantity >= quantity)
        {
            item.quantity -= quantity;
            if (item.quantity <= 0)
            {
                _items.Remove(item);
            }
            return true;
        }
        return false;
    }

    public bool HasItem(string itemName, int quantity = 1)
    {
        var item = _items.Find(i => i.name == itemName);
        return item != null && item.quantity >= quantity;
    }

    public List<Item> GetAllItems()
    {
        return new List<Item>(_items);
    }

    public void AddGold(int amount)
    {
        gold += amount;
        
        if (AchievementSystem.Instance != null)
        {
            AchievementSystem.Instance.OnGoldChanged(gold);
        }
    }

    public bool RemoveGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            return true;
        }
        return false;
    }
}
