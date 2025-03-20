using Godot;
using System.Collections.Generic;

public class ComponentProcessor
{
    public int RoundCount => _roundCount;
    public void Start(List<MonoPicture> componentInput)
    {
        _roundCount = 0;
        // incomplete...
    }
    public void Step()
    {
        _roundCount++;
        // incomplete...
    }

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private int _roundCount = 0;
}