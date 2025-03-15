using Godot;

public partial class PaletteMainLayer : TileMapLayer
{
    public void AssignChoice(Vector2I coords, PaletteChoice choice)
    {
        if (choice == PaletteChoice.None) return;
        SetCell(coords, _sourceID, PaletteMainLayer.AtlasCoordsFrom(choice));
    }
    public PaletteChoice GetChoice(Vector2I coords)
        => PaletteMainLayer.ChoiceFrom(GetCellAtlasCoords(coords));
    
    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private int _sourceID;
    public override void _Ready()
    {
        base._Ready();
        _sourceID = TileSet.GetSourceId(0);
    }
    private static Vector2I AtlasCoordsFrom(PaletteChoice choice)
    {
        switch(choice)
        {
            case PaletteChoice.Black:       return new Vector2I(0, 0);
            case PaletteChoice.White:       return new Vector2I(1, 0);
            case PaletteChoice.Red:         return new Vector2I(2, 0);
            case PaletteChoice.Blue:        return new Vector2I(3, 0);
            case PaletteChoice.Green:       return new Vector2I(4, 0);
            case PaletteChoice.Yellow:      return new Vector2I(5, 0);
            case PaletteChoice.Purple:      return new Vector2I(6, 0);
            case PaletteChoice.Orange:      return new Vector2I(7, 0);
            case PaletteChoice.Input:       return new Vector2I(0, 2);
            case PaletteChoice.Output:      return new Vector2I(1, 2);
            case PaletteChoice.Erase:       return new Vector2I(2, 2);
            case PaletteChoice.Clear:       return new Vector2I(3, 2);
            case PaletteChoice.GridOn:      return new Vector2I(4, 3);
            case PaletteChoice.GridOff:     return new Vector2I(5, 3);
            case PaletteChoice.Step:        return new Vector2I(4, 2);
            case PaletteChoice.Play:        return new Vector2I(5, 2);
            case PaletteChoice.Speed:       return new Vector2I(6, 2);
            case PaletteChoice.Pause:       return new Vector2I(7, 2);
            case PaletteChoice.Stop:        return new Vector2I(6, 3);
            default:                        return new Vector2I(-1, -1);
        }
    }
    private static PaletteChoice ChoiceFrom(Vector2I atlasCoords)
    {
        switch(atlasCoords)
        {
            case Vector2I(0, 0):    return PaletteChoice.Black;
            case Vector2I(1, 0):    return PaletteChoice.White;
            case Vector2I(2, 0):    return PaletteChoice.Red;
            case Vector2I(3, 0):    return PaletteChoice.Blue;
            case Vector2I(4, 0):    return PaletteChoice.Green;
            case Vector2I(5, 0):    return PaletteChoice.Yellow;
            case Vector2I(6, 0):    return PaletteChoice.Purple;
            case Vector2I(7, 0):    return PaletteChoice.Orange;
            case Vector2I(0, 2):    return PaletteChoice.Input;
            case Vector2I(1, 2):    return PaletteChoice.Output;
            case Vector2I(2, 2):    return PaletteChoice.Erase;
            case Vector2I(3, 2):    return PaletteChoice.Clear;
            case Vector2I(4, 3):    return PaletteChoice.GridOn;
            case Vector2I(5, 3):    return PaletteChoice.GridOff;
            case Vector2I(4, 2):    return PaletteChoice.Step;
            case Vector2I(5, 2):    return PaletteChoice.Play;
            case Vector2I(6, 2):    return PaletteChoice.Speed;
            case Vector2I(7, 2):    return PaletteChoice.Pause;
            case Vector2I(6, 3):    return PaletteChoice.Stop;
            default:                return PaletteChoice.None;
        }
    }
}
