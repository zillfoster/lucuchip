using Godot;
using System.Collections.Generic;

public partial class MouseHandleArea2D : Area2D
{
    public bool IsMouseInsideArea => _isMouseInsideArea;
    public IReadOnlyList<bool> IsMouseInsideShape => _isMouseInsideShape;
    public int GetCurrentShapeIdx()
    {
        for (int i = 0; i < _childCount; i++) { if (_isMouseInsideShape[i]) return i; }
        return -1;
    }
    // Warning:
    // Disable "IsLeavingProtected" can lead to buggy behavior,
    // because this class can NOT handle any mouse input outside its area.
    // If you really need to do it, you should emit "MouseLeftReleased"
    // and "MouseRightReleased" correctly (outside this class).
    public bool IsLeavingProtected { get; set; } = true;
    [Signal]
    public delegate void MouseLeftPressedEventHandler(Vector2 position);
    [Signal]
    public delegate void MouseRightPressedEventHandler(Vector2 position);
    [Signal]
    public delegate void MouseLeftReleasedEventHandler(Vector2 position);
    [Signal]
    public delegate void MouseRightReleasedEventHandler(Vector2 position);
    [Signal]
    public delegate void MouseDraggedEventHandler(Vector2 position, Vector2 relative);
    [Signal]
    public delegate void MouseLeftDraggedEventHandler(Vector2 position, Vector2 relative);
    [Signal]
    public delegate void MouseRightDraggedEventHandler(Vector2 position, Vector2 relative);


    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private bool _isMouseInsideArea = false;
    private List<bool> _isMouseInsideShape = new();
    private int _childCount;
    private MouseButton _lastPressedButton = MouseButton.None;
    public override void _Ready()
    {
        base._Ready();
        _childCount = GetChildCount();
        for (int i = 0; i < _childCount; i++) _isMouseInsideShape.Add(false);
    }
    private void OnInputEvent(Node viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton button)
        {
            if (button.Pressed)
            {
                _lastPressedButton = button.ButtonIndex;
                switch(button.ButtonIndex)
                {
                    case MouseButton.Left:
                        EmitSignal(SignalName.MouseLeftPressed, button.Position);
                        return;
                    case MouseButton.Right:
                        EmitSignal(SignalName.MouseRightPressed, button.Position);
                        return;
                }
            }
            else
            {
                if (_lastPressedButton == button.ButtonIndex) _lastPressedButton = MouseButton.None;
                switch(button.ButtonIndex)
                {
                    case MouseButton.Left: 
                        EmitSignal(SignalName.MouseLeftReleased, button.Position);
                        return;
                    case MouseButton.Right:
                        EmitSignal(SignalName.MouseRightReleased, button.Position);
                        return;
                }
            }
        }
        else if (@event is InputEventMouseMotion motion)
        {
            
            EmitSignal(SignalName.MouseDragged, motion.Position, motion.Relative);
            switch(_lastPressedButton)
            {
                case MouseButton.Left:
                    EmitSignal(SignalName.MouseLeftDragged, motion.Position, motion.Relative);
                    return;
                case MouseButton.Right:
                    EmitSignal(SignalName.MouseRightDragged, motion.Position, motion.Relative);
                    return;
            }
        }
    }
    private void OnMouseEntered() { _isMouseInsideArea = true; }
    private void OnMouseExited()
    {
        _isMouseInsideArea = false; 
        if (IsLeavingProtected) _lastPressedButton = MouseButton.None;
    }
    private void OnMouseShapeEntered(int shapeIdx) { _isMouseInsideShape[shapeIdx] = true; }
    private void OnMouseShapeExited(int shapeIdx) { _isMouseInsideShape[shapeIdx] = false; }
}
