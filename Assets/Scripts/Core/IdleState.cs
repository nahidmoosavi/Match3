using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public interface IdleState : IBoardState
    {
        new Task EnterAsync(BoardContext context, CancellationToken cancellationToken)
        {
            Debug.Log("[State] Entered Idle");
            return Task.CompletedTask;
        }

        new void Exit(BoardContext context)
        {
            Debug.Log("[State] Exit Idle");
        }
    }
}