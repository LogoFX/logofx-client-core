using System.Threading;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Core.Specs.Helpers
{
    [Binding]
    internal sealed class LifecycleHook
    {
        [AfterScenario]
        public void AfterScenario()
        {
            Dispatch.Current = null;
        }
    }
}
