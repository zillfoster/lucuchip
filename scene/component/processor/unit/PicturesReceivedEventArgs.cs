using System;
using System.Collections.Generic;

public class PicturesReceivedEventArgs(Direction toward, List<MonoPicture> picts) : EventArgs
{
    public Direction Toward => toward;
    public IReadOnlyList<MonoPicture> Pictures => _pictures;

    private readonly List<MonoPicture> _pictures = [.. picts];
}