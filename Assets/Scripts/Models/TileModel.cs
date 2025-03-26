using MVP.Model;
using UnityEngine;

public sealed class TileModel: ModelBase, ITileModel
{
    public TileData TileData { get; }
    public SpecialTileData SpecialTileData {get;}
    public Vector2Int Position { get;  set; }
    public bool IsSpecial => SpecialTileData != null;

    public TileModel(TileData tileData, Vector2Int position)
    {
        TileData = tileData;
        Position = position;
    }

    public TileModel(SpecialTileData specialTileData, Vector2Int position)
    {
        SpecialTileData = specialTileData;
        Position = position;
    }
    
    public override void Reset()
    {
        Position = Vector2Int.zero;
    }
}


