using System.Collections.Generic;

public class ComponentUnitProcessorInput : ComponentUnitProcessor
{
    public readonly List<MonoPicture> _inputPicts = new();
    public ComponentUnitProcessorInput() : base(false, false, false) {}
    protected override Dictionary<Directions, List<MonoPicture>> Send(Dictionary<Direction, List<MonoPicture>> received)
        => new() {{Directions.All, new(_inputPicts)}};
}