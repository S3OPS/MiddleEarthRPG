using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Enhanced item class with equipment stats
/// </summary>
[System.Serializable]
public class Equipment : Item
{
    public int attackBonus;
    public int defenseBonus;
    public int healthBonus;
    public int staminaBonus;
    public EquipmentSlot slot;
    public ItemRarity rarity;
    
    public Equipment(string name, string description, ItemType type, int value, 
                     EquipmentSlot slot, ItemRarity rarity = ItemRarity.Common,
                     int attackBonus = 0, int defenseBonus = 0, int healthBonus = 0, int staminaBonus = 0)
        : base(name, description, type, value, 1)
    {
        this.slot = slot;
        this.rarity = rarity;
        this.attackBonus = attackBonus;
        this.defenseBonus = defenseBonus;
        this.healthBonus = healthBonus;
        this.staminaBonus = staminaBonus;
    }
}

public enum EquipmentSlot
{
    Weapon,
    Armor,
    Accessory,
    None
}

public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

/// <summary>
/// Equipment system for managing equipped items and stat bonuses
/// </summary>
public class EquipmentSystem
{
    private Dictionary<EquipmentSlot, Equipment> _equippedItems = new Dictionary<EquipmentSlot, Equipment>();
    private CharacterStats _stats;
    
    public Equipment WeaponSlot => _equippedItems.ContainsKey(EquipmentSlot.Weapon) ? _equippedItems[EquipmentSlot.Weapon] : null;
    public Equipment ArmorSlot => _equippedItems.ContainsKey(EquipmentSlot.Armor) ? _equippedItems[EquipmentSlot.Armor] : null;
    public Equipment AccessorySlot => _equippedItems.ContainsKey(EquipmentSlot.Accessory) ? _equippedItems[EquipmentSlot.Accessory] : null;
    
    public void Initialize(CharacterStats stats)
    {
        _stats = stats;
    }
    
    public bool EquipItem(Equipment equipment)
    {
        if (equipment == null || equipment.slot == EquipmentSlot.None)
            return false;
        
        // Unequip current item in slot if exists
        if (_equippedItems.ContainsKey(equipment.slot))
        {
            UnequipItem(equipment.slot);
        }
        
        _equippedItems[equipment.slot] = equipment;
        ApplyEquipmentStats(equipment, true);
        
        Debug.Log($"Equipped {equipment.name} in {equipment.slot} slot!");
        return true;
    }
    
    public Equipment UnequipItem(EquipmentSlot slot)
    {
        if (!_equippedItems.ContainsKey(slot))
            return null;
        
        Equipment equipment = _equippedItems[slot];
        _equippedItems.Remove(slot);
        ApplyEquipmentStats(equipment, false);
        
        Debug.Log($"Unequipped {equipment.name} from {slot} slot!");
        return equipment;
    }
    
    private void ApplyEquipmentStats(Equipment equipment, bool isEquipping)
    {
        if (_stats == null) return;
        
        int multiplier = isEquipping ? 1 : -1;
        
        _stats.strength += equipment.attackBonus * multiplier;
        _stats.maxHealth += equipment.healthBonus * multiplier;
        _stats.maxStamina += equipment.staminaBonus * multiplier;
        
        // Heal proportionally when equipping health items
        if (isEquipping && equipment.healthBonus > 0)
        {
            _stats.currentHealth += equipment.healthBonus;
        }
        
        if (isEquipping && equipment.staminaBonus > 0)
        {
            _stats.currentStamina += equipment.staminaBonus;
        }
    }
    
    public int GetTotalAttackBonus()
    {
        int total = 0;
        foreach (var equipment in _equippedItems.Values)
        {
            total += equipment.attackBonus;
        }
        return total;
    }
    
    public int GetTotalDefenseBonus()
    {
        int total = 0;
        foreach (var equipment in _equippedItems.Values)
        {
            total += equipment.defenseBonus;
        }
        return total;
    }
    
    public Equipment GetEquippedItem(EquipmentSlot slot)
    {
        return _equippedItems.ContainsKey(slot) ? _equippedItems[slot] : null;
    }
    
    public Color GetRarityColor(ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemRarity.Common: return Color.white;
            case ItemRarity.Uncommon: return Color.green;
            case ItemRarity.Rare: return Color.blue;
            case ItemRarity.Epic: return new Color(0.6f, 0.2f, 0.8f); // Purple
            case ItemRarity.Legendary: return new Color(1f, 0.5f, 0f); // Orange
            default: return Color.white;
        }
    }
}

/// <summary>
/// Factory for creating legendary equipment
/// </summary>
public static class EquipmentFactory
{
    public static Equipment CreateAnduril()
    {
        return new Equipment(
            "Andúril",
            "The Flame of the West, reforged from the shards of Narsil",
            ItemType.Weapon,
            1000,
            EquipmentSlot.Weapon,
            ItemRarity.Legendary,
            attackBonus: 25,
            healthBonus: 20
        );
    }
    
    public static Equipment CreateMithrilCoat()
    {
        return new Equipment(
            "Mithril Coat",
            "A shirt of mail made from the precious metal mithril",
            ItemType.Armor,
            1500,
            EquipmentSlot.Armor,
            ItemRarity.Legendary,
            defenseBonus: 30,
            healthBonus: 50
        );
    }
    
    public static Equipment CreateElvenCloak()
    {
        return new Equipment(
            "Elven Cloak",
            "A gift from the Elves of Lothlórien",
            ItemType.Armor,
            500,
            EquipmentSlot.Armor,
            ItemRarity.Epic,
            defenseBonus: 15,
            staminaBonus: 20
        );
    }
    
    public static Equipment CreateOrcsword()
    {
        return new Equipment(
            "Orc Sword",
            "A crude but effective weapon",
            ItemType.Weapon,
            50,
            EquipmentSlot.Weapon,
            ItemRarity.Common,
            attackBonus: 5
        );
    }
    
    public static Equipment CreateElvenBlade()
    {
        return new Equipment(
            "Elven Blade",
            "A masterfully crafted sword of the Elves",
            ItemType.Weapon,
            300,
            EquipmentSlot.Weapon,
            ItemRarity.Rare,
            attackBonus: 15,
            staminaBonus: 10
        );
    }
    
    public static Equipment CreateRingOfPower()
    {
        return new Equipment(
            "Ring of Power",
            "One Ring to rule them all",
            ItemType.Treasure,
            5000,
            EquipmentSlot.Accessory,
            ItemRarity.Legendary,
            attackBonus: 20,
            defenseBonus: 20,
            healthBonus: 100,
            staminaBonus: 50
        );
    }
}
