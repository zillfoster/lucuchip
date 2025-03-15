using Godot;

public partial class Component : Node2D
{
    [Export]
    private ComponentPanel _panel;
    [Export]
    private ComponentPalette _palette;
    public override void _Ready()
    {
        base._Ready();

        Input.UseAccumulatedInput = false;

        GameSaver.Save("IsGridded", () => _panel.IsGridded);

        _palette.Panels.Add(_panel);

        AddChild(new MouseInputHandler() {_panel, _palette});
    }
}
