using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using DG.Tweening;

namespace MVP.Views
{
    public class BoardView : ViewBase<IBoardView>, IBoardView
    {
        private const string BackgroundName = "BoardBackground";
        private GameConfig _gameConfig;
        private LevelData _levelData;
        private SpriteRenderer _backgroundRenderer;

        public Transform Transform => transform;
        
        public override void InitializeView()
        {
            InitializeBackground();
        }

        private void InitializeBackground()
        {
            if (_backgroundRenderer != null) return;
            var backgroundGo = new GameObject(BackgroundName);
            backgroundGo.transform.SetParent(this.transform, false);

            _backgroundRenderer = backgroundGo.AddComponent<SpriteRenderer>();
            _backgroundRenderer.sprite = _gameConfig.background;
            _backgroundRenderer.sortingOrder = -1;

            AdjustBackgroundSize();
        }

        private void AdjustBackgroundSize()
        {
            if (_backgroundRenderer.sprite == null|| Camera.main == null) return;
            
            var screenHeight = Camera.main.orthographicSize * 2f;
            var screenWidth = screenHeight * Screen.width / Screen.height;

            Vector2 spriteSize = _backgroundRenderer.sprite.bounds.size;
            _backgroundRenderer.transform.localScale = new Vector3(
                screenWidth / spriteSize.x,
                screenHeight / spriteSize.y,
                1);
        }

        public void SetGameConfig(GameConfig config)
        {
            _gameConfig = config;
        }

        public void SetLevelData(LevelData levelData)
        {
            _levelData = levelData;
        }
        
        public override void DisposeView()
        {
            if (_backgroundRenderer == null) return;
            Destroy(_backgroundRenderer.gameObject);
            _backgroundRenderer = null;
        }
    }
}

