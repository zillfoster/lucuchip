using Godot;
using System.Collections.Generic;

public partial class ComponentProcessorMonitor : Node2D, IMouseInputable
{
    public bool IsMouseEnabled { get; set; } = false;
    public IDictionary<Vector2I, ComponentUnitMemory> Memories => _memories;
    public List<Vector2I> DetailedMemoriesCoords { get; } = [];
    public void Initialize()
    {
        _memories.Clear();
        _activationLayer.Clear();
    }
    public void Refresh()
    {
        foreach (var (coords, memory) in _memories)
            _activationLayer.Activate(coords, memory, DetailedMemoriesCoords.Contains(coords));
        if (IsMouseEnabled) UpdateDetailed(CoordsFrom(GetGlobalMousePosition()));
    }

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    [Export]
    private ComponentProcessorMonitorActivationLayer _activationLayer;
    [Export]
    private ComponentProcessorMonitorDisplayer _displayer;
    private readonly Dictionary<Vector2I, ComponentUnitMemory> _memories = [];
    private Vector2I _lastDetailedMemoryCoords = new(-1, -1);
    void IMouseInputable.OnMouseButton(Vector2 position, MouseButton button, bool isPressed)
    {
        if (!IsMouseEnabled) return;
    }

    void IMouseInputable.OnMouseMotion(Vector2 position, Vector2 relative, MouseButtonMask mask, MouseButton lastButton, Vector2? lastPosition)
    {
        if (!IsMouseEnabled) return;
        UpdateDetailed(CoordsFrom(position));
        /* if (_mainLayer.GetActivation(CoordsFrom(position)))
            _popupPanel.ShowAt(_mainLayer.MapToLocal(CoordsFrom(position)));
        else _popupPanel.Hide(); */
    }
    private Vector2I CoordsFrom(Vector2 position)
        => _activationLayer.LocalToMap(_activationLayer.ToLocal(position)) / new Vector2I(4, 4);
    private void UpdateDetailed(Vector2I coords)
    {
        if (Memories.TryGetValue(_lastDetailedMemoryCoords, out ComponentUnitMemory last))
            _activationLayer.Activate(_lastDetailedMemoryCoords, last, DetailedMemoriesCoords.Contains(_lastDetailedMemoryCoords));
        if (Memories.TryGetValue(coords, out ComponentUnitMemory current))
        {
            _activationLayer.Activate(coords, current, true);
            _lastDetailedMemoryCoords = coords;
        }
    }
}