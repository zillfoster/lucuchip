using Godot;
using System;

public partial class TheGame : Node2D
{
    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private Chip _chip;
    private MouseHandleArea2D _chipMouseHandle;
    private MouseHandleArea2D _paletteMouseHandle;
    private TileMapLayer _cursorLayer;
    private Vector2 _cursorPosition;
    private ChipColor _chosenColor = ChipColor.NONE;
    public override void _Ready()
    {
        base._Ready();
        _chip = (Chip)GetNode<Node2D>("Chip");
        _chipMouseHandle = (MouseHandleArea2D)GetNode<Area2D>("Chip/MouseHandleArea2D");
        _paletteMouseHandle = (MouseHandleArea2D)GetNode<Area2D>("PaletteLayer/MouseHandleArea2D");
        _cursorLayer = GetNode<TileMapLayer>("CursorLayer");
    }
    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        if (Input.IsActionJustPressed("act"))
        {
            if (_chipMouseHandle.IsMouseInsideArea)
            {
                _chip.AssignUnit(GetGlobalMousePosition(), _chosenColor);
                return ;
            }
            else if (_paletteMouseHandle.IsMouseInsideArea)
            {
                int s = _paletteMouseHandle.IsMouseInsideShape.Count;
                for (int i = 0; i < s; i++)
                {
                    if (_paletteMouseHandle.IsMouseInsideShape[i])
                    {
                        _chosenColor = GetPaletteColor(i);
                        break;
                    }
                }
                return ;
            }
        }
        else if (Input.IsActionJustPressed("cancel"))
        {

        }
    }
    private static ChipColor GetPaletteColor(int idx)
    {
        switch(idx)
        {
            case 0:  return ChipColor.BLACK;
            case 1:  return ChipColor.WHITE;
            case 2:  return ChipColor.RED;
            case 3:  return ChipColor.BLUE;
            case 4:  return ChipColor.GREEN;
            case 5:  return ChipColor.YELLOW;
            case 6:  return ChipColor.PURPLE;
            case 7:  return ChipColor.ORANGE;
            default: return ChipColor.NONE;
        }
    }
}
