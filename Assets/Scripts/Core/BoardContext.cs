using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public abstract class BoardContext
    {
        public IBoardState CurrentState { get; private set; }

        public async Task SetStateAsync(IBoardState newState,  CancellationToken cancellationToken)
        {
            CurrentState?.Exit(this);
            CurrentState = newState;
            await CurrentState.EnterAsync(this,  cancellationToken);
        }
    }
}