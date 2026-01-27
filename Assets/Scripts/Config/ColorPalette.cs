using UnityEngine;

namespace MiddleEarth.Config
{
    /// <summary>
    /// Centralized color definitions for visual consistency.
    /// All hardcoded RGBA values are replaced with semantic names.
    /// </summary>
    public static class ColorPalette
    {
        // ========================================
        // NPC COLORS
        // ========================================
        
        /// <summary>Default NPC color (gray)</summary>
        public static readonly Color NPC_DEFAULT = new Color(0.5f, 0.5f, 0.5f, 1f);
        
        
        // ========================================
        // ENEMY COLORS
        // ========================================
        
        /// <summary>Orc Scout color (green)</summary>
        public static readonly Color ENEMY_ORC_SCOUT = new Color(0.2f, 0.8f, 0.2f, 1f);
        
        /// <summary>Dark Servant color (dark gray/black)</summary>
        public static readonly Color ENEMY_DARK_SERVANT = new Color(0.2f, 0.2f, 0.2f, 1f);
        
        /// <summary>Enemy damaged flash color (red tint)</summary>
        public static readonly Color ENEMY_DAMAGED_FLASH = new Color(1f, 0.5f, 0.5f, 1f);
        
        
        // ========================================
        // TREASURE COLORS
        // ========================================
        
        /// <summary>Unopened treasure chest color (brown)</summary>
        public static readonly Color CHEST_UNOPENED = new Color(0.6f, 0.4f, 0.2f, 1f);
        
        /// <summary>Opened treasure chest color (dark brown)</summary>
        public static readonly Color CHEST_OPENED = new Color(0.3f, 0.2f, 0.1f, 1f);
        
        
        // ========================================
        // LOCATION/TERRAIN COLORS
        // ========================================
        
        /// <summary>The Shire terrain color (green)</summary>
        public static readonly Color LOCATION_SHIRE = new Color(0.2f, 0.8f, 0.2f, 1f);
        
        /// <summary>Plains of Rohan terrain color (golden)</summary>
        public static readonly Color LOCATION_ROHAN = new Color(0.8f, 0.7f, 0.2f, 1f);
        
        /// <summary>Lands of Mordor terrain color (dark red)</summary>
        public static readonly Color LOCATION_MORDOR = new Color(0.3f, 0.1f, 0.1f, 1f);
        
        /// <summary>Default world terrain color (green)</summary>
        public static readonly Color TERRAIN_DEFAULT = new Color(0.2f, 0.8f, 0.2f, 1f);
        
        
        // ========================================
        // UI ELEMENT COLORS
        // ========================================
        
        /// <summary>Health bar color (green)</summary>
        public static readonly Color UI_HEALTH_BAR = new Color(0f, 1f, 0f, 1f);
        
        /// <summary>Stamina bar color (blue)</summary>
        public static readonly Color UI_STAMINA_BAR = new Color(0f, 0.5f, 1f, 1f);
        
        /// <summary>XP bar color (gold)</summary>
        public static readonly Color UI_XP_BAR = new Color(1f, 0.84f, 0f, 1f);
        
        /// <summary>UI text color (white)</summary>
        public static readonly Color UI_TEXT = new Color(1f, 1f, 1f, 1f);
        
        
        // ========================================
        // DAMAGE NUMBER COLORS
        // ========================================
        
        /// <summary>Normal damage number color (yellow)</summary>
        public static readonly Color DAMAGE_NORMAL = new Color(1f, 1f, 0f, 1f);
        
        /// <summary>Critical hit damage number color (orange)</summary>
        public static readonly Color DAMAGE_CRITICAL = new Color(1f, 0.5f, 0f, 1f);
        
        
        // ========================================
        // PARTICLE EFFECT COLORS
        // ========================================
        
        /// <summary>Hit effect particle color (red)</summary>
        public static readonly Color PARTICLE_HIT = new Color(1f, 0f, 0f, 1f);
        
        /// <summary>Special ability particle color (cyan)</summary>
        public static readonly Color PARTICLE_SPECIAL = new Color(0f, 1f, 1f, 1f);
        
        /// <summary>Level up particle color (gold)</summary>
        public static readonly Color PARTICLE_LEVELUP = new Color(1f, 0.84f, 0f, 1f);
        
        /// <summary>Treasure particle color (yellow)</summary>
        public static readonly Color PARTICLE_TREASURE = new Color(1f, 1f, 0f, 1f);
        
        /// <summary>Quest complete particle color (green)</summary>
        public static readonly Color PARTICLE_QUEST = new Color(0f, 1f, 0f, 1f);
        
        
        // ========================================
        // LIGHTING COLORS
        // ========================================
        
        /// <summary>Directional light color (white)</summary>
        public static readonly Color LIGHT_DIRECTIONAL = new Color(1f, 1f, 1f, 1f);
        
        /// <summary>Fog color (light gray)</summary>
        public static readonly Color FOG_COLOR = new Color(0.5f, 0.5f, 0.5f, 1f);
    }
}
