using System;
using System.Threading.Tasks;
using MVP.Model;
using MVP.Presenter;
using UnityEngine;

namespace Core.Services
{
    public sealed class TileSpawnerService : ITileSpawnerService
    {
        private readonly TilePool _tilePool;
        private readonly BoardModel _boardModel;

        public TileSpawnerService(TilePool tilePool, BoardModel boardModel)
        {
            _tilePool = tilePool;
            _boardModel = boardModel;
        }

        public ITilePresenter SpawnTile(Vector2Int position, TileData tileData, Transform parent, Color color,
            Func<Vector2Int, Task> onTileClicked)
        {
            var tileGo = _tilePool.GetTile();
            tileGo.transform.SetParent(parent, false);
            tileGo.transform.localPosition = position.ToWorldPosition((_boardModel.Width, _boardModel.Height));
            tileGo.name = $"Tile_{tileData.tileType}_{color}_{position.x},{position.y}";
            tileGo.SetActive(true);

            var tileView = tileGo.GetComponent<ITileView>();
            tileView.SetTileAppearance(tileData.tileSprite, color);

            var tileModel = new TileModel(tileData, position);
            _boardModel.SetTileAt(position, tileModel);

            return new TilePresenter(tileView, tileModel, onTileClicked);
        }

        public ITilePresenter SpawnSpecialTile(Vector2Int position, SpecialTileData specialTileData, Transform parent,
            Func<Vector2Int, Task> onTileClicked)
        {
            var tileGo = _tilePool.GetTile();
            tileGo.transform.SetParent(parent, false);
            tileGo.transform.localPosition = position.ToWorldPosition((_boardModel.Width, _boardModel.Height));
            tileGo.name = $"Tile_{specialTileData.name}_Special_{position.x},{position.y}";
            tileGo.SetActive(true);

            var tileView = tileGo.GetComponent<ITileView>();
            tileView.SetTileAppearance(specialTileData.tileSprite, Color.white);

            var tileModel = new TileModel(specialTileData, position);
            _boardModel.SetTileAt(position, tileModel);

            return new TilePresenter(tileView, tileModel, onTileClicked);
        }
    }
}