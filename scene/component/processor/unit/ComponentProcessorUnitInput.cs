using System.Collections.Generic;

public class ComponentProcessorUnitInput : ComponentProcessorUnit
{
    public ComponentProcessorUnitInput() : base(false, false, false) {}
    protected override Dictionary<Directions, List<MonoPicture>> Send(Dictionary<Direction, List<MonoPicture>> received)
    {
        Dictionary<Directions, List<MonoPicture>> sending = [];
        foreach (var (dir, picts) in _inputPicts) sending.Add(dir.ToDirections(), [.. picts]);
        return sending;
    }
    private readonly Dictionary<Direction, List<MonoPicture>> _inputPicts = [];
    public override void Initialize()
    {
        base.Initialize();
        _inputPicts.Clear();
    }
    public void OnInputReceived(object sender, PicturesReceivedEventArgs e)
    {
        if (!_inputPicts.ContainsKey(e.Toward)) _inputPicts[e.Toward] = [];
        foreach (MonoPicture pict in e.Pictures) _inputPicts[e.Toward].Add(pict);
    }
}