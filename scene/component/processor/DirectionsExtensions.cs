using System;
using Godot;

public static class DirectionsExtensions
{
    public static Directions ToDirections(this Direction dir)
        => dir == Direction.Up?      Directions.Up:
           dir == Direction.Left?    Directions.Left:
           dir == Direction.Down?    Directions.Down:
           dir == Direction.Right?   Directions.Right:
           Directions.None;
    public static Direction ToOppositeDirection(this Direction dir)
        => dir == Direction.Up?      Direction.Down:
           dir == Direction.Left?    Direction.Right:
           dir == Direction.Down?    Direction.Up:
           dir == Direction.Right?   Direction.Left:
           Direction.None;
    public static Vector2I ToVector2I(this Direction dir)
        => dir == Direction.Up?     Vector2I.Up:
           dir == Direction.Left?   Vector2I.Left:
           dir == Direction.Down?   Vector2I.Down:
           dir == Direction.Right?  Vector2I.Right: 
           Vector2I.Zero;
    
    public static void DirectedAct(Directions dirs, Action<Direction> action)
    {
        if (dirs.HasFlag(Directions.Up))    action(Direction.Up);
        if (dirs.HasFlag(Directions.Left))  action(Direction.Left);
        if (dirs.HasFlag(Directions.Down))  action(Direction.Down);
        if (dirs.HasFlag(Directions.Right)) action(Direction.Right);
    }
}