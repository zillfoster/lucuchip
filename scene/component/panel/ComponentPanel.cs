using Godot;
using static System.Math;
using System.Collections.Generic;

public partial class ComponentPanel : Node2D, IMouseInputable
{
    public ComponentProcessor Processor { get; set; } = null;
    public ComponentProcessorMonitor Monitor { get; set; } = null;
    public bool IsEditable { get; set; } = true;
    public ComponentPanelTile Brush { get; set; } = ComponentPanelTile.None;
    public void DrawTile(Vector2 position)
    {
        if (IsEditable && FieldContains(position))
            _mainLayer.AssignTile(CoordsFrom(position), Brush);
    }
    public void DrawTile(Vector2I coords, ComponentPanelTile tile)
    {
        if (IsEditable && FieldContains(coords))
            _mainLayer.AssignTile(coords, tile);
    }
    public void EraseTile(Vector2 position)
    {
        if (IsEditable && FieldContains(position))
            _mainLayer.AssignTile(CoordsFrom(position), ComponentPanelTile.Erase);
    }
    public void ClearTile() { if (IsEditable) _mainLayer.Clear(); }
    public ComponentPanelTile GetTile(Vector2 position)
        => _mainLayer.GetTile(CoordsFrom(position));
    public Dictionary<Vector2I, ComponentPanelTile> GetTiles()
    {
        Dictionary<Vector2I, ComponentPanelTile> tiles = [];
        foreach (Vector2I coords in _mainLayer.GetUsedCells())
            tiles.Add(coords, _mainLayer.GetTile(coords));
        return tiles;
    }
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
    }
    
    void IMouseInputable.OnMouseButton(Vector2 position, MouseButton button, bool isPressed)
    {
        Vector2I coords = CoordsFrom(position);
        ComponentPanelTile tile = isPressed? GetCurrentBrush(button): Brush;
        if (FieldContains(coords))
        {
            if (isPressed && IsEditable) _mainLayer.AssignTile(coords, tile);
            _cursorLayer.SetCursor(coords, tile, _mainLayer.GetTile(coords));
        }
    }
    void IMouseInputable.OnMouseMotion(Vector2 position, Vector2 relative, MouseButtonMask mask, MouseButton lastButton, Vector2? lastPosition)
    {
        Vector2I coords = CoordsFrom(position);
        ComponentPanelTile brush = GetCurrentBrush(lastButton);
        if (FieldContains(coords))
        {
            if (IsEditable &&
                lastButton != MouseButton.None &&
                lastPosition.HasValue &&
                FieldContains(lastPosition.Value)) DrawThrough(position, relative, brush); 
            _cursorLayer.SetCursor(coords, brush, _mainLayer.GetTile(coords));
        }
        else _cursorLayer.Clear();
    }
    private Vector2I CoordsFrom(Vector2 position)
        => _mainLayer.LocalToMap(_mainLayer.ToLocal(position));
    private ComponentPanelTile GetCurrentBrush(MouseButton button)
        => button == MouseButton.Right? ComponentPanelTile.Erase: Brush;
    private bool FieldContains(Vector2I coords)
        => _field.HasArea()? _field.HasPoint(coords): false;
    private bool FieldContains(Vector2 position)
        => FieldContains(CoordsFrom(position));
    private void DrawThrough(Vector2 position, Vector2 relative, ComponentPanelTile brush)
    {
        int t = (int)Max(Abs(relative.X), Abs(relative.Y)) / _tileLength + 1;
        for (int i = 0; i <= t; i++)
        {
            if (FieldContains(position)) _mainLayer.AssignTile(CoordsFrom(position), brush);
            position += relative / t;
        }
    }
}
