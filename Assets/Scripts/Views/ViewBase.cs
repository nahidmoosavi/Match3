namespace MVP.Views
{
    using UnityEngine;

    public abstract class ViewBase<TInterface> : MonoBehaviour where TInterface : class
    {
        /// <summary>
        /// Returns this view as its interface type.
        /// Used for dependency injection or registration.
        /// </summary>
        public TInterface GetView() => this as TInterface;

        /// <summary>
        /// Optional method to initialize the view (e.g., hide panels, reset counters).
        /// Should be called by presenter or bootstrapper.
        /// </summary>
        public virtual void InitializeView() { }

        /// <summary>
        /// Optional method for teardown or cleanup logic.
        /// Call when you want to disable or reset the view explicitly.
        /// </summary>
        public virtual void DisposeView() { }
    }
}