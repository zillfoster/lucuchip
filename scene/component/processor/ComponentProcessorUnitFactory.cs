using static ComponentProcessorUnitLabel;

public static class ComponentProcessorUnitFactory
{
    public static IComponentProcessable CreateProcessorUnit(ComponentProcessorUnitLabel label)
    {
        switch(label.Type)
        {
            case ComponentProcessorUnitLabelType.Black:  return new ComponentProcessorUnitBlack();
            case ComponentProcessorUnitLabelType.White:  return new ComponentProcessorUnitWhite();
            case ComponentProcessorUnitLabelType.Red:    return new ComponentProcessorUnitRed();
            case ComponentProcessorUnitLabelType.Blue:   return new ComponentProcessorUnitBlue();
            case ComponentProcessorUnitLabelType.Green:  return new ComponentProcessorUnitGreen();
            case ComponentProcessorUnitLabelType.Yellow: return new ComponentProcessorUnitYellow();
            case ComponentProcessorUnitLabelType.Purple: return new ComponentProcessorUnitPurple();
            case ComponentProcessorUnitLabelType.Orange: return new ComponentProcessorUnitOrange();
            case ComponentProcessorUnitLabelType.Input:  return new ComponentProcessorUnitInput();
            case ComponentProcessorUnitLabelType.Output: return new ComponentProcessorUnitOutput();
            // case ComponentProcessorUnitLabelType.Component: ...
            default:                                     return null;
        }
    }
}