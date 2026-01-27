using UnityEngine;
using System.Collections.Generic;
using MiddleEarth.Core;
using MiddleEarth.Config;

namespace MiddleEarth.RPG
{
    /// <summary>
    /// Multi-floor dungeon system with procedural generation and boss encounters
    /// Part of v2.3 World Expansion - Priority 2 feature
    /// </summary>
    public class DungeonSystem : MonoBehaviour
    {
        [Header("Dungeon Settings")]
        [SerializeField] private int minFloors = 3;
        [SerializeField] private int maxFloors = 10;
        [SerializeField] private float floorDifficultyMultiplier = 1.2f;

        [Header("Room Generation")]
        [SerializeField] private int roomsPerFloor = 5;
        [SerializeField] private Vector2 roomSizeRange = new Vector2(10f, 20f);
        [SerializeField] private int enemiesPerRoom = 3;

        [Header("Boss Settings")]
        [SerializeField] private bool hasBossOnFinalFloor = true;
        [SerializeField] private float bossHealthMultiplier = 3f;
        [SerializeField] private float bossDamageMultiplier = 1.5f;

        [Header("Rewards")]
        [SerializeField] private int baseGoldReward = 100;
        [SerializeField] private int baseXpReward = 200;
        [SerializeField] private float rewardMultiplierPerFloor = 1.3f;

        private Dungeon currentDungeon;
        private int currentFloor = 0;

        [System.Serializable]
        public class Dungeon
        {
            public string dungeonId;
            public string dungeonName;
            public int totalFloors;
            public int currentFloor;
            public List<DungeonFloor> floors;
            public DungeonTheme theme;
            public int difficulty; // 1-10 scale

            public Dungeon(string id, string name, int floorCount, DungeonTheme dungeonTheme, int diff)
            {
                dungeonId = id;
                dungeonName = name;
                totalFloors = floorCount;
                currentFloor = 0;
                floors = new List<DungeonFloor>();
                theme = dungeonTheme;
                difficulty = diff;
            }
        }

        [System.Serializable]
        public class DungeonFloor
        {
            public int floorNumber;
            public List<DungeonRoom> rooms;
            public bool hasBoss;
            public bool isCompleted;
            public float difficultyModifier;

            public DungeonFloor(int floor, int roomCount, bool boss, float diffMod)
            {
                floorNumber = floor;
                rooms = new List<DungeonRoom>();
                hasBoss = boss;
                isCompleted = false;
                difficultyModifier = diffMod;

                // Generate rooms
                for (int i = 0; i < roomCount; i++)
                {
                    rooms.Add(new DungeonRoom(i, floor));
                }
            }
        }

        [System.Serializable]
        public class DungeonRoom
        {
            public int roomId;
            public int floorNumber;
            public RoomType type;
            public bool isCleared;
            public int enemyCount;
            public Vector3 spawnPosition;

            public DungeonRoom(int id, int floor)
            {
                roomId = id;
                floorNumber = floor;
                type = RoomType.Combat;
                isCleared = false;
                enemyCount = 0;
            }
        }

        public enum RoomType
        {
            Combat,     // Standard enemy encounters
            Boss,       // Boss fight room
            Treasure,   // Extra loot room
            Rest,       // Healing/checkpoint room
            Puzzle,     // Future: puzzle room
            Trap        // Future: trap room
        }

        public enum DungeonTheme
        {
            Cave,       // Dark caves with bats and orcs
            Crypt,      // Undead-filled tombs
            Fortress,   // Enemy stronghold
            Mine,       // Abandoned mines with dangers
            Tower       // Vertical tower structure
        }

        // Events
        public delegate void DungeonStarted(Dungeon dungeon);
        public delegate void FloorCompleted(int floorNumber);
        public delegate void DungeonCompleted(Dungeon dungeon);
        public delegate void BossEncounter(DungeonRoom bossRoom);

        public event DungeonStarted OnDungeonStarted;
        public event FloorCompleted OnFloorCompleted;
        public event DungeonCompleted OnDungeonCompleted;
        public event BossEncounter OnBossEncounter;

        // Public accessors
        public Dungeon CurrentDungeon => currentDungeon;
        public int CurrentFloor => currentFloor;
        public bool IsInDungeon => currentDungeon != null;

        private void Start()
        {
            GameLogger.Log("Dungeon System initialized - v2.3 World Expansion", LogCategory.General);
        }

        public void GenerateDungeon(string dungeonName, DungeonTheme theme, int difficulty)
        {
            // Determine floor count based on difficulty
            int floorCount = Mathf.Clamp(minFloors + (difficulty - 1), minFloors, maxFloors);

            // Create dungeon
            string dungeonId = $"dungeon_{dungeonName.ToLower().Replace(" ", "_")}_{Time.time}";
            currentDungeon = new Dungeon(dungeonId, dungeonName, floorCount, theme, difficulty);

            // Generate floors
            for (int i = 0; i < floorCount; i++)
            {
                float diffMod = 1f + (i * (floorDifficultyMultiplier - 1f));
                bool hasBoss = hasBossOnFinalFloor && (i == floorCount - 1);
                
                DungeonFloor floor = new DungeonFloor(i, roomsPerFloor, hasBoss, diffMod);
                
                // Set room types
                SetupFloorRooms(floor, hasBoss);
                
                currentDungeon.floors.Add(floor);
            }

            currentFloor = 0;
            OnDungeonStarted?.Invoke(currentDungeon);
            
            GameLogger.Log($"Generated dungeon: {dungeonName} with {floorCount} floors (Difficulty: {difficulty})", LogCategory.General);
        }

        private void SetupFloorRooms(DungeonFloor floor, bool hasBoss)
        {
            int roomCount = floor.rooms.Count;
            
            for (int i = 0; i < roomCount; i++)
            {
                DungeonRoom room = floor.rooms[i];
                
                if (hasBoss && i == roomCount - 1)
                {
                    // Last room is boss room
                    room.type = RoomType.Boss;
                    room.enemyCount = 1; // Boss only
                }
                else if (i == 0 && floor.floorNumber > 0)
                {
                    // First room of non-first floor is rest room
                    room.type = RoomType.Rest;
                    room.enemyCount = 0;
                }
                else if (Random.value < 0.15f)
                {
                    // 15% chance of treasure room
                    room.type = RoomType.Treasure;
                    room.enemyCount = Random.Range(1, 3); // Light guards
                }
                else
                {
                    // Standard combat room
                    room.type = RoomType.Combat;
                    room.enemyCount = Mathf.RoundToInt(enemiesPerRoom * floor.difficultyModifier);
                }
            }
        }

        public void EnterDungeon(Vector3 entrancePosition)
        {
            if (currentDungeon == null)
            {
                GameLogger.Log("No dungeon generated", LogCategory.General, LogLevel.Error);
                return;
            }

            currentFloor = 0;
            GameLogger.Log($"Entering {currentDungeon.dungeonName}", LogCategory.General);
            
            // Could trigger entrance cinematic or loading screen here
        }

        public void AdvanceToNextFloor()
        {
            if (currentDungeon == null) return;

            if (currentFloor < currentDungeon.totalFloors - 1)
            {
                currentDungeon.floors[currentFloor].isCompleted = true;
                OnFloorCompleted?.Invoke(currentFloor);
                
                currentFloor++;
                GameLogger.Log($"Advanced to floor {currentFloor + 1} of {currentDungeon.totalFloors}", LogCategory.General);
            }
            else
            {
                CompleteDungeon();
            }
        }

        private void CompleteDungeon()
        {
            if (currentDungeon == null) return;

            currentDungeon.floors[currentFloor].isCompleted = true;
            
            // Calculate rewards
            int totalGold = CalculateDungeonReward(baseGoldReward);
            int totalXp = CalculateDungeonReward(baseXpReward);
            
            GameLogger.Log($"Dungeon completed! Rewards: {totalGold} gold, {totalXp} XP", LogCategory.General);
            
            OnDungeonCompleted?.Invoke(currentDungeon);
            
            // Reset dungeon
            currentDungeon = null;
            currentFloor = 0;
        }

        private int CalculateDungeonReward(int baseReward)
        {
            if (currentDungeon == null) return baseReward;
            
            float multiplier = Mathf.Pow(rewardMultiplierPerFloor, currentDungeon.totalFloors);
            multiplier *= currentDungeon.difficulty * 0.5f;
            
            return Mathf.RoundToInt(baseReward * multiplier);
        }

        public void ClearRoom(int roomId)
        {
            if (currentDungeon == null || currentFloor >= currentDungeon.floors.Count) return;

            DungeonFloor floor = currentDungeon.floors[currentFloor];
            
            foreach (DungeonRoom room in floor.rooms)
            {
                if (room.roomId == roomId)
                {
                    room.isCleared = true;
                    GameLogger.Log($"Room {roomId} cleared on floor {currentFloor + 1}", LogCategory.General);
                    
                    if (room.type == RoomType.Boss)
                    {
                        OnBossEncounter?.Invoke(room);
                    }
                    
                    break;
                }
            }

            // Check if all rooms on floor are cleared
            CheckFloorCompletion();
        }

        private void CheckFloorCompletion()
        {
            if (currentDungeon == null || currentFloor >= currentDungeon.floors.Count) return;

            DungeonFloor floor = currentDungeon.floors[currentFloor];
            bool allCleared = true;

            foreach (DungeonRoom room in floor.rooms)
            {
                if (!room.isCleared && room.type != RoomType.Rest)
                {
                    allCleared = false;
                    break;
                }
            }

            if (allCleared)
            {
                GameLogger.Log($"All rooms cleared on floor {currentFloor + 1}!", LogCategory.General);
                // Could trigger floor completion rewards or next floor unlock
            }
        }

        public void ExitDungeon(bool forfeit = false)
        {
            if (currentDungeon == null) return;

            if (forfeit)
            {
                GameLogger.Log("Exited dungeon (forfeited)", LogCategory.General);
            }
            else
            {
                GameLogger.Log("Exited dungeon", LogCategory.General);
            }

            currentDungeon = null;
            currentFloor = 0;
        }

        // Public query methods
        public DungeonFloor GetCurrentFloor()
        {
            if (currentDungeon == null || currentFloor >= currentDungeon.floors.Count)
                return null;
            
            return currentDungeon.floors[currentFloor];
        }

        public List<DungeonRoom> GetCurrentRooms()
        {
            DungeonFloor floor = GetCurrentFloor();
            return floor?.rooms;
        }

        public int GetRemainingFloors()
        {
            if (currentDungeon == null) return 0;
            return currentDungeon.totalFloors - currentFloor - 1;
        }

        public float GetCurrentDifficultyModifier()
        {
            DungeonFloor floor = GetCurrentFloor();
            return floor?.difficultyModifier ?? 1f;
        }
    }
}
