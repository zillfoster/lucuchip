using Godot;
using System;
using System.Collections.Generic;

public struct ComponentProcessorUnitLabel
{
    public enum ComponentProcessorUnitLabelType { None, Black, White, Red, Blue, Green, Yellow, Purple, Orange, Input, Output, Component }
    public readonly ComponentProcessorUnitLabelType Type = ComponentProcessorUnitLabelType.None;
    public readonly IReadOnlyDictionary<Vector2I, ComponentProcessorUnitLabel> ContainingLabels = null;

    public ComponentProcessorUnitLabel(ComponentProcessorUnitLabelType type, Dictionary<Vector2I, ComponentProcessorUnitLabel> containingLabels = null)
    {
        Type = type;
        if (type == ComponentProcessorUnitLabelType.Component)
            ContainingLabels = new Dictionary<Vector2I, ComponentProcessorUnitLabel>(containingLabels);
    }
    public ComponentProcessorUnitLabel(ComponentPanelTile tile)
        => Type = TypeFrom(tile);
    
    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private static ComponentProcessorUnitLabelType TypeFrom(ComponentPanelTile unit)
    {
        switch(unit)
        {
            case ComponentPanelTile.Black:  return ComponentProcessorUnitLabelType.Black;
            case ComponentPanelTile.White:  return ComponentProcessorUnitLabelType.White;
            case ComponentPanelTile.Red:    return ComponentProcessorUnitLabelType.Red;
            case ComponentPanelTile.Blue:   return ComponentProcessorUnitLabelType.Blue;
            case ComponentPanelTile.Green:  return ComponentProcessorUnitLabelType.Green;
            case ComponentPanelTile.Yellow: return ComponentProcessorUnitLabelType.Yellow;
            case ComponentPanelTile.Purple: return ComponentProcessorUnitLabelType.Purple;
            case ComponentPanelTile.Orange: return ComponentProcessorUnitLabelType.Orange;
            case ComponentPanelTile.Input:  return ComponentProcessorUnitLabelType.Input;
            case ComponentPanelTile.Output: return ComponentProcessorUnitLabelType.Output;
            default:                        return ComponentProcessorUnitLabelType.None;
        }
    }

    public static bool operator ==(ComponentProcessorUnitLabel a, ComponentProcessorUnitLabel b)
    {
        if (a.Type != ComponentProcessorUnitLabelType.Component) return a.Type == b.Type;
        if (b.Type != ComponentProcessorUnitLabelType.Component) return false;
        foreach (var (coords, label) in a.ContainingLabels)
            if (!b.ContainingLabels.ContainsKey(coords) ||
                (b.ContainingLabels[coords] != a.ContainingLabels[coords]))
                return false;
        return true;
    }
    public static bool operator !=(ComponentProcessorUnitLabel a, ComponentProcessorUnitLabel b)
        => !(a == b);
    public override bool Equals(object obj)
    {
        if (obj is ComponentProcessorUnitLabel other) return this == other;
        return false;
    }
    public override int GetHashCode()
    {
        HashCode code = new();
        code.Add(Type);
        if (Type == ComponentProcessorUnitLabelType.Component)
            foreach (var (coords, label) in ContainingLabels)
            {
                code.Add(coords);
                code.Add(label.GetHashCode());
            }
        return code.ToHashCode();
    }
}