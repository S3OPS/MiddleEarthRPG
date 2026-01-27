using UnityEngine;

namespace MiddleEarth.Interfaces
{
    /// <summary>
    /// Interface for all interactable objects in the game world.
    /// Implements the "Break up the Fellowship" principle - modularization.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// The display name for this interactable object.
        /// </summary>
        string InteractionName { get; }
        
        /// <summary>
        /// Whether this object can currently be interacted with.
        /// </summary>
        bool CanInteract { get; }
        
        /// <summary>
        /// Called when a player interacts with this object.
        /// </summary>
        /// <param name="player">The player transform that initiated the interaction.</param>
        void OnInteract(Transform player);
    }
}
