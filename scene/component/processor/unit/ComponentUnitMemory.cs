using System.Collections.Generic;

public class ComponentUnitMemory
{
    public Directions InputAccum { get; set; } = Directions.None;
    public Directions OutputAccum { get; set; } = Directions.None;
    public IReadOnlyDictionary<Direction, IReadOnlyList<MonoPicture>> ReceivedPictures => _receivedPictures;
    public void Initialize()
    {
        InputAccum = Directions.None;
        OutputAccum = Directions.None;
        foreach (var (dir, picts) in _accessableReceivedPictures) picts.Clear();
    }
    public void CopyFrom(ComponentUnitMemory memory)
    {
        Initialize();
        foreach (var (dir, picts) in ReceivedPictures)
            foreach (MonoPicture pict in memory.ReceivedPictures[dir])
                Receive(dir, pict);
    }
    public void Receive(Direction dir, MonoPicture pict)
    {
        if (!_accessableReceivedPictures[dir].Contains(pict)) 
            _accessableReceivedPictures[dir].Add(pict);
    }
    
    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    public ComponentUnitMemory()
    {
        foreach(var (dir, picts) in _accessableReceivedPictures)
            _receivedPictures.Add(dir, picts);
    }
    private readonly Dictionary<Direction, IReadOnlyList<MonoPicture>> _receivedPictures = new();
    private readonly Dictionary<Direction, List<MonoPicture>> _accessableReceivedPictures = new()
    {
        {Direction.Up,     new()},
        {Direction.Left,   new()},
        {Direction.Down,   new()},
        {Direction.Right,  new()},
    };
}