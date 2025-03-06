using Godot;

public partial class Chip : Node2D
{
    public void AssignUnit(Vector2 pos, ChipColor color)
    {
        if (color == ChipColor.NONE) return ;
        _unitLayer.SetCell(_unitLayer.LocalToMap(ToLocal(pos)), 
                           _chipSourceID,
                           Chip.GetAtlasCoordinate(color));
    }
    
    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private TileMapLayer _unitLayer;
    private int _chipSourceID;
    public override void _Ready()
    {
        base._Ready();
        _unitLayer = GetNode<TileMapLayer>("UnitLayer");
        _chipSourceID = _unitLayer.TileSet.GetSourceId(0);
    }
    private static Vector2I GetAtlasCoordinate(ChipColor color)
    {
        switch(color)
        {
            case ChipColor.BLACK:   return new Vector2I(0, 0);
            case ChipColor.WHITE:   return new Vector2I(1, 0);
            case ChipColor.RED:     return new Vector2I(2, 0);
            case ChipColor.BLUE:    return new Vector2I(3, 0);
            case ChipColor.GREEN:   return new Vector2I(4, 0);
            case ChipColor.YELLOW:  return new Vector2I(5, 0);
            case ChipColor.PURPLE:  return new Vector2I(6, 0);
            case ChipColor.ORANGE:  return new Vector2I(7, 0);
            default:                return new Vector2I(0, 0);
        }
    }
}
