using Godot;

public partial class ComponentPaletteMainLayer : TileMapLayer
{
    public PaletteStatus Status { get; private set; } = PaletteStatus.Drawing;
    public enum PaletteStatus { Drawing, Processing, Pausing }
    public bool IsGridded
    {
        get => _isGridded;
        set
        {
            if (Status == PaletteStatus.Drawing)
            {
                _isGridded = value;
                if (value) AssignChoice(_gridCoords, ComponentPaletteChoice.GridOn);
                else AssignChoice(_gridCoords, ComponentPaletteChoice.GridOff);
            }
        }
    }
    public void AssignChoice(Vector2I coords, ComponentPaletteChoice choice)
    {
        if (choice == ComponentPaletteChoice.None) return;
        SetCell(coords, _sourceID, ComponentPaletteMainLayer.AtlasCoordsFrom(choice));
    }
    public ComponentPaletteChoice GetChoice(Vector2I coords)
        => ComponentPaletteMainLayer.ChoiceFrom(GetCellAtlasCoords(coords));
    public (ComponentPaletteChoice, bool) GetChoiceIncludingShadow(Vector2I coords)
    {
        bool isShadow = false;
        ComponentPaletteChoice choice = ChoiceFrom(GetCellAtlasCoords(coords));
        if (choice == ComponentPaletteChoice.None)
        {
            choice = ChoiceFrom(GetCellAtlasCoords(coords) - new Vector2I(0, 1));
            if (choice != ComponentPaletteChoice.None) isShadow = true;
        }
        return (choice, isShadow);
    }
    public void SetStatus(PaletteStatus status, Vector2I clickedCoords = default)
    {
        switch (status)
        {
            case PaletteStatus.Drawing:
                if (Status != PaletteStatus.Drawing)
                {
                    foreach (Vector2I coords in GetUsedCells())
                    {
                        ComponentPaletteChoice choice = GetChoiceIncludingShadow(coords).Item1;
                        switch (choice)
                        {
                            case ComponentPaletteChoice.Black:
                            case ComponentPaletteChoice.White:
                            case ComponentPaletteChoice.Red:
                            case ComponentPaletteChoice.Blue:
                            case ComponentPaletteChoice.Green:
                            case ComponentPaletteChoice.Yellow:
                            case ComponentPaletteChoice.Purple:
                            case ComponentPaletteChoice.Orange:
                            case ComponentPaletteChoice.Input:
                            case ComponentPaletteChoice.Output:
                            case ComponentPaletteChoice.Erase:
                            case ComponentPaletteChoice.Clear:
                                SetCell(coords, _sourceID, ComponentPaletteMainLayer.AtlasCoordsFrom(choice));
                                break;
                            case ComponentPaletteChoice.Halt:
                                if (_isGridded) AssignChoice(coords, ComponentPaletteChoice.GridOn);
                                else AssignChoice(coords, ComponentPaletteChoice.GridOff);
                                break;
                            case ComponentPaletteChoice.Pause:
                                if (coords == _playCoords) AssignChoice(coords, ComponentPaletteChoice.Play);
                                else if (coords == _speedCoords) AssignChoice(coords, ComponentPaletteChoice.Speed);
                                break;
                        }
                    }
                }
                Status = status;
                return;
            case PaletteStatus.Processing:
            case PaletteStatus.Pausing:
                if (Status == PaletteStatus.Drawing)
                {
                    foreach (Vector2I coords in GetUsedCells())
                    {
                        ComponentPaletteChoice choice = GetChoice(coords);
                        switch (choice)
                        {
                            case ComponentPaletteChoice.Black:
                            case ComponentPaletteChoice.White:
                            case ComponentPaletteChoice.Red:
                            case ComponentPaletteChoice.Blue:
                            case ComponentPaletteChoice.Green:
                            case ComponentPaletteChoice.Yellow:
                            case ComponentPaletteChoice.Purple:
                            case ComponentPaletteChoice.Orange:
                            case ComponentPaletteChoice.Input:
                            case ComponentPaletteChoice.Output:
                            case ComponentPaletteChoice.Erase:
                            case ComponentPaletteChoice.Clear:
                                SetCell(coords, _sourceID, ComponentPaletteMainLayer.AtlasCoordsFrom(choice) + new Vector2I(0, 1));
                                break;
                            case ComponentPaletteChoice.GridOff:
                            case ComponentPaletteChoice.GridOn:
                                AssignChoice(coords, ComponentPaletteChoice.Halt);
                                break;
                        }
                    }
                }
                switch (GetChoice(clickedCoords))
                {
                    case ComponentPaletteChoice.Step:
                        break;
                    case ComponentPaletteChoice.Play:
                    case ComponentPaletteChoice.Speed:
                        AssignChoice(clickedCoords, ComponentPaletteChoice.Pause);
                        break;
                    case ComponentPaletteChoice.Pause:
                        if (clickedCoords == _playCoords) AssignChoice(clickedCoords, ComponentPaletteChoice.Play);
                        else if (clickedCoords == _speedCoords) AssignChoice(clickedCoords, ComponentPaletteChoice.Speed);
                        break;
                }
                Status = status;
                return;
            default:
                return;
        }
    }
    
    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private int _sourceID;
    private Vector2I _playCoords = new(17, 0);
    private Vector2I _speedCoords = new(18, 0);
    private Vector2I _gridCoords = new(19, 0);
    private bool _isGridded = true;
    public override void _Ready()
    {
        base._Ready();
        _sourceID = TileSet.GetSourceId(0);
    }
    private static Vector2I AtlasCoordsFrom(ComponentPaletteChoice choice)
    {
        switch(choice)
        {
            case ComponentPaletteChoice.Black:       return new Vector2I(0, 0);
            case ComponentPaletteChoice.White:       return new Vector2I(1, 0);
            case ComponentPaletteChoice.Red:         return new Vector2I(2, 0);
            case ComponentPaletteChoice.Blue:        return new Vector2I(3, 0);
            case ComponentPaletteChoice.Green:       return new Vector2I(4, 0);
            case ComponentPaletteChoice.Yellow:      return new Vector2I(5, 0);
            case ComponentPaletteChoice.Purple:      return new Vector2I(6, 0);
            case ComponentPaletteChoice.Orange:      return new Vector2I(7, 0);
            case ComponentPaletteChoice.Input:       return new Vector2I(0, 2);
            case ComponentPaletteChoice.Output:      return new Vector2I(1, 2);
            case ComponentPaletteChoice.Erase:       return new Vector2I(2, 2);
            case ComponentPaletteChoice.Clear:       return new Vector2I(3, 2);
            case ComponentPaletteChoice.GridOn:      return new Vector2I(4, 3);
            case ComponentPaletteChoice.GridOff:     return new Vector2I(5, 3);
            case ComponentPaletteChoice.Step:        return new Vector2I(4, 2);
            case ComponentPaletteChoice.Play:        return new Vector2I(5, 2);
            case ComponentPaletteChoice.Speed:       return new Vector2I(6, 2);
            case ComponentPaletteChoice.Pause:       return new Vector2I(7, 2);
            case ComponentPaletteChoice.Halt:        return new Vector2I(6, 3);
            default:                                 return new Vector2I(-1, -1);
        }
    }
    private static ComponentPaletteChoice ChoiceFrom(Vector2I atlasCoords)
    {
        switch(atlasCoords)
        {
            case Vector2I(0, 0):    return ComponentPaletteChoice.Black;
            case Vector2I(1, 0):    return ComponentPaletteChoice.White;
            case Vector2I(2, 0):    return ComponentPaletteChoice.Red;
            case Vector2I(3, 0):    return ComponentPaletteChoice.Blue;
            case Vector2I(4, 0):    return ComponentPaletteChoice.Green;
            case Vector2I(5, 0):    return ComponentPaletteChoice.Yellow;
            case Vector2I(6, 0):    return ComponentPaletteChoice.Purple;
            case Vector2I(7, 0):    return ComponentPaletteChoice.Orange;
            case Vector2I(0, 2):    return ComponentPaletteChoice.Input;
            case Vector2I(1, 2):    return ComponentPaletteChoice.Output;
            case Vector2I(2, 2):    return ComponentPaletteChoice.Erase;
            case Vector2I(3, 2):    return ComponentPaletteChoice.Clear;
            case Vector2I(4, 3):    return ComponentPaletteChoice.GridOn;
            case Vector2I(5, 3):    return ComponentPaletteChoice.GridOff;
            case Vector2I(4, 2):    return ComponentPaletteChoice.Step;
            case Vector2I(5, 2):    return ComponentPaletteChoice.Play;
            case Vector2I(6, 2):    return ComponentPaletteChoice.Speed;
            case Vector2I(7, 2):    return ComponentPaletteChoice.Pause;
            case Vector2I(6, 3):    return ComponentPaletteChoice.Halt;
            default:                return ComponentPaletteChoice.None;
        }
    }
}
