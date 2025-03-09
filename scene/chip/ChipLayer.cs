using Godot;

public partial class ChipLayer : TileMapLayer
{
    public void AssignUnit(Vector2 position, ChipColor color)
    {
        if (color == ChipColor.None) return;
        SetCell(LocalToMap(ToLocal(position)),
                _sourceID,
                ChipLayer.AtlasCoordinateFrom(color));
    }
    public void EraseUnit(Vector2 position)
        => EraseCell(LocalToMap(ToLocal(position)));
    public ChipColor GetUnit(Vector2 position)
        => ChipLayer.ColorFrom(GetCellAtlasCoords(LocalToMap(ToLocal(position))));
    public void ClearUnit()
        => Clear();
    
    
    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private static int _sourceID;
    public override void _Ready()
    {
        base._Ready();
        _sourceID = TileSet.GetSourceId(0);
    }
    private static Vector2I AtlasCoordinateFrom(ChipColor color)
    {
        switch(color)
        {
            case ChipColor.Black:   return new Vector2I(0, 0);
            case ChipColor.White:   return new Vector2I(1, 0);
            case ChipColor.Red:     return new Vector2I(2, 0);
            case ChipColor.Blue:    return new Vector2I(3, 0);
            case ChipColor.Green:   return new Vector2I(4, 0);
            case ChipColor.Yellow:  return new Vector2I(5, 0);
            case ChipColor.Purple:  return new Vector2I(6, 0);
            case ChipColor.Orange:  return new Vector2I(7, 0);
            case ChipColor.Erase:   return new Vector2I(-1, -1);
            default:                return new Vector2I(-1, -1);
        }
    }
    private static ChipColor ColorFrom(Vector2I coordinate)
    {
        switch(coordinate)
        {
            case Vector2I(0, 0):   return ChipColor.Black;
            case Vector2I(1, 0):   return ChipColor.White;
            case Vector2I(2, 0):   return ChipColor.Red;
            case Vector2I(3, 0):   return ChipColor.Blue;
            case Vector2I(4, 0):   return ChipColor.Green;
            case Vector2I(5, 0):   return ChipColor.Yellow;
            case Vector2I(6, 0):   return ChipColor.Purple;
            case Vector2I(7, 0):   return ChipColor.Orange;
            default:               return ChipColor.None;
        }
    }
}
