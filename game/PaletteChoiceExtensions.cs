public static class PaletteChoiceExtensions
{
    public static ChipColor ToChipColor(this PaletteChoice choice)
    {
        switch(choice)
        {
            case PaletteChoice.Black:   return ChipColor.Black;
            case PaletteChoice.White:   return ChipColor.White;
            case PaletteChoice.Red:     return ChipColor.Red;
            case PaletteChoice.Blue:    return ChipColor.Blue;
            case PaletteChoice.Green:   return ChipColor.Green;
            case PaletteChoice.Yellow:  return ChipColor.Yellow;
            case PaletteChoice.Purple:  return ChipColor.Purple;
            case PaletteChoice.Orange:  return ChipColor.Orange;
            case PaletteChoice.Erase:   return ChipColor.Erase;
            default:                    return ChipColor.None;  
        }
    }
    public static PaletteChoice ToPaletteChoice(this ChipColor color)
    {
        switch(color)
        {
            case ChipColor.Black:   return PaletteChoice.Black;
            case ChipColor.White:   return PaletteChoice.White;
            case ChipColor.Red:     return PaletteChoice.Red;
            case ChipColor.Blue:    return PaletteChoice.Blue;
            case ChipColor.Green:   return PaletteChoice.Green;
            case ChipColor.Yellow:  return PaletteChoice.Yellow;
            case ChipColor.Purple:  return PaletteChoice.Purple;
            case ChipColor.Orange:  return PaletteChoice.Orange;
            case ChipColor.Erase:   return PaletteChoice.Erase;
            default:                return PaletteChoice.None;  
        }
    }
}