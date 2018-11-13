using System.Threading.Tasks;
// ReSharper disable once CheckNamespace
namespace System.Windows.Threading
{    
    /// <summary>
    /// Default UI-thread dispatcher
    /// </summary>
    public class DefaultDispatch : IDispatch
    {
        /// <summary>
        /// Begins the action on the UI thread
        /// </summary>
        /// <param name="action">Action</param>
        public void BeginOnUiThread(Action action)
        {
            Task.Run(action);
        }

        /// <summary>
        /// Executes the action on the UI thread
        /// </summary>
        /// <param name="action">Action</param>
        public void OnUiThread(Action action)
        {
            action();
        }

        /// <summary>
        /// Initializes the dispatcher
        /// </summary>
        public void InitializeDispatch()
        {
            
        }
    }
}
