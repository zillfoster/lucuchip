using Godot;
using System.Collections;
using System.Collections.Generic;

public partial class MouseInputHandler : Node2D, IEnumerable<IMouseInputable>
{
    public bool IsEnable { get; set; } = true;
    public List<IMouseInputable> Inputables { get; } = new();

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private MouseButton _lastButton = MouseButton.None;
    private Vector2? _lastPosition = null;
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventMouseButton button)
        {
            if (button.Pressed)
            {
                _lastButton = button.ButtonIndex;
                _lastPosition = button.Position;
            }
            else if (_lastButton == button.ButtonIndex)
            {
                _lastButton = MouseButton.None;
                _lastPosition = null;
            }
            foreach (IMouseInputable inputable in Inputables)
                inputable.OnMouseButton(button.Position, button.ButtonIndex, button.Pressed);
            return;
        }
        else if (@event is InputEventMouseMotion motion)
        {
            foreach (IMouseInputable inputable in Inputables)
                inputable.OnMouseMotion(motion.Position, motion.Relative, motion.ButtonMask, _lastButton, _lastPosition);
            return;
        }
    }
    
    IEnumerator<IMouseInputable> IEnumerable<IMouseInputable>.GetEnumerator()
        => Inputables.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => Inputables.GetEnumerator();
    public void Add(IMouseInputable inputable)
        => Inputables.Add(inputable);
}