namespace LogoFX.Client.Core.Specs.Objects
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