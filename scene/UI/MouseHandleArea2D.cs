using Godot;
using System.Collections.Generic;

public partial class MouseHandleArea2D : Area2D
{
    public bool IsMouseInsideArea => _isMouseInsideArea;
    public IReadOnlyList<bool> IsMouseInsideShape => _isMouseInsideShape;

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private bool _isMouseInsideArea = false;
    private List<bool> _isMouseInsideShape = new();
    public override void _Ready()
    {
        base._Ready();
        int childCount = GetChildCount();
        for (int i = 0; i < childCount; i++) _isMouseInsideShape.Add(false);
    }
    public void OnMouseEntered() { _isMouseInsideArea = true; }
    public void OnMouseExited() { _isMouseInsideArea = false; }
    public void OnMouseShapeEntered(int shapeIdx) { _isMouseInsideShape[shapeIdx] = true; }
    public void OnMouseShapeExited(int shapeIdx) { _isMouseInsideShape[shapeIdx] = false; }
}
