using Godot;
using System.Collections.Generic;

public partial class Palette : Node2D, IMouseInputable
{
    public List<Component> Components { get; } = new();

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private Rect2I _field = new(4, 0, 16, 1);
    [Export]
    private PaletteBackgroundLayer _backgroundLayer;
    [Export]
    private PaletteMainLayer _mainLayer;
    [Export]
    private PaletteCursorLayer _cursorLayer;
    private static readonly Vector2I _gridCoords = new(19, 0);
    public override void _Ready()
    {
        base._Ready();

        Variant? v = GameSaver.Load("IsGridded");
        if (v.HasValue)
        {
            if ((bool)v) _mainLayer.AssignChoice(_gridCoords, PaletteChoice.GridOn);
            else _mainLayer.AssignChoice(_gridCoords, PaletteChoice.GridOff);
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
        PaletteChoice choice = _mainLayer.GetChoice(coords);
        if (button == MouseButton.Left) switch (choice)
        {
            case PaletteChoice.Black:
            case PaletteChoice.White:
            case PaletteChoice.Red:
            case PaletteChoice.Blue:
            case PaletteChoice.Green:
            case PaletteChoice.Yellow:
            case PaletteChoice.Purple:
            case PaletteChoice.Orange:
            case PaletteChoice.Input:
            case PaletteChoice.Output:
            case PaletteChoice.Erase:
                if (_cursorLayer.Selection == coords) return;
                _cursorLayer.Selection = coords;
                foreach (Component comp in Components) comp.ChosenDrawStyle = UnitFrom(choice);
                return;
            case PaletteChoice.Clear:
                foreach (Component comp in Components) comp.ClearUnit();
                return;
            case PaletteChoice.GridOn:
                _mainLayer.AssignChoice(coords, PaletteChoice.GridOff);
                foreach (Component comp in Components) comp.IsGridded = false;
                return;
            case PaletteChoice.GridOff:
                _mainLayer.AssignChoice(coords, PaletteChoice.GridOn);
                foreach (Component comp in Components) comp.IsGridded = true;
                return;
            case PaletteChoice.Step:
            case PaletteChoice.Play:
            case PaletteChoice.Speed:
            case PaletteChoice.Pause:
            case PaletteChoice.Stop:
            default:
                return;
        }
        else if (button == MouseButton.Right)
        {
            foreach (Component comp in Components)
            {
                if (comp.ChosenDrawStyle == UnitFrom(choice))
                {
                    comp.ChosenDrawStyle = ComponentUnit.None;
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
    private static ComponentUnit UnitFrom(PaletteChoice choice)
    {
        switch(choice)
        {
            case PaletteChoice.Black:   return ComponentUnit.Black;
            case PaletteChoice.White:   return ComponentUnit.White;
            case PaletteChoice.Red:     return ComponentUnit.Red;
            case PaletteChoice.Blue:    return ComponentUnit.Blue;
            case PaletteChoice.Green:   return ComponentUnit.Green;
            case PaletteChoice.Yellow:  return ComponentUnit.Yellow;
            case PaletteChoice.Purple:  return ComponentUnit.Purple;
            case PaletteChoice.Orange:  return ComponentUnit.Orange;
            case PaletteChoice.Input:   return ComponentUnit.Input;
            case PaletteChoice.Output:  return ComponentUnit.Output;
            case PaletteChoice.Erase:   return ComponentUnit.Erase;
            default:                    return ComponentUnit.None;  
        }
    }
}
