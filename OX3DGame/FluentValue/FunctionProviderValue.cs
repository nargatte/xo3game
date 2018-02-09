using System;

namespace OX3DGame.FluentValue
{
    public class FunctionProviderValue : ValueBase
    {
        private Func<float> _func;

        public FunctionProviderValue(Func<float> func)
        {
            _func = func;
        }

        public override float GetValue() => _func();
    }
}