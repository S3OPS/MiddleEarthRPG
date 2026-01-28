using UnityEngine;
using System.Text;

/// <summary>
/// Character Sheet UI - Detailed stats and equipment display
/// Part of Phase 7 (v2.6) UI/UX Polish
/// </summary>
public class CharacterSheetUI : MonoBehaviour
{
    public static CharacterSheetUI Instance { get; private set; }
    
    private bool _showSheet = false;
    private CharacterStats _characterStats;
    private EquipmentSystem _equipmentSystem;
    private InventorySystem _inventorySystem;
    private StringBuilder _textBuilder = new StringBuilder(500);
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Find systems
        _characterStats = FindObjectOfType<CharacterStats>();
        _equipmentSystem = FindObjectOfType<EquipmentSystem>();
        _inventorySystem = FindObjectOfType<InventorySystem>();
    }

    private void Update()
    {
        // Toggle character sheet with C key
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleSheet();
        }
    }

    public void ToggleSheet()
    {
        _showSheet = !_showSheet;
    }

    public void ShowSheet()
    {
        _showSheet = true;
    }

    public void HideSheet()
    {
        _showSheet = false;
    }

    private void OnGUI()
    {
        if (!_showSheet) return;

        DrawCharacterSheet();
    }

    private void DrawCharacterSheet()
    {
        float width = 600f;
        float height = 500f;
        float x = (Screen.width - width) / 2f;
        float y = (Screen.height - height) / 2f;

        // Main window
        GUI.Box(new Rect(x, y, width, height), "");
        
        // Title
        GUI.Label(new Rect(x + 10, y + 10, width - 20, 30),
                  "<b><size=20>⚔️ Character Sheet</size></b>",
                  GetCenteredStyle());

        // Two column layout
        float columnWidth = (width - 30) / 2f;
        
        // Left column - Stats
        DrawStatsColumn(x + 10, y + 50, columnWidth, height - 100);
        
        // Right column - Equipment
        DrawEquipmentColumn(x + 20 + columnWidth, y + 50, columnWidth, height - 100);

        // Close button
        if (GUI.Button(new Rect(x + 10, y + height - 40, 150, 30), "Close [C]"))
        {
            HideSheet();
        }

        // Character name
        if (_characterStats != null)
        {
            GUI.Label(new Rect(x + width - 200, y + height - 40, 190, 30),
                      $"Level {_characterStats.level} Hero",
                      GetRightAlignedStyle());
        }
    }

    private void DrawStatsColumn(float x, float y, float width, float height)
    {
        GUI.Box(new Rect(x, y, width, height), "");
        
        // Header
        GUI.Label(new Rect(x + 10, y + 5, width - 20, 25),
                  "<b>Character Stats</b>",
                  GetRichTextStyle());

        if (_characterStats == null)
        {
            GUI.Label(new Rect(x + 10, y + 35, width - 20, 40),
                     "<i>Character stats not available</i>",
                     GetSmallStyle());
            return;
        }

        float currentY = y + 35;
        float lineHeight = 25f;

        // Level and Experience
        DrawStatLine(x + 10, currentY, width - 20, "Level", _characterStats.level.ToString());
        currentY += lineHeight;
        
        DrawStatLine(x + 10, currentY, width - 20, "Experience", 
                     $"{_characterStats.experience}/{_characterStats.experienceToNextLevel}");
        currentY += lineHeight;

        // Health
        DrawStatLine(x + 10, currentY, width - 20, "Health", 
                     $"{_characterStats.currentHealth:F0}/{_characterStats.maxHealth:F0}");
        currentY += lineHeight;

        // Stamina
        DrawStatLine(x + 10, currentY, width - 20, "Stamina", 
                     $"{_characterStats.currentStamina:F0}/{_characterStats.maxStamina:F0}");
        currentY += lineHeight;

        currentY += 10; // Spacer

        // Primary Stats
        GUI.Label(new Rect(x + 10, currentY, width - 20, 20),
                  "<b>Primary Stats</b>",
                  GetRichTextStyle());
        currentY += 25;

        DrawStatLine(x + 10, currentY, width - 20, "Strength", _characterStats.strength.ToString());
        currentY += lineHeight;
        
        DrawStatLine(x + 10, currentY, width - 20, "Wisdom", _characterStats.wisdom.ToString());
        currentY += lineHeight;
        
        DrawStatLine(x + 10, currentY, width - 20, "Agility", _characterStats.agility.ToString());
        currentY += lineHeight;

        currentY += 10; // Spacer

        // Derived Stats
        GUI.Label(new Rect(x + 10, currentY, width - 20, 20),
                  "<b>Combat Stats</b>",
                  GetRichTextStyle());
        currentY += 25;

        int totalAttack = CalculateTotalAttack();
        DrawStatLine(x + 10, currentY, width - 20, "Attack Power", totalAttack.ToString());
        currentY += lineHeight;

        int totalDefense = CalculateTotalDefense();
        DrawStatLine(x + 10, currentY, width - 20, "Defense", totalDefense.ToString());
        currentY += lineHeight;

        // Gold
        if (_inventorySystem != null)
        {
            currentY += 10;
            DrawStatLine(x + 10, currentY, width - 20, "Gold", _inventorySystem.GetGold().ToString());
        }
    }

    private void DrawEquipmentColumn(float x, float y, float width, float height)
    {
        GUI.Box(new Rect(x, y, width, height), "");
        
        // Header
        GUI.Label(new Rect(x + 10, y + 5, width - 20, 25),
                  "<b>Equipment</b>",
                  GetRichTextStyle());

        if (_equipmentSystem == null)
        {
            GUI.Label(new Rect(x + 10, y + 35, width - 20, 40),
                     "<i>Equipment system not available</i>",
                     GetSmallStyle());
            return;
        }

        float currentY = y + 35;
        float slotHeight = 60f;

        // Weapon slot
        DrawEquipmentSlot(x + 10, currentY, width - 20, slotHeight, 
                         "⚔️ Weapon", _equipmentSystem.equippedWeapon);
        currentY += slotHeight + 10;

        // Armor slot
        DrawEquipmentSlot(x + 10, currentY, width - 20, slotHeight, 
                         "🛡️ Armor", _equipmentSystem.equippedArmor);
        currentY += slotHeight + 10;

        // Accessory slot
        DrawEquipmentSlot(x + 10, currentY, width - 20, slotHeight, 
                         "💍 Accessory", _equipmentSystem.equippedAccessory);
        currentY += slotHeight + 10;

        // Equipment stats summary
        GUI.Label(new Rect(x + 10, currentY, width - 20, 20),
                  "<b>Equipment Bonuses</b>",
                  GetRichTextStyle());
        currentY += 25;

        int weaponBonus = _equipmentSystem.GetTotalAttackBonus();
        int armorBonus = _equipmentSystem.GetTotalDefenseBonus();
        
        _textBuilder.Clear();
        _textBuilder.Append("Attack: +");
        _textBuilder.Append(weaponBonus);
        _textBuilder.Append(" | Defense: +");
        _textBuilder.Append(armorBonus);
        
        GUI.Label(new Rect(x + 10, currentY, width - 20, 20),
                  _textBuilder.ToString(),
                  GetSmallStyle());
    }

    private void DrawEquipmentSlot(float x, float y, float width, float height, string slotName, Item equippedItem)
    {
        GUI.Box(new Rect(x, y, width, height), "");
        
        // Slot name
        GUI.Label(new Rect(x + 5, y + 5, width - 10, 20),
                  $"<b>{slotName}</b>",
                  GetRichTextStyle());

        if (equippedItem != null)
        {
            // Item name with rarity color
            string colorCode = GetRarityColor(equippedItem.rarity);
            GUI.Label(new Rect(x + 5, y + 25, width - 10, 20),
                      $"<color={colorCode}>{equippedItem.name}</color>",
                      GetRichTextStyle());

            // Item stats
            _textBuilder.Clear();
            if (equippedItem.attackBonus > 0)
            {
                _textBuilder.Append("ATK +");
                _textBuilder.Append(equippedItem.attackBonus);
            }
            if (equippedItem.defenseBonus > 0)
            {
                if (_textBuilder.Length > 0) _textBuilder.Append(" | ");
                _textBuilder.Append("DEF +");
                _textBuilder.Append(equippedItem.defenseBonus);
            }
            
            GUI.Label(new Rect(x + 5, y + 45, width - 10, 15),
                      _textBuilder.ToString(),
                      GetSmallStyle());
        }
        else
        {
            GUI.Label(new Rect(x + 5, y + 25, width - 10, 20),
                      "<i>Empty</i>",
                      GetSmallStyle());
        }
    }

    private void DrawStatLine(float x, float y, float width, string label, string value)
    {
        GUI.Label(new Rect(x, y, width * 0.6f, 20),
                  label + ":",
                  GetSmallStyle());
        
        GUI.Label(new Rect(x + width * 0.6f, y, width * 0.4f, 20),
                  value,
                  GetBoldSmallStyle());
    }

    private int CalculateTotalAttack()
    {
        int baseAttack = _characterStats != null ? _characterStats.strength * 2 : 0;
        int equipmentBonus = _equipmentSystem != null ? _equipmentSystem.GetTotalAttackBonus() : 0;
        return baseAttack + equipmentBonus;
    }

    private int CalculateTotalDefense()
    {
        int baseDefense = _characterStats != null ? (_characterStats.strength + _characterStats.agility) : 0;
        int equipmentBonus = _equipmentSystem != null ? _equipmentSystem.GetTotalDefenseBonus() : 0;
        return baseDefense + equipmentBonus;
    }

    private string GetRarityColor(ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemRarity.Common: return "#FFFFFF";
            case ItemRarity.Uncommon: return "#00FF00";
            case ItemRarity.Rare: return "#0088FF";
            case ItemRarity.Epic: return "#AA00FF";
            case ItemRarity.Legendary: return "#FF8800";
            default: return "#FFFFFF";
        }
    }

    private GUIStyle GetCenteredStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        style.alignment = TextAnchor.MiddleCenter;
        return style;
    }

    private GUIStyle GetRightAlignedStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        style.alignment = TextAnchor.MiddleRight;
        style.fontSize = 10;
        return style;
    }

    private GUIStyle GetRichTextStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        return style;
    }

    private GUIStyle GetSmallStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        style.fontSize = 10;
        return style;
    }

    private GUIStyle GetBoldSmallStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        style.fontSize = 10;
        style.fontStyle = FontStyle.Bold;
        return style;
    }
}
