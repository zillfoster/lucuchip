using System;
using Godot;

/// <summary>
/// 用于标识<c>TileMapLayer</c>瓦片的类。
/// </summary>
public class LayerTile(Action<Vector2I?, SetTile> coordsSet = null,
                 Action<Vector2I?, SetTile> styleSet = null,
                 Func<Vector2I?, bool> isCoordsVaild = null,
                 Func<SetTile, bool> isStyleVaild = null)
{
    public Vector2I? Coords
    {
        get => _coords;
        set
        {
            _coords = _isCoordsVaild(value) ? value : null;
            _coordsSet(_coords, _style);
        }
    }
    public SetTile Style
    {
        get => _style;
        set
        {
            _style = _isStyleVaild(value) ? value : null;
            _styleSet(_coords, _style);
        }
    }

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private Vector2I? _coords = null;
    private SetTile _style = null;
    private readonly Action<Vector2I?, SetTile> _coordsSet = coordsSet ?? ((_, _) => {});
    private readonly Action<Vector2I?, SetTile> _styleSet = styleSet ?? ((_, _) => {});
    private readonly Func<Vector2I?, bool> _isCoordsVaild = isCoordsVaild ?? ((_) => true);
    private readonly Func<SetTile, bool> _isStyleVaild = isStyleVaild ?? ((_) => true);
}