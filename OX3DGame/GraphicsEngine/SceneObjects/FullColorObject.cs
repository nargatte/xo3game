using MathNet.Numerics.LinearAlgebra;

namespace OX3DGame.GraphicsEngine
{
    public class FullColorObject : MeshObject
    {
        public FullColorObject(Mesh mesh, Vector<float> objectColor, float shininess) : base(mesh, objectColor, shininess)
        {
        }

        public override void Draw(Matrix<float> projection, Matrix<float> mvMatrix)
        {
            RenderManager.Shader.AmbientStrength = 1;
            base.Draw(projection, mvMatrix);
            RenderManager.Shader.AmbientStrength = RenderManager.AmbientStrength;
        }
    }
}