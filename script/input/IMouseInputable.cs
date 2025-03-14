using Godot;

public interface IMouseInputable
{
    void OnMouseButton(Vector2 position, MouseButton button, bool isPressed);
    void OnMouseMotion(Vector2 position, Vector2 relative, MouseButtonMask mask, MouseButton lastButton, Vector2? lastPosition);
}