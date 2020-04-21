using System.Reflection;
using System.Threading;
using FluentAssertions;
using LogoFX.Client.Core.Tests;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Core.Platform.NETCore.Tests
{
    [Binding]
    internal sealed class InvocationSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public InvocationSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"The dispatcher is set to custom dispatcher")]
        public void GivenTheDispatcherIsSetToCustomDispatcher()
        {
            var dispatch = new TestPlatformDispatch(new PlatformDispatch());
            if (_scenarioContext.ContainsKey("dispatch"))
            {
                _scenarioContext["dispatch"] = dispatch;
            }
            else
            {
                _scenarioContext.Add("dispatch", dispatch);
            }
        }

        //TODO: Merge with the same class in LogoFX.Client.Core.Tests
        [When(@"The '(.*)' is created here with '(.*)' parameter")]
        public void WhenTheIsCreatedHereWithParameter(string name, string parameter)
        {
            var @class = TestClassHelper.CreateTestClassImpl(Assembly.GetExecutingAssembly(), name, _scenarioContext.Get<object>(parameter));
            if (@class != null)
            {
                var isCalledRef = TestClassHelper.ListenToPropertyChange(@class, "Number");
                _scenarioContext.Add("class", @class);
                _scenarioContext.Add("isCalledRef", isCalledRef);
            }
        }


        [Then(@"The property change notification is raised via the custom action invocation")]
        public void ThenThePropertyChangeNotificationIsRaisedViaTheCustomActionInvocation()
        {
            var fakeDispatch = _scenarioContext.Get<TestPlatformDispatch>("dispatch");
            fakeDispatch.IsCustomActionInvoked.Should().BeTrue();
        }

    }
}
