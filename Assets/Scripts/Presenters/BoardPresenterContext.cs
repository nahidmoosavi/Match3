
using Core;

namespace MVP.Presenter
{
    public class BoardPresenterContext : BoardContext
    {
        public IBoardPresenter Presenter { get; }

        public BoardPresenterContext(IBoardPresenter presenter)
        {
            Presenter = presenter;
        }
    }
}