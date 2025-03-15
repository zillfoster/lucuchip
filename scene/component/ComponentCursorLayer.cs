using Godot;

public partial class ComponentCursorLayer : TileMapLayer
{
    public void SetCursor(Vector2I coords, ComponentUnit style, ComponentUnit hovered)
    {
        Clear();
        if (style == ComponentUnit.None || style == hovered) return;
        Vector2I atlasCoords = ComponentCursorLayer.AtlasCoordsFrom(style);
        if (style == ComponentUnit.Erase && hovered == ComponentUnit.Red) atlasCoords = _altEraseAtlasCoords;
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
    private static Vector2I AtlasCoordsFrom(ComponentUnit unit)
    {
        switch(unit)
        {
            case ComponentUnit.Black:    return new Vector2I(0, 1);
            case ComponentUnit.White:    return new Vector2I(1, 1);
            case ComponentUnit.Red:      return new Vector2I(2, 1);
            case ComponentUnit.Blue:     return new Vector2I(3, 1);
            case ComponentUnit.Green:    return new Vector2I(4, 1);
            case ComponentUnit.Yellow:   return new Vector2I(5, 1);
            case ComponentUnit.Purple:   return new Vector2I(6, 1);
            case ComponentUnit.Orange:   return new Vector2I(7, 1);
            case ComponentUnit.Input:    return new Vector2I(0, 3);
            case ComponentUnit.Output:   return new Vector2I(1, 3);
            case ComponentUnit.Erase:    return new Vector2I(2, 2);
            default:                return new Vector2I(-1, -1);
        }
    }
    private static readonly Vector2I _altEraseAtlasCoords = new(2, 3);
}
