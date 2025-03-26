

using Core;

namespace MVP.Presenter
{
    public sealed class GamePresenter : PresenterBase<IGameView>, IGamePresenter
    {
        private readonly GameModel _gameModel;
        private readonly IEventAggregator _eventAggregator;
        private readonly ScoringRules _scoringRules;

        public GamePresenter(
            GameModel gameModel, 
            IGameView view, 
            IEventAggregator eventAggregator,
            ScoringRules scoringRules
            ): base(view)
        {
            _gameModel = gameModel;
            _eventAggregator = eventAggregator;
            _scoringRules = scoringRules;

            _eventAggregator.Subscribe<int>(OnTilesMatched);
        }

        private void OnTilesMatched(int matchedCount)
        {
            // 1. Update score
            int points = matchedCount * _scoringRules.pointsPerTile;
            _gameModel.AddScore(points);

            // 2. Use a move (you can move this to tile click instead if preferred)
            _gameModel.UseMove();

            // 3. Update the UI
            View.UpdateScore(_gameModel.Score);
            View.UpdateMoves(_gameModel.MovesRemaining);

            // 4. Check win/loss state
            if (_gameModel.IsLevelComplete())
                View.ShowWinScreen(); // or raise event: _eventAggregator.Publish(new GameWonEvent());
            
            else if (_gameModel.IsOutOfMoves())
            {
                View.ShowLoseScreen(); // or _eventAggregator.Publish(new GameLostEvent());
            }
        }
    }
}
