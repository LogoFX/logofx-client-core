namespace LogoFX.Client.Core.Tests
{
    public class ValueWrapper
    {
        public ValueWrapper(object value)
        {
            Value = value;
        }

        public object Value { get; set; }
    }
}