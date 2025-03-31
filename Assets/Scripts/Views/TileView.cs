using UnityEngine;
using System;
using System.Threading.Tasks;

namespace MVP.Views
{
    public class TileView : ViewBase<ITileView>, ITileView
    {
        [SerializeField] 
        private SpriteRenderer tileRenderer;

        public event Func<Task> OnTileClicked;
        public Transform Transform => transform;
        
        public override void InitializeView()
        {
            tileRenderer.sprite = null;
            tileRenderer.color = Color.white;
        }

        public void SetTileAppearance(Sprite sprite, Color color)
        {
            tileRenderer.sprite = sprite;
            tileRenderer.color = color;
        }

        private async void OnMouseDown()
        {
            try
            {
                if (OnTileClicked != null)
                    await OnTileClicked.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error handling tile click: {ex}");
            }
        }
    }
}