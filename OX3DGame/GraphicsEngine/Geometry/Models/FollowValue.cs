using OX3DGame.FluentValue;

namespace OX3DGame.GraphicsEngine
{
    public class FollowValue : ClockValue
    {
        private float _currentPosition;
        private ValueBase _destination;
        private float _force;

        public FollowValue(float currentPosition, ValueBase destination, float force)
        {
            _currentPosition = currentPosition;
            _destination = destination;
            _force = force;
        }

        protected override float PrepareValue()
        {
            float vec = _destination - _currentPosition;
            vec = (float) (vec * _force * RenderManager.MsPerFrame / 100);
            _currentPosition += vec;
            return _currentPosition;
        }
    }
}