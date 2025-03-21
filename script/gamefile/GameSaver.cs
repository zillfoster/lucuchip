using Godot;
using System;
using System.Collections.Generic;

public partial class GameSaver : Node
{
    public static Variant? Load(string saveKey)
    {
        if (_loadedData.ContainsKey(saveKey))
            return _loadedData[saveKey];
        else return null;
    }
    public static void Save(string saveKey, Func<Variant> saveMethod)
        => _willSaveData[saveKey] = saveMethod;
    public static void Load(ISavable savable)
    {
        Variant? v = Load(savable.GetIdentity());
        if (v.HasValue) savable.Load(new Dictionary<string, Variant>((Godot.Collections.Dictionary<string, Variant>)v));
    }
    public static void Save(ISavable savable)
        => Save(savable.GetIdentity(), () => new Godot.Collections.Dictionary<string, Variant>(savable.Save()));
    public static void LoadAndSave(ISavable savable) { Load(savable); Save(savable); }

    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private static GameSaver This;
    private const string VERSION = "demo 0.1";
    private const string SAVE_PATH = "user://save.json";
    private static readonly Dictionary<string, Variant> _loadedData = new();
    private static readonly Dictionary<string, Func<Variant>> _willSaveData = new();
    private static void OnVersionChanged()
    {
        // write operation for transforming data from previous version
    }
    public override void _Ready()
    {
        base._Ready();

        if (This == null) This = this;
        else QueueFree();

        if (FileAccess.FileExists(SAVE_PATH))
        {
            using FileAccess saveFile = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Read);
            
            Json json = new();
            Error parseResult = json.Parse(saveFile.GetLine());
            if (parseResult != Error.Ok) GD.Print("JSON Parse Error.");
            Godot.Collections.Dictionary<string, Variant> loadedData = 
                new((Godot.Collections.Dictionary<string, Variant>)json.Data);
            foreach(var (k, v) in loadedData) _loadedData.Add(k, v);
        }

        if (!_loadedData.ContainsKey("version")) _loadedData.Clear();
        else if ((string)_loadedData["version"] != VERSION) OnVersionChanged();
        GameSaver.Save("version", () => VERSION);
    }
    public override void _Notification(int what)
    {
        if (what == 1006) // Mainloop.NOTIFICATION_WM_CLOSE_REQUEST = 1006
        {
            Godot.Collections.Dictionary<string, Variant> savingData = new();
            foreach(var (key, method) in _willSaveData) savingData[key] = method();

            using FileAccess saveFile = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Write);
            saveFile.StoreLine(Json.Stringify(savingData));
        }
    }
}