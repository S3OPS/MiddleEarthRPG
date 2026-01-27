using UnityEngine;
using MiddleEarth.Config;

/// <summary>
/// Active combat system with mouse-based attacks, combos, and special abilities
/// </summary>
public class CombatSystem : MonoBehaviour
{
    [Header("Combat Stats")]
    public float baseDamage = 20f;
    public float attackRange = 3f;
    public float attackCooldown = 0.5f;
    public float comboWindow = 1.5f;
    
    [Header("Special Abilities")]
    public float specialAbilityCooldown = 5f;
    public float specialAbilityDamage = 50f;
    public float specialAbilityStaminaCost = 30f;
    
    private CharacterStats _stats;
    private Camera _mainCamera;
    private float _lastAttackTime;
    private float _lastComboTime;
    private int _comboCount;
    private float _lastSpecialTime;
    
    public int ComboCount => _comboCount;
    public bool CanAttack => Time.time >= _lastAttackTime + attackCooldown;
    public bool CanUseSpecial => Time.time >= _lastSpecialTime + specialAbilityCooldown;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    
    public void Initialize(CharacterStats stats)
    {
        _stats = stats;
    }
    
    private void Update()
    {
        // Reset combo if too much time has passed
        if (Time.time > _lastComboTime + comboWindow)
        {
            _comboCount = 0;
        }
        
        // Basic attack on left mouse
        if (Input.GetMouseButtonDown(0) && CanAttack)
        {
            PerformAttack();
        }
        
        // Special ability on right mouse
        if (Input.GetMouseButtonDown(1) && CanUseSpecial)
        {
            PerformSpecialAbility();
        }
    }
    
    private void PerformAttack()
    {
        // Null safety check
        if (_mainCamera == null)
        {
            Debug.LogWarning("CombatSystem: Main camera reference is null");
            return;
        }
        
        _lastAttackTime = Time.time;
        _lastComboTime = Time.time;
        _comboCount++;
        
        // Calculate damage with combo bonus using constants
        float damage = baseDamage + (_stats != null ? _stats.strength * GameConstants.STRENGTH_DAMAGE_MULTIPLIER : 0f);
        float comboMultiplier = 1f + (_comboCount - 1) * GameConstants.COMBO_DAMAGE_BONUS;
        damage *= comboMultiplier;
        
        // Raycast to find target
        Ray ray = _mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, attackRange))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                bool isCritical = Random.value < GameConstants.CRITICAL_HIT_BASE_CHANCE + (_stats != null ? _stats.agility * 0.01f : 0f);
                if (isCritical)
                {
                    damage *= GameConstants.CRITICAL_HIT_DAMAGE_MULTIPLIER;
                }
                
                enemy.TakeDamage(damage);
                
                // Trigger visual effects
                if (EffectsManager.Instance != null)
                {
                    EffectsManager.Instance.PlayHitEffect(hit.point, isCritical);
                    EffectsManager.Instance.ShowDamageNumber(hit.point, damage, isCritical);
                }
                
                // Play attack sound
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayAttackSound(isCritical);
                }
                
                Debug.Log($"Hit {enemy.enemyName} for {damage:0} damage! {(isCritical ? "CRITICAL!" : "")} Combo x{_comboCount}");
            }
        }
        else
        {
            // Missed - play swing sound
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySwingSound();
            }
        }
    }
    
    private void PerformSpecialAbility()
    {
        if (_stats == null || _stats.currentStamina < specialAbilityStaminaCost)
        {
            Debug.Log("Not enough stamina for special ability!");
            return;
        }
        
        _lastSpecialTime = Time.time;
        _stats.UseStamina(specialAbilityStaminaCost);
        
        // AOE attack in front of player using constants
        Collider[] hitColliders = Physics.OverlapSphere(
            transform.position + transform.forward * 2f, 
            GameConstants.SPECIAL_AOE_RADIUS
        );
        int enemiesHit = 0;
        
        foreach (var collider in hitColliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                float damage = specialAbilityDamage + (_stats.wisdom * 3f);
                enemy.TakeDamage(damage);
                enemiesHit++;
                
                // Trigger visual effects
                if (EffectsManager.Instance != null)
                {
                    EffectsManager.Instance.PlaySpecialAbilityEffect(collider.transform.position);
                    EffectsManager.Instance.ShowDamageNumber(collider.transform.position, damage, true);
                }
            }
        }
        
        // Play special ability sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySpecialAbilitySound();
        }
        
        Debug.Log($"Special Ability! Hit {enemiesHit} enemies for {specialAbilityDamage:0} damage each!");
    }
    
    public void ResetCombo()
    {
        _comboCount = 0;
    }
}
