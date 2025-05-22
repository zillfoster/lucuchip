using Godot;
using System.Collections.Generic;
using System.Linq;

public static class Rect2ILibrary
{
    public static bool Contains(this Rect2I rect, Vector2I coords)
        => rect.HasArea() && rect.HasPoint(coords);
    public static bool Contains(this IList<Rect2I> rects, Vector2I coords)
        => rects.Any(rect => rect.Contains(coords));
    public static bool IsValidIn(this Vector2I? coords, IList<Rect2I> rects)
        => coords == null || rects.Contains(coords.Value);
}