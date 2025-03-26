
using MVP.Model;
using UnityEngine;

public sealed class GameModel: ModelBase, IGameModel
{
    private readonly ScoringRules _scoringRules;

    public int MovesRemaining { get; private set; }
    public int Score { get; private set; }
    private int TargetScore { get; }


    public GameModel(LevelData levelData, ScoringRules scoringRules)
    {
        Score = 0;
        _scoringRules = scoringRules;
        MovesRemaining = levelData.movesAllowed;
        TargetScore = levelData.targetScore;
    }
    
    public void AddScore(int points) => Score += points;

    public void UseMove()
    {
        if (MovesRemaining > 0)
            MovesRemaining--;
    }

    public bool IsOutOfMoves() => MovesRemaining <= 0;
    public bool IsLevelComplete() => Score >= TargetScore;
    
    public override void Reset()
    {
        Score = 0;
        MovesRemaining = 0; // or use initial value if stored
    }

}
