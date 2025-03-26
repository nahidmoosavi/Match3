using System;
using System.Threading.Tasks;
using UnityEngine;

namespace MVP.Presenter
{
    public sealed class TilePresenter : PresenterBase<ITileView>, ITilePresenter
    {
        public ITileView TileView => View;
        public TileModel TileModel => _tileModel;


        private readonly TileModel _tileModel;
        private readonly Func<Vector2Int, Task> _onTileClicked;

        public TilePresenter(
            ITileView view, 
            TileModel tileModel, 
            Func<Vector2Int, Task> onTileClicked): base(view)
        {
            _tileModel = tileModel;
            _onTileClicked = onTileClicked;

            View.OnTileClicked += HandleTileClicked;
        }

        private async Task HandleTileClicked()
        {
            if (_onTileClicked != null)
            {
                await _onTileClicked.Invoke(_tileModel.Position);
            }
        }

        public void UpdatePosition(Vector2Int newPosition)
        {
            _tileModel.Position = newPosition;
        }
        
        public override void Dispose()
        {
            View.OnTileClicked -= HandleTileClicked;
        }
    }
}
