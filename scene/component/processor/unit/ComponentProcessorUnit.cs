using System.Collections.Generic;
using static DirectionsExtensions;

public abstract class ComponentProcessorUnit: IComponentProcessable, IComponentInputable
{
    protected abstract Dictionary<Directions, List<MonoPicture>> Send(Dictionary<Direction, List<MonoPicture>> received);
    
    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    public ComponentProcessorUnit(bool isInputLock,
                                  bool isOutputLock,
                                  bool isInputable = true)
    {
        _isInputLock = isInputLock;
        _isOutputLock = isOutputLock;
        _isInputable = isInputable;
    }
    private readonly bool _isInputLock = false;
    private readonly bool _isOutputLock = false;
    private readonly bool _isInputable = true;
    private readonly ComponentUnitMemory _currentMemory = new();
    private readonly ComponentUnitMemory _nextMemory = new();
    private Dictionary<Direction, IComponentInputable> _neighbors = new();
    public void Receive(Direction from, List<MonoPicture> picts)
    {
        if (_isOutputLock && _currentMemory.OutputAccum.HasDirection(from)) return;
        foreach (MonoPicture pict in picts) _nextMemory.Receive(from, pict);
    }
    public IComponentInputable TryGetComponentInputable()
        => _isInputable? this: null;
    public ComponentUnitMemory GetCurrentMemory() => _currentMemory;
    public virtual void SetNeighbor(Direction dir, IComponentInputable neighbor)
        => _neighbors[dir] = neighbor;
    public virtual void Initialize()
    {
        _currentMemory.Initialize();
        _nextMemory.Initialize();
    }
    public virtual void StepInitialize()
    {
        _currentMemory.CopyFrom(_nextMemory);
        _nextMemory.Initialize();
    }
    public virtual void StepProcess()
    {
        Dictionary<Directions, List<MonoPicture>> sending = Send(GetReceivedPictures());
        foreach (var (dirs, picts) in sending)
        {
            if (picts.Count == 0) continue;
            DirectedAct(dirs, (d) =>
            {
                if (_neighbors.ContainsKey(d) &&
                    !(_isInputLock && _currentMemory.InputAccum.HasDirection(d)))
                {
                    foreach (MonoPicture pict in picts) _nextMemory.Send(d, pict);
                    _neighbors[d].Receive(d.ToOppositeDirection(), picts);
                }
            });
        }
    }
    private Dictionary<Direction, List<MonoPicture>> GetReceivedPictures()
    {
        Dictionary<Direction, List<MonoPicture>> receivedPictures = new();
        foreach (var (dir, picts) in _currentMemory.ReceivedPictures)
        {
            if (picts.Count == 0) continue;
            receivedPictures[dir] = new();
            foreach (MonoPicture pict in _currentMemory.ReceivedPictures[dir])
                receivedPictures[dir].Add(pict);
        }
        return receivedPictures;
    }
}