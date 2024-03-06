using System.Reactive.Disposables;
using ReactiveUI;

namespace ReactiveTodoList.Wpf.ViewModel
{
    public abstract class ViewModelBase : ReactiveObject, IDisposable
    {
        public abstract string Id { get; }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Subscriptions?.Dispose();
            }
        }

        /// <summary>
        /// Dispose all of your Rx subscriptions with this property. 
        /// </summary>
        protected readonly CompositeDisposable Subscriptions = new CompositeDisposable();

    }
}