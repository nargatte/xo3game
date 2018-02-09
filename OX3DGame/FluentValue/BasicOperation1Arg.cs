using System;

namespace OX3DGame.FluentValue
{
    public class BasicOperation1Arg : ValueBase
    {
        private ValueBase _valueBase;

        private Func<float, float> _operation;

        public BasicOperation1Arg(ValueBase valueBase, Func<float, float> operation)
        {
            _valueBase = valueBase;
            _operation = operation;
        }

        public override float GetValue() => _operation(_valueBase);
    }
}