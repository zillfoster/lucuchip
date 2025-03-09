using Godot;
using System;
using System.Collections.Generic;

public partial class JSONSaver : Node
{
    public static Variant? PleaseSaveAndTryLoad(string saveKey, 
                                                Func<Variant> saveMethod)
    {
        _willSaveData[saveKey] = saveMethod;
        if (_loadedData.ContainsKey(saveKey)) 
            return _loadedData[saveKey];
        else return null;
    }


    // Below this comment, all the members are (somehow) private.
    // No need to read them unless you are modifying this class.
    private static JSONSaver Instance;
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
        if (Instance == null) Instance = this;
        else QueueFree();
        if (FileAccess.FileExists(SAVE_PATH))
        {
            using FileAccess saveFile = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Read);
            string jsonString = saveFile.GetLine();
            Json json = new();
            Error parseResult = json.Parse(jsonString);
            if (parseResult != Error.Ok) GD.Print("JSON Parse Error.");
            Godot.Collections.Dictionary<string, Variant> loadedData = 
                new((Godot.Collections.Dictionary)json.Data);
            foreach(var (k, v) in loadedData) _loadedData.Add(k, v);
        }
        if (!_loadedData.ContainsKey("version")) _loadedData.Clear();
        else if ((string)_loadedData["version"] != VERSION) OnVersionChanged();
        JSONSaver.PleaseSaveAndTryLoad("version", () => VERSION);
    }
    public override void _Notification(int what)
    {
        if (what == 1006) // Mainloop.NOTIFICATION_WM_CLOSE_REQUEST = 1006
        {
            Godot.Collections.Dictionary<string, Variant> savingData = new();
            foreach(var (k, fv) in _willSaveData) savingData[k] = fv();
            string jsonString = Json.Stringify(savingData);
            using FileAccess saveFile = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Write);
            saveFile.StoreLine(jsonString);
        }
    }
}