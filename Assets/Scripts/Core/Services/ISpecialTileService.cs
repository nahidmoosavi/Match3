using System.Collections.Generic;
using MVP.Model;
using UnityEngine;

namespace Core.Services
{
    public interface ISpecialTileService
    {
        List<Vector2Int> GetAffectedTiles(IBoardModel boardModel, Vector2Int position, SpecialTileData specialTileData);
        List<Vector2Int> GetCascadeAffectedTiles(IBoardModel boardModel, List<Vector2Int> initialPositions);
    }
}