using Godot;
using System.Collections.Generic;

public partial class ComponentPalette : Node2D, IMouseInputable
{
    public List<ComponentPanel> Panels { get; } = new();

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private Rect2I _field = new(4, 0, 16, 1);
    [Export]
    private ComponentPaletteBackgroundLayer _backgroundLayer;
    [Export]
    private ComponentPaletteMainLayer _mainLayer;
    [Export]
    private ComponentPaletteCursorLayer _cursorLayer;
    private static readonly Vector2I _gridCoords = new(19, 0);
    public override void _Ready()
    {
        base._Ready();

        Variant? v = GameSaver.Load("IsGridded");
        if (v.HasValue)
        {
            if ((bool)v) _mainLayer.AssignChoice(_gridCoords, ComponentPaletteChoice.GridOn);
            else _mainLayer.AssignChoice(_gridCoords, ComponentPaletteChoice.GridOff);
        }
    }

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
                foreach (ComponentPanel panel in Panels) panel.ChosenDrawStyle = UnitFrom(choice);
                return;
            case ComponentPaletteChoice.Clear:
                foreach (ComponentPanel panel in Panels) panel.ClearUnit();
                return;
            case ComponentPaletteChoice.GridOn:
                _mainLayer.AssignChoice(coords, ComponentPaletteChoice.GridOff);
                foreach (ComponentPanel panel in Panels) panel.IsGridded = false;
                return;
            case ComponentPaletteChoice.GridOff:
                _mainLayer.AssignChoice(coords, ComponentPaletteChoice.GridOn);
                foreach (ComponentPanel panel in Panels) panel.IsGridded = true;
                return;
            case ComponentPaletteChoice.Step:
            case ComponentPaletteChoice.Play:
            case ComponentPaletteChoice.Speed:
            case ComponentPaletteChoice.Pause:
            case ComponentPaletteChoice.Stop:
            default:
                return;
        }
        else if (button == MouseButton.Right)
        {
            foreach (ComponentPanel panel in Panels)
            {
                if (panel.ChosenDrawStyle == UnitFrom(choice))
                {
                    panel.ChosenDrawStyle = ComponentPanelUnit.None;
                    _cursorLayer.Selection = null;
                    _cursorLayer.SetCursor(coords);
                }
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
    private static ComponentPanelUnit UnitFrom(ComponentPaletteChoice choice)
    {
        switch(choice)
        {
            case ComponentPaletteChoice.Black:   return ComponentPanelUnit.Black;
            case ComponentPaletteChoice.White:   return ComponentPanelUnit.White;
            case ComponentPaletteChoice.Red:     return ComponentPanelUnit.Red;
            case ComponentPaletteChoice.Blue:    return ComponentPanelUnit.Blue;
            case ComponentPaletteChoice.Green:   return ComponentPanelUnit.Green;
            case ComponentPaletteChoice.Yellow:  return ComponentPanelUnit.Yellow;
            case ComponentPaletteChoice.Purple:  return ComponentPanelUnit.Purple;
            case ComponentPaletteChoice.Orange:  return ComponentPanelUnit.Orange;
            case ComponentPaletteChoice.Input:   return ComponentPanelUnit.Input;
            case ComponentPaletteChoice.Output:  return ComponentPanelUnit.Output;
            case ComponentPaletteChoice.Erase:   return ComponentPanelUnit.Erase;
            default:                    return ComponentPanelUnit.None;  
        }
    }
}
