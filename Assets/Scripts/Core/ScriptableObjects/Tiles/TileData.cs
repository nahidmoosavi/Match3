using UnityEngine;

[CreateAssetMenu(fileName = "TileData", menuName = "Match3/TileData")]
public class TileData : ScriptableObject
{
    public string tileName;
    public TileType tileType;
    public Sprite tileSprite;
    public Color tileColor;
}

public enum TileType
{
    Normal,
    Bomb,
    Blast,
    Missile
}
