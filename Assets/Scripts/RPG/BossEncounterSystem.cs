using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages boss encounters with unique mechanics, phases, and special abilities.
/// Bosses have increased health, damage, and special attack patterns.
/// </summary>
public class BossEncounterSystem : MonoBehaviour
{
    public static BossEncounterSystem Instance { get; private set; }
    
    private Dictionary<string, BossData> _bossDefinitions = new Dictionary<string, BossData>();
    private BossEncounter _currentBoss;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeBosses();
    }

    private void InitializeBosses()
    {
        // Boss 1: Cave Troll (Cave Dungeon Boss)
        _bossDefinitions["cave_troll"] = new BossData(
            "cave_troll",
            "Ancient Cave Troll",
            500, // Health
            50,  // Damage
            new List<BossAbility>
            {
                new BossAbility("Ground Slam", 30f, 100, "AOE attack that damages all nearby enemies"),
                new BossAbility("Regeneration", 20f, 0, "Heals 50 HP over time")
            },
            new List<BossPhase>
            {
                new BossPhase(100, 50, "Enraged", "Boss enters rage mode at 50% health")
            },
            DungeonTheme.Cave
        );

        // Boss 2: Lich King (Crypt Dungeon Boss)
        _bossDefinitions["lich_king"] = new BossData(
            "lich_king",
            "The Lich King",
            600,
            45,
            new List<BossAbility>
            {
                new BossAbility("Death Bolt", 15f, 80, "Powerful dark magic projectile"),
                new BossAbility("Summon Undead", 30f, 0, "Summons skeleton minions"),
                new BossAbility("Life Drain", 25f, 40, "Drains life from player")
            },
            new List<BossPhase>
            {
                new BossPhase(100, 30, "Desperate", "Summons reinforcements at 30% health")
            },
            DungeonTheme.Crypt
        );

        // Boss 3: Orc Warlord (Fortress Dungeon Boss)
        _bossDefinitions["orc_warlord"] = new BossData(
            "orc_warlord",
            "Orc Warlord",
            700,
            60,
            new List<BossAbility>
            {
                new BossAbility("Battle Cry", 40f, 0, "Increases damage by 50% for 10 seconds"),
                new BossAbility("Whirlwind", 20f, 75, "Spinning attack hitting multiple times"),
                new BossAbility("Shield Bash", 10f, 45, "Stuns player briefly")
            },
            new List<BossPhase>
            {
                new BossPhase(100, 25, "Last Stand", "Enters berserk mode at 25% health")
            },
            DungeonTheme.Fortress
        );

        // Boss 4: Dragon Hatchling (Mine Dungeon Boss)
        _bossDefinitions["dragon_hatchling"] = new BossData(
            "dragon_hatchling",
            "Dragon Hatchling",
            800,
            55,
            new List<BossAbility>
            {
                new BossAbility("Fire Breath", 18f, 90, "Cone of fire damage"),
                new BossAbility("Wing Buffet", 25f, 50, "Knocks player back"),
                new BossAbility("Tail Swipe", 12f, 60, "Sweeping tail attack")
            },
            new List<BossPhase>
            {
                new BossPhase(100, 50, "Airborne", "Takes flight, changes attack pattern"),
                new BossPhase(50, 20, "Enraged", "Becomes more aggressive")
            },
            DungeonTheme.Mine
        );

        // Boss 5: Dark Sorcerer (Tower Dungeon Boss)
        _bossDefinitions["dark_sorcerer"] = new BossData(
            "dark_sorcerer",
            "Dark Sorcerer",
            550,
            70,
            new List<BossAbility>
            {
                new BossAbility("Shadow Bolt", 8f, 65, "Fast dark magic projectile"),
                new BossAbility("Teleport", 20f, 0, "Teleports to random location"),
                new BossAbility("Curse", 30f, 40, "Reduces player damage for 15 seconds"),
                new BossAbility("Meteor Storm", 45f, 120, "Ultimate ability - rains fire from sky")
            },
            new List<BossPhase>
            {
                new BossPhase(100, 50, "Shielded", "Gains magical shield"),
                new BossPhase(50, 25, "Desperate", "Casts more frequently")
            },
            DungeonTheme.Tower
        );

        // World Boss: Balrog
        _bossDefinitions["balrog"] = new BossData(
            "balrog",
            "Balrog of Morgoth",
            1000,
            80,
            new List<BossAbility>
            {
                new BossAbility("Flame Whip", 10f, 85, "Whip of fire attack"),
                new BossAbility("Inferno", 35f, 150, "Massive area fire explosion"),
                new BossAbility("Shadow Cloak", 40f, 0, "Becomes harder to hit"),
                new BossAbility("Summon Flames", 25f, 60, "Creates fire zones on ground")
            },
            new List<BossPhase>
            {
                new BossPhase(100, 66, "Awakened", "Full power unleashed"),
                new BossPhase(66, 33, "Furious", "Faster attacks, more aggressive"),
                new BossPhase(33, 0, "Death Throes", "Desperate, uses all abilities")
            },
            DungeonTheme.Cave // Special world boss
        );
    }

    public BossEncounter SpawnBoss(string bossId, Vector3 position)
    {
        if (!_bossDefinitions.ContainsKey(bossId))
        {
            Debug.LogError($"Boss definition not found: {bossId}");
            return null;
        }

        var bossData = _bossDefinitions[bossId];
        var bossObject = new GameObject($"Boss_{bossData.bossName}");
        bossObject.transform.position = position;
        
        var boss = bossObject.AddComponent<BossEncounter>();
        boss.Initialize(bossData);
        
        _currentBoss = boss;
        Debug.Log($"⚔️ BOSS ENCOUNTER: {bossData.bossName}");
        
        return boss;
    }

    public BossData GetBossData(string bossId)
    {
        return _bossDefinitions.ContainsKey(bossId) ? _bossDefinitions[bossId] : null;
    }

    public List<string> GetBossesByTheme(DungeonTheme theme)
    {
        var bossList = new List<string>();
        foreach (var kvp in _bossDefinitions)
        {
            if (kvp.Value.dungeonTheme == theme)
            {
                bossList.Add(kvp.Key);
            }
        }
        return bossList;
    }

    public BossEncounter GetCurrentBoss()
    {
        return _currentBoss;
    }

    public void ClearCurrentBoss()
    {
        _currentBoss = null;
    }
}

[System.Serializable]
public class BossData
{
    public string bossId;
    public string bossName;
    public int maxHealth;
    public int baseDamage;
    public List<BossAbility> abilities;
    public List<BossPhase> phases;
    public DungeonTheme dungeonTheme;

    public BossData(string id, string name, int health, int damage, 
                    List<BossAbility> bossAbilities, List<BossPhase> bossPhases, 
                    DungeonTheme theme)
    {
        bossId = id;
        bossName = name;
        maxHealth = health;
        baseDamage = damage;
        abilities = bossAbilities ?? new List<BossAbility>();
        phases = bossPhases ?? new List<BossPhase>();
        dungeonTheme = theme;
    }
}

[System.Serializable]
public class BossAbility
{
    public string abilityName;
    public float cooldown;
    public int damage;
    public string description;
    public float lastUsedTime;

    public BossAbility(string name, float cd, int dmg, string desc)
    {
        abilityName = name;
        cooldown = cd;
        damage = dmg;
        description = desc;
        lastUsedTime = -cooldown; // Can use immediately
    }

    public bool IsReady()
    {
        return Time.time >= lastUsedTime + cooldown;
    }

    public void Use()
    {
        lastUsedTime = Time.time;
        Debug.Log($"💥 Boss uses: {abilityName} - {description}");
    }
}

[System.Serializable]
public class BossPhase
{
    public int healthThresholdHigh;
    public int healthThresholdLow;
    public string phaseName;
    public string description;
    public bool isActive;

    public BossPhase(int highThreshold, int lowThreshold, string name, string desc)
    {
        healthThresholdHigh = highThreshold;
        healthThresholdLow = lowThreshold;
        phaseName = name;
        description = desc;
        isActive = false;
    }
}

/// <summary>
/// Component attached to boss enemies with special behavior and abilities.
/// </summary>
public class BossEncounter : MonoBehaviour
{
    private BossData _bossData;
    private int _currentHealth;
    private BossPhase _currentPhase;
    private float _abilityTimer;
    
    public void Initialize(BossData data)
    {
        _bossData = data;
        _currentHealth = data.maxHealth;
        _abilityTimer = 0f;
        
        // Activate first phase if exists
        if (_bossData.phases.Count > 0)
        {
            _currentPhase = _bossData.phases[0];
            CheckPhaseTransition();
        }
    }

    private void Update()
    {
        if (_bossData == null) return;
        
        _abilityTimer += Time.deltaTime;
        
        // Use abilities periodically
        if (_abilityTimer >= 5f) // Check every 5 seconds
        {
            TryUseAbility();
            _abilityTimer = 0f;
        }
    }

    private void TryUseAbility()
    {
        // Try to use a random ready ability
        var readyAbilities = _bossData.abilities.FindAll(a => a.IsReady());
        if (readyAbilities.Count > 0)
        {
            var ability = readyAbilities[Random.Range(0, readyAbilities.Count)];
            ability.Use();
            
            // Apply ability effects here (would integrate with combat system)
            // For now, just log it
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Max(0, _currentHealth);
        
        float healthPercent = (_currentHealth / (float)_bossData.maxHealth) * 100f;
        
        Debug.Log($"Boss Health: {_currentHealth}/{_bossData.maxHealth} ({healthPercent:F0}%)");
        
        CheckPhaseTransition();
        
        if (_currentHealth <= 0)
        {
            OnBossDefeated();
        }
    }

    private void CheckPhaseTransition()
    {
        float healthPercent = (_currentHealth / (float)_bossData.maxHealth) * 100f;
        
        foreach (var phase in _bossData.phases)
        {
            if (!phase.isActive && 
                healthPercent <= phase.healthThresholdHigh && 
                healthPercent > phase.healthThresholdLow)
            {
                phase.isActive = true;
                _currentPhase = phase;
                Debug.Log($"⚡ PHASE TRANSITION: {phase.phaseName} - {phase.description}");
                
                // Trigger phase effects here
                OnPhaseTransition(phase);
            }
        }
    }

    private void OnPhaseTransition(BossPhase phase)
    {
        // Reset ability cooldowns on phase transition
        foreach (var ability in _bossData.abilities)
        {
            ability.lastUsedTime = Time.time - ability.cooldown / 2f; // Half cooldown
        }
        
        // Visual/audio effects would go here
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(AudioEventType.EnemyDeath); // Repurpose for dramatic effect
        }
    }

    private void OnBossDefeated()
    {
        Debug.Log($"🏆 BOSS DEFEATED: {_bossData.bossName}!");
        
        // Award special loot
        DropBossLoot();
        
        // Trigger achievement
        if (AchievementSystem.Instance != null)
        {
            AchievementSystem.Instance.OnBossDefeated(_bossData.bossId);
        }
        
        // Clean up
        if (BossEncounterSystem.Instance != null)
        {
            BossEncounterSystem.Instance.ClearCurrentBoss();
        }
        
        Destroy(gameObject, 2f); // Delay for effect
    }

    private void DropBossLoot()
    {
        // Boss-specific legendary loot drops
        Debug.Log($"💎 {_bossData.bossName} dropped legendary loot!");
        // Integration with loot system would go here
    }

    public BossData GetBossData()
    {
        return _bossData;
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

    public BossPhase GetCurrentPhase()
    {
        return _currentPhase;
    }
}
