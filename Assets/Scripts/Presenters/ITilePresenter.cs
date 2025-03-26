

using UnityEngine;

public interface ITilePresenter
{
    ITileView TileView { get; }
    public TileModel TileModel { get; }
    void UpdatePosition(Vector2Int newPosition);
}