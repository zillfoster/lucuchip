using Godot;

public partial class MyOptionButton : OptionButton
{
    [Signal]
    public delegate void InputEventEventHandler(Node viewport, InputEvent @event);
    public override void _Ready()
    {
        base._Ready();
        GetPopup().WindowInput += (@event) => 
            EmitSignal(SignalName.InputEvent, GetPopup(), @event);
    }
}
