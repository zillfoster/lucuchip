using Godot;
using System;
using System.Collections.Generic;
using static ComponentProcessorUnitFactory;
using static DirectionsExtensions;

public partial class ComponentProcessor: Node2D
{
    public ComponentProcessorMonitor Monitor { get; set; } = null;
    public int RoundCount => _roundCount;
    public const int MAX_ROUND_COUNT = 65536;
    public void Start(Dictionary<Direction, List<MonoPicture>> inputPicts)
    {
        DirectSetStatus(ProcessorStatus.Pausing);
        _roundCount = 0;
        _outputs.Clear();
        Monitor.ClearMemory();
        foreach (var (coords, processable) in _processables) processable.Initialize();
        foreach (var (dir, picts) in inputPicts) InputReceived?.Invoke(this, new(dir, picts));
    }
    public void Step()
    {
        if (Status == ProcessorStatus.Halting) return;
        _roundCount++;
        foreach (var (coords, processable) in _processables) processable.StepInitialize();
        foreach (var (coords, processable) in _processables) processable.StepProcess();

        Monitor.ClearMemory();
        foreach (var (coords, processable) in _processables) Monitor.AssignMemory(coords, processable.GetCurrentMemory());
        if (_roundCount == 1)
        {
            foreach (var (coords, processable) in _processables)
                if (processable is ComponentProcessorUnitInput input)
                    Monitor.AssignActivation(coords, true);
        }

        if (_roundCount >= MAX_ROUND_COUNT || _outputs.Count != 0)
        {
            SetStatus(ProcessorStatus.Halting);
            foreach (var (dir, picts) in _outputs)
                foreach (MonoPicture pict in picts)
                    foreach (MonoPicture.MonoColor color in pict.Data)
                        GD.Print(color);
        }
    }
    public ProcessorStatus Status { get; private set; } = ProcessorStatus.Halting;
    public enum ProcessorStatus { Pausing, Stepping, Processing, Speeding, Halting }
    public event EventHandler Halted;
    public void SetStatus(ProcessorStatus status)
    {
        if (Status == ProcessorStatus.Halting) return;
        Status = status;
        switch (status)
        {
            case ProcessorStatus.Pausing:
                _timer.Stop();
                return;
            case ProcessorStatus.Halting:
                _timer.Stop();
                Halted?.Invoke(this, default);
                return;
            case ProcessorStatus.Stepping:
                Step();
                return;
            case ProcessorStatus.Processing:
                _timer.WaitTime = 0.25;
                _timer.Start();
                return;
            case ProcessorStatus.Speeding:
                _timer.WaitTime = 0.08;
                _timer.Start();
                return;
            default:
                return;
        }
    }

    public ComponentProcessor(Dictionary<Vector2I, ComponentProcessorUnitLabel> units)
    {
        foreach (var (coords, label) in units)
        {
            IComponentProcessable processable = CreateProcessorUnit(label);
            if (processable is ComponentProcessorUnitInput input) InputReceived += input.OnInputReceived;
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
        _timer.OneShot = false;
        _timer.Timeout += Step;
        AddChild(_timer);
    }
    public ComponentProcessor(ComponentPanel panel): this(UnitsFrom(panel)) {}

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private int _roundCount = 0;
    private Timer _timer = new();
    private readonly Dictionary<Vector2I, IComponentProcessable> _processables = [];
    private readonly Dictionary<Direction, List<MonoPicture>> _outputs = [];
    private event EventHandler<PicturesReceivedEventArgs> InputReceived;
    private void OnOutputReceived(object sender, PicturesReceivedEventArgs e)
    {
        if (e.Pictures.Count != 0)
        {
            if (!_outputs.ContainsKey(e.Toward)) _outputs[e.Toward] = [];
            foreach (MonoPicture pict in e.Pictures) _outputs[e.Toward].Add(pict);
        }
    }
    private void DirectSetStatus(ProcessorStatus status) => Status = status;
    private static Dictionary<Vector2I, ComponentProcessorUnitLabel> UnitsFrom(ComponentPanel panel)
    {
        Dictionary<Vector2I, ComponentProcessorUnitLabel> units = [];
        foreach (var (coords, tile) in panel.GetTiles())
            units.Add(coords, new(tile));
        return units;
    }
}