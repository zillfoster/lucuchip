using Godot;
using System;
using System.Collections.Generic;
using static System.Math;

public partial class TheGame : Node2D
{
    private ChipColor _chosenColor = ChipColor.None;
    public override void _Ready()
    {
        base._Ready();
        Input.UseAccumulatedInput = false;

        #region 
        TileMapLayer paletteLayer = GetNode<TileMapLayer>("PaletteLayer");
        ChipLayer chipLayer = (ChipLayer)GetNode<TileMapLayer>("ChipLayer");
        TileMapLayer cursorLayer = GetNode<TileMapLayer>("CursorLayer");
        TileMapLayer selectionLayer = GetNode<TileMapLayer>("SelectionLayer");
        MouseHandlerArea2D paletteMouseHandler = (MouseHandlerArea2D)GetNode<Area2D>("PaletteLayer/MouseHandlerArea2D");
        MouseHandlerArea2D chipMouseHandler = (MouseHandlerArea2D)GetNode<Area2D>("ChipLayer/MouseHandlerArea2D");
        CollisionShape2D shape2D = chipMouseHandler.GetNode<CollisionShape2D>("CollisionShape2D");
        TileMapLayer framedChipBackground = GetNode<TileMapLayer>("Background/FramedChipBackground");
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
        chipMouseHandler.MouseLeftPressed += (position) => chipLayer.AssignUnit(position, _chosenColor);
        // click right mouse button to erase
        chipMouseHandler.MouseRightPressed += (position) => chipLayer.EraseUnit(position);
        // drag when clicking left mouse button to paint continuously
        chipMouseHandler.MouseLeftDragged += (position, relative) => 
            drag(position, relative, (p) => chipLayer.AssignUnit(p, _chosenColor));
        // drag when clicking right mouse button to erase continuously
        chipMouseHandler.MouseRightDragged += (position, relative) =>
            drag(position, relative, (p) => chipLayer.EraseUnit(p));
        #endregion

        #region 
        int paletteSourceID = paletteLayer.TileSet.GetSourceId(0);
        // click left mouse button to select color on palette
        paletteMouseHandler.MouseLeftPressed += (position) =>
        {
            PaletteChoice choice = TheGame.ChoiceFrom(paletteMouseHandler.GetCurrentShapeIdx());
            if (choice.ToChipColor() != ChipColor.None)
            {
                _chosenColor = choice.ToChipColor();
                selectionLayer.Clear();
                selectionLayer.SetCell(selectionLayer.LocalToMap(position),
                                selectionLayer.TileSet.GetSourceId(0),
                                new Vector2I(0, 6));
            }
            else switch(choice)
            {
                case PaletteChoice.Clear:
                    chipLayer.ClearUnit();
                    return;
                case PaletteChoice.GridOrStop:
                    if (framedChipBackground.Visible)
                    {
                        framedChipBackground.Visible = false;
                        paletteLayer.SetCell(paletteLayer.LocalToMap(position),
                                             paletteSourceID,
                                             new Vector2I(4, 3));
                    }
                    else
                    {
                        framedChipBackground.Visible = true;
                        paletteLayer.SetCell(paletteLayer.LocalToMap(position),
                                             paletteSourceID,
                                             new Vector2I(3, 3));
                    }
                    return;
                case PaletteChoice.Step:
                case PaletteChoice.PlayOrPause:
                case PaletteChoice.SpeedOrPause:
                default: 
                    return;
            }
        };
        // click right mouse button to cancel selection on palette
        paletteMouseHandler.MouseRightPressed += (position) =>
        {
            if (_chosenColor == TheGame.ChoiceFrom(
                                paletteMouseHandler.GetCurrentShapeIdx()).
                                ToChipColor())
            {
                _chosenColor = ChipColor.None;
                selectionLayer.Clear();
            }
        };
        #endregion

        #region 
        int cursorSourceID = cursorLayer.TileSet.GetSourceId(0);
        Action<Vector2> cursorLeftUpdate = (position) =>
        {
            cursorLayer.Clear();
            Vector2I shadow = TheGame.ShadowAtlasCoordinateFrom(_chosenColor);
            if (chipLayer.GetUnit(position) == _chosenColor) return;
            if (_chosenColor == ChipColor.Erase && 
                chipLayer.GetUnit(position) == ChipColor.Red) shadow = new Vector2I(4, 2);
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
                                                    () => framedChipBackground.Visible);
        if (v.HasValue) framedChipBackground.Visible = (bool)v;
        #endregion

        #region 
        InputEvent += (@event) => chipMouseHandler.OnOutsideInputEvent(@event);
        GUIInputEvent += (@event) => chipMouseHandler.OnOutsideInputEvent(@event, true);
        GUIInputEvent += (@event) => paletteMouseHandler.OnOutsideInputEvent(@event, true);
        #endregion
    }
    [Signal]
    public delegate void InputEventEventHandler(InputEvent @event);
    [Signal]
    public delegate void GUIInputEventEventHandler(InputEvent @event);
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
    private static PaletteChoice ChoiceFrom(int idx)
    {
        switch(idx)
        {
            case 0:  return PaletteChoice.Erase;
            case 1:  return PaletteChoice.Black;
            case 2:  return PaletteChoice.White;
            case 3:  return PaletteChoice.Red;
            case 4:  return PaletteChoice.Blue;
            case 5:  return PaletteChoice.Green;
            case 6:  return PaletteChoice.Yellow;
            case 7:  return PaletteChoice.Purple;
            case 8:  return PaletteChoice.Orange;
            case 9:  return PaletteChoice.Clear;
            case 10: return PaletteChoice.Step;
            case 11: return PaletteChoice.PlayOrPause;
            case 12: return PaletteChoice.SpeedOrPause;
            case 13: return PaletteChoice.GridOrStop;
            default: return PaletteChoice.None;
        }
    }
    private static Vector2I ShadowAtlasCoordinateFrom(ChipColor color)
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
            case ChipColor.Erase:   return new Vector2I(0, 2);
            default:                return new Vector2I(-1, -1);
        }
    }
}
