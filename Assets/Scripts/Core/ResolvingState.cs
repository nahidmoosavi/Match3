using System.Threading;
using System.Threading.Tasks;
using MVP.Presenter;
using UnityEngine;

namespace Core
{
    public sealed class ResolvingState : IBoardState
    {
        public async Task EnterAsync(BoardContext context, CancellationToken cancellationToken)
        {
            if (context is BoardPresenterContext ctx)
            {
                await ctx.Presenter.ResolveMatchesAndCollapseAsync(cancellationToken);
            }
        }

        public void Exit(BoardContext context)
        {
            Debug.Log("[State] Exit Resolving");
        }
    }
}