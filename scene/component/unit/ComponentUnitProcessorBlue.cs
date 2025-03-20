using System;
using System.Collections.Generic;

public class ComponentUnitProcessorBlue : ComponentUnitProcessor
{
    public ComponentUnitProcessorBlue() : base(true, false) {}
    protected override Dictionary<Directions, List<MonoPicture>> Send(Dictionary<Direction, List<MonoPicture>> received)
    {
        List<MonoPicture> sending = new();
        foreach (var (dir, picts) in received)
            foreach (MonoPicture pict in picts)
                if (!sending.Contains(pict)) sending.Add(pict);
        if (sending.Count == 0) return new();
        MonoPicture sendingPict = sending[new Random().Next(sending.Count)];
        return new() {{Directions.All, new() {sendingPict}}};
    }
}