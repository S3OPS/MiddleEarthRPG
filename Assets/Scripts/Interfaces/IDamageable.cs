namespace MiddleEarth.Interfaces
{
    /// <summary>
    /// Interface for all damageable entities.
    /// Implements the "Break up the Fellowship" principle - modularization.
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// Current health of the entity.
        /// </summary>
        float CurrentHealth { get; }
        
        /// <summary>
        /// Maximum health of the entity.
        /// </summary>
        float MaxHealth { get; }
        
        /// <summary>
        /// Whether the entity is still alive.
        /// </summary>
        bool IsAlive { get; }
        
        /// <summary>
        /// Apply damage to this entity.
        /// </summary>
        /// <param name="damage">Amount of damage to apply.</param>
        void TakeDamage(float damage);
        
        /// <summary>
        /// Heal this entity.
        /// </summary>
        /// <param name="amount">Amount of healing to apply.</param>
        void Heal(float amount);
    }
}
