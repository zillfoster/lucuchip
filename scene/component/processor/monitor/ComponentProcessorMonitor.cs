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
    private Rect2I _field = new(4, 1, 16, 16);
    [Export]
    private ComponentProcessorMonitorActivationGeneralLayer _activationGeneralLayer;
    [Export]
    private ComponentProcessorMonitorActivationSpecialLayer _activationSpecialLayer;
    [Export]
    private ComponentProcessorMonitorDisplayer _displayer;
    [Export]
    private CursorLayer _cursorLayer;
    private readonly Dictionary<Vector2I, ComponentUnitMemory> _memories = [];
    private Vector2I _lastDetailedMemoryCoords = new(-1, -1);
    public override void _Ready()
    {
        base._Ready();
        _cursorLayer.Cursor.Style = new(_cursorLayer.GetSourceId(0), new(2, 4));
        _cursorLayer.Selection.Style = new(_cursorLayer.GetSourceId(0), new(3, 4));
        _cursorLayer.CursorField.Add(_field);
        _cursorLayer.SelectionField.Add(_field);
    }
    void IMouseInputable.OnMouseButton(Vector2 position, MouseButton button, bool isPressed)
    {
        if (!IsMouseEnabled)
        {
            _cursorLayer.Clear();
            return;
        }
        if (button == MouseButton.Right && isPressed)
        {
            IsMouseEnabled = false;
            Initialize();
            _cursorLayer.Clear();
            return;
        }
        ((IMouseInputable)_cursorLayer).OnMouseButton(position, button, isPressed);
    }

    void IMouseInputable.OnMouseMotion(Vector2 position, Vector2 relative, MouseButtonMask mask, MouseButton lastButton, Vector2? lastPosition)
    {
        if (!IsMouseEnabled)
        {
            _cursorLayer.Clear();
            return;
        }
        ((IMouseInputable)_cursorLayer).OnMouseMotion(position, relative, mask, lastButton, lastPosition);
        /* if (_mainLayer.GetActivation(CoordsFrom(position)))
            _popupPanel.ShowAt(_mainLayer.MapToLocal(CoordsFrom(position)));
        else _popupPanel.Hide(); */
    }
    private Vector2I CoordsFrom(Vector2 position)
        => _activationSpecialLayer.LocalToMap(_activationSpecialLayer.ToLocal(position)) / new Vector2I(4, 4);
}