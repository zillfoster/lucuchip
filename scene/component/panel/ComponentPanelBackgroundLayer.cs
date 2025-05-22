using Godot;

public partial class ComponentPanelBackgroundLayer : TileMapLayer
{
    public void SetBackground(Rect2I field, bool isGridded)
    {
        Clear();
        for (int i = field.Position.X; i < (field.Position + field.Size).X; i++)
            for (int j = field.Position.Y; j < (field.Position + field.Size).Y; j++)
                SetCell(new Vector2I(i, j), 
                        _sourceID, 
                        AtlasCoordsFrom(isGridded));
    }

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    
    private int _sourceID;
    public override void _Ready()
    {
        base._Ready();
        _sourceID = TileSet.GetSourceId(0);
    }
    private static Vector2I AtlasCoordsFrom(bool isGridded)
    {
        if (isGridded) return new(1, 4);
        else return new(0, 4);
    }
}
