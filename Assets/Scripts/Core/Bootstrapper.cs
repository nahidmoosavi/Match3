using System;
using System.Collections.Generic;
using System.Threading;
using Core;
using Core.Services;
using MVP.Model;
using MVP.Presenter;
using MVP.Views;
using UnityEngine;

namespace Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private GameView gameView;
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private LevelData levelData;
        [SerializeField] private ScoringRules scoringRules;
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private List<SpecialTileData> specialTiles;

        private async void Awake()
        {
            try
            {
                DontDestroyOnLoad(gameObject);

                var cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = cancellationTokenSource.Token;

                // Instantiate models
                var gameModel = new GameModel(levelData, scoringRules);
                var boardModel = new BoardModel(levelData.boardWidth, levelData.boardHeight);
                // Instantiate utilities
                var eventAggregator = new EventAggregator();
                var tilePool = new TilePool(tilePrefab, levelData.boardWidth * levelData.boardHeight);
                //Initialize Services
                var matchFinderService = new MatchFinderService();
                var boardAnimatorService = new BoardAnimatorService();
                var tileSpawnerService = new TileSpawnerService(tilePool, boardModel);
                var specialTileService = new SpecialTileService();
                var collapseTileService = new BoardCollapseService();
                // Instantiate GameView dynamically
                var gameViewInterface = gameView.GetView();
                // Instantiate BoardView dynamically
                var boardViewGo = new GameObject("BoardView");
                var boardView = boardViewGo.AddComponent<BoardView>();
                boardView.SetGameConfig(gameConfig);
                boardView.SetLevelData(levelData);
                var boardViewInterface = boardView.GetView();
                // Instantiate presenters
                IGamePresenter gamePresenter =
                    new GamePresenter(gameModel, gameViewInterface, eventAggregator, scoringRules);
                IBoardPresenter boardPresenter = new BoardPresenter(
                    boardModel,
                    boardViewInterface,
                    tilePool,
                    eventAggregator,
                    levelData,
                    specialTiles,
                    matchFinderService,
                    boardAnimatorService,
                    tileSpawnerService,
                    specialTileService,
                    collapseTileService
                );

                // Initialize the board
                await boardPresenter.InitializeBoard(cancellationToken);
            }

            catch (Exception e)
            {
                Debug.LogError($"[Bootstrapper] Exception during startup: {e.Message}");
                throw;
            }

        }
    }
}
