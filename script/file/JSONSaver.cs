using Godot;
using System;
using System.Collections.Generic;

public partial class JSONSaver : Node
{
    public static JSONSaver Instance;
    public Variant? PleaseSaveAndTryLoad(string saveKey, Func<Variant> saveMethod)
    {
        _willSaveData[saveKey] = saveMethod;
        if (_loadedData.ContainsKey(saveKey)) return _loadedData[saveKey];
        else return null;
    }
    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else QueueFree();
        _path = "user://save.json";
        if (FileAccess.FileExists(_path))
        {
            using FileAccess saveFile = FileAccess.Open(_path, FileAccess.ModeFlags.Read);
            string jsonString = saveFile.GetLine();
            Json json = new();
            Error parseResult = json.Parse(jsonString);
            if (parseResult != Error.Ok) GD.Print("JSON Parse Error.");
            Godot.Collections.Dictionary<string, Variant> loadedData = 
                new((Godot.Collections.Dictionary)json.Data);
            foreach(var (k, v) in loadedData) _loadedData.Add(k, v);
        }
    }
    private string _path;
    private readonly Dictionary<string, Variant> _loadedData = new();
    private readonly Dictionary<string, Func<Variant>> _willSaveData = new();
    public override void _Notification(int what)
    {
        if (what == 1006) // Mainloop.NOTIFICATION_WM_CLOSE_REQUEST = 1006
        {
            Godot.Collections.Dictionary<string, Variant> savingData = new();
            foreach(var (k, fv) in _willSaveData) savingData[k] = fv();
            string jsonString = Json.Stringify(savingData);
            using FileAccess saveFile = FileAccess.Open(_path, FileAccess.ModeFlags.Write);
            saveFile.StoreLine(jsonString);
        }
    }
}