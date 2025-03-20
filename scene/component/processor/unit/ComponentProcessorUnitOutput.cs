using System;
using System.Collections.Generic;

public class ComponentProcessorUnitOutput : ComponentProcessorUnit
{
    public event EventHandler<PicturesReceivedEventArgs> OutputReceived;
    public ComponentProcessorUnitOutput() : base(false, false) {}
    protected override Dictionary<Directions, List<MonoPicture>> Send(Dictionary<Direction, List<MonoPicture>> received)
    {
        foreach (var (dir, picts) in received)
        {
            List<MonoPicture> sending = new();
            foreach (MonoPicture pict in picts)
                if (!sending.Contains(pict)) sending.Add(pict);
            if (sending.Count != 0)
                OutputReceived?.Invoke(this, new(dir.ToOppositeDirection(), sending));
        }
        return new() {{Directions.None, new()}};
    }
}