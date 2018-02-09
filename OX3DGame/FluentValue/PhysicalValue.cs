using System;
using System.Diagnostics;
using OX3DGame.GraphicsEngine;

namespace OX3DGame.FluentValue
{
    public class PhysicalValue : ClockValue
    {
        private readonly ValueBase _force;
        private readonly float _maxMotion;
        private readonly float _suppression;
        private readonly float? _maxValue;
        private readonly float? _minValue;

        private float _current;
        private float _motion;

        public PhysicalValue(ValueBase force, float startValue, float maxMotion, float suppression, float? minValue = null, float? maxValue = null)
        {
            _force = force;
            _current = startValue;
            _maxMotion = maxMotion;
            _suppression = suppression;
            _maxValue = maxValue;
            _minValue = minValue;
            _motion = 0;
        }

        protected override float PrepareValue()
        {
            _motion += _force * (float)RenderManager.MsPerFrame/100;
            _motion = Math.Max(-_maxMotion, _motion);
            _motion = Math.Min(_maxMotion, _motion);

            _motion -= _motion * _suppression * (float) RenderManager.MsPerFrame/100;

            _current += _motion * (float) RenderManager.MsPerFrame/100;
            _current = Math.Max(_minValue??_current, _current);
            _current = Math.Min(_maxValue??_current, _current);

            return _current;
        }

        public static ValueBase GetDefaultWithKeys(Func<bool> keyDown, Func<bool> keyUp, float scale,
            float start = 0, float? minValue = null,
            float? maxValue = null) => 
            new BasicOperation2Arg(
            new PhysicalValue(new BasicOperation2Arg(
                new KeyProviderValue(0, -1.5f, keyDown),
                new KeyProviderValue(0, 1.5f, keyUp),
                (f1, f2) => f1 + f2), start, 15, 0.5f, minValue, maxValue),
            scale, (f1, f2) => f1 * f2);
    }
}