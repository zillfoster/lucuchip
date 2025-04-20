using System.Collections.Generic;
using static DirectionsExtensions;

public abstract class ComponentProcessorUnit(bool isInputLock,
                                             bool isOutputLock,
                                             bool isInputable = true) : 
                      IComponentProcessable,
                      IComponentInputable
{
    protected abstract Dictionary<Directions, List<MonoPicture>> Send(Dictionary<Direction, List<MonoPicture>> received);

    private readonly ComponentUnitMemory _currentMemory = new();
    private readonly ComponentUnitMemory _nextMemory = new();
    private readonly Dictionary<Direction, IComponentInputable> _neighbors = [];
    public void Receive(Direction from, List<MonoPicture> picts)
    {
        if (isOutputLock && _currentMemory.SendingPictures[from].Count != 0) return;
        foreach (MonoPicture pict in picts) _nextMemory.Receive(from, pict);
    }
    public IComponentInputable TryGetComponentInputable()
        => isInputable? this: null;
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
                if (_neighbors.TryGetValue(d, out IComponentInputable neighbor) &&
                    !(isInputLock && _currentMemory.ReceivedPictures[d].Count != 0))
                {
                    foreach (MonoPicture pict in picts) _nextMemory.Send(d, pict);
                    neighbor.Receive(d.ToOppositeDirection(), picts);
                }
            });
        }
    }
    private Dictionary<Direction, List<MonoPicture>> GetReceivedPictures()
    {
        Dictionary<Direction, List<MonoPicture>> receivedPictures = [];
        foreach (var (dir, picts) in _currentMemory.ReceivedPictures)
        {
            if (picts.Count == 0) continue;
            receivedPictures[dir] = [.. _currentMemory.ReceivedPictures[dir]];
        }
        return receivedPictures;
    }
}