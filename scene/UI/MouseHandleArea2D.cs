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
    public delegate void MouseDraggedEventHandler(Vector2 position);
    [Signal]
    public delegate void MouseLeftDraggedEventHandler(Vector2 position);
    [Signal]
    public delegate void MouseRightDraggedEventHandler(Vector2 position);

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
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed)
            {
                _lastPressedButton = mouseButton.ButtonIndex;
                switch(mouseButton.ButtonIndex)
                {
                    case MouseButton.Left: 
                        EmitSignal(SignalName.MouseLeftPressed, mouseButton.Position);
                        return ;
                    case MouseButton.Right:
                        EmitSignal(SignalName.MouseRightPressed, mouseButton.Position);
                        return ;
                }
            }
            else
            {
                if (_lastPressedButton == mouseButton.ButtonIndex) _lastPressedButton = MouseButton.None;
                switch(mouseButton.ButtonIndex)
                {
                    case MouseButton.Left: 
                        EmitSignal(SignalName.MouseLeftReleased, mouseButton.Position);
                        return ;
                    case MouseButton.Right:
                        EmitSignal(SignalName.MouseRightReleased, mouseButton.Position);
                        return ;
                }
            }
        }
        else if (@event is InputEventMouseMotion mouseMotion)
        {
            EmitSignal(SignalName.MouseDragged, mouseMotion.Position);
            switch(_lastPressedButton)
            {
                case MouseButton.Left:
                    EmitSignal(SignalName.MouseLeftDragged, mouseMotion.Position);
                    return ;
                case MouseButton.Right:
                    EmitSignal(SignalName.MouseRightDragged, mouseMotion.Position);
                    return ;
            }
        }
    }
    private void OnMouseEntered() { _isMouseInsideArea = true; }
    private void OnMouseExited() { _isMouseInsideArea = false; }
    private void OnMouseShapeEntered(int shapeIdx) { _isMouseInsideShape[shapeIdx] = true; }
    private void OnMouseShapeExited(int shapeIdx) { _isMouseInsideShape[shapeIdx] = false; }
}
