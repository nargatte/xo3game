using MathNet.Numerics.LinearAlgebra;

namespace OX3DGame.GraphicsEngine
{
    public class PhloxObject : MeshObject
    {
        public PhloxObject() : base(RenderManager.GeometryStore.Phlox,
            Vector<float>.Build.Dense(new[] { 0.93f, 0.2f, 0.0f }), 1000)
        {
            Transform.ScaleY = 2.5f;
            Transform.ScaleX = 1.5f;
            Transform.ScaleZ = 1.5f;
        }

        public override void Draw(Matrix<float> projection, Matrix<float> mvMatrix)
        {
            RenderManager.Shader.AmbientStrength = 1f;
            base.Draw(projection, mvMatrix);
            RenderManager.Shader.AmbientStrength = 0.2f;
        }
    }
}