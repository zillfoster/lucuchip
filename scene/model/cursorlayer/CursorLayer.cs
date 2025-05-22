using Godot;
using System.Collections.Generic;

/// <summary>
/// 基于<c>TileMapLayer</c>的光标。
/// </summary>
public partial class CursorLayer : TileMapLayer, IMouseInputable
{
    /// <summary>
    /// 光标的瓦片信息。
    /// </summary>
    public LayerTile Cursor { get; }
    /// <summary>
    /// 光标选中的瓦片信息。
    /// </summary>
    public LayerTile Selection { get; }
    /// <summary>
    /// 光标生效的区域。
    /// </summary>
    public List<Rect2I> CursorField { get; } = [];
    /// <summary>
    /// 光标能选中的区域。
    /// </summary>
    public List<Rect2I> SelectionField { get; } = [];
    /// <summary>
    /// 光标能否选中网格。
    /// </summary>
    public bool IsSelectable { get; set; } = true;

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    public CursorLayer()
    {
        Cursor = new((_, _) => Refresh(),
                        (_, _) => Refresh(),
                        (coords) => coords.IsValidIn(CursorField));
        Selection = new((_, _) => Refresh(),
                        (_, _) => Refresh(),
                        (coords) => coords.IsValidIn(SelectionField));
    }
    private void Refresh()
    {
        Clear();
        TryAssignTile(Cursor.Coords, Cursor.Style);
        TryAssignTile(Selection.Coords, Selection.Style);
    }
    private void TryAssignTile(Vector2I? coords, SetTile style)
    {
        if (coords.HasValue && style != null)
            SetCell(coords.Value, style.SourceID, style.AtlasCoords);
    }
    void IMouseInputable.OnMouseButton(Vector2 position, MouseButton button, bool isPressed)
    {
        Vector2I coords = this.CoordsFrom(position);
        Cursor.Coords = coords;
        if (!IsSelectable || !SelectionField.Contains(coords)) return;
        if (isPressed)
        {
            if (button == MouseButton.Left)
            {
                Selection.Coords = this.CoordsFrom(position);
                return;
            }
            if (button == MouseButton.Right)
            {
                if (Selection.Coords == this.CoordsFrom(position)) Selection.Coords = null;
                return;
            }
        }
    }

    void IMouseInputable.OnMouseMotion(Vector2 position, Vector2 relative, MouseButtonMask mask, MouseButton lastButton, Vector2? lastPosition)
    {
        Cursor.Coords = this.CoordsFrom(position);
    }
}