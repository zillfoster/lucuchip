using Godot;
using static System.Math;

public partial class Chip : Node2D, IMouseInputable
{
    public Rect2I Field
    {
        get => _field;
        set { _field = value; _backgroundLayer.SetBackground(_field, _isGridded); }
    }
    public bool IsGridded
    {
        get => _isGridded;
        set { _isGridded = value; _backgroundLayer.SetBackground(_field, _isGridded); }
    }
    public ChipUnit ChosenDrawStyle { get; set; } = ChipUnit.None;
    public void ClearUnit()
        => _mainLayer.Clear();

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    [Export]
    private Rect2I _field = new(0, 0, 0, 0);
    [Export]
    private bool _isGridded = true;
    [Export]
    private ChipBackgroundLayer _backgroundLayer;
    [Export]
    private ChipMainLayer _mainLayer;
    [Export]
    private ChipCursorLayer _cursorLayer;
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
        ChipUnit style = isPressed? GetCurrentStyle(button): ChosenDrawStyle;
        if (FieldContains(coords))
        {
            if (isPressed) _mainLayer.AssignUnit(coords, style);
            _cursorLayer.SetCursor(coords, style, _mainLayer.GetUnit(coords));
        }
    }
    void IMouseInputable.OnMouseMotion(Vector2 position, Vector2 relative, MouseButtonMask mask, MouseButton lastButton, Vector2? lastPosition)
    {
        Vector2I coords = CoordsFrom(position);
        ChipUnit style = GetCurrentStyle(lastButton);
        if (FieldContains(coords))
        {
            if (lastButton != MouseButton.None && 
                lastPosition.HasValue &&
                FieldContains(lastPosition.Value)) DrawThrough(position, relative, style); 
            _cursorLayer.SetCursor(coords, style, _mainLayer.GetUnit(coords));
        }
        else _cursorLayer.Clear();
    }
    private Vector2I CoordsFrom(Vector2 position)
        => _mainLayer.LocalToMap(_mainLayer.ToLocal(position));
    private ChipUnit GetCurrentStyle(MouseButton button)
        => button == MouseButton.Right? ChipUnit.Erase: ChosenDrawStyle;
    private bool FieldContains(Vector2I coords)
        => _field.HasArea()? _field.HasPoint(coords): false;
    private bool FieldContains(Vector2 position)
        => FieldContains(CoordsFrom(position));
    private void DrawThrough(Vector2 position, Vector2 relative, ChipUnit style)
    {
        int t = (int)Max(Abs(relative.X), Abs(relative.Y)) / _tileLength + 1;
        for (int i = 0; i <= t; i++)
        {
            if (FieldContains(position)) _mainLayer.AssignUnit(CoordsFrom(position), style);
            position += relative / t;
        }
    }
}
