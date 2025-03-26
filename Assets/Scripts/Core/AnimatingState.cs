using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public class AnimatingState : IBoardState
    {
        public async Task EnterAsync(BoardContext context,  CancellationToken cancellationToken)
        {
            Debug.Log("[State] Entered Animating");
            await Task.Delay(100, cancellationToken); // placeholder for animation sequence
        }

        public void Exit(BoardContext context)
        {
            Debug.Log("[State] Exit Animating");
        }
    }
}