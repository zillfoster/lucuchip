using System;
using System.Collections.Generic;

public class ComponentProcessorUnitBlue : ComponentProcessorUnit
{
    public ComponentProcessorUnitBlue() : base(true, true) {}
    protected override Dictionary<Directions, List<MonoPicture>> Send(Dictionary<Direction, List<MonoPicture>> received)
    {
        List<MonoPicture> sending = [];
        foreach (var (dir, picts) in received)
            foreach (MonoPicture pict in picts)
                if (!sending.Contains(pict)) sending.Add(pict);
        if (sending.Count == 0) return [];
        MonoPicture sendingPict = sending[new Random().Next(sending.Count)];
        return new() {{Directions.All, new() {sendingPict}}};
    }
}