using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName = "Orc Scout";
    public float maxHealth = 50f;
    public float currentHealth = 50f;
    public int damageAmount = 10;
    public int experienceReward = 50;
    public int goldReward = 25;
    public float attackRange = 2f;
    public float detectionRange = 10f;
    public float attackCooldown = 2f;

    private Transform _player;
    private float _lastAttackTime;
    private bool _isDead = false;

    private void Start()
    {
        if (RPGBootstrap.Instance != null)
        {
            _player = RPGBootstrap.Instance.PlayerTransform;
        }
    }

    private void Update()
    {
        if (_isDead || _player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if (distanceToPlayer <= attackRange && Time.time >= _lastAttackTime + attackCooldown)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        Vector3 direction = (_player.position - transform.position).normalized;
        direction.y = 0;
        transform.position += direction * 2f * Time.deltaTime;
        transform.LookAt(new Vector3(_player.position.x, transform.position.y, _player.position.z));
    }

    private void AttackPlayer()
    {
        _lastAttackTime = Time.time;
        // This would trigger player damage in a full implementation
        Debug.Log($"{enemyName} attacks the player for {damageAmount} damage!");
    }

    public void TakeDamage(float damage)
    {
        if (_isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;
        Debug.Log($"{enemyName} has been defeated!");
        
        // Notify quest system via singleton
        if (RPGBootstrap.Instance != null)
        {
            RPGBootstrap.Instance.OnEnemyDefeated(enemyName);
        }

        Destroy(gameObject, 1f);
    }
}
