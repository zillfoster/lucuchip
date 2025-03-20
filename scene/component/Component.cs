using Godot;
using System.Collections.Generic;

public partial class Component : Node2D
{
    [Export]
    private ComponentPanel _panel;
    [Export]
    private ComponentPalette _palette;
    public override void _Ready()
    {
        base._Ready();

        Input.UseAccumulatedInput = false;

        GameSaver.Save("IsGridded", () => _panel.IsGridded);
        GameSaver.Save("PanelTiles", () => 
        {
            Godot.Collections.Dictionary<int, Godot.Collections.Dictionary<string, int>> tiles = new();
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
            return tiles;
        });

        _palette.Panels.Add(_panel);

        AddChild(new MouseInputHandler() {_panel, _palette});
    }
}
