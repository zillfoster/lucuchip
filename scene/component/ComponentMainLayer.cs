using Godot;

public partial class ComponentMainLayer : TileMapLayer
{
    public void AssignUnit(Vector2I coords, ComponentUnit style)
    {
        if (style == ComponentUnit.None) return;
        SetCell(coords, _sourceID, ComponentMainLayer.AtlasCoordsFrom(style));
    }
    public ComponentUnit GetUnit(Vector2I coords)
        => ComponentMainLayer.UnitFrom(GetCellAtlasCoords(coords));
    
    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private int _sourceID;
    public override void _Ready()
    {
        base._Ready();
        _sourceID = TileSet.GetSourceId(0);
    }
    private static Vector2I AtlasCoordsFrom(ComponentUnit style)
    {
        switch(style)
        {
            case ComponentUnit.Black:    return new Vector2I(0, 0);
            case ComponentUnit.White:    return new Vector2I(1, 0);
            case ComponentUnit.Red:      return new Vector2I(2, 0);
            case ComponentUnit.Blue:     return new Vector2I(3, 0);
            case ComponentUnit.Green:    return new Vector2I(4, 0);
            case ComponentUnit.Yellow:   return new Vector2I(5, 0);
            case ComponentUnit.Purple:   return new Vector2I(6, 0);
            case ComponentUnit.Orange:   return new Vector2I(7, 0);
            case ComponentUnit.Input:    return new Vector2I(0, 2);
            case ComponentUnit.Output:   return new Vector2I(1, 2);
            default:                return new Vector2I(-1, -1);
        }
    }
    private static ComponentUnit UnitFrom(Vector2I atlasCoords)
    {
        switch(atlasCoords)
        {
            case Vector2I(0, 0):   return ComponentUnit.Black;
            case Vector2I(1, 0):   return ComponentUnit.White;
            case Vector2I(2, 0):   return ComponentUnit.Red;
            case Vector2I(3, 0):   return ComponentUnit.Blue;
            case Vector2I(4, 0):   return ComponentUnit.Green;
            case Vector2I(5, 0):   return ComponentUnit.Yellow;
            case Vector2I(6, 0):   return ComponentUnit.Purple;
            case Vector2I(7, 0):   return ComponentUnit.Orange;
            case Vector2I(0, 2):   return ComponentUnit.Input;
            case Vector2I(1, 2):   return ComponentUnit.Output;
            default:               return ComponentUnit.None;
        }
    }
}
