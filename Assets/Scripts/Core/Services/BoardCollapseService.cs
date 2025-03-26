using System.Collections.Generic;
using MVP.Model;
using UnityEngine;

namespace Core.Services
{
    public sealed class CollapseResult
    {
        public Dictionary<Vector2Int, Vector2Int> MovedTiles { get; } = new();
        public List<Vector2Int> RefillPositions { get; } = new();
    }
    
    public sealed class BoardCollapseService : IBoardCollapseService
    {
        public CollapseResult Collapse(IBoardModel board,
            Dictionary<Vector2Int, ITilePresenter> presenters)
        {
            var result = new CollapseResult();

            for (int x = 0; x < board.Width; x++)
            {
                CollapseColumn(board, presenters, x, result);
            }

            return result;
        }

        private static void CollapseColumn(IBoardModel board, Dictionary<Vector2Int, ITilePresenter> presenters, int columnX,
            CollapseResult  result)
        {
            var nextY = 0;

            for (int currentY = 0; currentY < board.Height; currentY++)
            {
                var currentPos = new Vector2Int(columnX, currentY);
                var tile = board.GetTileAt(currentPos);

                if (tile == null)
                    continue;

                var targetPos = new Vector2Int(columnX, nextY);

                if (currentPos != targetPos)
                {
                    board.SetTileAt(targetPos, tile);
                    board.SetTileAt(currentPos, null);
                    tile.Position = targetPos;
                    if (presenters.Remove(currentPos, out var presenter))
                    {
                        presenters[targetPos] = presenter;
                        presenter.UpdatePosition(targetPos);
                    }

                    result.MovedTiles[currentPos] = targetPos;
                }

                nextY++;
            }
            for (int refillY = nextY; refillY < board.Height; refillY++)
            {
                result.RefillPositions.Add(new Vector2Int(columnX, refillY));
            }
        }
        
    }
}