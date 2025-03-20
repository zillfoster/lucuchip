public static class ComponentUnitFactory
{
    public static IComponentProcessable CreateUnitProcessor(ComponentUnitColor unit)
    {
        switch(unit)
        {
            case ComponentUnitColor.Black:  return new ComponentUnitProcessorBlack();
            case ComponentUnitColor.White:  return new ComponentUnitProcessorWhite();
            case ComponentUnitColor.Red:    return new ComponentUnitProcessorRed();
            case ComponentUnitColor.Blue:   return new ComponentUnitProcessorBlue();
            case ComponentUnitColor.Green:  return new ComponentUnitProcessorGreen();
            case ComponentUnitColor.Yellow: return new ComponentUnitProcessorYellow();
            case ComponentUnitColor.Purple: return new ComponentUnitProcessorPurple();
            case ComponentUnitColor.Orange: return new ComponentUnitProcessorOrange();
            case ComponentUnitColor.Input:  return new ComponentUnitProcessorInput();
            case ComponentUnitColor.Output: return new ComponentUnitProcessorOutput();
            default:                        return null;
        }
    }
    public static IComponentProcessable CreateUnitProcessor(ComponentPanelTile unit)
        => CreateUnitProcessor(ComponentUnitFrom(unit));
    
    public static ComponentUnitColor ComponentUnitFrom(ComponentPanelTile unit)
    {
        switch(unit)
        {
            case ComponentPanelTile.Black:  return ComponentUnitColor.Black;
            case ComponentPanelTile.White:  return ComponentUnitColor.White;
            case ComponentPanelTile.Red:    return ComponentUnitColor.Red;
            case ComponentPanelTile.Blue:   return ComponentUnitColor.Blue;
            case ComponentPanelTile.Green:  return ComponentUnitColor.Green;
            case ComponentPanelTile.Yellow: return ComponentUnitColor.Yellow;
            case ComponentPanelTile.Purple: return ComponentUnitColor.Purple;
            case ComponentPanelTile.Orange: return ComponentUnitColor.Orange;
            case ComponentPanelTile.Input:  return ComponentUnitColor.Input;
            case ComponentPanelTile.Output: return ComponentUnitColor.Output;
            default:                        return ComponentUnitColor.None;
        }
    }
}