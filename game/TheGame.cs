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
            _chip.AssignUnit(position, _chosenColor);
        chipMouseHandle.MouseRightPressed += (position) =>
            _chip.EraseUnit(position);
        chipMouseHandle.MouseLeftDragged += (position) =>
            _chip.AssignUnit(position, _chosenColor);
        chipMouseHandle.MouseRightDragged += (position) =>
            _chip.EraseUnit(position);
        MouseHandleArea2D paletteMouseHandle = (MouseHandleArea2D)GetNode<Area2D>("PaletteLayer/MouseHandleArea2D");
        paletteMouseHandle.MouseLeftPressed += (position) =>
        {
            ChipColor color = TheGame.ColorFrom(paletteMouseHandle.GetCurrentShapeIdx());
            if (color == ChipColor.Clear)
            {
                _chip.ClearUnit();
                return;
            }
            _chosenColor = color;
        };
        paletteMouseHandle.MouseRightPressed += (position) =>
        {
            if (_chosenColor == TheGame.ColorFrom(paletteMouseHandle.GetCurrentShapeIdx()))
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
            case 8:  return ChipColor.Erase;
            case 9:  return ChipColor.Clear;
            default: return ChipColor.None;
        }
    }
}
