using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple minimap system for navigation
/// </summary>
public class MinimapSystem : MonoBehaviour
{
    public static MinimapSystem Instance { get; private set; }
    
    private Camera _minimapCamera;
    private RenderTexture _minimapTexture;
    private RawImage _minimapImage;
    private Transform _playerTransform;
    
    private const int MINIMAP_SIZE = 200;
    private const float MINIMAP_HEIGHT = 50f;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    public void Initialize(Transform playerTransform, Canvas hudCanvas)
    {
        _playerTransform = playerTransform;
        SetupMinimapCamera();
        SetupMinimapUI(hudCanvas);
    }
    
    private void SetupMinimapCamera()
    {
        var cameraObj = new GameObject("MinimapCamera");
        cameraObj.transform.SetParent(transform);
        
        _minimapCamera = cameraObj.AddComponent<Camera>();
        _minimapCamera.orthographic = true;
        _minimapCamera.orthographicSize = 30f;
        _minimapCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        _minimapCamera.clearFlags = CameraClearFlags.SolidColor;
        _minimapCamera.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        _minimapCamera.cullingMask = LayerMask.GetMask("Default");
        _minimapCamera.depth = 1; // Render after main camera
        
        _minimapTexture = new RenderTexture(MINIMAP_SIZE, MINIMAP_SIZE, 16);
        _minimapCamera.targetTexture = _minimapTexture;
    }
    
    private void SetupMinimapUI(Canvas hudCanvas)
    {
        // Minimap background
        var minimapContainer = new GameObject("MinimapContainer");
        minimapContainer.transform.SetParent(hudCanvas.transform);
        
        var containerRect = minimapContainer.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(1, 1);
        containerRect.anchorMax = new Vector2(1, 1);
        containerRect.pivot = new Vector2(1, 1);
        containerRect.anchoredPosition = new Vector2(-20, -20);
        containerRect.sizeDelta = new Vector2(MINIMAP_SIZE + 10, MINIMAP_SIZE + 10);
        
        var bgImage = minimapContainer.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.7f);
        
        // Minimap image
        var minimapObj = new GameObject("Minimap");
        minimapObj.transform.SetParent(minimapContainer.transform);
        
        var minimapRect = minimapObj.AddComponent<RectTransform>();
        minimapRect.anchorMin = new Vector2(0.5f, 0.5f);
        minimapRect.anchorMax = new Vector2(0.5f, 0.5f);
        minimapRect.pivot = new Vector2(0.5f, 0.5f);
        minimapRect.anchoredPosition = Vector2.zero;
        minimapRect.sizeDelta = new Vector2(MINIMAP_SIZE, MINIMAP_SIZE);
        
        _minimapImage = minimapObj.AddComponent<RawImage>();
        _minimapImage.texture = _minimapTexture;
        
        // Title
        var titleObj = new GameObject("MinimapTitle");
        titleObj.transform.SetParent(minimapContainer.transform);
        
        var titleText = titleObj.AddComponent<Text>();
        titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        titleText.text = "MAP";
        titleText.fontSize = 14;
        titleText.fontStyle = FontStyle.Bold;
        titleText.color = Color.white;
        titleText.alignment = TextAnchor.UpperCenter;
        
        var titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 1);
        titleRect.anchorMax = new Vector2(0.5f, 1);
        titleRect.pivot = new Vector2(0.5f, 1);
        titleRect.anchoredPosition = new Vector2(0, -5);
        titleRect.sizeDelta = new Vector2(MINIMAP_SIZE, 20);
    }
    
    private void LateUpdate()
    {
        if (_playerTransform != null && _minimapCamera != null)
        {
            // Follow player position
            Vector3 minimapPosition = _playerTransform.position;
            minimapPosition.y = MINIMAP_HEIGHT;
            _minimapCamera.transform.position = minimapPosition;
        }
    }
    
    public void SetZoom(float orthographicSize)
    {
        if (_minimapCamera != null)
        {
            _minimapCamera.orthographicSize = Mathf.Clamp(orthographicSize, 10f, 50f);
        }
    }
}
