using Godot;

public partial class ComponentPanelMainLayer : TileMapLayer
{
    public void AssignUnit(Vector2I coords, ComponentPanelUnit style)
    {
        if (style == ComponentPanelUnit.None) return;
        SetCell(coords, _sourceID, ComponentPanelMainLayer.AtlasCoordsFrom(style));
    }
    public ComponentPanelUnit GetUnit(Vector2I coords)
        => ComponentPanelMainLayer.UnitFrom(GetCellAtlasCoords(coords));
    
    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private int _sourceID;
    public override void _Ready()
    {
        base._Ready();
        _sourceID = TileSet.GetSourceId(0);
    }
    private static Vector2I AtlasCoordsFrom(ComponentPanelUnit style)
    {
        switch(style)
        {
            case ComponentPanelUnit.Black:    return new Vector2I(0, 0);
            case ComponentPanelUnit.White:    return new Vector2I(1, 0);
            case ComponentPanelUnit.Red:      return new Vector2I(2, 0);
            case ComponentPanelUnit.Blue:     return new Vector2I(3, 0);
            case ComponentPanelUnit.Green:    return new Vector2I(4, 0);
            case ComponentPanelUnit.Yellow:   return new Vector2I(5, 0);
            case ComponentPanelUnit.Purple:   return new Vector2I(6, 0);
            case ComponentPanelUnit.Orange:   return new Vector2I(7, 0);
            case ComponentPanelUnit.Input:    return new Vector2I(0, 2);
            case ComponentPanelUnit.Output:   return new Vector2I(1, 2);
            default:                return new Vector2I(-1, -1);
        }
    }
    private static ComponentPanelUnit UnitFrom(Vector2I atlasCoords)
    {
        switch(atlasCoords)
        {
            case Vector2I(0, 0):   return ComponentPanelUnit.Black;
            case Vector2I(1, 0):   return ComponentPanelUnit.White;
            case Vector2I(2, 0):   return ComponentPanelUnit.Red;
            case Vector2I(3, 0):   return ComponentPanelUnit.Blue;
            case Vector2I(4, 0):   return ComponentPanelUnit.Green;
            case Vector2I(5, 0):   return ComponentPanelUnit.Yellow;
            case Vector2I(6, 0):   return ComponentPanelUnit.Purple;
            case Vector2I(7, 0):   return ComponentPanelUnit.Orange;
            case Vector2I(0, 2):   return ComponentPanelUnit.Input;
            case Vector2I(1, 2):   return ComponentPanelUnit.Output;
            default:               return ComponentPanelUnit.None;
        }
    }
}
