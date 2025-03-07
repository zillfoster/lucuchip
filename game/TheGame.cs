using Godot;
using System;
using static System.Math;
using PaletteColor = ChipColor;

public partial class TheGame : Node2D
{
    private PaletteColor _chosenColor = PaletteColor.None;
    public override void _Ready()
    {
        base._Ready();

        #region 
        TileMapLayer framedChipBackgroundLayer = GetNode<TileMapLayer>("FramedChipBackgroundLayer");
        TileMapLayer paletteLayer = GetNode<TileMapLayer>("PaletteLayer");
        Chip chip = (Chip)GetNode<Node2D>("Chip");
        TileMapLayer chipLayer = GetNode<TileMapLayer>("Chip/ChipLayer");
        TileMapLayer cursorLayer = GetNode<TileMapLayer>("CursorLayer");
        TileMapLayer selectionLayer = GetNode<TileMapLayer>("SelectionLayer");
        MouseHandleArea2D paletteMouseHandle = (MouseHandleArea2D)GetNode<Area2D>("PaletteLayer/MouseHandleArea2D");
        MouseHandleArea2D chipMouseHandle = (MouseHandleArea2D)GetNode<Area2D>("Chip/MouseHandleArea2D");
        CollisionShape2D shape2D = chipMouseHandle.GetNode<CollisionShape2D>("CollisionShape2D");
        #endregion

        #region 
        int tileLength = chipLayer.TileSet.TileSize.X;
        Rect2 chipRect = shape2D.Shape.GetRect().Abs();
        Action<Vector2, Vector2, Action<Vector2>> drag = (position, relative, operation) =>
        {
            int t = (int)Max(Abs(relative.X), Abs(relative.Y)) / tileLength + 1;
            for (int i = 0; i <= t; i++)
            {
                if (chipRect.HasPoint(shape2D.ToLocal(position))) operation(position);
                position += relative/t;
            }
        };
        // click left mouse button to paint
        chipMouseHandle.MouseLeftPressed += (position) => chip.AssignUnit(position, _chosenColor);
        // click right mouse button to erase
        chipMouseHandle.MouseRightPressed += (position) => chip.EraseUnit(position);
        // drag when clicking left mouse button to paint continuously
        chipMouseHandle.MouseLeftDragged += (position, relative) => 
            drag(position, relative, (p) => chip.AssignUnit(p, _chosenColor));
        // drag when clicking right mouse button to erase continuously
        chipMouseHandle.MouseRightDragged += (position, relative) =>
            drag(position, relative, (p) => chip.EraseUnit(p));
        #endregion

        #region 
        int paletteSourceID = paletteLayer.TileSet.GetSourceId(0);
        // click left mouse button to select color on palette
        paletteMouseHandle.MouseLeftPressed += (position) =>
        {
            PaletteColor color = TheGame.ColorFrom(paletteMouseHandle.GetCurrentShapeIdx());
            switch(color)
            {
                case PaletteColor.Black:
                case PaletteColor.White:
                case PaletteColor.Red:
                case PaletteColor.Blue:
                case PaletteColor.Green:
                case PaletteColor.Yellow:
                case PaletteColor.Purple:
                case PaletteColor.Orange:
                case PaletteColor.Erase:
                    _chosenColor = color;
                    selectionLayer.Clear();
                    selectionLayer.SetCell(selectionLayer.LocalToMap(position),
                                   selectionLayer.TileSet.GetSourceId(0),
                                   new Vector2I(0, 6));
                    return;
                case PaletteColor.Clear:
                    chip.ClearUnit();
                    return;
                case PaletteColor.Grid:
                    if (framedChipBackgroundLayer.Visible)
                    {
                        framedChipBackgroundLayer.Visible = false;
                        paletteLayer.SetCell(paletteLayer.LocalToMap(position),
                                             paletteSourceID,
                                             new Vector2I(4, 3));
                    }
                    else
                    {
                        framedChipBackgroundLayer.Visible = true;
                        paletteLayer.SetCell(paletteLayer.LocalToMap(position),
                                             paletteSourceID,
                                             new Vector2I(3, 3));
                    }
                    return;
                case PaletteColor.New:
                case PaletteColor.Load:
                case PaletteColor.Save:
                default: 
                    return;
            }
        };
        // click right mouse button to cancel selection on palette
        paletteMouseHandle.MouseRightPressed += (position) =>
        {
            if (_chosenColor == TheGame.ColorFrom(paletteMouseHandle.GetCurrentShapeIdx()))
            {
                _chosenColor = PaletteColor.None;
                selectionLayer.Clear();
            }
        };
        #endregion

        #region 
        int cursorSourceID = cursorLayer.TileSet.GetSourceId(0);
        Action<Vector2> cursorLeftUpdate = (position) =>
        {
            cursorLayer.Clear();
            Vector2I shadow = TheGame.AtlasCoordinateFrom(_chosenColor);
            if (chip.GetUnit(position) == _chosenColor) return;
            if (_chosenColor == PaletteColor.Erase && 
                chip.GetUnit(position) == PaletteColor.Red) shadow = new(4, 2);
            cursorLayer.SetCell(cursorLayer.LocalToMap(position),
                                cursorSourceID,
                                shadow);
        };
        Action<Vector2> cursorRightUpdate = (position) =>
        {
            cursorLayer.Clear();
            cursorLayer.SetCell(cursorLayer.LocalToMap(position),
                                cursorSourceID,
                                new(0, 2));
        };
        chipMouseHandle.MouseLeftPressed += (position) => cursorLeftUpdate(position);
        chipMouseHandle.MouseRightReleased += (position) => cursorLeftUpdate(position);
        chipMouseHandle.MouseDragged += (position, relative) => cursorLeftUpdate(position);
        chipMouseHandle.MouseRightPressed += (position) => cursorRightUpdate(position);
        chipMouseHandle.MouseRightDragged += (position, relative) => cursorRightUpdate(position);
        chipMouseHandle.MouseExited += () => cursorLayer.Clear();
        paletteMouseHandle.MouseExited += () => cursorLayer.Clear();
        paletteMouseHandle.MouseDragged += (position, relative) => 
        {
            cursorLayer.Clear();
            cursorLayer.SetCell(cursorLayer.LocalToMap(position),
                                cursorSourceID,
                                new Vector2I(1, 6));
        };
        #endregion
    }
    private static PaletteColor ColorFrom(int idx)
    {
        switch(idx)
        {
            case 0:  return PaletteColor.Black;
            case 1:  return PaletteColor.White;
            case 2:  return PaletteColor.Red;
            case 3:  return PaletteColor.Blue;
            case 4:  return PaletteColor.Green;
            case 5:  return PaletteColor.Yellow;
            case 6:  return PaletteColor.Purple;
            case 7:  return PaletteColor.Orange;
            case 8:  return PaletteColor.Erase;
            case 9:  return PaletteColor.Clear;
            case 10:  return PaletteColor.Grid;
            case 11:  return PaletteColor.New;
            case 12:  return PaletteColor.Load;
            case 13:  return PaletteColor.Save;
            default: return PaletteColor.None;
        }
    }
    private static Vector2I AtlasCoordinateFrom(PaletteColor color)
    {
        switch(color)
        {
            case PaletteColor.Black:   return new Vector2I(0, 1);
            case PaletteColor.White:   return new Vector2I(1, 1);
            case PaletteColor.Red:     return new Vector2I(2, 1);
            case PaletteColor.Blue:    return new Vector2I(3, 1);
            case PaletteColor.Green:   return new Vector2I(4, 1);
            case PaletteColor.Yellow:  return new Vector2I(5, 1);
            case PaletteColor.Purple:  return new Vector2I(6, 1);
            case PaletteColor.Orange:  return new Vector2I(7, 1);
            case PaletteColor.Erase:   return new Vector2I(0, 2);
            default:                   return new Vector2I(-1, -1);
        }
    }
}
