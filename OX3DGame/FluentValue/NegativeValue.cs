namespace OX3DGame.FluentValue
{
    public class NegativeValue : ValueBase
    {
        private ValueBase _provider;

        public NegativeValue(ValueBase provider)
        {
            _provider = provider;
        }

        public override float GetValue() => -_provider;
    }
}