using Godot;

public partial class ComponentProcessorMonitorActivationLayer : TileMapLayer
{
    public void EraseTile(Vector2I coords)
        => Erase(BaseCoordsFrom(coords));
    
    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private int _sourceID;
    public override void _Ready()
    {
        base._Ready();
        _sourceID = TileSet.GetSourceId(0);
    }
    protected static Vector2I BaseCoordsFrom(Vector2I coords)
        => new(coords.X * 4, coords.Y * 4);
    protected void Erase(Vector2I baseCoords)
    {
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                EraseCell(baseCoords + new Vector2I(i, j));
    }
    protected void Assign(Vector2I baseCoords, Vector2I atlasBaseCoords, Vector2I[] offsets)
    {
        foreach (var offset in offsets)
            SetCell(baseCoords + offset, _sourceID, atlasBaseCoords + offset);
    }
    protected void Erase(Vector2I baseCoords, Vector2I[] offsets)
    {
        foreach (var offset in offsets)
            EraseCell(baseCoords + offset);
    }
}
