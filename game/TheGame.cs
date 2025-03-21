using Godot;
using System;

public partial class TheGame : Node2D
{
    [Export]
    private Component _component;

    public override void _Ready()
    {
        base._Ready();
        GameSaver.LoadAndSave(_component);
    }
}
