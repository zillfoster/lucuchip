using Godot;

public partial class ChipMainLayer : TileMapLayer
{
    public void AssignUnit(Vector2I coords, ChipUnit style)
    {
        if (style == ChipUnit.None) return;
        SetCell(coords, _sourceID, ChipMainLayer.AtlasCoordsFrom(style));
    }
    public ChipUnit GetUnit(Vector2I coords)
        => ChipMainLayer.UnitFrom(GetCellAtlasCoords(coords));
    
    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private int _sourceID;
    public override void _Ready()
    {
        base._Ready();
        _sourceID = TileSet.GetSourceId(0);
    }
    private static Vector2I AtlasCoordsFrom(ChipUnit style)
    {
        switch(style)
        {
            case ChipUnit.Black:    return new Vector2I(0, 0);
            case ChipUnit.White:    return new Vector2I(1, 0);
            case ChipUnit.Red:      return new Vector2I(2, 0);
            case ChipUnit.Blue:     return new Vector2I(3, 0);
            case ChipUnit.Green:    return new Vector2I(4, 0);
            case ChipUnit.Yellow:   return new Vector2I(5, 0);
            case ChipUnit.Purple:   return new Vector2I(6, 0);
            case ChipUnit.Orange:   return new Vector2I(7, 0);
            case ChipUnit.Input:    return new Vector2I(0, 2);
            case ChipUnit.Output:   return new Vector2I(1, 2);
            default:                return new Vector2I(-1, -1);
        }
    }
    private static ChipUnit UnitFrom(Vector2I atlasCoords)
    {
        switch(atlasCoords)
        {
            case Vector2I(0, 0):   return ChipUnit.Black;
            case Vector2I(1, 0):   return ChipUnit.White;
            case Vector2I(2, 0):   return ChipUnit.Red;
            case Vector2I(3, 0):   return ChipUnit.Blue;
            case Vector2I(4, 0):   return ChipUnit.Green;
            case Vector2I(5, 0):   return ChipUnit.Yellow;
            case Vector2I(6, 0):   return ChipUnit.Purple;
            case Vector2I(7, 0):   return ChipUnit.Orange;
            case Vector2I(0, 2):   return ChipUnit.Input;
            case Vector2I(1, 2):   return ChipUnit.Output;
            default:               return ChipUnit.None;
        }
    }
}
