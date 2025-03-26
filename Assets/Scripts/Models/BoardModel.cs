using System.Collections.Generic;
using MVP.Model;
using UnityEngine;

namespace MVP.Model
{


    public sealed class BoardModel : ModelBase, IBoardModel
    {
        private readonly int _width;
        private readonly int _height;
        private readonly ITileModel[,] _tiles;

        public int Width => _width;
        public int Height => _height;
        public ITileModel[,] TilesGrid => _tiles;

        public BoardModel(int width, int height)
        {
            _width = width;
            _height = height;
            _tiles = new ITileModel[width, height];
        }

        public void SetTileAt(Vector2Int position, ITileModel tile)
        {
            _tiles[position.x, position.y] = tile;
        }

        public ITileModel GetTileAt(Vector2Int position)
        {
            if (position.x < 0 || position.y < 0 || position.x >= _width || position.y >= _height)
                return null;

            return _tiles[position.x, position.y];
        }

        public bool IsPositionValid(Vector2Int position)
        {
            return position.x >= 0 && position.x < _width &&
                   position.y >= 0 && position.y < _height;
        }
        
        public override void Reset()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    _tiles[x, y] = null;
                }
            }
        }
    }
}