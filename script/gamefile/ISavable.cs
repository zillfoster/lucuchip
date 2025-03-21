using Godot;
using System.Collections.Generic;

public interface ISavable
{
    public Dictionary<string, Variant> Save();
    public void Load(IReadOnlyDictionary<string, Variant> loadedData);
    public string GetIdentity();
}