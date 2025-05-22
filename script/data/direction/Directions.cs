using System;

[Flags]
public enum Directions
{
    None = 0,
    Up = 1 << 0,
    Left = 1 << 1,
    Down = 1 << 2,
    Right = 1 << 3,
    Vertical = Up | Down,
    Horizontal = Left | Right,
    All = Up | Left | Down | Right,
}
