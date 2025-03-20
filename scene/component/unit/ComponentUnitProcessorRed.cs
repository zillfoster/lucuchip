using System.Collections.Generic;

public class ComponentUnitProcessorRed : ComponentUnitProcessor
{
    public ComponentUnitProcessorRed() : base(true, false) {}
    protected override Dictionary<Directions, List<MonoPicture>> Send(Dictionary<Direction, List<MonoPicture>> received)
    {
        List<MonoPicture> sending = new();
        foreach (var (dir, picts) in received)
        {
            if (sending.Count == 0)
                foreach (MonoPicture pict in picts) sending.Add(pict);
            else
            {
                if (picts.Count != sending.Count) return new();
                foreach (MonoPicture pict in picts)
                    if (!sending.Contains(pict)) return new();
            }
        }
        return new() {{Directions.All, sending}};
    }
}