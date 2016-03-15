#if WINDOWS_UWP || NETFX_CORE
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
#endif
using LogoFX.Client.Core;

// ReSharper disable once CheckNamespace
namespace System.Windows.Threading
{
    /// <summary>
    /// Represents UI-thread dispatcher
    /// </summary>
    public interface IDispatch
    {
        /// <summary>
        /// Begins the action on the UI thread
        /// </summary>
        /// <param name="action">Action</param>
        void BeginOnUiThread(Action action);

        /// <summary>
        /// Begins the action on the UI thread according to the specified priority
        /// </summary>
        /// <param name="prio">Desired priority</param>
        /// <param name="action">Action</param>
        void BeginOnUiThread(
#if NET45
            DispatcherPriority
#endif
#if NETFX_CORE || WINDOWS_UWP
            CoreDispatcherPriority
#endif

            prio, Action action);

        /// <summary>
        /// Executes the action on the UI thread
        /// </summary>
        /// <param name="action">Action</param>
        void OnUiThread(Action action);
        /// <summary>
        /// Executes the action on the UI thread according to the specified priority
        /// </summary>
        /// <param name="priority">Desired priority</param>
        /// <param name="action">Action</param>
        void OnUiThread(
#if NET45
            DispatcherPriority
#endif
#if NETFX_CORE || WINDOWS_UWP
            CoreDispatcherPriority
#endif
            priority, Action action);

        /// <summary>
        /// Initializes the dispatcher
        /// </summary>
        void InitializeDispatch();
    }

    /// <summary>
    /// Default UI-thread dispatcher
    /// </summary>
    public class DefaultDispatch : IDispatch
    {
        private Action<Action, bool,
#if NET45
            DispatcherPriority
#endif
#if NETFX_CORE || WINDOWS_UWP
            CoreDispatcherPriority
#endif
            > _dispatch;        

        private void EnsureDispatch()
        {
            if (_dispatch == null)
            {
                throw new InvalidOperationException("Dispatch is not initialized correctly");
            }
        }

        /// <summary>
        /// Initializes the framework using the current dispatcher.
        /// </summary>
        public void InitializeDispatch()
        {
#if NET45
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
            if (dispatcher == null)
                throw new InvalidOperationException("Dispatch is not initialized correctly");
#endif
#if NETFX_CORE || WINDOWS_UWP
            CoreDispatcher dispatcher = new UserControl().Dispatcher;            
#endif
            //#if !WinRT
            //            Dispatcher dispatcher = null;
            //#else
            //            CoreDispatcher dispatcher = null;
            //#endif

            //#if SILVERLIGHT
            //            dispatcher = Deployment.Current.Dispatcher;
            //#elif WinRT
            //            dispatcher = new UserControl().Dispatcher;
            //#else
            //            dispatcher = Dispatcher.CurrentDispatcher;
            //            //if (Application.Current != null && Application.Current.Dispatcher != null)
            //            //    dispatcher = Application.Current.Dispatcher;
            //#endif            


            _dispatch = (action, @async, prio) =>
            {
#if NET45
                if (!@async && dispatcher.CheckAccess())
#else
                if (!@async)
#endif
                {
                    action();
                }               
                else
                {
#if NET45
                    dispatcher.BeginInvoke(action, prio);
#endif
#if NETFX_CORE || WINDOWS_UWP
                    dispatcher.RunAsync(prio, () => action());
#endif
                                       
//#if WinRT
//#pragma warning disable 4014
//                    dispatcher.RunAsync(prio,()=>action());
//#pragma warning restore 4014
//#elif SILVERLIGHT
//                    dispatcher.BeginInvoke(action);
//#else
//                    dispatcher.BeginInvoke(action, prio);
//#endif
                }
            };
        }

        /// <summary>
        /// Begins the action on the UI thread
        /// </summary>
        /// <param name="action">Action</param>
        public void BeginOnUiThread(Action action)
        {
            BeginOnUiThread(Consts.DispatcherPriority, action);

            //#if SILVERLIGHT
            //            EnsureDispatch();
            //            _dispatch(action, true);            

            //#elif WinRT
            //            EnsureDispatch();
            //            _dispatch(action, true,CoreDispatcherPriority.Normal);
            //#else
            //            BeginOnUiThread(Consts.DispatcherPriority, action);
            //#endif
        }

        /// <summary>
        /// Begins the action on the UI thread according to the specified priority
        /// </summary>
        /// <param name="prio">Desired priority</param>
        /// <param name="action">Action</param>
        public void BeginOnUiThread(
#if NET45
            DispatcherPriority
#endif
#if NETFX_CORE || WINDOWS_UWP
            CoreDispatcherPriority
#endif
            prio, Action action)
        {
            EnsureDispatch();
            _dispatch(action, true, prio);
        }

        /// <summary>
        /// Executes the action on the UI thread
        /// </summary>
        /// <param name="action">Action</param>
        public void OnUiThread(Action action)
        {
            OnUiThread(Consts.DispatcherPriority, action);
        }

        /// <summary>
        /// Executes the action on the UI thread according to the specified priority
        /// </summary>
        /// <param name="priority">Desired priority</param>
        /// <param name="action">Action</param>
        public void OnUiThread(
#if NET45
            DispatcherPriority
#endif
#if NETFX_CORE || WINDOWS_UWP
            CoreDispatcherPriority
#endif 
            priority, Action action)
        {
            EnsureDispatch();
            _dispatch(action, false, priority);
        }
    }
}
