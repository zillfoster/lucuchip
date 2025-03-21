using Godot;

public partial class ComponentPalette : Node2D, IMouseInputable
{
    public ComponentPanel Panel { get; set; } = new();
    public bool IsGridded
    {
        get => _mainLayer.GetChoice(_gridCoords) == ComponentPaletteChoice.GridOn;
        set
        {
            if (value) _mainLayer.AssignChoice(_gridCoords, ComponentPaletteChoice.GridOn);
            else _mainLayer.AssignChoice(_gridCoords, ComponentPaletteChoice.GridOff);
        }
    }

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private Rect2I _field = new(4, 0, 16, 1);
    private Vector2I _gridCoords = new(19, 0);
    [Export]
    private ComponentPaletteBackgroundLayer _backgroundLayer;
    [Export]
    private ComponentPaletteMainLayer _mainLayer;
    [Export]
    private ComponentPaletteCursorLayer _cursorLayer;

    void IMouseInputable.OnMouseButton(Vector2 position, MouseButton button, bool isPressed)
    {
        Vector2I coords = CoordsFrom(position);
        if (!FieldContains(coords)) return;
        if (!isPressed)
        {
            _cursorLayer.SetCursor(coords);
            return;
        }
        ComponentPaletteChoice choice = _mainLayer.GetChoice(coords);
        if (button == MouseButton.Left) switch (choice)
        {
            case ComponentPaletteChoice.Black:
            case ComponentPaletteChoice.White:
            case ComponentPaletteChoice.Red:
            case ComponentPaletteChoice.Blue:
            case ComponentPaletteChoice.Green:
            case ComponentPaletteChoice.Yellow:
            case ComponentPaletteChoice.Purple:
            case ComponentPaletteChoice.Orange:
            case ComponentPaletteChoice.Input:
            case ComponentPaletteChoice.Output:
            case ComponentPaletteChoice.Erase:
                if (_cursorLayer.Selection == coords) return;
                _cursorLayer.Selection = coords;
                Panel.Brush = UnitFrom(choice);
                return;
            case ComponentPaletteChoice.Clear:
                Panel.ClearTile();
                return;
            case ComponentPaletteChoice.GridOn:
                this.IsGridded = false;
                Panel.IsGridded = false;
                return;
            case ComponentPaletteChoice.GridOff:
                this.IsGridded = true;
                Panel.IsGridded = true;
                return;
            case ComponentPaletteChoice.Step:
                if (Panel.Processor == null)
                {
                    Panel.Processor = new(Panel);
                    Panel.Processor.Start(new()
                    {
                        {Direction.Right, new() { new(1, 1, MonoPicture.MonoColor.Black) }}
                    });
                }
                else Panel.Processor.Step();
                return;
            case ComponentPaletteChoice.Play:
            case ComponentPaletteChoice.Speed:
            case ComponentPaletteChoice.Pause:
            case ComponentPaletteChoice.Halt:
            default:
                return;
        }
        else if (button == MouseButton.Right)
        {
            if (Panel.Brush == UnitFrom(choice))
            {
                Panel.Brush = ComponentPanelTile.None;
                _cursorLayer.Selection = null;
                _cursorLayer.SetCursor(coords);
            }
        }
    }
    void IMouseInputable.OnMouseMotion(Vector2 position, Vector2 relative, MouseButtonMask mask, MouseButton lastButton, Vector2? lastPosition)
    {
        Vector2I coords = CoordsFrom(position);
        if (FieldContains(coords)) _cursorLayer.SetCursor(coords);
        else _cursorLayer.Selection = _cursorLayer.Selection;
    }
    private Vector2I CoordsFrom(Vector2 position)
        => _mainLayer.LocalToMap(_mainLayer.ToLocal(position));
    private bool FieldContains(Vector2I coords)
        => _field.HasArea()? _field.HasPoint(coords): false;
    private static ComponentPanelTile UnitFrom(ComponentPaletteChoice choice)
    {
        switch(choice)
        {
            case ComponentPaletteChoice.Black:   return ComponentPanelTile.Black;
            case ComponentPaletteChoice.White:   return ComponentPanelTile.White;
            case ComponentPaletteChoice.Red:     return ComponentPanelTile.Red;
            case ComponentPaletteChoice.Blue:    return ComponentPanelTile.Blue;
            case ComponentPaletteChoice.Green:   return ComponentPanelTile.Green;
            case ComponentPaletteChoice.Yellow:  return ComponentPanelTile.Yellow;
            case ComponentPaletteChoice.Purple:  return ComponentPanelTile.Purple;
            case ComponentPaletteChoice.Orange:  return ComponentPanelTile.Orange;
            case ComponentPaletteChoice.Input:   return ComponentPanelTile.Input;
            case ComponentPaletteChoice.Output:  return ComponentPanelTile.Output;
            case ComponentPaletteChoice.Erase:   return ComponentPanelTile.Erase;
            default:                             return ComponentPanelTile.None;
        }
    }
}
