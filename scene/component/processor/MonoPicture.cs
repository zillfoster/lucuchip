using System;
using static System.Math;

public struct MonoPicture
{
    public enum MonoColor { Black, White }
    public readonly int Height = 0;
    public readonly int Width = 0;
    public readonly MonoColor[,] Data = new MonoColor[0, 0];

    public MonoPicture() {}
    public MonoPicture(int height, int width, MonoColor[,] data = null)
    {
        Height = Min(height, data.GetLength(0));
        Width = Min(width, data.GetLength(1));
        Data = new MonoColor[height, width];
        if (data != null)
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    Data[i, j] = data[i, j];
        }
    }
    public MonoPicture(MonoColor[,] data) : 
    this(data.GetLength(0), data.GetLength(1), data) {}
    public MonoPicture(int height, int width, MonoColor color) : 
    this(height, width, new MonoColor[height, width])
    {
        for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
                Data[i, j] = color;
    }
    public MonoPicture(MonoPicture pict) :
    this(pict.Height, pict.Width, pict.Data) {}
    public MonoPicture(MonoPicture pict, MonoColor color) :
    this(pict.Height, pict.Width, color) {}

    public static bool operator true(MonoPicture it)
        => (it.Height != 0) && (it.Width != 0);
    public static bool operator false(MonoPicture it)
        => (it.Height == 0) || (it.Width == 0);
    public static bool operator ==(MonoPicture a, MonoPicture b)
    {
        if (a.Height != b.Height || a.Width != b.Width) return false;
        for (int i = 0; i < a.Height; i++)
            for (int j = 0; j < a.Width; j++)
                if (a.Data[i, j] != b.Data[i, j]) return false;
        return true;
    }
    public static bool operator !=(MonoPicture a, MonoPicture b)
        => !(a == b);
    
    public override readonly bool Equals(object obj)
    {
        if (obj is MonoPicture other) return this == other;
        return false;
    }
    public override readonly int GetHashCode()
    {
        HashCode code = new();
        code.Add(Height);
        code.Add(Width);
        foreach (MonoColor color in Data) code.Add(color);
        return code.ToHashCode();
    }
    public override readonly string ToString()
    {
        string s = base.ToString() + ": ";
        foreach (MonoColor color in Data) s += color.ToString();
        return s;
    }
}