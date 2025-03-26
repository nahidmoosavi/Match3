using System.Collections.Generic;
using UnityEngine;

public sealed class TilePool
{
    private readonly Queue<GameObject> _pool = new();
    private readonly GameObject _tilePrefab;
    private readonly Transform _poolContainer;

    public TilePool(GameObject tilePrefab, int initialSize, Transform poolContainer = null)
    {
        _tilePrefab = tilePrefab;
        _poolContainer = poolContainer;

        for (int i = 0; i < initialSize; i++)
        {
            var tile = CreateTileInstance();
            _pool.Enqueue(tile);
        }
    }

    public GameObject GetTile()
    {
        var tile = _pool.Count > 0
            ? _pool.Dequeue()
            : CreateTileInstance();

        tile.SetActive(true);
        return tile;
    }

    public void ReturnTile(GameObject tile)
    {
        tile.transform.SetParent(_poolContainer, false);
        tile.SetActive(false);
        _pool.Enqueue(tile);
    }

    private GameObject CreateTileInstance()
    {
        var tile = Object.Instantiate(_tilePrefab);
        tile.name = $"{_tilePrefab.name}_Pooled";
        tile.SetActive(false);
        if (_poolContainer != null)
            tile.transform.SetParent(_poolContainer, false);
        
        return tile;
    }
}