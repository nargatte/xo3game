namespace OX3DGame.FluentValue
{
    public class SteadyValue : ValueBase
    {
        private float _value;

        public SteadyValue(float value)
        {
            _value = value;
        }

        public override float GetValue() => _value;
    }
}