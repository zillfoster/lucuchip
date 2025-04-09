using Godot;
using System.Collections.Generic;

public partial class ComponentProcessorMonitor : Node2D
{
    // To-do list:
    // 1. Activate corresponding tile when unit is activated
    // 2. setup popup-menu
    // 3. make ComponentProcessorMonitor implement IMouseInputable
    public void AssignActivation(Vector2I coords, ComponentUnitMemory memory)
    {
        
    }

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    [Export]
    private ComponentProcessorMonitorMainLayer _mainLayer;
}