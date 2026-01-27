using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Visual effects manager for particles and UI feedback
/// </summary>
public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance { get; private set; }
    
    [Header("Prefab References")]
    private GameObject _damageNumberPrefab;
    private Canvas _worldCanvas;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        SetupWorldCanvas();
    }
    
    private void SetupWorldCanvas()
    {
        var canvasObj = new GameObject("WorldCanvas");
        canvasObj.transform.SetParent(transform);
        _worldCanvas = canvasObj.AddComponent<Canvas>();
        _worldCanvas.renderMode = RenderMode.WorldSpace;
        canvasObj.AddComponent<CanvasScaler>();
    }
    
    /// <summary>
    /// Create a simple particle effect using GameObjects
    /// </summary>
    public void PlayHitEffect(Vector3 position, bool isCritical)
    {
        int particleCount = isCritical ? 15 : 8;
        Color particleColor = isCritical ? new Color(1f, 0.3f, 0f) : new Color(1f, 0f, 0f);
        
        for (int i = 0; i < particleCount; i++)
        {
            CreateParticle(position, particleColor, 0.5f);
        }
    }
    
    public void PlaySpecialAbilityEffect(Vector3 position)
    {
        // Blue/purple magical particles
        for (int i = 0; i < 20; i++)
        {
            CreateParticle(position, new Color(0.5f, 0.3f, 1f), 0.8f);
        }
    }
    
    public void PlayLevelUpEffect(Vector3 position)
    {
        // Golden ascending particles
        for (int i = 0; i < 30; i++)
        {
            CreateAscendingParticle(position, new Color(1f, 0.9f, 0.3f), 1.5f);
        }
    }
    
    public void PlayTreasureEffect(Vector3 position)
    {
        // Sparkle effect
        for (int i = 0; i < 12; i++)
        {
            CreateParticle(position, new Color(1f, 1f, 0.5f), 0.6f);
        }
    }
    
    public void PlayQuestCompleteEffect(Vector3 position)
    {
        // Triumphant burst
        for (int i = 0; i < 25; i++)
        {
            CreateParticle(position, new Color(0.3f, 1f, 0.3f), 1f);
        }
    }
    
    private void CreateParticle(Vector3 position, Color color, float lifetime)
    {
        GameObject particle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        particle.transform.position = position;
        particle.transform.localScale = Vector3.one * Random.Range(0.05f, 0.15f);
        
        var renderer = particle.GetComponent<Renderer>();
        renderer.material.color = color;
        
        // Add particle behavior
        var particleScript = particle.AddComponent<SimpleParticle>();
        particleScript.Initialize(
            Random.insideUnitSphere * Random.Range(2f, 5f),
            lifetime,
            color
        );
        
        Destroy(particle.GetComponent<Collider>());
    }
    
    private void CreateAscendingParticle(Vector3 position, Color color, float lifetime)
    {
        GameObject particle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        particle.transform.position = position;
        particle.transform.localScale = Vector3.one * Random.Range(0.08f, 0.2f);
        
        var renderer = particle.GetComponent<Renderer>();
        renderer.material.color = color;
        
        // Add particle behavior with upward bias
        var particleScript = particle.AddComponent<SimpleParticle>();
        Vector3 velocity = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(3f, 6f),
            Random.Range(-1f, 1f)
        );
        particleScript.Initialize(velocity, lifetime, color);
        
        Destroy(particle.GetComponent<Collider>());
    }
    
    /// <summary>
    /// Show floating damage numbers
    /// </summary>
    public void ShowDamageNumber(Vector3 worldPosition, float damage, bool isCritical)
    {
        GameObject numberObj = new GameObject("DamageNumber");
        numberObj.transform.SetParent(_worldCanvas.transform);
        numberObj.transform.position = worldPosition + Vector3.up * 1.5f;
        
        var text = numberObj.AddComponent<Text>();
        text.text = damage.ToString("0");
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = isCritical ? 48 : 36;
        text.fontStyle = isCritical ? FontStyle.Bold : FontStyle.Normal;
        text.color = isCritical ? new Color(1f, 0.5f, 0f) : Color.white;
        text.alignment = TextAnchor.MiddleCenter;
        
        var rectTransform = numberObj.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(200, 100);
        
        // Add floating animation
        var floater = numberObj.AddComponent<FloatingText>();
        floater.Initialize(1.5f);
    }
    
    public void ShowHealNumber(Vector3 worldPosition, float healAmount)
    {
        GameObject numberObj = new GameObject("HealNumber");
        numberObj.transform.SetParent(_worldCanvas.transform);
        numberObj.transform.position = worldPosition + Vector3.up * 1.5f;
        
        var text = numberObj.AddComponent<Text>();
        text.text = "+" + healAmount.ToString("0");
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 32;
        text.color = new Color(0.3f, 1f, 0.3f);
        text.alignment = TextAnchor.MiddleCenter;
        
        var rectTransform = numberObj.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(200, 100);
        
        var floater = numberObj.AddComponent<FloatingText>();
        floater.Initialize(1.2f);
    }
}

/// <summary>
/// Simple particle behavior
/// </summary>
public class SimpleParticle : MonoBehaviour
{
    private Vector3 _velocity;
    private float _lifetime;
    private float _startTime;
    private Color _startColor;
    private Renderer _renderer;
    
    public void Initialize(Vector3 velocity, float lifetime, Color startColor)
    {
        _velocity = velocity;
        _lifetime = lifetime;
        _startTime = Time.time;
        _startColor = startColor;
        _renderer = GetComponent<Renderer>();
    }
    
    private void Update()
    {
        float elapsed = Time.time - _startTime;
        float t = elapsed / _lifetime;
        
        if (t >= 1f)
        {
            Destroy(gameObject);
            return;
        }
        
        // Apply velocity with gravity
        transform.position += _velocity * Time.deltaTime;
        _velocity.y -= 9.8f * Time.deltaTime; // Gravity
        _velocity *= 0.95f; // Air resistance
        
        // Fade out
        Color color = _startColor;
        color.a = 1f - t;
        _renderer.material.color = color;
        
        // Shrink
        float scale = 1f - (t * 0.5f);
        transform.localScale = Vector3.one * scale * 0.1f;
    }
}

/// <summary>
/// Floating text animation
/// </summary>
public class FloatingText : MonoBehaviour
{
    private float _lifetime;
    private float _startTime;
    private Vector3 _startPosition;
    private Text _text;
    
    public void Initialize(float lifetime)
    {
        _lifetime = lifetime;
        _startTime = Time.time;
        _startPosition = transform.position;
        _text = GetComponent<Text>();
    }
    
    private void Update()
    {
        float elapsed = Time.time - _startTime;
        float t = elapsed / _lifetime;
        
        if (t >= 1f)
        {
            Destroy(gameObject);
            return;
        }
        
        // Float upward
        transform.position = _startPosition + Vector3.up * (t * 2f);
        
        // Face camera
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0);
        }
        
        // Fade out
        if (_text != null)
        {
            Color color = _text.color;
            color.a = 1f - t;
            _text.color = color;
        }
    }
}
