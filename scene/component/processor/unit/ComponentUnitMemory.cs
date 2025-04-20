using System.Collections.Generic;

public class ComponentUnitMemory
{
    public IReadOnlyDictionary<Direction, IReadOnlyList<MonoPicture>> ReceivedPictures => _receivedPictures;
    public IReadOnlyDictionary<Direction, IReadOnlyList<MonoPicture>> SendingPictures => _sendingPictures;
    public void Initialize()
    {
        foreach (var (_, picts) in _accessableReceivedPictures) picts.Clear();
        foreach (var (_, picts) in _accessableSendingPictures) picts.Clear();
    }
    public void CopyFrom(ComponentUnitMemory memory)
    {
        Initialize();
        foreach (var (dir, picts) in ReceivedPictures)
            foreach (MonoPicture pict in memory.ReceivedPictures[dir])
                Receive(dir, pict);
        foreach (var (dir, picts) in SendingPictures)
            foreach (MonoPicture pict in memory.SendingPictures[dir])
                Send(dir, pict);
    }
    public void Receive(Direction dir, MonoPicture pict)
    {
        if (!_accessableReceivedPictures[dir].Contains(pict)) 
            _accessableReceivedPictures[dir].Add(pict);
    }
    public void Send(Direction dir, MonoPicture pict)
    {
        if (!_accessableSendingPictures[dir].Contains(pict))
            _accessableSendingPictures[dir].Add(pict);
    }
    
    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    public ComponentUnitMemory()
    {
        foreach(var (dir, picts) in _accessableReceivedPictures)
            _receivedPictures.Add(dir, picts);
        foreach(var (dir, picts) in _accessableSendingPictures)
            _sendingPictures.Add(dir, picts);
    }
    private readonly Dictionary<Direction, IReadOnlyList<MonoPicture>> _receivedPictures = [];
    private readonly Dictionary<Direction, IReadOnlyList<MonoPicture>> _sendingPictures = [];
    private readonly Dictionary<Direction, List<MonoPicture>> _accessableReceivedPictures = new()
    {
        {Direction.Up,     new()},
        {Direction.Left,   new()},
        {Direction.Down,   new()},
        {Direction.Right,  new()},
    };
    private readonly Dictionary<Direction, List<MonoPicture>> _accessableSendingPictures = new()
    {
        {Direction.Up,     new()},
        {Direction.Left,   new()},
        {Direction.Down,   new()},
        {Direction.Right,  new()},
    };
}