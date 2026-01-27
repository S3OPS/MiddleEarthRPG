using UnityEngine;
using UnityEngine.UI;

public class RPGBootstrap : MonoBehaviour
{
    public static RPGBootstrap Instance { get; private set; }
    public Transform PlayerTransform { get; private set; }

    private RPGConfig _config;
    private CharacterStats _playerStats;
    private InventorySystem _inventory;
    private QuestManager _questManager;
    private EquipmentSystem _equipmentSystem;
    private CombatSystem _combatSystem;
    private Text _hud;
    private Canvas _hudCanvas;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AutoBoot()
    {
        var bootstrap = new GameObject("RPGBootstrap");
        bootstrap.AddComponent<RPGBootstrap>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _config = RPGConfig.Load();
        _playerStats = new CharacterStats();
        _playerStats.characterName = _config.characterName;
        _inventory = new InventorySystem();
        _inventory.gold = _config.startingGold;
        _equipmentSystem = new EquipmentSystem();
        _equipmentSystem.Initialize(_playerStats);
    }

    private void Start()
    {
        SetupScene();
        SetupManagers();
        SetupWorld();
        SetupPlayer();
        SetupQuestSystem();
        SetupUi();
    }
    
    private void SetupManagers()
    {
        // Audio Manager
        var audioObj = new GameObject("AudioManager");
        audioObj.AddComponent<AudioManager>();
        
        // Effects Manager
        var effectsObj = new GameObject("EffectsManager");
        effectsObj.AddComponent<EffectsManager>();
        
        // Achievement System
        var achievementObj = new GameObject("AchievementSystem");
        achievementObj.AddComponent<AchievementSystem>();
    }

    private void SetupScene()
    {
        // Middle-earth atmosphere - warmer, more magical lighting
        RenderSettings.ambientLight = new Color(0.6f, 0.55f, 0.5f);
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.7f, 0.7f, 0.8f);
        RenderSettings.fogDensity = 0.01f;

        var lightObj = new GameObject("Directional Light");
        var light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.5f;
        light.color = new Color(1f, 0.95f, 0.85f); // Warm sunlight
        light.transform.rotation = Quaternion.Euler(45f, -30f, 0f);
    }

    private void SetupWorld()
    {
        // Create The Shire - green peaceful area
        CreateLocation("The Shire", new Vector3(-25f, 0f, -25f), new Vector3(15f, 1f, 15f), 
            new Color(0.3f, 0.7f, 0.3f), "shire_burden", "explore_shire");

        // Create Rohan - golden plains
        CreateLocation("Plains of Rohan", new Vector3(20f, 0f, 0f), new Vector3(20f, 1f, 20f), 
            new Color(0.8f, 0.7f, 0.3f), "rohan_riders", "explore_rohan");

        // Create Mordor - dark, foreboding land
        CreateLocation("Lands of Mordor", new Vector3(0f, 0f, 35f), new Vector3(15f, 1f, 15f), 
            new Color(0.25f, 0.2f, 0.2f), "path_mordor", "explore_mordor");

        // Central ground - neutral area
        var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Middle-earth";
        ground.transform.localScale = new Vector3(20f, 1f, 20f);
        var groundRenderer = ground.GetComponent<Renderer>();
        groundRenderer.material.color = new Color(0.5f, 0.6f, 0.4f);

        // Add NPCs
        CreateNPC("Gandalf the Grey", new Vector3(-15f, 1.5f, -15f), new Vector3(0.8f, 2f, 0.8f), 
            new Color(0.9f, 0.9f, 0.9f), "Welcome, traveler! Dark times are upon us.", "fellowship");
        
        CreateNPC("Legolas", new Vector3(15f, 1.5f, -5f), new Vector3(0.7f, 2f, 0.7f), 
            new Color(0.7f, 0.9f, 0.7f), "The orcs are gathering strength.", "fellowship");
        
        CreateNPC("Gimli", new Vector3(10f, 1.2f, 5f), new Vector3(0.9f, 1.5f, 0.9f), 
            new Color(0.7f, 0.5f, 0.3f), "And my axe!", "fellowship");

        // Add enemies - Orcs in Rohan
        CreateEnemy("Orc Scout", new Vector3(25f, 1f, 5f), new Vector3(0.8f, 1.8f, 0.8f), 
            new Color(0.3f, 0.4f, 0.3f));
        CreateEnemy("Orc Scout", new Vector3(22f, 1f, -3f), new Vector3(0.8f, 1.8f, 0.8f), 
            new Color(0.3f, 0.4f, 0.3f));
        CreateEnemy("Orc Scout", new Vector3(18f, 1f, 3f), new Vector3(0.8f, 1.8f, 0.8f), 
            new Color(0.3f, 0.4f, 0.3f));
        CreateEnemy("Orc Scout", new Vector3(28f, 1f, -5f), new Vector3(0.8f, 1.8f, 0.8f), 
            new Color(0.3f, 0.4f, 0.3f));
        CreateEnemy("Orc Scout", new Vector3(20f, 1f, 8f), new Vector3(0.8f, 1.8f, 0.8f), 
            new Color(0.3f, 0.4f, 0.3f));

        // Add more enemies in Mordor
        for (int i = 0; i < 12; i++)
        {
            float x = Random.Range(-8f, 8f);
            float z = Random.Range(32f, 42f);
            CreateEnemy("Dark Servant", new Vector3(x, 1f, z), new Vector3(0.9f, 2f, 0.9f), 
                new Color(0.2f, 0.2f, 0.3f));
        }

        // Add treasure chests with equipment
        CreateEquipmentChest(EquipmentFactory.CreateOrcsword(), new Vector3(-20f, 0.5f, -20f), 
            new Vector3(1f, 0.8f, 1f), new Color(0.7f, 0.6f, 0.3f));
        CreateEquipmentChest(EquipmentFactory.CreateElvenBlade(), new Vector3(12f, 0.5f, -8f), 
            new Vector3(1f, 0.8f, 1f), new Color(0.8f, 0.8f, 0.9f));
        CreateEquipmentChest(EquipmentFactory.CreateElvenCloak(), new Vector3(-5f, 0.5f, 32f), 
            new Vector3(1f, 0.8f, 1f), new Color(0.5f, 0.8f, 0.5f));
        CreateEquipmentChest(EquipmentFactory.CreateMithrilCoat(), new Vector3(5f, 0.5f, 35f), 
            new Vector3(1f, 0.8f, 1f), new Color(0.9f, 0.9f, 1f));
        CreateEquipmentChest(EquipmentFactory.CreateAnduril(), new Vector3(-25f, 0.5f, -28f), 
            new Vector3(1f, 0.8f, 1f), new Color(1f, 0.9f, 0.3f));
    }

    private void CreateLocation(string name, Vector3 position, Vector3 scale, Color color, string questId, string objectiveId)
    {
        var location = GameObject.CreatePrimitive(PrimitiveType.Plane);
        location.name = name;
        location.transform.position = position;
        location.transform.localScale = scale;
        var renderer = location.GetComponent<Renderer>();
        renderer.material.color = color;

        // Add trigger for location discovery
        var trigger = location.AddComponent<LocationTrigger>();
        trigger.locationName = name;
        trigger.questId = questId;
        trigger.objectiveId = objectiveId;

        // Make the collider a trigger
        var collider = location.GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }

        // Add label
        var textObj = new GameObject(name + " Label");
        textObj.transform.position = position + new Vector3(0f, 2f, 0f);
        var text = textObj.AddComponent<TextMesh>();
        text.text = name;
        text.characterSize = 0.5f;
        text.fontSize = 64;
        text.color = Color.white;
        text.alignment = TextAlignment.Center;
        text.anchor = TextAnchor.MiddleCenter;
    }

    private void CreateNPC(string name, Vector3 position, Vector3 scale, Color color, string greeting, string questId)
    {
        var npcObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        npcObj.name = name;
        npcObj.tag = "NPC";
        npcObj.transform.position = position;
        npcObj.transform.localScale = scale;
        var renderer = npcObj.GetComponent<Renderer>();
        renderer.material.color = color;

        var npc = npcObj.AddComponent<NPC>();
        npc.npcName = name;
        npc.greeting = greeting;
        npc.questId = questId;

        // Make trigger for interaction
        var collider = npcObj.GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }

        // Add label
        var textObj = new GameObject(name + " Label");
        textObj.transform.position = position + Vector3.up * (scale.y * 0.6f + 0.5f);
        var text = textObj.AddComponent<TextMesh>();
        text.text = name;
        text.characterSize = 0.2f;
        text.fontSize = 64;
        text.color = Color.yellow;
        text.alignment = TextAlignment.Center;
        text.anchor = TextAnchor.MiddleCenter;
    }

    private void CreateEnemy(string enemyName, Vector3 position, Vector3 scale, Color color)
    {
        var enemyObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        enemyObj.name = enemyName;
        enemyObj.tag = "Enemy";
        enemyObj.transform.position = position;
        enemyObj.transform.localScale = scale;
        var renderer = enemyObj.GetComponent<Renderer>();
        renderer.material.color = color;

        var enemy = enemyObj.AddComponent<Enemy>();
        enemy.enemyName = enemyName;

        // Add label
        var textObj = new GameObject(enemyName + " Label");
        textObj.transform.position = position + Vector3.up * (scale.y * 0.6f + 0.5f);
        var text = textObj.AddComponent<TextMesh>();
        text.text = enemyName;
        text.characterSize = 0.15f;
        text.fontSize = 64;
        text.color = Color.red;
        text.alignment = TextAlignment.Center;
        text.anchor = TextAnchor.MiddleCenter;
    }

    private void CreateTreasureChest(string itemName, Vector3 position, Vector3 scale, Color color)
    {
        var chest = GameObject.CreatePrimitive(PrimitiveType.Cube);
        chest.name = "Chest_" + itemName;
        chest.tag = "TreasureChest";
        chest.transform.position = position;
        chest.transform.localScale = scale;
        var renderer = chest.GetComponent<Renderer>();
        renderer.material.color = color;

        var treasure = chest.AddComponent<TreasureChest>();
        treasure.itemName = itemName;
        treasure.itemType = ItemType.QuestItem;
        treasure.itemValue = 50;
        treasure.goldAmount = 30;

        // Make trigger
        var collider = chest.GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }
    
    private void CreateEquipmentChest(Equipment equipment, Vector3 position, Vector3 scale, Color color)
    {
        var chest = GameObject.CreatePrimitive(PrimitiveType.Cube);
        chest.name = "Chest_" + equipment.name;
        chest.tag = "EquipmentChest";
        chest.transform.position = position;
        chest.transform.localScale = scale;
        var renderer = chest.GetComponent<Renderer>();
        renderer.material.color = color;

        var equipChest = chest.AddComponent<EquipmentChest>();
        equipChest.equipment = equipment;
        equipChest.goldAmount = 50;

        // Make trigger
        var collider = chest.GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }

    private void SetupPlayer()
    {
        var player = new GameObject("Player");
        player.tag = "Player";
        player.transform.position = new Vector3(0f, 1.6f, -15f);
        PlayerTransform = player.transform;

        var capsule = player.AddComponent<CapsuleCollider>();
        capsule.height = 1.8f;
        capsule.radius = 0.3f;

        var rigidbody = player.AddComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        var controller = player.AddComponent<FirstPersonController>();
        controller.MoveSpeed = _config.moveSpeed;
        controller.SprintMultiplier = _config.sprintMultiplier;
        
        // Add combat system
        _combatSystem = player.AddComponent<CombatSystem>();
        _combatSystem.Initialize(_playerStats);

        var cameraRoot = new GameObject("CameraRoot");
        cameraRoot.transform.SetParent(player.transform);
        cameraRoot.transform.localPosition = new Vector3(0f, 0.6f, 0f);

        var cam = new GameObject("Main Camera");
        cam.tag = "MainCamera";
        cam.transform.SetParent(cameraRoot.transform);
        cam.transform.localPosition = Vector3.zero;
        cam.AddComponent<Camera>();

        var look = cam.AddComponent<SimpleCameraLook>();
        look.Target = player.transform;
        look.Sensitivity = _config.mouseSensitivity;
    }

    private void SetupQuestSystem()
    {
        var questObj = new GameObject("QuestManager");
        _questManager = questObj.AddComponent<QuestManager>();
        _questManager.Initialize(_playerStats, _inventory);

        // Activate starting quests
        _questManager.ActivateQuest("shire_burden");
        _questManager.ActivateQuest("fellowship");
        _questManager.ActivateQuest("master_arms");
        _questManager.ActivateQuest("treasure_seeker");
    }

    private void SetupUi()
    {
        var canvasObj = new GameObject("HUD");
        var canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        _hudCanvas = canvas;

        var textObj = new GameObject("StatusText");
        textObj.transform.SetParent(canvasObj.transform);
        var text = textObj.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 16;
        text.alignment = TextAnchor.UpperLeft;
        text.color = Color.white;

        var rect = text.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 1f);
        rect.anchoredPosition = new Vector2(16f, -16f);
        rect.sizeDelta = new Vector2(700f, 500f);

        _hud = text;
        
        // Setup minimap
        var minimapObj = new GameObject("MinimapSystem");
        var minimap = minimapObj.AddComponent<MinimapSystem>();
        minimap.Initialize(PlayerTransform, canvas);
    }

    private void Update()
    {
        UpdateHUD();
        
        // Passive stamina regeneration
        _playerStats.RestoreStamina(10f * Time.deltaTime);
    }

    private void UpdateHUD()
    {
        if (_hud == null) return;

        var hudText = $"<b>=== MIDDLE-EARTH ADVENTURE ===</b>\n\n";
        hudText += $"<b>{_playerStats.characterName}</b> - Level {_playerStats.level}\n";
        hudText += $"Health: {_playerStats.currentHealth:0}/{_playerStats.maxHealth:0}  ";
        hudText += $"Stamina: {_playerStats.currentStamina:0}/{_playerStats.maxStamina:0}\n";
        hudText += $"XP: {_playerStats.experience}/{_playerStats.experienceToNextLevel}  ";
        hudText += $"Gold: {_inventory.gold}\n";
        
        // Equipment display
        var weapon = _equipmentSystem.WeaponSlot;
        var armor = _equipmentSystem.ArmorSlot;
        hudText += $"Weapon: {(weapon != null ? weapon.name : "None")}  ";
        hudText += $"Armor: {(armor != null ? armor.name : "None")}\n\n";

        // Combat info
        if (_combatSystem != null && _combatSystem.ComboCount > 0)
        {
            hudText += $"<color=yellow>COMBO x{_combatSystem.ComboCount}!</color>\n";
        }

        hudText += "<b>Active Quests:</b>\n";
        var activeQuests = _questManager.GetActiveQuests();
        if (activeQuests.Count == 0)
        {
            hudText += "  No active quests\n";
        }
        else
        {
            foreach (var quest in activeQuests)
            {
                if (!quest.isCompleted)
                {
                    hudText += $"  • {quest.questName} ({quest.GetCompletionPercentage():0}%)\n";
                    foreach (var objective in quest.objectives)
                    {
                        string status = objective.isCompleted ? "[✓]" : "[ ]";
                        hudText += $"    {status} {objective.description} ({objective.currentProgress}/{objective.requiredProgress})\n";
                    }
                }
            }
        }
        
        // Achievements
        if (AchievementSystem.Instance != null)
        {
            hudText += $"\n<b>Achievements:</b> {AchievementSystem.Instance.GetUnlockedCount()}/{AchievementSystem.Instance.GetTotalCount()} ({AchievementSystem.Instance.GetCompletionPercentage():0}%)\n";
        }

        hudText += "\n<b>Controls:</b> WASD Move | Mouse Look | Shift Sprint | Space Jump\n";
        hudText += "<b>Combat:</b> Left Click Attack | Right Click Special Ability\n";
        hudText += "<b>Interact:</b> Walk into NPCs, Chests, and Locations";

        _hud.text = hudText;
    }

    public void OnEnemyDefeated(string enemyName)
    {
        int damage = 20; // Base damage for tracking
        _playerStats.GainExperience(50);
        _inventory.AddGold(25);

        if (enemyName.Contains("Orc"))
        {
            _questManager.UpdateQuestProgress("rohan_riders", "defeat_orcs", 1);
        }
        else if (enemyName.Contains("Dark"))
        {
            _questManager.UpdateQuestProgress("path_mordor", "defeat_darkness", 1);
        }
        
        // Track for achievements
        if (AchievementSystem.Instance != null)
        {
            AchievementSystem.Instance.OnEnemyDefeated(damage);
        }
        
        // Track combo achievements
        if (_combatSystem != null && AchievementSystem.Instance != null)
        {
            AchievementSystem.Instance.OnComboAchieved(_combatSystem.ComboCount);
        }
    }

    public void OnNPCInteraction(string npcName, string questId)
    {
        if (questId == "fellowship")
        {
            if (npcName.Contains("Gandalf"))
            {
                _questManager.UpdateQuestProgress("fellowship", "talk_gandalf", 1);
            }
            else if (npcName.Contains("Legolas"))
            {
                _questManager.UpdateQuestProgress("fellowship", "talk_legolas", 1);
            }
            else if (npcName.Contains("Gimli"))
            {
                _questManager.UpdateQuestProgress("fellowship", "talk_gimli", 1);
            }
        }
    }

    public void OnChestOpened(string itemName, ItemType itemType, int itemValue, int goldAmount)
    {
        var item = new Item(itemName, "A valuable item", itemType, itemValue, 1);
        _inventory.AddItem(item);
        _inventory.AddGold(goldAmount);

        if (itemName.Contains("Lembas"))
        {
            _questManager.UpdateQuestProgress("shire_burden", "gather_lembas", 1);
        }
        else if (itemName.Contains("Artifact") || itemName.Contains("Ring"))
        {
            _questManager.UpdateQuestProgress("path_mordor", "find_ring", 1);
        }
    }
    
    public void OnEquipmentChestOpened(Equipment equipment, int goldAmount)
    {
        // Auto-equip if the slot is empty or item is better
        bool equipped = _equipmentSystem.EquipItem(equipment);
        _inventory.AddGold(goldAmount);
        
        if (!equipped)
        {
            // Add to inventory if couldn't equip
            _inventory.AddItem(equipment);
        }
        
        Debug.Log($"Found {equipment.name}! Rarity: {equipment.rarity}");
    }

    public void OnLocationDiscovered(string locationName, string questId, string objectiveId)
    {
        _playerStats.GainExperience(25);
        _questManager.UpdateQuestProgress(questId, objectiveId, 1);
    }
}
