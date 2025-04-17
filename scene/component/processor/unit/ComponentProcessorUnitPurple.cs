using System.Collections.Generic;
using static MonoPicture;

public class ComponentProcessorUnitPurple : ComponentProcessorUnit
{
    public ComponentProcessorUnitPurple() : base(true, true) {}
    protected override Dictionary<Directions, List<MonoPicture>> Send(Dictionary<Direction, List<MonoPicture>> received)
    {
        Directions dirs = Directions.None;
        foreach (var (dir, picts) in received) dirs |= dir.ToDirections();
        if ((dirs & Directions.Vertical) != Directions.None &&
            (dirs & Directions.Horizontal) == Directions.None) 
        {
            List<MonoPicture> leftlist = [];
            List<MonoPicture> rightlist = [];
            foreach (var (dir, picts) in received)
                foreach (var pict in picts)
                    SplitWidth(pict, ref leftlist, ref rightlist);
            return new()
            {
                {Directions.Left, leftlist},
                {Directions.Right, rightlist},
            };
        }
        if ((dirs & Directions.Horizontal) != Directions.None &&
            (dirs & Directions.Vertical) == Directions.None) 
        {
            List<MonoPicture> uplist = [];
            List<MonoPicture> downlist = [];
            foreach (var (dir, picts) in received)
                foreach (var pict in picts)
                    SplitHeight(pict, ref uplist, ref downlist);
            return new()
            {
                {Directions.Up, uplist},
                {Directions.Down, downlist},
            };
        }
        return [];
    }
    public static void SplitWidth(MonoPicture from, ref List<MonoPicture> leftlist, ref List<MonoPicture> rightlist)
    {
        int newWidth = (from.Width + 1) / 2;
        MonoColor[,] left = new MonoColor[from.Height, newWidth];
        MonoColor[,] right = new MonoColor[from.Height, newWidth];
        for (int i = 0; i < from.Height; i++)
            for (int j = 0; j < from.Width; j++)
            {
                if (j < newWidth)
                    left[i, j] = from.Data[i, j];
                if (j >= from.Width - newWidth)
                    right[i, j - from.Width + newWidth] = from.Data[i, j];
            }
        leftlist.Add(new MonoPicture(left));
        rightlist.Add(new MonoPicture(right));
    }
    public static void SplitHeight(MonoPicture from, ref List<MonoPicture> uplist, ref List<MonoPicture> downlist)
    {
        int newHeight = (from.Height + 1) / 2;
        MonoColor[,] up = new MonoColor[newHeight, from.Width];
        MonoColor[,] down = new MonoColor[newHeight, from.Width];
        for (int i = 0; i < from.Height; i++)
            for (int j = 0; j < from.Width; j++)
            {
                if (i < newHeight)
                    up[i, j] = from.Data[i, j];
                if (i >= from.Height - newHeight)
                    down[i - from.Height + newHeight, j] = from.Data[i, j];
            }
        uplist.Add(new MonoPicture(up));
        downlist.Add(new MonoPicture(down));
    }
}