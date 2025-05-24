using Godot;

public partial class ComponentPaletteBackgroundLayer : TileMapLayer
{
    public void SetBackground(Vector2I coords)
    {
        Clear();
        SetCell(coords, _sourceID, _backgroundAtlasCoords);
    }

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    
    private int _sourceID;
    public override void _Ready()
    {
        base._Ready();
        _sourceID = TileSet.GetSourceId(0);
    }
    private static readonly Vector2I _backgroundAtlasCoords = new(4, 4);
}
