using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour
{
    private PrototypeConfig _config;
    private FactorySim _factorySim;

    // NOTE: AutoBoot disabled to prevent conflict with RPGBootstrap.
    // This project uses the RPG game (RPGBootstrap.cs), not the factory simulation.
    // To re-enable the factory simulation, uncomment the attribute below and
    // comment out the [RuntimeInitializeOnLoadMethod] in RPGBootstrap.cs.
    // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AutoBoot()
    {
        var bootstrap = new GameObject("Bootstrap");
        bootstrap.AddComponent<Bootstrap>();
    }

    private void Awake()
    {
        _config = PrototypeConfig.Load();
    }

    private void Start()
    {
        SetupScene();
        SetupFactory();
        SetupPlayer();
        SetupUi();
    }

    private void SetupScene()
    {
        RenderSettings.ambientLight = new Color(0.35f, 0.35f, 0.42f);

        var lightObj = new GameObject("Directional Light");
        var light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.2f;
        light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "MoonSurface";
        ground.transform.localScale = new Vector3(10f, 1f, 10f);
        var groundRenderer = ground.GetComponent<Renderer>();
        groundRenderer.material.color = new Color(0.55f, 0.55f, 0.6f);
    }

    private void SetupFactory()
    {
        _factorySim = new GameObject("FactorySim").AddComponent<FactorySim>();
        _factorySim.Initialize(_config);

        CreateMachine("Extractor", new Vector3(-6f, 0.5f, 2f), new Vector3(2f, 1f, 2f), new Color(0.2f, 0.8f, 0.8f));
        CreateMachine("Conveyor", new Vector3(-2f, 0.25f, 2f), new Vector3(6f, 0.5f, 1f), new Color(0.8f, 0.6f, 0.2f));
        CreateMachine("Refinery", new Vector3(4f, 0.75f, 2f), new Vector3(2.5f, 1.5f, 2.5f), new Color(0.9f, 0.3f, 0.3f));
    }

    private void CreateMachine(string label, Vector3 position, Vector3 scale, Color color)
    {
        var machine = GameObject.CreatePrimitive(PrimitiveType.Cube);
        machine.name = label;
        machine.transform.position = position;
        machine.transform.localScale = scale;
        var renderer = machine.GetComponent<Renderer>();
        renderer.material.color = color;

        var textObj = new GameObject(label + "Label");
        textObj.transform.position = position + Vector3.up * (scale.y * 0.75f + 0.6f);
        var text = textObj.AddComponent<TextMesh>();
        text.text = label;
        text.characterSize = 0.25f;
        text.fontSize = 64;
        text.color = Color.white;
        text.alignment = TextAlignment.Center;
        text.anchor = TextAnchor.MiddleCenter;
    }

    private void SetupPlayer()
    {
        var player = new GameObject("Player");
        player.transform.position = new Vector3(0f, 1.6f, -8f);

        var capsule = player.AddComponent<CapsuleCollider>();
        capsule.height = 1.8f;
        capsule.radius = 0.3f;

        var rigidbody = player.AddComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        var controller = player.AddComponent<FirstPersonController>();
        controller.MoveSpeed = _config.moveSpeed;
        controller.SprintMultiplier = _config.sprintMultiplier;

        var cameraRoot = new GameObject("CameraRoot");
        cameraRoot.transform.SetParent(player.transform);
        cameraRoot.transform.localPosition = new Vector3(0f, 0.6f, 0f);

        var cam = new GameObject("Main Camera");
        cam.tag = "MainCamera";
        cam.transform.SetParent(cameraRoot.transform);
        cam.transform.localPosition = Vector3.zero;
        cam.AddComponent<Camera>();

        var look = cam.AddComponent<SimpleCameraLook>();
        look.Target = player.transform;
        look.Sensitivity = _config.mouseSensitivity;
    }

    private void SetupUi()
    {
        var canvasObj = new GameObject("HUD");
        var canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        var textObj = new GameObject("StatusText");
        textObj.transform.SetParent(canvasObj.transform);
        var text = textObj.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 18;
        text.alignment = TextAnchor.UpperLeft;
        text.color = Color.white;

        var rect = text.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 1f);
        rect.anchoredPosition = new Vector2(16f, -16f);
        rect.sizeDelta = new Vector2(560f, 200f);

        _factorySim.SetHud(text);
    }
}