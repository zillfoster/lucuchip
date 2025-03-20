using Godot;

public partial class ComponentPanelMainLayer : TileMapLayer
{
    public void AssignTile(Vector2I coords, ComponentPanelTile tile)
    {
        if (tile == ComponentPanelTile.None) return;
        SetCell(coords, _sourceID, ComponentPanelMainLayer.AtlasCoordsFrom(tile));
    }
    public ComponentPanelTile GetTile(Vector2I coords)
        => ComponentPanelMainLayer.TileFrom(GetCellAtlasCoords(coords));
    
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
            case ComponentPanelTile.Black:    return new Vector2I(0, 0);
            case ComponentPanelTile.White:    return new Vector2I(1, 0);
            case ComponentPanelTile.Red:      return new Vector2I(2, 0);
            case ComponentPanelTile.Blue:     return new Vector2I(3, 0);
            case ComponentPanelTile.Green:    return new Vector2I(4, 0);
            case ComponentPanelTile.Yellow:   return new Vector2I(5, 0);
            case ComponentPanelTile.Purple:   return new Vector2I(6, 0);
            case ComponentPanelTile.Orange:   return new Vector2I(7, 0);
            case ComponentPanelTile.Input:    return new Vector2I(0, 2);
            case ComponentPanelTile.Output:   return new Vector2I(1, 2);
            default:                return new Vector2I(-1, -1);
        }
    }
    private static ComponentPanelTile TileFrom(Vector2I atlasCoords)
    {
        switch(atlasCoords)
        {
            case Vector2I(0, 0):   return ComponentPanelTile.Black;
            case Vector2I(1, 0):   return ComponentPanelTile.White;
            case Vector2I(2, 0):   return ComponentPanelTile.Red;
            case Vector2I(3, 0):   return ComponentPanelTile.Blue;
            case Vector2I(4, 0):   return ComponentPanelTile.Green;
            case Vector2I(5, 0):   return ComponentPanelTile.Yellow;
            case Vector2I(6, 0):   return ComponentPanelTile.Purple;
            case Vector2I(7, 0):   return ComponentPanelTile.Orange;
            case Vector2I(0, 2):   return ComponentPanelTile.Input;
            case Vector2I(1, 2):   return ComponentPanelTile.Output;
            default:               return ComponentPanelTile.None;
        }
    }
}
