using System.Collections.Generic;

public interface IComponentProcessable
{
    public void SetNeighbor(Direction dir, IComponentProcessable proc);
    public void Receive(Direction from, List<MonoPicture> picts);
    public void Initialize();
    public void Step();
}