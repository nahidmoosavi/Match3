using System.Collections.Generic;
using System.Linq;
using MVP.Model;
using UnityEngine;

namespace Core.Services
{
    public sealed partial class MatchFinderService : IMatchFinderService
    {
        private static readonly Vector2Int[] Directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        public List<Vector2Int> FindMatches(IBoardModel board, Vector2Int startPos)
        {
            if (!board.IsPositionValid(startPos))
                return new List<Vector2Int>();

            var startTile = board.GetTileAt(startPos);
            if (startTile?.TileData == null)
                return new List<Vector2Int>();

            var matchSet = BfsFloodFillMatches(board, startPos, startTile.TileData);
            return matchSet.Count >= 3 ? matchSet.ToList() : new List<Vector2Int>();
        }

        private static HashSet<Vector2Int> BfsFloodFillMatches(IBoardModel board, Vector2Int startPos,
            TileData targetTile)
        {
            var visited = new HashSet<Vector2Int>();
            var matchSet = new HashSet<Vector2Int>();
            var queue = new Queue<Vector2Int>();

            queue.Enqueue(startPos);
            visited.Add(startPos);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var currentTile = board.GetTileAt(current);

                if (currentTile == null || currentTile.TileData == null)
                    continue;

                if (currentTile.TileData.tileType != targetTile.tileType ||
                    currentTile.TileData.tileColor != targetTile.tileColor)
                    continue;

                matchSet.Add(current);

                foreach (var dir in Directions)
                {
                    var neighbor = current + dir;

                    if (visited.Contains(neighbor) || !board.IsPositionValid(neighbor)) continue;
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }

            return matchSet;
        }
    }
}
