using UnityEngine;
using TMPro;

namespace MVP.Views
{
    public class GameView : ViewBase<IGameView>, IGameView
    {
        [SerializeField] 
        private TMP_Text scoreText;
        [SerializeField] 
        private TMP_Text movesText;
        [SerializeField]
        private GameObject winPanel;
        [SerializeField] 
        private GameObject losePanel;

        public override void InitializeView()
        {
            winPanel.SetActive(false);
            losePanel.SetActive(false);
            UpdateScore(0);
            UpdateMoves(0);
        }
        
        public void UpdateScore(int score)
        {
            scoreText.text = $"Score: {score}";
        }

        public void UpdateMoves(int moves)
        {
            movesText.text = $"Moves: {moves}";
        }

        public void ShowWinScreen()
        {
            winPanel.SetActive(true);
        }

        public void ShowLoseScreen()
        {
            losePanel.SetActive(true);
        }
    }
}

