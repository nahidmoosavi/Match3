using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Match3/GameConfig")]
public sealed class GameConfig : ScriptableObject
{
    public float tileDropSpeed;
    public float tileRemovalAnimationTime;
    public Sprite background;
}
