using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Services
{
    public interface ITileSpawnerService
    {
        ITilePresenter SpawnTile(Vector2Int position, TileData tileData, Transform parent, Color color,
            Func<Vector2Int, Task> onTileClicked);
        ITilePresenter SpawnSpecialTile(Vector2Int position, SpecialTileData specialTileData, Transform parent,
            Func<Vector2Int, Task> onTileClicked);
    }
}