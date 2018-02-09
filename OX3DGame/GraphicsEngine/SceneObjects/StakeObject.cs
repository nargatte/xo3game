using MathNet.Numerics.LinearAlgebra;

namespace OX3DGame.GraphicsEngine
{
    public class StakeObject : MeshObject
    {
        public StakeObject() : base(RenderManager.GeometryStore.WaltzPoor,
            Vector<float>.Build.Dense(new[] { 0.55f, 0.35f, 0f }), 20)
        {
            float radius = 0.5f;
            float height = 4f;
            Transform.ScaleX = radius;
            Transform.ScaleZ = radius;
            Transform.ScaleY = height;
        }
    }
}