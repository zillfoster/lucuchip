using static MonoPicture;
using System.Collections.Generic;

public class ComponentProcessorUnitYellow : ComponentProcessorUnit
{
    public ComponentProcessorUnitYellow() : base(false, false, false) {}
    protected override Dictionary<Directions, List<MonoPicture>> Send(Dictionary<Direction, List<MonoPicture>> received)
    {
        MonoColor randomColor = (MonoColor)new System.Random().Next(2);
        MonoPicture randomPict = new(1, 1, randomColor);
        return new() {{Directions.All, new() {randomPict}}};
    }
}