using Godot;
using System;
using static System.Math;

public partial class TheGame : Node2D
{
    private ChipColor _chosenColor = ChipColor.None;
    public override void _Ready()
    {
        base._Ready();

        #region 
        Chip chip = (Chip)GetNode<Node2D>("Chip");
        MouseHandleArea2D chipMouseHandle = (MouseHandleArea2D)GetNode<Area2D>("Chip/MouseHandleArea2D");
        // click left mouse button to paint
        chipMouseHandle.MouseLeftPressed += (position) => chip.AssignUnit(position, _chosenColor);
        // click right mouse button to erase
        chipMouseHandle.MouseRightPressed += (position) => chip.EraseUnit(position);
        int tileLength = chip.GetNode<TileMapLayer>("ChipLayer").TileSet.TileSize.X;
        CollisionShape2D shape2D = chipMouseHandle.GetNode<CollisionShape2D>("CollisionShape2D");
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
        // drag when clicking left mouse button to paint continuously
        chipMouseHandle.MouseLeftDragged += (position, relative) => 
            drag(position, relative, (p) => chip.AssignUnit(p, _chosenColor));
        // drag when clicking right mouse button to erase continuously
        chipMouseHandle.MouseRightDragged += (position, relative) =>
            drag(position, relative, (p) => chip.EraseUnit(p));
        #endregion

        #region 
        MouseHandleArea2D paletteMouseHandle = (MouseHandleArea2D)GetNode<Area2D>("PaletteLayer/MouseHandleArea2D");
        TileMapLayer selectionLayer = GetNode<TileMapLayer>("SelectionLayer");
        // click left mouse button to select color on palette
        paletteMouseHandle.MouseLeftPressed += (position) =>
        {
            ChipColor color = TheGame.ColorFrom(paletteMouseHandle.GetCurrentShapeIdx());
            if (color == ChipColor.Clear)
            {
                chip.ClearUnit();
                return;
            }
            _chosenColor = color;
            selectionLayer.Clear();
            selectionLayer.SetCell(selectionLayer.LocalToMap(position),
                                   selectionLayer.TileSet.GetSourceId(0),
                                   new Vector2I(0, 6));
        };
        // click right mouse button to cancel selection on palette
        paletteMouseHandle.MouseRightPressed += (position) =>
        {
            if (_chosenColor == TheGame.ColorFrom(paletteMouseHandle.GetCurrentShapeIdx()))
            {
                _chosenColor = ChipColor.None;
                selectionLayer.Clear();
            }
        };
        #endregion

        #region 
        TileMapLayer cursorLayer = GetNode<TileMapLayer>("CursorLayer");
        int sourceID = cursorLayer.TileSet.GetSourceId(0);
        chipMouseHandle.MouseDragged += (position, relative) => 
        {
            cursorLayer.Clear();
            cursorLayer.SetCell(cursorLayer.LocalToMap(position),
                                sourceID,
                                TheGame.AtlasCoordinateFrom(_chosenColor));
        };
        chipMouseHandle.MouseExited += () => cursorLayer.Clear();
        paletteMouseHandle.MouseDragged += (position, relative) => 
        {
            cursorLayer.Clear();
            cursorLayer.SetCell(cursorLayer.LocalToMap(position),
                                sourceID,
                                new Vector2I(1, 6));
        };
        paletteMouseHandle.MouseExited += () => cursorLayer.Clear();
        #endregion
    }
    private static ChipColor ColorFrom(int idx)
    {
        switch(idx)
        {
            case 0:  return ChipColor.Black;
            case 1:  return ChipColor.White;
            case 2:  return ChipColor.Red;
            case 3:  return ChipColor.Blue;
            case 4:  return ChipColor.Green;
            case 5:  return ChipColor.Yellow;
            case 6:  return ChipColor.Purple;
            case 7:  return ChipColor.Orange;
            case 8:  return ChipColor.Erase;
            case 9:  return ChipColor.Clear;
            default: return ChipColor.None;
        }
    }
    private static Vector2I AtlasCoordinateFrom(ChipColor color)
    {
        switch(color)
        {
            case ChipColor.Black:   return new Vector2I(0, 1);
            case ChipColor.White:   return new Vector2I(1, 1);
            case ChipColor.Red:     return new Vector2I(2, 1);
            case ChipColor.Blue:    return new Vector2I(3, 1);
            case ChipColor.Green:   return new Vector2I(4, 1);
            case ChipColor.Yellow:  return new Vector2I(5, 1);
            case ChipColor.Purple:  return new Vector2I(6, 1);
            case ChipColor.Orange:  return new Vector2I(7, 1);
            default:                return new Vector2I(-1, -1);
        }
    }
}
