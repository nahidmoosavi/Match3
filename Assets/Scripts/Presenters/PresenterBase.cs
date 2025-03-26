using System.Threading;
using System.Threading.Tasks;

namespace MVP.Presenter
{
    public abstract class PresenterBase<TViewInterface>
        where TViewInterface : class
    {
        protected readonly TViewInterface View;

        protected PresenterBase(TViewInterface view)
        {
            View = view;
        }

        /// <summary>
        /// Optional async initialization logic for the presenter.
        /// </summary>
        public virtual Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Optional cleanup logic.
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}