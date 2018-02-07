using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;

namespace OX3DGame.GraphicsEngine
{
    public class CandleObject : CompositeObject
    {
        public CandleObject(float height, int number)
        {
            MeshObject candlestick = new MeshObject(RenderManager.GeometryStore.Candlestick,
                Vector<float>.Build.Dense(new[] { 1f, 1f, 0.3f }), 10);

            MeshObject wax = new MeshObject(RenderManager.GeometryStore.Waltz,
                Vector<float>.Build.Dense(new[] { 0.93f, 0.84f, 0.72f }), 10);
            float radius = 1.5f;
            wax.Transform.ScaleX = radius;
            wax.Transform.ScaleZ = radius;
            wax.Transform.ScaleY = height;
            wax.Transform.PositionY = 3f;

            PhloxObject phlox = new PhloxObject();
            phlox.Transform.PositionY = height + 3.1f;


            LightObject light = new LightObject(Vector<float>.Build.Dense(new[] { 0.93f, 0.0f, 0.0f }), number);
            light.Transform.PositionY = height + 3.5f;

            AddSceneObject(candlestick);
            AddSceneObject(wax);
            AddSceneObject(phlox);
            AddSceneObject(light);
        }
    }
}