using Godot;

public partial class TheGame : Node2D
{
    [Export]
    private Component _component;
    [Export]
    private Palette _palette;
    public override void _Ready()
    {
        base._Ready();

        Input.UseAccumulatedInput = false;

        GameSaver.Save("IsGridded", () => _component.IsGridded);

        _palette.Components.Add(_component);

        AddChild(new MouseInputHandler() {_component, _palette});
    }
}
