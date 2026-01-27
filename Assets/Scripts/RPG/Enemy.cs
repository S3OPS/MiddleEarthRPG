using UnityEngine;
using MiddleEarth.Config;

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
    public float moveSpeed = 2f;
    public float fleeHealthPercent = 0.2f;

    private Transform _player;
    private float _lastAttackTime;
    private bool _isDead = false;
    private bool _isFleeing = false;
    private Vector3 _patrolPoint;
    private Vector3 _spawnPosition;
    private float _nextPatrolTime;

    private void Start()
    {
        if (RPGBootstrap.Instance != null)
        {
            _player = RPGBootstrap.Instance.PlayerTransform;
        }
        _spawnPosition = transform.position;
        SetNewPatrolPoint();
    }

    private void Update()
    {
        if (_isDead || _player == null) return;

        // Use squared distance to avoid expensive sqrt operations (~2000/sec eliminated)
        float sqrDistanceToPlayer = (transform.position - _player.position).sqrMagnitude;
        float sqrAttackRange = attackRange * attackRange;
        float sqrDetectionRange = detectionRange * detectionRange;
        
        // Flee if health is low
        if (currentHealth <= maxHealth * fleeHealthPercent && !_isFleeing)
        {
            _isFleeing = true;
        }
        
        if (_isFleeing)
        {
            FleeFromPlayer();
        }
        else if (sqrDistanceToPlayer <= sqrAttackRange && Time.time >= _lastAttackTime + attackCooldown)
        {
            AttackPlayer();
        }
        else if (sqrDistanceToPlayer <= sqrDetectionRange)
        {
            FollowPlayer();
        }
        else
        {
            Patrol();
        }
    }
    
    private void Patrol()
    {
        if (Time.time >= _nextPatrolTime)
        {
            // Use squared distance for patrol point check
            if ((transform.position - _patrolPoint).sqrMagnitude < GameConstants.PATROL_REACH_DISTANCE_SQR)
            {
                SetNewPatrolPoint();
            }
            
            Vector3 direction = (_patrolPoint - transform.position).normalized;
            direction.y = 0;
            transform.position += direction * (moveSpeed * 0.5f) * Time.deltaTime;
            
            if (direction != Vector3.zero)
            {
                transform.LookAt(new Vector3(_patrolPoint.x, transform.position.y, _patrolPoint.z));
            }
        }
    }
    
    private void SetNewPatrolPoint()
    {
        Vector2 randomCircle = Random.insideUnitCircle * 5f;
        _patrolPoint = _spawnPosition + new Vector3(randomCircle.x, 0, randomCircle.y);
        _nextPatrolTime = Time.time + Random.Range(2f, 5f);
    }
    
    private void FleeFromPlayer()
    {
        Vector3 direction = (transform.position - _player.position).normalized;
        direction.y = 0;
        transform.position += direction * moveSpeed * 1.5f * Time.deltaTime;
        transform.LookAt(new Vector3(transform.position.x + direction.x, transform.position.y, transform.position.z + direction.z));
    }

    private void FollowPlayer()
    {
        Vector3 direction = (_player.position - transform.position).normalized;
        direction.y = 0;
        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.LookAt(new Vector3(_player.position.x, transform.position.y, _player.position.z));
    }

    private void AttackPlayer()
    {
        _lastAttackTime = Time.time;
        Debug.Log($"{enemyName} attacks the player for {damageAmount} damage!");
        
        // Play attack sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("hit", 0.4f);
        }
    }

    public void TakeDamage(float damage)
    {
        if (_isDead) return;

        currentHealth -= damage;
        
        // Visual feedback
        StartCoroutine(FlashRed());
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private System.Collections.IEnumerator FlashRed()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Color originalColor = renderer.material.color;
            renderer.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            renderer.material.color = originalColor;
        }
    }

    private void Die()
    {
        _isDead = true;
        Debug.Log($"{enemyName} has been defeated!");
        
        // Play death effects
        if (EffectsManager.Instance != null)
        {
            EffectsManager.Instance.PlayHitEffect(transform.position, false);
        }
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayEnemyDeathSound();
        }
        
        // Notify quest system via singleton
        if (RPGBootstrap.Instance != null)
        {
            RPGBootstrap.Instance.OnEnemyDefeated(enemyName);
        }

        Destroy(gameObject, 1f);
    }
}
