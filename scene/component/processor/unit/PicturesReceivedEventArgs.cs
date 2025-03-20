using System;
using System.Collections.Generic;

public class PicturesReceivedEventArgs : EventArgs
{
    public Direction Toward => _toward;
    public IReadOnlyList<MonoPicture> Pictures => _pictures;
    public PicturesReceivedEventArgs(Direction toward, List<MonoPicture> picts)
    {
        _toward = toward;
        _pictures = new(picts);
    }
    
    private Direction _toward = Direction.None;
    private List<MonoPicture> _pictures = new();
}