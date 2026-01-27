using System.Collections.Generic;

namespace MiddleEarth.Interfaces
{
    /// <summary>
    /// Interface for objects that can provide loot to players.
    /// Implements the "Break up the Fellowship" principle - modularization.
    /// </summary>
    public interface ILootable
    {
        /// <summary>
        /// Whether this object has already been looted.
        /// </summary>
        bool IsLooted { get; }
        
        /// <summary>
        /// Get the loot from this object.
        /// Note: Returns objects as System.Object to avoid circular dependencies.
        /// Cast to Item type at the application layer.
        /// </summary>
        /// <returns>List of items that can be looted.</returns>
        IEnumerable<object> GetLoot();
        
        /// <summary>
        /// Get the gold amount from this object.
        /// </summary>
        int GetGold();
        
        /// <summary>
        /// Mark this object as looted.
        /// </summary>
        void MarkAsLooted();
    }
}
