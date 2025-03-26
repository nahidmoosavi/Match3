using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Services
{
    public interface IBoardAnimatorService
    {
        Task AnimateTileMovement(Transform tileTransform, Vector3 targetPosition, CancellationToken cancellationToken);
        Task AnimateTileRemoval(Transform tileTransform, CancellationToken cancellationToken);
    }
}
