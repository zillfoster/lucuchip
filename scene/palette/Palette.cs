using Godot;
using System.Collections.Generic;

public partial class Palette : Node2D, IMouseInputable
{
    public List<Chip> Chips { get; } = new();

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
                foreach (Chip chip in Chips) chip.ChosenDrawStyle = UnitFrom(choice);
                return;
            case PaletteChoice.Clear:
                foreach (Chip chip in Chips) chip.ClearUnit();
                return;
            case PaletteChoice.GridOn:
                _mainLayer.AssignChoice(coords, PaletteChoice.GridOff);
                foreach (Chip chip in Chips) chip.IsGridded = false;
                return;
            case PaletteChoice.GridOff:
                _mainLayer.AssignChoice(coords, PaletteChoice.GridOn);
                foreach (Chip chip in Chips) chip.IsGridded = true;
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
            foreach (Chip chip in Chips)
            {
                if (chip.ChosenDrawStyle == UnitFrom(choice))
                {
                    chip.ChosenDrawStyle = ChipUnit.None;
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
    private static ChipUnit UnitFrom(PaletteChoice choice)
    {
        switch(choice)
        {
            case PaletteChoice.Black:   return ChipUnit.Black;
            case PaletteChoice.White:   return ChipUnit.White;
            case PaletteChoice.Red:     return ChipUnit.Red;
            case PaletteChoice.Blue:    return ChipUnit.Blue;
            case PaletteChoice.Green:   return ChipUnit.Green;
            case PaletteChoice.Yellow:  return ChipUnit.Yellow;
            case PaletteChoice.Purple:  return ChipUnit.Purple;
            case PaletteChoice.Orange:  return ChipUnit.Orange;
            case PaletteChoice.Input:   return ChipUnit.Input;
            case PaletteChoice.Output:  return ChipUnit.Output;
            case PaletteChoice.Erase:   return ChipUnit.Erase;
            default:                    return ChipUnit.None;  
        }
    }
}
