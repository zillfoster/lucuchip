using Godot;

public partial class ComponentPanelCursorLayer : TileMapLayer
{
    public void SetCursor(Vector2I coords, ComponentPanelTile chosen, ComponentPanelTile hovering)
    {
        Clear();
        if (chosen == ComponentPanelTile.None || chosen == hovering) return;
        Vector2I atlasCoords = ComponentPanelCursorLayer.AtlasCoordsFrom(chosen);
        if (chosen == ComponentPanelTile.Erase && hovering == ComponentPanelTile.Red) atlasCoords = _altEraseAtlasCoords;
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
    private static Vector2I AtlasCoordsFrom(ComponentPanelTile tile)
    {
        switch(tile)
        {
            case ComponentPanelTile.Black:    return new Vector2I(0, 1);
            case ComponentPanelTile.White:    return new Vector2I(1, 1);
            case ComponentPanelTile.Red:      return new Vector2I(2, 1);
            case ComponentPanelTile.Blue:     return new Vector2I(3, 1);
            case ComponentPanelTile.Green:    return new Vector2I(4, 1);
            case ComponentPanelTile.Yellow:   return new Vector2I(5, 1);
            case ComponentPanelTile.Purple:   return new Vector2I(6, 1);
            case ComponentPanelTile.Orange:   return new Vector2I(7, 1);
            case ComponentPanelTile.Input:    return new Vector2I(0, 3);
            case ComponentPanelTile.Output:   return new Vector2I(1, 3);
            case ComponentPanelTile.Erase:    return new Vector2I(2, 2);
            default:                return new Vector2I(-1, -1);
        }
    }
    private static readonly Vector2I _altEraseAtlasCoords = new(2, 3);
}
