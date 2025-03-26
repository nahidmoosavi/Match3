using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Core.Services;
using MVP.Model;
using MVP.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MVP.Presenter
{
    public sealed class BoardPresenter : PresenterBase<IBoardView>, IBoardPresenter
    {
        private readonly BoardModel _boardModel;
        private readonly IMatchFinderService _matchFinderService;
        private readonly IBoardAnimatorService _boardAnimator;
        private readonly ITileSpawnerService _tileSpawner;
        private readonly ISpecialTileService _specialTileService;
        private readonly IBoardCollapseService _boardCollapseService;
        private readonly Dictionary<Vector2Int, ITilePresenter> _tilePresenters;
        private readonly TilePool _tilePool;
        private readonly BoardPresenterContext _boardContext;
        private readonly IEventAggregator _eventAggregator;
        private readonly LevelData _levelData;
        private readonly List<SpecialTileData> _specialTiles;
        private Vector2Int _lastClickedPosition;

        public BoardPresenter(
            BoardModel boardModel,
            IBoardView view,
            TilePool tilePool,
            IEventAggregator eventAggregator,
            LevelData levelData,
            List<SpecialTileData> specialTiles,
            IMatchFinderService matchFinderService,
            IBoardAnimatorService boardAnimator,
            ITileSpawnerService tileSpawner,
            ISpecialTileService specialTileService,
            IBoardCollapseService boardCollapseService
        ): base(view)
        {
            _boardModel = boardModel;
            _tilePool = tilePool;
            _eventAggregator = eventAggregator;
            _levelData = levelData;
            _specialTiles = specialTiles;
            _matchFinderService = matchFinderService;
            _boardAnimator = boardAnimator;
            _tileSpawner = tileSpawner;
            _specialTileService = specialTileService;
            _boardCollapseService = boardCollapseService;
            _tilePresenters = new Dictionary<Vector2Int, ITilePresenter>();
            _boardContext = new BoardPresenterContext(this);
        }

        private void RegisterTile(Vector2Int position, ITilePresenter tilePresenter)
        {
            _tilePresenters[position] = tilePresenter;
        }

        private async Task ExecuteTileRemovalAndCollapseAsync(List<Vector2Int> tilesToRemove, CancellationToken cancellationToken)
        {
            if (tilesToRemove.Count == 0) return;

            var removalAnimations = new List<Task>();

            foreach (var tilePos in tilesToRemove)
            {
                if (!_tilePresenters.TryGetValue(tilePos, out var presenter)) continue;
                var tileTransform = (presenter.TileView as MonoBehaviour)?.transform;
                if (tileTransform != null)
                {
                    removalAnimations.Add(_boardAnimator.AnimateTileRemoval(tileTransform, cancellationToken));
                }
            }

            await Task.WhenAll(removalAnimations);

            foreach (var tilePos in tilesToRemove)
                RemoveTile(tilePos);

            await CollapseTilesAsync(cancellationToken);

            _eventAggregator.Publish(tilesToRemove.Count);
        }

        private async Task CollapseTilesAsync(CancellationToken cancellationToken)
        {
            var movedTiles = _boardCollapseService.Collapse(_boardModel, _tilePresenters);

            var animationTasks = new List<Task>();

            foreach (var movement in movedTiles.MovedTiles)
            {
                if (!_tilePresenters.TryGetValue(movement.Value, out var presenter)) continue;
                var tileTransform = (presenter.TileView as MonoBehaviour)?.transform;

                if (tileTransform == null) continue;
                var targetPos = movement.Value.ToWorldPosition(_boardModel.Width, _boardModel.Height);
                animationTasks.Add(_boardAnimator.AnimateTileMovement(tileTransform, targetPos, cancellationToken));
            }

            await Task.WhenAll(animationTasks);
            await RefillBoardAsync(movedTiles.RefillPositions, cancellationToken);
        }

        private async Task RefillBoardAsync(List<Vector2Int> refillPositions, CancellationToken cancellationToken)
        {
            var animationTasks = new List<Task>();

            foreach (var position in refillPositions)
            {
                var tileData = GetRandomTile(_levelData.tileDistributions);

                var tilePresenter = _tileSpawner.SpawnTile(
                    position,
                    tileData,
                    View.Transform,
                    tileData.tileColor,
                    (pos) => HandleTileClicked(pos, CancellationToken.None)

                );

                RegisterTile(position, tilePresenter);

                var tileGo = ((TileView)tilePresenter.TileView).gameObject;
                tileGo.SetActive(true);

                var spawnPos =
                    position.ToWorldPosition(_boardModel.Width, _boardModel.Height, yOffset: _boardModel.Height);
                var targetPos = position.ToWorldPosition(_boardModel.Width, _boardModel.Height);
                tileGo.transform.localPosition = spawnPos;

                animationTasks.Add(_boardAnimator.AnimateTileMovement(tileGo.transform, targetPos, cancellationToken));
            }

            await Task.WhenAll(animationTasks);
            await _boardContext.SetStateAsync(new WaitingForInputState(), cancellationToken);
        }

        private SpecialTileData GetSpecialTileByMatchCount(int matchCount)
        {
            _specialTiles.Sort((a, b) => b.requiredMatchCount.CompareTo(a.requiredMatchCount));

            foreach (var specialTile in _specialTiles)
            {
                if (matchCount >= specialTile.requiredMatchCount)
                    return specialTile;
            }

            return null;
        }

        private void SpawnTileAtPosition(Vector2Int position, TileData tileData, Color color)
        {
            var tilePresenter = _tileSpawner.SpawnTile(
                position,
                tileData,
                View.Transform,
                color,
                (pos) => HandleTileClicked(pos, CancellationToken.None)
            );

            RegisterTile(position, tilePresenter);
        }

        private void SpawnTileAtPosition(Vector2Int position, SpecialTileData specialTileData)
        {
            var tilePresenter = _tileSpawner.SpawnSpecialTile(
                position,
                specialTileData,
                View.Transform,
                (pos) => HandleTileClicked(pos, CancellationToken.None)
            );

            RegisterTile(position, tilePresenter);
        }

        private async Task HandleTileClicked(Vector2Int position, CancellationToken cancellationToken)
        {
            if (!(_boardContext.CurrentState is WaitingForInputState)) return;
            try
            {
                _lastClickedPosition = position;
                await _boardContext.SetStateAsync(new ResolvingState(), cancellationToken);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error handling tile click at {position}: {e}");
            }
        }

        private void RemoveTile(Vector2Int position)
        {
            if (!_tilePresenters.TryGetValue(position, out var presenter)) return;
            var tileGo = ((TileView)presenter.TileView).gameObject;
            _tilePool.ReturnTile(tileGo);
            _tilePresenters.Remove(position);

            _boardModel.SetTileAt(position, null);
        }

        public async Task InitializeBoard(CancellationToken cancellationToken)
        {
            for (int x = 0; x < _levelData.boardWidth; x++)
            {
                for (int y = 0; y < _levelData.boardHeight; y++)
                {
                    TileData tileData;
                    List<Vector2Int> matches;

                    do
                    {
                        tileData = GetRandomTile(_levelData.tileDistributions);
                        var tempTileModel = new TileModel(tileData, new Vector2Int(x, y));
                        _boardModel.SetTileAt(new Vector2Int(x, y), tempTileModel);
                        matches = _matchFinderService.FindMatches(_boardModel, new Vector2Int(x, y));
                    } while (matches.Count > 3); // Avoid initial matches bigger than 3

                    SpawnTileAtPosition(new Vector2Int(x, y), tileData, tileData.tileColor);
                }
            }

            await _boardContext.SetStateAsync(new WaitingForInputState(), cancellationToken);
        }

        private static TileData GetRandomTile(TileDistribution[] distributions)
        {
            float total = 0;
            foreach (var dist in distributions)
                total += dist.spawnChance;

            var randomPoint = Random.value * total;
            foreach (var dist in distributions)
            {
                if (randomPoint < dist.spawnChance)
                    return dist.tile;
                randomPoint -= dist.spawnChance;
            }

            return distributions[0].tile;
        }

        public async Task ResolveMatchesAndCollapseAsync(CancellationToken cancellationToken)
        {
            var position = _lastClickedPosition;
            var tileModel = _boardModel.GetTileAt(position);

            if (tileModel == null)
            {
                await _boardContext.SetStateAsync(new WaitingForInputState(), cancellationToken);
                return;
            }

            // Handle special tiles immediately
            if (tileModel.IsSpecial && tileModel.SpecialTileData != null)
            {
                var specialAffected =
                    _specialTileService.GetAffectedTiles(_boardModel, position, tileModel.SpecialTileData);
                var cascadeAffected = _specialTileService.GetCascadeAffectedTiles(_boardModel, specialAffected);
                await ExecuteTileRemovalAndCollapseAsync(cascadeAffected, cancellationToken);
                return;
            }

            // Handle normal match
            var matchGroup = _matchFinderService.FindMatches(_boardModel, position);
            if (matchGroup.Count < 3)
            {
                await _boardContext.SetStateAsync(new WaitingForInputState(), cancellationToken);
                return;
            }

            // Check if we need to spawn a special tile at click position
            var specialTile = GetSpecialTileByMatchCount(matchGroup.Count);
            if (specialTile != null)
            {
                SpawnTileAtPosition(position, specialTile);
                matchGroup.Remove(position);
            }

            var allAffected = _specialTileService.GetCascadeAffectedTiles(_boardModel, matchGroup);
            await ExecuteTileRemovalAndCollapseAsync(allAffected, cancellationToken);
        }
    }
}


