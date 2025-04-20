using Godot;
using System.Collections.Generic;

public partial class ComponentProcessorMonitorActivationSpecialLayer : ComponentProcessorMonitorActivationLayer
{
    public void ActivateTile(Vector2I coords, ComponentUnitMemory memory)
    {
        Vector2I baseCoords = BaseCoordsFrom(coords);
        Erase(baseCoords);
        foreach (var dir in _directionOffsets.Keys)
        {
            if (memory.ReceivedPictures[dir].Count != 0)
                Assign(baseCoords, _inputAltasBaseCoords, _directionOffsets[dir]);
            if (memory.SendingPictures[dir].Count != 0)
                Assign(baseCoords, _outputAtlasBaseCoords, _directionOffsets[dir]);
        }
    }

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private static readonly Vector2I _inputAltasBaseCoords = new(4, 20);
    private static readonly Vector2I _outputAtlasBaseCoords = new(0, 20);
    private static readonly Dictionary<Direction, Vector2I[]> _directionOffsets = new()
    {
        {Direction.Up,    [new(1, 0), new(2, 0)]},
        {Direction.Left,  [new(0, 1), new(0, 2)]},
        {Direction.Down,  [new(1, 3), new(2, 3)]},
        {Direction.Right, [new(3, 1), new(3, 2)]},
    };
}
