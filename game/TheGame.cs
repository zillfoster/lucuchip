using Godot;
using System;
using System.Collections.Generic;
using static System.Math;
using PaletteColor = ChipColor;

public partial class TheGame : Node2D
{
    private PaletteColor _chosenColor = PaletteColor.None;
    [Signal]
    public delegate void InputEventEventHandler(InputEvent @event);
    [Signal]
    public delegate void GUIInputEventEventHandler(InputEvent @event);
    public override void _Ready()
    {
        base._Ready();
        Input.UseAccumulatedInput = false;

        #region 
        TileMapLayer framedChipBackgroundLayer = GetNode<TileMapLayer>("FramedChipBackgroundLayer");
        TileMapLayer paletteLayer = GetNode<TileMapLayer>("PaletteLayer");
        Chip chip = (Chip)GetNode<Node2D>("Chip");
        TileMapLayer chipLayer = GetNode<TileMapLayer>("Chip/ChipLayer");
        TileMapLayer cursorLayer = GetNode<TileMapLayer>("CursorLayer");
        TileMapLayer selectionLayer = GetNode<TileMapLayer>("SelectionLayer");
        MouseHandlerArea2D paletteMouseHandler = (MouseHandlerArea2D)GetNode<Area2D>("PaletteLayer/MouseHandlerArea2D");
        MouseHandlerArea2D chipMouseHandler = (MouseHandlerArea2D)GetNode<Area2D>("Chip/MouseHandlerArea2D");
        CollisionShape2D shape2D = chipMouseHandler.GetNode<CollisionShape2D>("CollisionShape2D");
        #endregion

        #region 
        InputEvent += (@event) => chipMouseHandler.OnOutsideInputEvent(@event);
        GUIInputEvent += (@event) => chipMouseHandler.OnOutsideInputEvent(@event, true);
        GUIInputEvent += (@event) => paletteMouseHandler.OnOutsideInputEvent(@event, true);
        #endregion

        #region 
        int tileLength = chipLayer.TileSet.TileSize.X;
        Dictionary<CollisionShape2D, Rect2> chipShape2Ds = new();
        foreach(Node s in chipMouseHandler.GetChildren()) 
            chipShape2Ds.Add((CollisionShape2D)s, ((CollisionShape2D)s).Shape.GetRect().Abs());
        Func<Vector2, bool> chipHasPoint = (position) =>
        {
            foreach(var (s, r) in chipShape2Ds) if (r.HasPoint(s.ToLocal(position))) return true;
            return false;
        };
        Action<Vector2, Vector2, Action<Vector2>> drag = (position, relative, operation) =>
        {
            int t = (int)Max(Abs(relative.X), Abs(relative.Y)) / tileLength + 1;
            for (int i = 0; i <= t; i++)
            {
                if (chipHasPoint(position)) operation(position);
                position += relative / t;
            }
        };
        // click left mouse button to paint
        chipMouseHandler.MouseLeftPressed += (position) => chip.AssignUnit(position, _chosenColor);
        // click right mouse button to erase
        chipMouseHandler.MouseRightPressed += (position) => chip.EraseUnit(position);
        // drag when clicking left mouse button to paint continuously
        chipMouseHandler.MouseLeftDragged += (position, relative) => 
            drag(position, relative, (p) => chip.AssignUnit(p, _chosenColor));
        // drag when clicking right mouse button to erase continuously
        chipMouseHandler.MouseRightDragged += (position, relative) =>
            drag(position, relative, (p) => chip.EraseUnit(p));
        #endregion

        #region 
        int paletteSourceID = paletteLayer.TileSet.GetSourceId(0);
        // click left mouse button to select color on palette
        paletteMouseHandler.MouseLeftPressed += (position) =>
        {
            PaletteColor color = TheGame.ColorFrom(paletteMouseHandler.GetCurrentShapeIdx());
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
                case PaletteColor.Step:
                case PaletteColor.Play:
                case PaletteColor.Speed:
                default: 
                    return;
            }
        };
        // click right mouse button to cancel selection on palette
        paletteMouseHandler.MouseRightPressed += (position) =>
        {
            if (_chosenColor == TheGame.ColorFrom(paletteMouseHandler.GetCurrentShapeIdx()))
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
                chip.GetUnit(position) == PaletteColor.Red) shadow = new Vector2I(4, 2);
            cursorLayer.SetCell(cursorLayer.LocalToMap(position),
                                cursorSourceID,
                                shadow);
        };
        Action<Vector2> cursorRightUpdate = (position) =>
        {
            cursorLayer.Clear();
            cursorLayer.SetCell(cursorLayer.LocalToMap(position),
                                cursorSourceID,
                                new Vector2I(0, 2));
        };
        chipMouseHandler.MouseLeftPressed += (position) => cursorLeftUpdate(position);
        chipMouseHandler.MouseRightReleased += (position) => cursorLeftUpdate(position);
        chipMouseHandler.MouseDragged += (position, relative) => cursorLeftUpdate(position);
        chipMouseHandler.MouseRightPressed += (position) => cursorRightUpdate(position);
        chipMouseHandler.MouseRightDragged += (position, relative) => cursorRightUpdate(position);
        chipMouseHandler.MouseExited += () => cursorLayer.Clear();
        paletteMouseHandler.MouseExited += () => cursorLayer.Clear();
        paletteMouseHandler.MouseDragged += (position, relative) => 
        {
            cursorLayer.Clear();
            cursorLayer.SetCell(cursorLayer.LocalToMap(position),
                                cursorSourceID,
                                new Vector2I(1, 6));
        };
        #endregion

        #region 
        Variant? v = JSONSaver.PleaseSaveAndTryLoad("IsGridEnabled",
                                                    () => framedChipBackgroundLayer.Visible);
        if (v.HasValue) framedChipBackgroundLayer.Visible = (bool)v;
        #endregion
    }
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        EmitSignal(SignalName.InputEvent, @event);
    }
    public void OnGUIInput(Node viewport, InputEvent @event) 
    {
        if (@event is InputEventMouseButton button)
        {
            button.Position = GetViewport().GetMousePosition();
            EmitSignal(SignalName.GUIInputEvent, button);
        }
        else if (@event is InputEventMouseMotion motion)
        {
            motion.Position = GetViewport().GetMousePosition();
            EmitSignal(SignalName.GUIInputEvent, motion);
        }
        else EmitSignal(SignalName.GUIInputEvent, @event);
    }
    private static PaletteColor ColorFrom(int idx)
    {
        switch(idx)
        {
            case 0:  return PaletteColor.Erase;
            case 1:  return PaletteColor.Black;
            case 2:  return PaletteColor.White;
            case 3:  return PaletteColor.Red;
            case 4:  return PaletteColor.Blue;
            case 5:  return PaletteColor.Green;
            case 6:  return PaletteColor.Yellow;
            case 7:  return PaletteColor.Purple;
            case 8:  return PaletteColor.Orange;
            case 9:  return PaletteColor.Clear;
            case 10: return PaletteColor.Step;
            case 11: return PaletteColor.Play;
            case 12: return PaletteColor.Speed;
            case 13: return PaletteColor.Grid;
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
