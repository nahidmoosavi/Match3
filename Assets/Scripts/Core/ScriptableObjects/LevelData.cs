using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Match3/LevelData")]
public sealed class LevelData : ScriptableObject
{
    public int boardWidth;
    public int boardHeight;
    public int movesAllowed;
    public int targetScore;
    public TileDistribution[] tileDistributions;
}

[Serializable]
public class TileDistribution
{
    public TileData tile;
    public float spawnChance;
}
