using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Core.Tests
{
    [Binding]
    internal sealed class NotifyPropertyChangedSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public NotifyPropertyChangedSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [When(@"The '(.*)' is created")]
        public void WhenTheIsCreated(string name)
        {
            var @class = CreateTestClass(name);
            if (@class != null)
            {
                var isCalledRef = ListenToPropertyChange(@class, "Number");
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
                var isCalledRef = ListenToPropertyChange(@class, string.Empty);
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
                var isCallRefCollection = new List<WeakReference>();
                var isQuantityCalledRef = ListenToPropertyChange(@class, "Quantity");
                isCallRefCollection.Add(isQuantityCalledRef);
                var isTotalCalledRef = ListenToPropertyChange(@class, "Total");
                isCallRefCollection.Add(isTotalCalledRef);
                _scenarioContext.Add("isCalledRefCollection", isCallRefCollection);
            }
        }

        private INotifyPropertyChanged CreateTestClass(string name)
        {
            var types = Assembly.GetExecutingAssembly().DefinedTypes.ToArray();
            var type = types.FirstOrDefault(t => t.Name == name)?.AsType();
            return type == null ? null : Activator.CreateInstance(type) as INotifyPropertyChanged;
        }

        private WeakReference ListenToPropertyChange(INotifyPropertyChanged @class, string propertyName)
        {
            var isCalled = false;
            var isCalledRef = new WeakReference(isCalled);
            @class.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == propertyName)
                {
                    isCalledRef.Target = true;
                }
            };
            return isCalledRef;
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
            var @class = _scenarioContext.Get<TestRegularClass>("class");
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
            var isCalledRef = _scenarioContext.Get<WeakReference>("isCalledRef");
            isCalledRef.Target.Should().Be(expectedResult);
        }

        [Then(@"The property change notification result is '(.*)' for all notifications")]
        public void ThenThePropertyChangeNotificationResultIsForAllNotifications(string expectedResultStr)
        {
            bool.TryParse(expectedResultStr, out var expectedResult);
            var isCalledRefCollection = _scenarioContext.Get<IEnumerable<WeakReference>>("isCalledRefCollection");
            isCalledRefCollection.Select(t => t.Target).Should().AllBeEquivalentTo(expectedResult);
        }
    }
}
