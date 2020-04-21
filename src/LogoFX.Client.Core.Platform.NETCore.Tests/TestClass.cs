using System;
using System.Threading;
using System.Windows.Threading;
using LogoFX.Client.Core.Tests;

namespace LogoFX.Client.Core.Platform.NETCore.Tests
{
    public class TestCustomActionInvocationClass : TestClassBase
    {
        private readonly TestPlatformDispatch _dispatch;

        public TestCustomActionInvocationClass(TestPlatformDispatch dispatch)
        {
            _dispatch = dispatch;
            _dispatch.InitializeDispatch();
        }

        private int _number;
        public override int Number
        {
            get => _number;
            set => SetProperty(ref _number, value, new SetPropertyOptions
            {
               CustomActionInvocation = action => _dispatch.OnUiThread(DispatcherPriority.DataBind, action)
            });
        }
    }

    public class TestPlatformDispatch : PlatformDispatch
    {
        private readonly PlatformDispatch _dispatch;

        public TestPlatformDispatch(PlatformDispatch dispatch)
        {
            _dispatch = dispatch;
        }

        internal bool IsCustomActionInvoked { get; private set; }

        internal new void OnUiThread(DispatcherPriority priority, Action action)
        {
            IsCustomActionInvoked = true;
            _dispatch.OnUiThread(priority, action);
        }

        internal new void InitializeDispatch()
        {
            _dispatch.InitializeDispatch();
        }
    }
}
