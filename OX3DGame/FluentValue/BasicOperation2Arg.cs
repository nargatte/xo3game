using System;

namespace OX3DGame.FluentValue
{
    public class BasicOperation2Arg : ValueBase
    {
        private ValueBase _valueBase1;
        private ValueBase _valueBase2;

        private Func<float, float, float> _operation;

        public BasicOperation2Arg(ValueBase valueBase1, ValueBase valueBase2, Func<float, float, float> operation)
        {
            _valueBase1 = valueBase1;
            _valueBase2 = valueBase2;
            _operation = operation;
        }

        public override float GetValue() => _operation(_valueBase1, _valueBase2);
    }
}