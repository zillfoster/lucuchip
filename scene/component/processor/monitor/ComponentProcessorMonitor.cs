using Godot;
using System.Collections.Generic;

public partial class ComponentProcessorMonitor : Node2D
{
    // To-do list:
    // 1. setup popup-menu
    // 2. make ComponentProcessorMonitor implement IMouseInputable
    public void AssignMemory(Vector2I coords, ComponentUnitMemory memory)
    {
        _memories[coords] = memory;
        _mainLayer.AssignActivation(coords, (memory.InputAccum | memory.OutputAccum) != Directions.None);
    }
    public void RemoveMemory(Vector2I coords)
    {
        _memories.Remove(coords);
        _mainLayer.AssignActivation(coords, false);
    }
    public void ClearMemory()
    {
        _memories.Clear();
        _mainLayer.Clear();
    }
    public void AssignActivation(Vector2I coords, bool isActivated)
        => _mainLayer.AssignActivation(coords, isActivated);

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    [Export]
    private ComponentProcessorMonitorMainLayer _mainLayer;
    private Dictionary<Vector2I, ComponentUnitMemory> _memories = new();
}