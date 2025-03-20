public interface IComponentProcessable
{
    public void SetNeighbor(Direction dir, IComponentInputable neighbor);
    public void StepInitialize();
    public void StepProcess();
}