using Godot;

public partial class ComponentPanelCursorLayer : TileMapLayer
{
    public void SetCursor(Vector2I coords, ComponentPanelTile chosen, ComponentPanelTile hovering, bool isEditable)
    {
        Clear();
        if (!isEditable) return;
        if (chosen == hovering && chosen != ComponentPanelTile.None) return;
        Vector2I atlasCoords = AtlasCoordsFrom(chosen);
        if (chosen == ComponentPanelTile.None) atlasCoords = _standardAtlasCoords;
        else if (chosen == ComponentPanelTile.Erase && hovering == ComponentPanelTile.Red) atlasCoords = _altEraseAtlasCoords;
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
        return tile switch
        {
            ComponentPanelTile.Black => new Vector2I(0, 1),
            ComponentPanelTile.White => new Vector2I(1, 1),
            ComponentPanelTile.Red => new Vector2I(2, 1),
            ComponentPanelTile.Blue => new Vector2I(3, 1),
            ComponentPanelTile.Green => new Vector2I(4, 1),
            ComponentPanelTile.Yellow => new Vector2I(5, 1),
            ComponentPanelTile.Purple => new Vector2I(6, 1),
            ComponentPanelTile.Orange => new Vector2I(7, 1),
            ComponentPanelTile.Input => new Vector2I(0, 3),
            ComponentPanelTile.Output => new Vector2I(1, 3),
            ComponentPanelTile.Erase => new Vector2I(2, 2),
            _ => new Vector2I(-1, -1),
        };
    }
    private static readonly Vector2I _altEraseAtlasCoords = new(2, 3);
    private static readonly Vector2I _standardAtlasCoords = new(2, 4);
}
