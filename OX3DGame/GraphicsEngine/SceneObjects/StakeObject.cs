using MathNet.Numerics.LinearAlgebra;

namespace OX3DGame.GraphicsEngine
{
    public class StakeObject : MeshObject
    {
        public StakeObject() : base(RenderManager.GeometryStore.Waltz,
            Vector<float>.Build.Dense(new[] { 0.55f, 0.35f, 0f }), 10)
        {
            float radius = 0.5f;
            float height = 8f;
            Transform.ScaleX = radius;
            Transform.ScaleZ = radius;
            Transform.ScaleY = height;
        }
    }
}