using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiddleEarth.Core
{
    /// <summary>
    /// Event-driven communication system for loose coupling.
    /// Implements the "Break up the Fellowship" principle - modularization.
    /// 
    /// Systems can publish events and subscribe to events without direct references.
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> _subscribers = new Dictionary<Type, List<Delegate>>();
        
        /// <summary>
        /// Subscribe to an event type.
        /// </summary>
        /// <typeparam name="T">The event type to subscribe to.</typeparam>
        /// <param name="handler">The handler to call when the event is published.</param>
        public static void Subscribe<T>(Action<T> handler) where T : IGameEvent
        {
            Type type = typeof(T);
            
            if (!_subscribers.ContainsKey(type))
            {
                _subscribers[type] = new List<Delegate>();
            }
            
            _subscribers[type].Add(handler);
        }
        
        /// <summary>
        /// Unsubscribe from an event type.
        /// </summary>
        /// <typeparam name="T">The event type to unsubscribe from.</typeparam>
        /// <param name="handler">The handler to remove.</param>
        public static void Unsubscribe<T>(Action<T> handler) where T : IGameEvent
        {
            Type type = typeof(T);
            
            if (_subscribers.ContainsKey(type))
            {
                _subscribers[type].Remove(handler);
            }
        }
        
        /// <summary>
        /// Publish an event to all subscribers.
        /// </summary>
        /// <typeparam name="T">The event type to publish.</typeparam>
        /// <param name="gameEvent">The event data.</param>
        public static void Publish<T>(T gameEvent) where T : IGameEvent
        {
            Type type = typeof(T);
            
            if (_subscribers.ContainsKey(type))
            {
                var handlers = _subscribers[type];
                for (int i = handlers.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        var handler = handlers[i] as Action<T>;
                        handler?.Invoke(gameEvent);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"EventBus: Error invoking handler for {type.Name}: {ex.Message}");
                    }
                }
            }
        }
        
        /// <summary>
        /// Clear all subscribers. Typically called on scene unload.
        /// </summary>
        public static void Clear()
        {
            _subscribers.Clear();
        }
        
        /// <summary>
        /// Get the subscriber count for an event type.
        /// </summary>
        /// <typeparam name="T">The event type to check.</typeparam>
        /// <returns>The number of subscribers.</returns>
        public static int GetSubscriberCount<T>() where T : IGameEvent
        {
            Type type = typeof(T);
            return _subscribers.ContainsKey(type) ? _subscribers[type].Count : 0;
        }
    }
    
    /// <summary>
    /// Base interface for all game events.
    /// </summary>
    public interface IGameEvent { }
    
    // ========================================
    // GAME EVENTS
    // ========================================
    
    /// <summary>
    /// Event fired when an enemy is defeated.
    /// </summary>
    public struct EnemyDefeatedEvent : IGameEvent
    {
        public string EnemyName { get; }
        public int ExperienceReward { get; }
        public int GoldReward { get; }
        public Vector3 Position { get; }
        
        public EnemyDefeatedEvent(string enemyName, int experienceReward, int goldReward, Vector3 position)
        {
            EnemyName = enemyName;
            ExperienceReward = experienceReward;
            GoldReward = goldReward;
            Position = position;
        }
    }
    
    /// <summary>
    /// Event fired when the player levels up.
    /// </summary>
    public struct LevelUpEvent : IGameEvent
    {
        public int NewLevel { get; }
        public int PreviousLevel { get; }
        
        public LevelUpEvent(int newLevel, int previousLevel)
        {
            NewLevel = newLevel;
            PreviousLevel = previousLevel;
        }
    }
    
    /// <summary>
    /// Event fired when a quest is completed.
    /// </summary>
    public struct QuestCompletedEvent : IGameEvent
    {
        public string QuestId { get; }
        public string QuestName { get; }
        public int ExperienceReward { get; }
        public int GoldReward { get; }
        
        public QuestCompletedEvent(string questId, string questName, int experienceReward, int goldReward)
        {
            QuestId = questId;
            QuestName = questName;
            ExperienceReward = experienceReward;
            GoldReward = goldReward;
        }
    }
    
    /// <summary>
    /// Event fired when a location is discovered.
    /// </summary>
    public struct LocationDiscoveredEvent : IGameEvent
    {
        public string LocationName { get; }
        public Vector3 Position { get; }
        
        public LocationDiscoveredEvent(string locationName, Vector3 position)
        {
            LocationName = locationName;
            Position = position;
        }
    }
    
    /// <summary>
    /// Event fired when a treasure chest is opened.
    /// </summary>
    public struct ChestOpenedEvent : IGameEvent
    {
        public string ItemName { get; }
        public int GoldAmount { get; }
        public Vector3 Position { get; }
        
        public ChestOpenedEvent(string itemName, int goldAmount, Vector3 position)
        {
            ItemName = itemName;
            GoldAmount = goldAmount;
            Position = position;
        }
    }
    
    /// <summary>
    /// Event fired when an achievement is unlocked.
    /// </summary>
    public struct AchievementUnlockedEvent : IGameEvent
    {
        public string AchievementId { get; }
        public string AchievementName { get; }
        
        public AchievementUnlockedEvent(string achievementId, string achievementName)
        {
            AchievementId = achievementId;
            AchievementName = achievementName;
        }
    }
    
    /// <summary>
    /// Event fired when equipment is changed.
    /// </summary>
    public struct EquipmentChangedEvent : IGameEvent
    {
        public string ItemName { get; }
        public EquipmentSlot Slot { get; }
        public bool IsEquipped { get; }
        
        public EquipmentChangedEvent(string itemName, EquipmentSlot slot, bool isEquipped)
        {
            ItemName = itemName;
            Slot = slot;
            IsEquipped = isEquipped;
        }
    }
    
    /// <summary>
    /// Event fired when combat state changes.
    /// </summary>
    public struct CombatEvent : IGameEvent
    {
        public enum CombatEventType { AttackStarted, AttackHit, AttackMissed, SpecialUsed, ComboAchieved }
        
        public CombatEventType EventType { get; }
        public float Damage { get; }
        public bool IsCritical { get; }
        public int ComboCount { get; }
        
        public CombatEvent(CombatEventType eventType, float damage = 0, bool isCritical = false, int comboCount = 0)
        {
            EventType = eventType;
            Damage = damage;
            IsCritical = isCritical;
            ComboCount = comboCount;
        }
    }
}
