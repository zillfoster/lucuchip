using Godot;
using System.Collections.Generic;

public partial class ComponentProcessorMonitor : Node2D, IMouseInputable
{
    public bool IsMouseEnabled { get; set; } = false;
    public IDictionary<Vector2I, ComponentUnitMemory> Memories => _memories;
    public List<Vector2I> SpecialCoords { get; } = [];
    public void Initialize()
    {
        _memories.Clear();
        _activationGeneralLayer.Clear();
        _activationSpecialLayer.Clear();
    }
    public void Refresh()
    {
        foreach (var (coords, memory) in _memories)
            _activationGeneralLayer.ActivateTile(coords, memory);
        foreach (var coords in SpecialCoords)
            if (Memories.TryGetValue(coords, out ComponentUnitMemory memory))
                _activationSpecialLayer.ActivateTile(coords, memory);
    }

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    [Export]
    private ComponentProcessorMonitorActivationGeneralLayer _activationGeneralLayer;
    [Export]
    private ComponentProcessorMonitorActivationSpecialLayer _activationSpecialLayer;
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
        /* if (_mainLayer.GetActivation(CoordsFrom(position)))
            _popupPanel.ShowAt(_mainLayer.MapToLocal(CoordsFrom(position)));
        else _popupPanel.Hide(); */
    }
    private Vector2I CoordsFrom(Vector2 position)
        => _activationSpecialLayer.LocalToMap(_activationSpecialLayer.ToLocal(position)) / new Vector2I(4, 4);
}