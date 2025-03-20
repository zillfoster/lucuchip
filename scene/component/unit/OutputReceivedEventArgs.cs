using System;
using System.Collections.Generic;

public class OutputReceivedEventArgs : EventArgs
{
    public Direction Toward { get; }
    public IReadOnlyList<MonoPicture> Pictures => _pictures;
    public OutputReceivedEventArgs(Direction toward, List<MonoPicture> picts)
    {
        _toward = toward;
        _pictures = new(picts);
    }
    
    private Direction _toward = Direction.None;
    private List<MonoPicture> _pictures = new();
}