using System.Collections.Generic;
using static DirectionsExtensions;

public abstract class ComponentUnitProcessor: IComponentProcessable, IComponentInputable
{
    protected abstract Dictionary<Directions, List<MonoPicture>> Send(Dictionary<Direction, List<MonoPicture>> received);
    
    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    public ComponentUnitProcessor(bool isInputLock, 
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
    public void SetNeighbor(Direction dir, IComponentInputable neighbor)
        => _neighbors[dir] = neighbor;
    public void Receive(Direction from, List<MonoPicture> picts)
    {
        if (!_isInputable) return;
        if (_isOutputLock && _currentMemory.OutputAccum.HasFlag(from)) return;
        _nextMemory.InputAccum |= from.ToDirections();
        foreach (MonoPicture pict in picts) _nextMemory.Receive(from, pict);
    }
    public void StepInitialize()
    {
        _currentMemory.CopyFrom(_nextMemory);
        _nextMemory.Initialize();
    }
    public void StepProcess()
    {
        Dictionary<Directions, List<MonoPicture>> sending = Send(GetReadyForSend(_currentMemory.ReceivedPictures));
        foreach (var (dirs, picts) in sending)
            DirectedAct(dirs, (d) =>
            {
                if (_neighbors.ContainsKey(d) &&
                    !(_isInputLock && _currentMemory.InputAccum.HasFlag(d)))
                {
                    _nextMemory.OutputAccum |= d.ToDirections();
                    _neighbors[d].Receive(d.ToOppositeDirection(), picts);
                }
            });
    }
    public static Dictionary<Direction, List<MonoPicture>> GetReadyForSend(IReadOnlyDictionary<Direction, IReadOnlyList<MonoPicture>> receivedPictures)
    {
        Dictionary<Direction, List<MonoPicture>> readyForSend = new();
        foreach (var (dir, picts) in receivedPictures)
        {
            if (readyForSend[dir].Count == 0) continue;
            readyForSend[dir] = new();
            foreach (MonoPicture pict in receivedPictures[dir]) readyForSend[dir].Add(pict);
        }
        return readyForSend;
    }
}