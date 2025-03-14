using Godot;

public partial class TheGame : Node2D
{
    [Export]
    private Chip _chip;
    [Export]
    private Palette _palette;
    public override void _Ready()
    {
        base._Ready();

        Input.UseAccumulatedInput = false;

        GameSaver.Save("IsGridded", () => _chip.IsGridded);

        _palette.Chips.Add(_chip);

        AddChild(new MouseInputHandler() {_chip, _palette});
    }
}
