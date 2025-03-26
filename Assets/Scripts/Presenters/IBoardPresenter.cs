using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface IBoardPresenter
{
    Task InitializeBoard(CancellationToken cancellationToken);
    Task ResolveMatchesAndCollapseAsync(CancellationToken cancellationToken);
}