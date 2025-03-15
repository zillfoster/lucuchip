using Godot;
using static System.Math;
using System.Collections.Generic;

public partial class ComponentPanel : Node2D, IMouseInputable
{
    public Rect2I Field
    {
        get => _field;
        set
        {
            _field = value;
            _backgroundLayer.SetBackground(_field, _isGridded);
            foreach (Vector2I coords in _mainLayer.GetUsedCells())
                if (!Field.HasPoint(coords)) _mainLayer.EraseCell(coords);
        }
    }
    public bool IsGridded
    {
        get => _isGridded;
        set 
        {
            _isGridded = value;
            _backgroundLayer.SetBackground(_field, _isGridded);
        }
    }
    public bool IsEditable { get; set; } = true;
    public ComponentPanelUnit ChosenDrawStyle { get; set; } = ComponentPanelUnit.None;
    public void DrawUnit(Vector2 position)
    {
        if (IsEditable && FieldContains(position))
            _mainLayer.AssignUnit(CoordsFrom(position), ChosenDrawStyle);
    }
    public void EraseUnit(Vector2 position)
    {
        if (IsEditable && FieldContains(position))
            _mainLayer.AssignUnit(CoordsFrom(position), ComponentPanelUnit.Erase);
    }
    public void ClearUnit() { if (IsEditable) _mainLayer.Clear(); }
    public ComponentPanelUnit GetUnit(Vector2 position)
        => _mainLayer.GetUnit(CoordsFrom(position));
    public Dictionary<Vector2I, ComponentPanelUnit> GetUnits()
    {
        Dictionary<Vector2I, ComponentPanelUnit> units = new();
        foreach (Vector2I coords in _mainLayer.GetUsedCells())
            units.Add(coords, _mainLayer.GetUnit(coords));
        return units;
    }

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    [Export]
    private Rect2I _field = new(0, 0, 0, 0);
    [Export]
    private bool _isGridded = true;
    [Export]
    private ComponentPanelBackgroundLayer _backgroundLayer;
    [Export]
    private ComponentPanelMainLayer _mainLayer;
    [Export]
    private ComponentPanelCursorLayer _cursorLayer;
    private int _tileLength;
    public override void _Ready()
    {
        base._Ready();

        _tileLength = _mainLayer.TileSet.TileSize.X;

        Variant? v = GameSaver.Load("IsGridded");
        if (v.HasValue) IsGridded = (bool)v;
    }
    
    void IMouseInputable.OnMouseButton(Vector2 position, MouseButton button, bool isPressed)
    {
        Vector2I coords = CoordsFrom(position);
        ComponentPanelUnit style = isPressed? GetCurrentStyle(button): ChosenDrawStyle;
        if (FieldContains(coords))
        {
            if (isPressed && IsEditable) _mainLayer.AssignUnit(coords, style);
            _cursorLayer.SetCursor(coords, style, _mainLayer.GetUnit(coords));
        }
    }
    void IMouseInputable.OnMouseMotion(Vector2 position, Vector2 relative, MouseButtonMask mask, MouseButton lastButton, Vector2? lastPosition)
    {
        Vector2I coords = CoordsFrom(position);
        ComponentPanelUnit style = GetCurrentStyle(lastButton);
        if (FieldContains(coords))
        {
            if (IsEditable &&
                lastButton != MouseButton.None &&
                lastPosition.HasValue &&
                FieldContains(lastPosition.Value)) DrawThrough(position, relative, style); 
            _cursorLayer.SetCursor(coords, style, _mainLayer.GetUnit(coords));
        }
        else _cursorLayer.Clear();
    }
    private Vector2I CoordsFrom(Vector2 position)
        => _mainLayer.LocalToMap(_mainLayer.ToLocal(position));
    private ComponentPanelUnit GetCurrentStyle(MouseButton button)
        => button == MouseButton.Right? ComponentPanelUnit.Erase: ChosenDrawStyle;
    private bool FieldContains(Vector2I coords)
        => _field.HasArea()? _field.HasPoint(coords): false;
    private bool FieldContains(Vector2 position)
        => FieldContains(CoordsFrom(position));
    private void DrawThrough(Vector2 position, Vector2 relative, ComponentPanelUnit style)
    {
        int t = (int)Max(Abs(relative.X), Abs(relative.Y)) / _tileLength + 1;
        for (int i = 0; i <= t; i++)
        {
            if (FieldContains(position)) _mainLayer.AssignUnit(CoordsFrom(position), style);
            position += relative / t;
        }
    }
}
