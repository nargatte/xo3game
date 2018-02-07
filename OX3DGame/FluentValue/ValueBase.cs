namespace OX3DGame.FluentValue
{
    public abstract class ValueBase
    {
        public abstract float GetValue();

        public static implicit operator float(ValueBase vb) => vb.GetValue();
        public static implicit operator ValueBase(float f) => new SteadyValue(f);
    }
}