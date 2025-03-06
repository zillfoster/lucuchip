using Godot;

public partial class Chip : Node2D
{
    public void AssignUnit(Vector2 position, ChipColor color)
    {
        if (color == ChipColor.None) { EraseUnit(position); return; }
        _unitLayer.SetCell(_unitLayer.LocalToMap(ToLocal(position)), 
                           _chipSourceID,
                           Chip.AtlasCoordinateFrom(color));
    }
    public void EraseUnit(Vector2 position)
        => _unitLayer.EraseCell(_unitLayer.LocalToMap(ToLocal(position)));
    
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
            default:                return new Vector2I(0, 0);
        }
    }
}
