using UnityEngine;

[CreateAssetMenu(fileName = "ScoringRules", menuName = "Match3/ScoringRules")]
public sealed class ScoringRules: ScriptableObject
{
    public int pointsPerTile;
    public int bonusForCombos;
}