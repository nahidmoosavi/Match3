using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public sealed class WaitingForInputState : IBoardState
    {
            public Task EnterAsync(BoardContext context, CancellationToken cancellationToken)
            {
                Debug.Log("[State] Entered WaitingForInput");
                return Task.CompletedTask;
            }

            public void Exit(BoardContext context)
            {
                Debug.Log("[State] Exit WaitingForInput");
            }
    }
}