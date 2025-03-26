using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public interface IBoardState
    {
        Task EnterAsync(BoardContext context, CancellationToken cancellationToken);
        void Exit(BoardContext context);
    }
}