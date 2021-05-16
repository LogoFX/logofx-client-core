#nullable enable

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using FluentAssertions;
using LogoFX.Client.Core.Specs.Helpers;
using LogoFX.Client.Core.Specs.Objects;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Core.Specs.Steps
{
    [Binding]
    internal sealed class NotifyPropertyChangedSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public NotifyPropertyChangedSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"The dispatcher is set to test dispatcher")]
        public void GivenTheDispatcherIsSetToTestDispatcher()
        {
            var dispatch = new FakeDispatch();
            _scenarioContext.Add("dispatch", dispatch);
            Dispatch.Current = dispatch;
        }

        [Given(@"The dispatcher is set to overridden dispatcher")]
        public void GivenTheDispatcherIsSetToOverriddenDispatcher()
        {
            var dispatch = new OverriddenDispatch();
            if (_scenarioContext.ContainsKey("dispatch"))
            {
                _scenarioContext["dispatch"] = dispatch;
            }
            else
            {
                _scenarioContext.Add("dispatch", dispatch);
            }
        }

        [When(@"The '(.*)' is created")]
        public void WhenTheIsCreated(string name)
        {
            var @class = CreateTestClass(name);
            if (@class != null)
            {
                var isCalledRef = TestClassHelper.ListenToPropertyChange(@class, "Number");
                _scenarioContext.Add("class", @class);
                _scenarioContext.Add("isCalledRef", isCalledRef);
            }
        }

        [When(@"The '(.*)' is created with '(.*)' parameter")]
        public void WhenTheIsCreatedWithParameter(string name, string parameter)
        {
            var @class = CreateTestClass(name, _scenarioContext.Get<object>(parameter));
            if (@class != null)
            {
                var isCalledRef = TestClassHelper.ListenToPropertyChange(@class, "Number");
                _scenarioContext.Add("class", @class);
                _scenarioContext.Add("isCalledRef", isCalledRef);
            }
        }

        [When(@"The '(.*)' is created and empty notification is listened to")]
        public void WhenTheIsCreatedAndEmptyNotificationIsListenedTo(string name)
        {
            var @class = CreateTestClass(name);
            if (@class != null)
            {
                var isCalledRef = TestClassHelper.ListenToPropertyChange(@class, string.Empty);
                _scenarioContext.Add("class", @class);
                _scenarioContext.Add("isCalledRef", isCalledRef);
            }
        }

        [When(@"The '(.*)' is created and all notifications are listened to")]
        public void WhenTheIsCreatedAndAllNotificationsAreListenedTo(string name)
        {
            var @class = CreateTestClass(name);
            if (@class != null)
            {
                _scenarioContext.Add("class", @class);
                var isCallRefCollection = new List<ValueWrapper>();
                var isQuantityCalledRef = TestClassHelper.ListenToPropertyChange(@class, "Quantity");
                isCallRefCollection.Add(isQuantityCalledRef);
                var isTotalCalledRef = TestClassHelper.ListenToPropertyChange(@class, "Total");
                isCallRefCollection.Add(isTotalCalledRef);
                _scenarioContext.Add("isCalledRefCollection", isCallRefCollection);
            }
        }

        private INotifyPropertyChanged CreateTestClass(string name, params object?[]? args)
        {
            return TestClassHelper.CreateTestClassImpl(Assembly.GetExecutingAssembly(), name, args);
        }

        [When(@"The number is changed to (.*)  in regular mode")]
        public void WhenTheNumberIsChangedToInRegularMode(int value)
        {
            var @class = _scenarioContext.Get<TestClassBase>("class");
            @class.Number = value;
        }

        [When(@"The number is changed to (.*) in silent mode")]
        public void WhenTheNumberIsChangedToInSilentMode(int value)
        {
            var @class = _scenarioContext.Get<TestClassBase>("class");
            @class.UpdateSilent(() =>
            {
                @class.Number = value;
            });
        }

        [When(@"The number is changed to (.*) via SetProperty API")]
        public void WhenTheNumberIsChangedToViaSetPropertyAPI(int value)
        {
            var @class = _scenarioContext.Get<TestClassBase>("class");
            @class.Number = value;
        }

        [When(@"The quantity is changed to (.*) via SetProperty API")]
        public void WhenTheQuantityIsChangedToViaSetPropertyAPI(int value)
        {
            var @class = _scenarioContext.Get<TestMultipleClass>("class");
            @class.Quantity = value;
        }

        [When(@"The all properties change is invoked")]
        public void WhenTheAllPropertiesChangeIsInvoked()
        {
            var @class = _scenarioContext.Get<TestNameClass>("class");
            @class.Refresh();
        }

        [Then(@"The property change notification result is '(.*)'")]
        public void ThenThePropertyChangeNotificationResultIs(string expectedResultStr)
        {
            bool.TryParse(expectedResultStr, out var expectedResult);
            var isCalledRef = _scenarioContext.Get<ValueWrapper>("isCalledRef");
            isCalledRef.Value.Should().Be(expectedResult);
        }

        [Then(@"The property change notification result is '(.*)' for all notifications")]
        public void ThenThePropertyChangeNotificationResultIsForAllNotifications(string expectedResultStr)
        {
            bool.TryParse(expectedResultStr, out var expectedResult);
            var isCalledRefCollection = _scenarioContext.Get<IEnumerable<ValueWrapper>>("isCalledRefCollection");
            isCalledRefCollection.Select(t => t.Value).Should().AllBeEquivalentTo(expectedResult);
        }

        [Then(@"The before value update logic is invoked before the value update")]
        public void ThenTheBeforeValueUpdateLogicIsInvokedBeforeTheValueUpdate()
        {
            var @class = _scenarioContext.Get<TestBeforeValueUpdateClass>("class");
            @class.PreviousValue.Should().Be(4);
        }

        [Then(@"The after value update logic is invoked after the value update")]
        public void ThenTheAfterValueUpdateLogicIsInvokedAfterTheValueUpdate()
        {
            var @class = _scenarioContext.Get<TestAfterValueUpdateClass>("class");
            @class.Number.Should().Be(6);
        }

        [Then(@"The property change notification is raised via the test dispatcher")]
        public void ThenThePropertyChangeNotificationIsRaisedViaTheTestDispatcher()
        {
            var fakeDispatch = _scenarioContext.Get<FakeDispatch>("dispatch");
            fakeDispatch.IsOnUiThreadCalled.Should().BeTrue();
        }

        [Then(@"The property change notification is raised via the overridden dispatcher")]
        public void ThenThePropertyChangeNotificationIsRaisedViaTheOverriddenDispatcher()
        {
            var fakeDispatch = _scenarioContext.Get<FakeDispatch>("dispatch");
            fakeDispatch.IsOnUiThreadCalled.Should().BeTrue();
            fakeDispatch.Should().BeOfType<OverriddenDispatch>();
        }
    }
}
