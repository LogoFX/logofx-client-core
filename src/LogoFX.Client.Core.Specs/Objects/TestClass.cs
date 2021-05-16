using System;
using System.Reflection;
using System.Threading;

namespace LogoFX.Client.Core.Specs.Objects
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
            set => NotifyOfPropertyChange();
        }
    }

    public class TestRegularClass : TestClassBase
    {
        private int _number;
        public override int Number
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

    public class TestBeforeValueUpdateClass : TestClassBase
    {
        private int _number = 4;
        public override int Number
        {
            get => _number;
            set => SetProperty(ref _number, value, new SetPropertyOptions()
            {
                BeforeValueUpdate = () => PreviousValue = _number
            });
        }

        public int PreviousValue { get; private set; }
    }

    public class TestAfterValueUpdateClass : TestClassBase
    {
        private int _number = 4;
        public override int Number
        {
            get => _number;
            set => SetProperty(ref _number, value, new SetPropertyOptions()
            {
                AfterValueUpdate = () => _number = 6
            });
        }
    }

    public class TestOverriddenDispatcherClass : TestRegularClass
    {
        private readonly IDispatch _dispatch;

        public TestOverriddenDispatcherClass(IDispatch dispatch)
        {
            _dispatch = dispatch;
        }

        protected override IDispatch GetDispatch()
        {
            return _dispatch;
        }
    }

    public class TestOverridenNameClass : TestNameClass
    {
        private readonly IDispatch _dispatch;

        public TestOverridenNameClass(IDispatch dispatch)
        {
            _dispatch = dispatch;
        }

        protected override IDispatch GetDispatch()
        {
            return _dispatch;
        }
    }

    public class TestOverridenPropertyInfoClass : TestNameClass
    {
        private readonly IDispatch _dispatch;

        public TestOverridenPropertyInfoClass(IDispatch dispatch)
        {
            _dispatch = dispatch;
        }

        protected override IDispatch GetDispatch()
        {
            return _dispatch;
        }
    }

    public class TestOverriddenExpressionClass : TestNameClass
    {
        private readonly IDispatch _dispatch;

        public TestOverriddenExpressionClass(IDispatch dispatch)
        {
            _dispatch = dispatch;
        }

        protected override IDispatch GetDispatch()
        {
            return _dispatch;
        }
    }
}