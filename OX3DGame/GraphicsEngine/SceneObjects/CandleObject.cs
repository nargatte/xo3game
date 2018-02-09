using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;
using OX3DGame.FluentValue;

namespace OX3DGame.GraphicsEngine
{
    public class CandleObject : CompositeObject
    {
        private readonly LightObject _light;
        private readonly CompositeObject _phloxScaler;
        private int _glimmerCount = 0;
        private MotionIntervalValue _glimmerInterval;
        private MotionIntervalValue _nextGlimerringTimer;

        private int _number;

        public override void SetLight(Matrix<float> mvMatrix)
        {
            _nextGlimerringTimer.GetValue();
            if (_glimmerInterval != null)
            {
                float f = _glimmerInterval.GetValue();

                _light.LightColorDiff = Vector<float>.Build.Dense(new[] {1 - f/2, f, f});
                _phloxScaler.Transform.ScaleY = f*2 + 1;
                _phloxScaler.Transform.ScaleX = f / 2 + 0.5f;
                _phloxScaler.Transform.ScaleZ = f / 2 + 0.5f;
            }
            else
            {
                _light.LightColorDiff = Vector<float>.Build.Dense(new[] { 1f, 1f, 1f });
                _phloxScaler.Transform.ScaleY = 2;
                _phloxScaler.Transform.ScaleX = 1;
                _phloxScaler.Transform.ScaleZ = 1;
            }

            base.SetLight(mvMatrix);
        }

        public CandleObject(float height, int number)
        {
            _number = number;
            MeshObject candlestick = new MeshObject(RenderManager.GeometryStore.Candlestick,
                Vector<float>.Build.Dense(new[] { 1f, 1f, 0.3f }), 10000);
            candlestick.Transform.ScaleY = 0.5f;

            MeshObject wax = new MeshObject(RenderManager.GeometryStore.WaltzPoor,
                Vector<float>.Build.Dense(new[] { 0.93f, 0.84f, 0.72f }), 250);
            float radius = 1f;
            wax.Transform.ScaleX = radius;
            wax.Transform.ScaleZ = radius;
            wax.Transform.ScaleY = height;
            wax.Transform.PositionY = 1.5f;

            _phloxScaler = new CompositeObject();

            PhloxObject phlox = new PhloxObject();
            _phloxScaler.Transform.PositionY = height + 1.6f;
            phlox.Transform.ScaleX = 0.8f;
            phlox.Transform.ScaleY = 0.7f;
            phlox.Transform.ScaleZ = 0.8f;

            _phloxScaler.AddSceneObject(phlox);


            _light = new LightObject(number);
            _light.Transform.PositionY = height + 2f;

            AddSceneObject(candlestick);
            AddSceneObject(wax);
            AddSceneObject(_phloxScaler);
            AddSceneObject(_light);

            Transform.ScaleX = 0.3f;
            Transform.ScaleZ = 0.3f;
            SetGimerringTimer();
        }

        private void SetGimerringTimer()
        {
            Random random = new Random(_number);
            _nextGlimerringTimer = new MotionIntervalValue(0,1,(float) (random.NextDouble()*25000+5000), (t) => Wink());
            _glimmerCount = random.Next(0, 3);
        }

        private void Wink()
        {
            if (_glimmerCount == 0)
            {
                _glimmerInterval = null;
                SetGimerringTimer();
                return;
            }
            _glimmerCount--;
            _glimmerInterval = new MotionIntervalValue(0, 1, 250, (t) => Wink());
        }
    }
}