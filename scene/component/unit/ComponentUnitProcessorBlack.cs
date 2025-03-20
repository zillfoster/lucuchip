using System.Collections.Generic;

public class ComponentUnitProcessorBlack : ComponentUnitProcessor
{
    public ComponentUnitProcessorBlack() : base(true, false) {}
    protected override Dictionary<Directions, List<MonoPicture>> Send(Dictionary<Direction, List<MonoPicture>> received)
    {
        List<MonoPicture> sending = new();
        foreach (var (dir, picts) in received)
            foreach (MonoPicture pict in picts)
            {
                MonoPicture blackpict = new(pict, MonoPicture.MonoColor.Black);
                if (!sending.Contains(blackpict)) sending.Add(blackpict);
            }
        return new() {{Directions.All, sending}};
    }
}