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
        /// </summary>
        /// <returns>List of items that can be looted.</returns>
        IEnumerable<Item> GetLoot();
        
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
