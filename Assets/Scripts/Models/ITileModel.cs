using UnityEngine;

namespace MVP.Model
{
    public interface ITileModel
    {
        TileData TileData { get; }
        SpecialTileData SpecialTileData { get; }
        Vector2Int Position { get; set; }
        bool IsSpecial { get; }
    }
}