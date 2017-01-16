#if NET45
using System.Windows.Threading;
#endif

namespace LogoFX.Client.Core
{
    /// <summary>
    /// Dispatcher-related constants.
    /// </summary>
    public static class Consts
    {
        /// <summary>
        /// The dispatcher priority
        /// </summary>
        public const
#if NET45
            DispatcherPriority
#endif
#if NETFX_CORE || WINDOWS_UWP
            Windows.UI.Core.CoreDispatcherPriority
#endif
            DispatcherPriority =
#if NET45
            System.Windows.Threading.DispatcherPriority.DataBind
#endif
#if NETFX_CORE || WINDOWS_UWP
            Windows.UI.Core.CoreDispatcherPriority.Normal
#endif
        ;
    }
}
