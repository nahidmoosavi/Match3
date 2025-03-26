using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Core.Services
{
    public sealed class BoardAnimatorService : IBoardAnimatorService
    {
        public async Task AnimateTileMovement(Transform tileTransform, Vector3 targetPosition, CancellationToken cancellationToken)
        {
            await tileTransform
                .DOLocalMove(targetPosition, 0.2f)
                .SetEase(Ease.OutQuad)
                .AsyncWaitForCompletion();
        }

        public async Task AnimateTileRemoval(Transform tileTransform, CancellationToken cancellationToken)
        {
            var scale = tileTransform.localScale;
            await tileTransform
                .DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InBack)
                .AsyncWaitForCompletion();
            tileTransform.localScale = scale;
        }
    }
}
