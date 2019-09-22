using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace LogoFX.Client.Core.Tests
{
    public abstract class TestClassBase : NotifyPropertyChangedBase<TestClassBase>
    {        
        public abstract int Number { get;
            set;
        }

        public void Refresh()
        {
            NotifyOfPropertiesChange();
        }

        public void UpdateSilent(Action action)
        {
            using (SuppressNotify)
            {
                action();
            }
        }
    }

    public class TestNameClass : TestClassBase
    {
        public override int Number
        {
            get { return 0; }
            set
            {
                NotifyOfPropertyChange("Number");
            }
        }
    }

    public class TestPropertyInfoClass : TestClassBase
    {
        private readonly PropertyInfo _propertyInfo;

        public TestPropertyInfoClass()
        {
            _propertyInfo = GetType().GetRuntimeProperty("Number");
        }

        public override int Number
        {
            get { return 0; }
            set
            {
                NotifyOfPropertyChange(_propertyInfo);
            }
        }
    }

    public class TestExpressionClass : TestClassBase
    {
        public override int Number
        {
            get { return 0; }
            set
            {
                NotifyOfPropertyChange(() => Number);
            }
        }
    }

    public class NotifyPropertyChangedTests
    {
        [Theory]
        [MemberData(nameof(NpcIsRaisedCases))]
        public void PropertyChanged_PropertyValueIsChanged_NotificationIsRaisedIsTrue(
            TestClassBase testClass, bool expectedIsCalled)
        {
            bool isCalled = false;
            testClass.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Number")
                {
                    isCalled = true;
                }
            };

            testClass.Number = 5;

            isCalled.Should().Be(expectedIsCalled);
        }

        [Theory]
        [MemberData(nameof(NpcIsNotRaisedCases))]
        public void PropertyChanged_PropertyValueIsChanged_NotificationIsRaised(
            TestClassBase testClass, bool expectedIsCalled)
        {
            bool isCalled = false;
            testClass.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Number")
                {
                    isCalled = true;
                }
            };

            testClass.UpdateSilent(() =>
            {
                testClass.Number = 5;
            });

            isCalled.Should().Be(expectedIsCalled);
        }

        [Fact]
        public void PropertyChanged_NotifyOfPropertiesChangeIsInvoked_EmptyNotificationIsRaised()
        {
            var testClass = new TestNameClass();
            bool isCalled = false;
            testClass.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "")
                {
                    isCalled = true;
                }
            };

            testClass.Refresh();

            isCalled.Should().BeTrue();
        }

        public static readonly IEnumerable<object[]> NpcIsRaisedCases =
            new List<object[]>
            {
                new object[] {new TestNameClass(), true},
                new object[] {new TestPropertyInfoClass(), true},
                new object[] {new TestExpressionClass(), true}
            };

        public static readonly IEnumerable<object[]> NpcIsNotRaisedCases =
            new List<object[]>
            {
                new object[] {new TestNameClass(), false},
                new object[] {new TestPropertyInfoClass(), false},
                new object[] {new TestExpressionClass(), false}
            };
    }
}
