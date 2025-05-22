using Godot;

public static class TileMapLayerLibrary
{
    public static Vector2I CoordsFrom(this TileMapLayer layer, Vector2 globalPosition)
        => layer.LocalToMap(layer.ToLocal(globalPosition));
    public static int GetSourceId(this TileMapLayer layer, int index)
        => layer.TileSet.GetSourceId(index);
}