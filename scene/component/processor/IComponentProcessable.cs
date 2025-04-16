public interface IComponentProcessable
{
    public IComponentInputable TryGetComponentInputable();
    public ComponentUnitMemory GetCurrentMemory();
    public void SetNeighbor(Direction dir, IComponentInputable neighbor);
    public void Initialize();
    public void StepInitialize();
    public void StepProcess();
}