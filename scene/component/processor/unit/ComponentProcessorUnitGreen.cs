using System.Collections.Generic;

public class ComponentProcessorUnitGreen : ComponentProcessorUnit
{
    public ComponentProcessorUnitGreen() : base(false, false) {}
    protected override Dictionary<Directions, List<MonoPicture>> Send(Dictionary<Direction, List<MonoPicture>> received)
    {
        List<MonoPicture> sending = new();
        foreach (var (dir, picts) in received)
            foreach (MonoPicture pict in picts)
                if (!sending.Contains(pict)) sending.Add(pict);
        return new() {{Directions.All, sending}};
    }
}