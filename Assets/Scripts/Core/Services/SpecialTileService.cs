using System.Collections.Generic;
using System.Linq;
using MVP.Model;
using UnityEngine;

namespace Core.Services
{
    public class SpecialTileService : ISpecialTileService
    {
        public List<Vector2Int> GetAffectedTiles(IBoardModel boardModel, Vector2Int position,
            SpecialTileData specialTileData)
        {
            var affectedTiles = new List<Vector2Int>();

            if (specialTileData == null || !boardModel.IsPositionValid(position))
                return affectedTiles;

            if (specialTileData.powerRadius == 0)
            {
                for (int x = 0; x < boardModel.Width; x++)
                {
                    for (int y = 0; y < boardModel.Height; y++)
                    {
                        affectedTiles.Add(new Vector2Int(x, y));
                    }
                }
            }
            else
            {
                for (int dx = -specialTileData.powerRadius; dx <= specialTileData.powerRadius; dx++)
                {
                    for (int dy = -specialTileData.powerRadius; dy <= specialTileData.powerRadius; dy++)
                    {
                        var checkPos = new Vector2Int(position.x + dx, position.y + dy);
                        if (boardModel.IsPositionValid(checkPos))
                            affectedTiles.Add(checkPos);
                    }
                }
            }

            return affectedTiles;
        }

        public List<Vector2Int> GetCascadeAffectedTiles(IBoardModel boardModel, List<Vector2Int> initialPositions)
        {
            var result = new HashSet<Vector2Int>(initialPositions);
            var processed = new HashSet<Vector2Int>();
            var queue = new Queue<Vector2Int>(initialPositions);

            while (queue.Count > 0)
            {
                var pos = queue.Dequeue();

                if (processed.Contains(pos))
                    continue;

                processed.Add(pos);

                var tile = boardModel.GetTileAt(pos);
                if (tile?.IsSpecial != true || tile.SpecialTileData == null)
                    continue;

                var affected = GetAffectedTiles(boardModel, pos, tile.SpecialTileData);
                foreach (var affectedPos in affected)
                {
                    if (result.Add(affectedPos))
                    {
                        queue.Enqueue(affectedPos);
                    }
                }
            }

            return result.ToList();
        }

    }
}