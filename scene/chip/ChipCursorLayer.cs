using Godot;

public partial class ChipCursorLayer : TileMapLayer
{
    public void SetCursor(Vector2I coords, ChipUnit style, ChipUnit hovered)
    {
        Clear();
        if (style == ChipUnit.None || style == hovered) return;
        Vector2I atlasCoords = ChipCursorLayer.AtlasCoordsFrom(style);
        if (style == ChipUnit.Erase && hovered == ChipUnit.Red) atlasCoords = _altEraseAtlasCoords;
        SetCell(coords, _sourceID, atlasCoords);
    }

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private int _sourceID;
    public override void _Ready()
    {
        base._Ready();
        _sourceID = TileSet.GetSourceId(0);
    }
    private static Vector2I AtlasCoordsFrom(ChipUnit unit)
    {
        switch(unit)
        {
            case ChipUnit.Black:    return new Vector2I(0, 1);
            case ChipUnit.White:    return new Vector2I(1, 1);
            case ChipUnit.Red:      return new Vector2I(2, 1);
            case ChipUnit.Blue:     return new Vector2I(3, 1);
            case ChipUnit.Green:    return new Vector2I(4, 1);
            case ChipUnit.Yellow:   return new Vector2I(5, 1);
            case ChipUnit.Purple:   return new Vector2I(6, 1);
            case ChipUnit.Orange:   return new Vector2I(7, 1);
            case ChipUnit.Input:    return new Vector2I(0, 3);
            case ChipUnit.Output:   return new Vector2I(1, 3);
            case ChipUnit.Erase:    return new Vector2I(2, 2);
            default:                return new Vector2I(-1, -1);
        }
    }
    private static readonly Vector2I _altEraseAtlasCoords = new(2, 3);
}
