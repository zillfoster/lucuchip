using System.Collections.Generic;

public interface IComponentProcessable
{
    public IComponentInputable TryGetComponentInputable();
    public void SetNeighbor(Direction dir, IComponentInputable neighbor);
    public void Initialize();
    public void StepInitialize();
    public void StepProcess();
    public Dictionary<Direction, List<MonoPicture>> GetReceivedPictures();
}