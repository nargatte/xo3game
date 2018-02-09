using System;
using SharpGL.Enumerations;

namespace OX3DGame.FluentValue
{
    public class KeyProviderValue : ValueBase
    {
        private readonly ValueBase _upValue;
        private readonly ValueBase _downValue;

        private readonly Func<bool> _keyProvider;

        public KeyProviderValue(ValueBase upValue, ValueBase downValue, Func<bool> keyProvider)
        {
            _upValue = upValue;
            _downValue = downValue;
            _keyProvider = keyProvider;
        }

        public override float GetValue()
        {
            if (_keyProvider()) return _upValue;
            return _downValue;
        }
    }
}