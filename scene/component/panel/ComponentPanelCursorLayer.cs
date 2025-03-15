using Godot;

public partial class ComponentPanelCursorLayer : TileMapLayer
{
    public void SetCursor(Vector2I coords, ComponentPanelUnit style, ComponentPanelUnit hovered)
    {
        Clear();
        if (style == ComponentPanelUnit.None || style == hovered) return;
        Vector2I atlasCoords = ComponentPanelCursorLayer.AtlasCoordsFrom(style);
        if (style == ComponentPanelUnit.Erase && hovered == ComponentPanelUnit.Red) atlasCoords = _altEraseAtlasCoords;
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
    private static Vector2I AtlasCoordsFrom(ComponentPanelUnit unit)
    {
        switch(unit)
        {
            case ComponentPanelUnit.Black:    return new Vector2I(0, 1);
            case ComponentPanelUnit.White:    return new Vector2I(1, 1);
            case ComponentPanelUnit.Red:      return new Vector2I(2, 1);
            case ComponentPanelUnit.Blue:     return new Vector2I(3, 1);
            case ComponentPanelUnit.Green:    return new Vector2I(4, 1);
            case ComponentPanelUnit.Yellow:   return new Vector2I(5, 1);
            case ComponentPanelUnit.Purple:   return new Vector2I(6, 1);
            case ComponentPanelUnit.Orange:   return new Vector2I(7, 1);
            case ComponentPanelUnit.Input:    return new Vector2I(0, 3);
            case ComponentPanelUnit.Output:   return new Vector2I(1, 3);
            case ComponentPanelUnit.Erase:    return new Vector2I(2, 2);
            default:                return new Vector2I(-1, -1);
        }
    }
    private static readonly Vector2I _altEraseAtlasCoords = new(2, 3);
}
