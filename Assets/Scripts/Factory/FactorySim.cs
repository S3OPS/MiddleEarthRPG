using UnityEngine;
using UnityEngine.UI;

public class FactorySim : MonoBehaviour
{
    private PrototypeConfig _config;
    private Text _hud;
    private float _ore;
    private float _buffer;
    private float _ingots;

    public void Initialize(PrototypeConfig config)
    {
        _config = config;
        _ore = config.startOre;
        _ingots = config.startIngots;
    }

    public void SetHud(Text hud)
    {
        _hud = hud;
    }

    private void Update()
    {
        if (_config == null)
        {
            return;
        }

        var dt = Time.deltaTime;
        _ore += _config.extractorRate * dt;

        var transfer = Mathf.Min(_ore, _config.conveyorRate * dt);
        _ore -= transfer;
        _buffer += transfer;

        var refine = Mathf.Min(_buffer, _config.refineryRate * dt);
        _buffer -= refine;
        _ingots += refine;

        if (_hud != null)
        {
            _hud.text =
                "Lunar Crust Prototype\n" +
                "Extractor → Conveyor → Refinery\n\n" +
                $"Ore: {_ore:0.0}\n" +
                $"Refinery Buffer: {_buffer:0.0}\n" +
                $"Ingots: {_ingots:0.0}\n\n" +
                "WASD Move | Mouse Look | Shift Sprint | Space Jump";
        }
    }
}