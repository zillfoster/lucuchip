using Godot;

public partial class TheGame : Node2D
{
    private Chip _chip;
    private MouseHandleArea2D _paletteMouseHandle;
    private ChipColor _chosenColor = ChipColor.None;
    public override void _Ready()
    {
        base._Ready();
        _chip = (Chip)GetNode<Node2D>("Chip");
        MouseHandleArea2D chipMouseHandle = (MouseHandleArea2D)GetNode<Area2D>("Chip/MouseHandleArea2D");
        chipMouseHandle.MouseLeftPressed += (position) =>
            _chip.AssignUnit(GetGlobalMousePosition(), _chosenColor);
        chipMouseHandle.MouseRightPressed += (position) =>
            _chip.EraseUnit(GetGlobalMousePosition());
        MouseHandleArea2D paletteMouseHandle = (MouseHandleArea2D)GetNode<Area2D>("PaletteLayer/MouseHandleArea2D");
        paletteMouseHandle.MouseLeftPressed += (position) =>
            _chosenColor = ColorFrom(paletteMouseHandle.GetCurrentShapeIdx());
        paletteMouseHandle.MouseRightPressed += (position) =>
        {
            if (_chosenColor == ColorFrom(paletteMouseHandle.GetCurrentShapeIdx()))
                _chosenColor = ChipColor.None;
        };
    }
    private static ChipColor ColorFrom(int idx)
    {
        switch(idx)
        {
            case 0:  return ChipColor.Black;
            case 1:  return ChipColor.White;
            case 2:  return ChipColor.Red;
            case 3:  return ChipColor.Blue;
            case 4:  return ChipColor.Green;
            case 5:  return ChipColor.Yellow;
            case 6:  return ChipColor.Purple;
            case 7:  return ChipColor.Orange;
            default: return ChipColor.None;
        }
    }
}
