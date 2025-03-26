using UnityEngine;

[CreateAssetMenu(fileName = "SpecialTileData", menuName = "Match3/SpecialTileData")]
public class SpecialTileData : ScriptableObject
{
    public string tileName;

    [Tooltip("The minimum number of matched tiles required to spawn this special tile")]
    public int requiredMatchCount;
    public Sprite tileSprite;
    public int powerRadius;
}