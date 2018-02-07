using MathNet.Numerics.LinearAlgebra;

namespace OX3DGame.GraphicsEngine
{
    public class StandObject : MeshObject
    {
        public StandObject() : base(RenderManager.GeometryStore.Waltz,
            Vector<float>.Build.Dense(new[] {0.55f, 0.35f, 0f}), 10)
        {
            float radius = 15f;
            float height = 2f;
            Transform.ScaleX = radius;
            Transform.ScaleZ = radius;
            Transform.ScaleY = height;
        }
    }
}