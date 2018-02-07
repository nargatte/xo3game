using MathNet.Numerics.LinearAlgebra;

namespace OX3DGame.GraphicsEngine
{
    public class TagObject : MeshObject
    {
        public TagObject(bool black) : base(RenderManager.GeometryStore.Torus, black?Vector<float>.Build.Dense(new []{0.3f, 0.3f, 0.3f}): Vector<float>.Build.Dense(new[] { 0.7f, 0.7f, 0.7f }), 10f)
        {
        }
    }
}