using Godot;
using System.Collections.Generic;

public partial class ComponentProcessorMonitorActivationGeneralLayer : ComponentProcessorMonitorActivationLayer
{
    public void ActivateTile(Vector2I coords, ComponentUnitMemory memory)
    {
        Vector2I baseCoords = BaseCoordsFrom(coords);
        foreach (var dir in _directionOffsets.Keys)
        {
            if (memory.ReceivedPictures[dir].Count != 0)
                GeneralAssign(baseCoords, dir, true);
            else if (memory.SendingPictures[dir].Count != 0)
                GeneralAssign(baseCoords, dir, false);
            else Erase(baseCoords + _directionOffsets[dir], offsets);
        }
    }

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private void GeneralAssign(Vector2I baseCoords, Direction direction, bool isFlipped)
    {
        Vector2I assignBaseCoords = baseCoords + _directionOffsets[direction];
        Vector2I currentAtlasCoords = GetCellAtlasCoords(assignBaseCoords);
        if (currentAtlasCoords == new Vector2I(-1, -1))
            Assign(assignBaseCoords,
                   isFlipped ?
                   _directionAtlasBaseCoords[direction.ToOppositeDirection()] :
                   _directionAtlasBaseCoords[direction],
                   offsets);
        if (currentAtlasCoords == _directionAtlasBaseCoords[isFlipped ? direction: direction.ToOppositeDirection()])
            Assign(assignBaseCoords,
                   isFlipped ? 
                   _directionAtlasBaseCoords[direction.ToOppositeDirection()] + _atlasDualOffset :
                   _directionAtlasBaseCoords[direction] + _atlasDualOffset,
                   offsets);
    }
    private static readonly Vector2I[] offsets = [new(0, 0), new(0, 1), new(1, 0), new(1, 1)];

    private static readonly Vector2I _atlasDualOffset = new(4, 0);
    private static readonly Dictionary<Direction, Vector2I> _directionAtlasBaseCoords = new()
    {
        {Direction.Up,    new(2, 24)},
        {Direction.Left,  new(2, 26)},
        {Direction.Down,  new(0, 26)},
        {Direction.Right, new(0, 24)},
    };
    private static readonly Dictionary<Direction, Vector2I> _directionOffsets = new()
    {
        {Direction.Up,    new(1, -1)},
        {Direction.Left,  new(-1, 1)},
        {Direction.Down,  new(1, 3)},
        {Direction.Right, new(3, 1)},
    };
}
