using Godot;

public partial class ComponentProcessorMonitorMainLayer : TileMapLayer
{
    public void AssignActivation(Vector2I coords, bool isActivated)
        => SetCell(coords, _sourceID, isActivated? _activationAtlasCoords: new Vector2I(-1, -1));
    public bool GetActivation(Vector2I coords)
        => GetCellAtlasCoords(coords) == _activationAtlasCoords;

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private int _sourceID;
    public override void _Ready()
    {
        base._Ready();
        _sourceID = TileSet.GetSourceId(0);
    }
    private static readonly Vector2I _activationAtlasCoords = new(0, 5);
}
