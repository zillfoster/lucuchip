using System.Collections.Generic;

public class ComponentProcessorUnitRed : ComponentProcessorUnit
{
    public ComponentProcessorUnitRed() : base(true, false) {}
    protected override Dictionary<Directions, List<MonoPicture>> Send(Dictionary<Direction, List<MonoPicture>> received)
    {
        List<MonoPicture> sending = [];
        foreach (var (dir, picts) in received)
        {
            if (sending.Count == 0)
                foreach (MonoPicture pict in picts) sending.Add(pict);
            else
            {
                if (picts.Count != sending.Count) return [];
                foreach (MonoPicture pict in picts)
                    if (!sending.Contains(pict)) return [];
            }
        }
        return new() {{Directions.All, sending}};
    }
}