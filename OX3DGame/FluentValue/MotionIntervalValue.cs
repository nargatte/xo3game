using System;
using System.Diagnostics;
using OX3DGame.GraphicsEngine;

namespace OX3DGame.FluentValue
{
    public class MotionIntervalValue : ClockValue
    {
        private float _start;
        private float _stop;
        private float _time;
        private Action<MotionIntervalValue> Elipsed;

        private float _progress = 0;

        protected override float PrepareValue()
        {
            if (_progress >= 1)
            {
                return _stop;
            }
            _progress += (float)(RenderManager.MsPerFrame / _time);
            if (_progress >= 1)
            {
                Elipsed?.Invoke(this);
                return _stop;
            }
            return _start + _progress * (_stop - _start);
        }

        public void Restart() => _progress = 0;

        public MotionIntervalValue(float start, float stop, float time, Action<MotionIntervalValue> elipsed)
        {
            _start = start;
            _stop = stop;
            _time = time;
            Elipsed = elipsed;
        }
    }
}