using System;
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
            var types = Assembly.GetExecutingAssembly().DefinedTypes.ToArray();
            var type = types.FirstOrDefault(t => t.Name == name)?.AsType();
            if (type != null)
            {
                var @class = Activator.CreateInstance(type) as INotifyPropertyChanged;
                var isCalled = false;
                var isCalledRef = new WeakReference(isCalled);
                @class.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "Number")
                    {
                        isCalledRef.Target = true;
                    }
                };
                _scenarioContext.Add("class", @class);
                _scenarioContext.Add("isCalledRef", isCalledRef);
            }
        }

        [When(@"The number is changed to (.*)")]
        public void WhenTheNumberIsChangedTo(int value)
        {
            var @class = _scenarioContext.Get<TestClassBase>("class");
            @class.Number = value;
        }

        [Then(@"The property notification result is '(.*)'")]
        public void ThenThePropertyNotificationResultIs(string expectedResultStr)
        {
            bool.TryParse(expectedResultStr, out var expectedResult);
            var isCalledRef = _scenarioContext.Get<WeakReference>("isCalledRef");
            isCalledRef.Target.Should().Be(expectedResult);
        }
    }
}
