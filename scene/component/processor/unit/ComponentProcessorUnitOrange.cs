using System.Collections.Generic;
using static MonoPicture;

public class ComponentProcessorUnitOrange : ComponentProcessorUnit
{
    public ComponentProcessorUnitOrange() : base(true, true) {}
    protected override Dictionary<Directions, List<MonoPicture>> Send(Dictionary<Direction, List<MonoPicture>> received)
    {
        Directions dirs = Directions.None;
        foreach (var (dir, picts) in received) dirs |= dir.ToDirections();
        if ((dirs & Directions.Vertical) == Directions.Vertical &&
            (dirs & Directions.Horizontal) == Directions.None)
        {
            List<MonoPicture> list = new();
            foreach (MonoPicture up in received[Direction.Up])
                foreach (MonoPicture down in received[Direction.Down])
                    JoinHeight(up, down, ref list);
            Godot.GD.Print(dirs, " ", list);
            return new() {{Directions.Horizontal, list}};
        }
        if ((dirs & Directions.Horizontal) == Directions.Horizontal &&
            (dirs & Directions.Vertical) == Directions.None) 
        {
            List<MonoPicture> list = new();
            foreach (MonoPicture left in received[Direction.Left])
                foreach (MonoPicture right in received[Direction.Right])
                    JoinHeight(left, right, ref list);
            return new() {{Directions.Vertical, list}};
        }
        return new();
    }
    public static void JoinWidth(MonoPicture left, MonoPicture right, ref List<MonoPicture> list)
    {
        if (left.Height != right.Height) return;
        MonoColor[,] joined = new MonoColor[left.Height, left.Width + right.Width];
        for (int i = 0; i < left.Height; i++)
            for (int j = 0; j < left.Width; j++)
                joined[i, j] = left.Data[i, j];
        for (int i = 0; i < left.Height; i++)
            for (int j = 0; j < right.Width; j++)
                joined[i, j + left.Width] = right.Data[i, j];
        list.Add(new(joined));
    }
    public static void JoinHeight(MonoPicture up, MonoPicture down, ref List<MonoPicture> list)
    {
        if (up.Width != down.Width) return;
        MonoColor[,] joined = new MonoColor[up.Height + down.Height, up.Width];
        for (int i = 0; i < up.Height; i++)
            for (int j = 0; j < up.Width; j++)
                joined[i, j] = up.Data[i, j];
        for (int i = 0; i < down.Height; i++)
            for (int j = 0; j < up.Width; j++)
                joined[i + up.Height, j] = down.Data[i, j];
        list.Add(new(joined));
    }
}