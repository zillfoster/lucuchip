using Godot;

public partial class ComponentPaletteCursorLayer : TileMapLayer
{
    public Vector2I? Selection
    {
        get => _selectingCoords;
        set
        {
            Clear();
            _selectingCoords = value;
            if (value.HasValue) SetCell(value.Value, _sourceID, _selectionAtlasCoords);
        }
    }
    public void SetCursor(Vector2I coords)
    {
        Clear();
        SetCell(coords, _sourceID, _cursorAtlasCoords);
        if (_selectingCoords.HasValue) 
            SetCell(_selectingCoords.Value, _sourceID, _selectionAtlasCoords);
    }

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private Vector2I? _selectingCoords = null;
    private int _sourceID;
    public override void _Ready()
    {
        base._Ready();
        _sourceID = TileSet.GetSourceId(0);
    }
    private static readonly Vector2I _cursorAtlasCoords = new(2, 4);
    private static readonly Vector2I _selectionAtlasCoords = new(3, 4);
}
