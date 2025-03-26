using UnityEngine;

namespace MVP.Model
{
    public interface IBoardModel
    {
        int Width { get; }
        int Height { get; }
        ITileModel[,] TilesGrid { get; }

        void SetTileAt(Vector2Int position, ITileModel tile);
        ITileModel GetTileAt(Vector2Int position);
        bool IsPositionValid(Vector2Int position);
    }
}