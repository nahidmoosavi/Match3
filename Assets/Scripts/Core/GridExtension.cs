using UnityEngine;

namespace Core
{
    public static class GridExtension
    {
        public static Vector3 ToWorldPosition(this Vector2Int gridPos, int boardWidth, int boardHeight, float yOffset = 0f)
        {
            return new Vector3(
                gridPos.x - (boardWidth / 2f) + 0.5f,
                gridPos.y - (boardHeight / 2f) + 0.5f + yOffset,
                0f
            );
        }

        public static Vector3 ToWorldPosition(this Vector2Int gridPos, (int Width, int Height) board)
        {
            return new Vector3(
                gridPos.x - (board.Width / 2f) + 0.5f,
                gridPos.y - (board.Height / 2f) + 0.5f,
                0f
            );
        }
    }
}