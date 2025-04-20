using Godot;
using System.Collections.Generic;

public partial class ComponentProcessorMonitorActivationLayer : TileMapLayer
{
    public void Activate(Vector2I coords, ComponentUnitMemory memory, bool isDetailed)
    {
        Vector2I baseCoords = new(coords.X * 4, coords.Y * 4);
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                EraseCell(baseCoords + new Vector2I(i, j));
        if (!isDetailed)
        {
            foreach (var dir in _directionOffsets.Keys)
            {
                if (memory.ReceivedPictures[dir].Count != 0 ||
                    memory.SendingPictures[dir].Count != 0)
                {
                    Assign(baseCoords, _diamondBaseCoords, _diamondOffsets);
                    return;
                }
            }
            return;
        }
        else foreach (var dir in _directionOffsets.Keys)
        {
            if (memory.ReceivedPictures[dir].Count != 0)
                Assign(baseCoords, _inputBaseCoords, _directionOffsets[dir]);
            if (memory.SendingPictures[dir].Count != 0)
                Assign(baseCoords, _outputBaseCoords, _directionOffsets[dir]);
        }
    }

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private int _sourceID;
    public override void _Ready()
    {
        base._Ready();
        _sourceID = TileSet.GetSourceId(0);
    }
    private void Assign(Vector2I baseCoords, Vector2I atlasBaseCoords, Vector2I[] offsets)
    {
        foreach (var offset in offsets)
        {
            SetCell(baseCoords + offset,
                    _sourceID,
                    atlasBaseCoords + offset);
        }
    }
    private void Erase(Vector2I baseCoords, Direction direction)
    {
        EraseCell(baseCoords + _directionOffsets[direction][0]);
        EraseCell(baseCoords + _directionOffsets[direction][1]);
    }
    private static readonly Vector2I _inputBaseCoords = new(4, 20);
    private static readonly Vector2I _outputBaseCoords = new(0, 20);
    private static readonly Dictionary<Direction, Vector2I[]> _directionOffsets = new()
    {
        {Direction.Up,    [new(1, 0), new(2, 0)]},
        {Direction.Left,  [new(0, 1), new(0, 2)]},
        {Direction.Down,  [new(1, 3), new(2, 3)]},
        {Direction.Right, [new(3, 1), new(3, 2)]},
    };
    private static readonly Vector2I _diamondBaseCoords = new(8, 20);
    private static readonly Vector2I[] _diamondOffsets = [new(1, 1), new(1, 2), new(2, 1), new(2, 2)];
}
