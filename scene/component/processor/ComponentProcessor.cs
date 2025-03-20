using Godot;
using System;
using System.Collections.Generic;
using static ComponentProcessorUnitFactory;
using static DirectionsExtensions;

public class ComponentProcessor
{
    public int RoundCount => _roundCount;
    public const int MAX_ROUND_COUNT = 65536;
    public void Start(Dictionary<Direction, List<MonoPicture>> inputPicts)
    {
        _roundCount = 0;
        GD.Print("round ", _roundCount);
        _outputs.Clear();
        foreach (var (coords, processable) in _processables) processable.Initialize();
        foreach (var (dir, picts) in inputPicts) InputReceived?.Invoke(this, new(dir, picts));
    }
    public void Step()
    {
        _roundCount++;
        GD.Print("round ", _roundCount);
        foreach (var (coords, processable) in _processables) processable.StepInitialize();
        foreach (var (coords, processable) in _processables) processable.StepProcess();
        if (_roundCount >= MAX_ROUND_COUNT || _outputs.Count != 0) Halt();
    }
    public void Halt()
    {
        foreach (var (dir, picts) in _outputs)
            foreach (MonoPicture pict in picts)
                foreach (MonoPicture.MonoColor color in pict.Data)
                    GD.Print(color);
    }

    public ComponentProcessor(Dictionary<Vector2I, ComponentProcessorUnitLabel> units)
    {
        foreach (var (coords, label) in units)
        {
            IComponentProcessable processable = CreateProcessorUnit(label);
            if (processable is ComponentProcessorUnitInput input) this.InputReceived += input.OnInputReceived;
            if (processable is ComponentProcessorUnitOutput output) output.OutputReceived += OnOutputReceived;
            _processables[coords] = processable;
        }
        foreach (var (coords, label) in units)
        {
            DirectedAct(Directions.All, (d) =>
            {
                Vector2I neighborCoords = coords + d.ToVector2I();
                if (_processables.ContainsKey(neighborCoords))
                {
                    IComponentInputable inputable = _processables[neighborCoords].TryGetComponentInputable();
                    if (inputable != null) _processables[coords].SetNeighbor(d, inputable);
                }
            });
        }
    }
    public ComponentProcessor(ComponentPanel panel): this(UnitsFrom(panel)) {}

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private int _roundCount = 0;
    private Dictionary<Vector2I, IComponentProcessable> _processables = new();
    private Dictionary<Direction, List<MonoPicture>> _outputs = new();
    private event EventHandler<PicturesReceivedEventArgs> InputReceived;
    private void OnOutputReceived(object sender, PicturesReceivedEventArgs e)
    {
        if (e.Pictures.Count != 0)
        {
            if (!_outputs.ContainsKey(e.Toward)) _outputs[e.Toward] = new();
            foreach (MonoPicture pict in e.Pictures) _outputs[e.Toward].Add(pict);
        }
    }
    private static Dictionary<Vector2I, ComponentProcessorUnitLabel> UnitsFrom(ComponentPanel panel)
    {
        Dictionary<Vector2I, ComponentProcessorUnitLabel> units = new();
        foreach (var (coords, tile) in panel.GetTiles())
            units.Add(coords - panel.Field.Position, new(tile));
        return units;
    }
}