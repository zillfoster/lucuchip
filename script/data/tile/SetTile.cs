using Godot;

/// <summary>
/// 用于标识<c>TileSet</c>瓦片的类。
/// </summary>
/// <param name="SourceID">瓦片来自哪个地图册资源，用资源ID表示。<br/>如有疑问，请在Godot官方文档中搜索<c>TileSet.GetSourceID</c>方法。</param>
/// <param name="AtlasCoords">瓦片在地图册资源上的位置。</param>
public record SetTile(int SourceID, Vector2I AtlasCoords);