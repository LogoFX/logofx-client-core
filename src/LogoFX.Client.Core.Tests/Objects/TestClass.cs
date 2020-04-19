using System;
using System.Reflection;

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
            get => 0;
            set => NotifyOfPropertyChange("Number");
        }
    }

    public class TestRegularClass : NotifyPropertyChangedBase<TestRegularClass>
    {
        private int _number;
        public int Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
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
            get => 0;
            set => NotifyOfPropertyChange(_propertyInfo);
        }
    }

    public class TestExpressionClass : TestClassBase
    {
        public override int Number
        {
            get => 0;
            set => NotifyOfPropertyChange(() => Number);
        }
    }

    public class TestMultipleClass : NotifyPropertyChangedBase<TestMultipleClass>
    {
        private double _cost;
        public double Cost
        {
            get => _cost;
            set => SetProperty(ref _cost, value);
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value, new SetPropertyOptions
            {
                AfterValueUpdate = () => NotifyOfPropertyChange(() => Total)
            });
        }

        public double Total => _cost * _quantity;
    }
}