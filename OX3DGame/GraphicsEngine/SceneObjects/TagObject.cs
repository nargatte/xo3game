using System;
using MathNet.Numerics.LinearAlgebra;
using OX3DGame.FluentValue;

namespace OX3DGame.GraphicsEngine
{
    public class TagObject : MeshObject
    {
        private static Vector<float> whiteColor = Vector<float>.Build.Dense(new[] {1f, 1f, 1f});
        private static Vector<float> blackColor = Vector<float>.Build.Dense(new[] {0.2f, 0.2f, 0.2f});
        private static Vector<float> yelowColor = Vector<float>.Build.Dense(new[] {1f, 1f, 0f});

        private bool _isBlack;

        public TagObject(bool black) : base(RenderManager.GeometryStore.Torus, black?blackColor:whiteColor, 50f)
        {
            _isBlack = black;
        }

        public void MakeMeSpin(bool allTheTime, float time)
        {
            Action<MotionIntervalValue> action = null;
            if (allTheTime) action = t => t.Restart(); 

            Random random = new Random();
            Transform.RotationX = new MotionIntervalValue(0, (float) (2 * Math.PI) * random.Next(1, 5), time, action);
            Transform.RotationY = new MotionIntervalValue(0, (float) (2 * Math.PI) * random.Next(1, 5), time, action);
            Transform.RotationZ = new MotionIntervalValue(0, (float) (2 * Math.PI) * random.Next(1, 5), time, action);
        }

        public void StopSpin()
        {
            Transform.RotationX = 0;
            Transform.RotationY = 0;
            Transform.RotationZ = 0;
        }

        private bool _lighted;
        private MotionIntervalValue blinkTimer = null;

        public void Blink()
        {
            blinkTimer = new MotionIntervalValue(0, 1, 1000, (t) => this.Blink());
            if (_lighted)
            {
                base.ObjectColor = yelowColor;
                _lighted = !_lighted;
            }
            else
            {
                base.ObjectColor = _isBlack ? blackColor : whiteColor;
                _lighted = !_lighted;
            }

        }

        public override void Draw(Matrix<float> projection, Matrix<float> mvMatrix)
        {
            blinkTimer?.GetValue();
            base.Draw(projection, mvMatrix);
        }
    }
}