using Godot;

public partial class ComponentPanelMainLayer : TileMapLayer
{
    public void AssignTile(Vector2I coords, ComponentPanelTile tile)
    {
        if (tile == ComponentPanelTile.None) return;
        SetCell(coords, _sourceID, AtlasCoordsFrom(tile));
    }
    public ComponentPanelTile GetTile(Vector2I coords)
        => TileFrom(GetCellAtlasCoords(coords));
    
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
            ComponentPanelTile.Black => new Vector2I(0, 0),
            ComponentPanelTile.White => new Vector2I(1, 0),
            ComponentPanelTile.Red => new Vector2I(2, 0),
            ComponentPanelTile.Blue => new Vector2I(3, 0),
            ComponentPanelTile.Green => new Vector2I(4, 0),
            ComponentPanelTile.Yellow => new Vector2I(5, 0),
            ComponentPanelTile.Purple => new Vector2I(6, 0),
            ComponentPanelTile.Orange => new Vector2I(7, 0),
            ComponentPanelTile.Input => new Vector2I(0, 2),
            ComponentPanelTile.Output => new Vector2I(1, 2),
            _ => new Vector2I(-1, -1),
        };
    }
    private static ComponentPanelTile TileFrom(Vector2I atlasCoords)
    {
        return atlasCoords switch
        {
            Vector2I(0, 0) => ComponentPanelTile.Black,
            Vector2I(1, 0) => ComponentPanelTile.White,
            Vector2I(2, 0) => ComponentPanelTile.Red,
            Vector2I(3, 0) => ComponentPanelTile.Blue,
            Vector2I(4, 0) => ComponentPanelTile.Green,
            Vector2I(5, 0) => ComponentPanelTile.Yellow,
            Vector2I(6, 0) => ComponentPanelTile.Purple,
            Vector2I(7, 0) => ComponentPanelTile.Orange,
            Vector2I(0, 2) => ComponentPanelTile.Input,
            Vector2I(1, 2) => ComponentPanelTile.Output,
            _ => ComponentPanelTile.None,
        };
    }
}
