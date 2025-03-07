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
    
    // If consistent behavior is needed when leaving this Area2D, call OnOutsideInputEvent.
    public void OnOutsideInputEvent(InputEvent @event)
    {
        if (_isMouseInsideArea) return;
        _isHandledOutside = true;
        if (@event is InputEventMouseButton button && 
            !button.Pressed && 
            _outsideLastButton == button.ButtonIndex) _outsideLastButton = MouseButton.None;
    }


    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private bool _isMouseInsideArea = false;
    private List<bool> _isMouseInsideShape = new();
    private int _childCount;
    private MouseButton _lastButton = MouseButton.None;
    private bool _isHandledOutside = false;
    private MouseButton _outsideLastButton = MouseButton.None;
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
                _lastButton = button.ButtonIndex;
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
                if (_lastButton == button.ButtonIndex) _lastButton = MouseButton.None;
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
            switch(_lastButton)
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
    private void OnMouseEntered()
    {
        _isMouseInsideArea = true;
        if (_isHandledOutside) _lastButton = _outsideLastButton;
    }
    private void OnMouseExited()
    {
        _isMouseInsideArea = false;
        _outsideLastButton = _lastButton;
        _lastButton = MouseButton.None;
    }
    private void OnMouseShapeEntered(int shapeIdx) { _isMouseInsideShape[shapeIdx] = true; }
    private void OnMouseShapeExited(int shapeIdx) { _isMouseInsideShape[shapeIdx] = false; }
}
