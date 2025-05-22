using Godot;
using System.Collections.Generic;

public partial class Component : Node2D, ISavable
{
    [Export]
    private ComponentPanel _panel;
    [Export]
    private ComponentPalette _palette;
    [Export]
    private ComponentProcessorMonitor _monitor;
    public override void _Ready()
    {
        base._Ready();

        Input.UseAccumulatedInput = false;

        _palette.Panel = _panel;
        _panel.Monitor = _monitor;
        _monitor.

        AddChild(new MouseInputHandler() {_panel, _palette, _monitor});
    }
    public Dictionary<string, Variant> Save()
    {
        Dictionary<string, Variant> save = new()
        {
            { "IsGridded", _panel.IsGridded }
        };

        Godot.Collections.Dictionary<int, Godot.Collections.Dictionary<string, int>> tiles = [];
        int i = 0;
        foreach (var (coords, tile) in _panel.GetTiles())
        {
            tiles.Add(i, new()
            {
                {"coords.X", coords.X},
                {"coords.Y", coords.Y},
                {"tile", (int)tile},
            });
            i++;
        }
        save.Add("PanelTiles", tiles);

        return save;
    }
    public void Load(IReadOnlyDictionary<string, Variant> loadedData)
    {
        if (loadedData.ContainsKey("IsGridded"))
        {
            bool isGridded = (bool)loadedData["IsGridded"];
            _palette.SetIsGridded(isGridded);
            _panel.IsGridded = isGridded;
        }
        if (loadedData.ContainsKey("PanelTiles"))
        {
            foreach (var (_, data) in
                    (Godot.Collections.Dictionary<int,
                     Godot.Collections.Dictionary<string, int>>)loadedData["PanelTiles"])
            {
                if (data.TryGetValue("coords.X", out int coordsX) &&
                    data.TryGetValue("coords.Y", out int coordsY) &&
                    data.TryGetValue("tile", out int tile))
                    _panel.DrawTile(new(coordsX, coordsY), 
                                    (ComponentPanelTile)tile);
            }
        }
    }
    public string GetIdentity() => Name;

}
