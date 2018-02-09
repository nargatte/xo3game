using OX3DGame.GraphicsEngine;

namespace OX3DGame.FluentValue
{
    public abstract class ClockValue : ValueBase
    {
        private long lastFrame = -1;
        private float LastValue = 0;

        protected abstract float PrepareValue();

        public override float GetValue()
        {
            if (lastFrame == RenderManager.FrameCount) return LastValue;
            lastFrame = RenderManager.FrameCount;
            LastValue = PrepareValue();
            return LastValue;
        }
    }
}