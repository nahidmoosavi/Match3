namespace MVP.Model
{
    public interface IGameModel
    {
        int Score { get; }
        int MovesRemaining { get; }
    
        void AddScore(int points);
        void UseMove();
    
        bool IsOutOfMoves();
        bool IsLevelComplete();
    }
}