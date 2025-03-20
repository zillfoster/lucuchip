using System.Collections.Generic;

public class ComponentProcessorUnitWhite : ComponentProcessorUnit
{
    public ComponentProcessorUnitWhite() : base(true, false) {}
    protected override Dictionary<Directions, List<MonoPicture>> Send(Dictionary<Direction, List<MonoPicture>> received)
    {
        List<MonoPicture> sending = new();
        foreach (var (dir, picts) in received)
            foreach (MonoPicture pict in picts)
            {
                MonoPicture whitepict = new(pict, MonoPicture.MonoColor.White);
                if (!sending.Contains(whitepict)) sending.Add(whitepict);
            }
        return new() {{Directions.All, sending}};
    }
}