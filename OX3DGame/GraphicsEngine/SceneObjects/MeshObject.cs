using MathNet.Numerics.LinearAlgebra;

namespace OX3DGame.GraphicsEngine
{
    public class MeshObject : PointObject
    {
        private readonly Mesh _mesh;

        public Vector<float> ObjectColor { get; set; }
        public float Shininess { get; set; }

        public MeshObject(Mesh mesh, Vector<float> objectColor, float shininess)
        {
            _mesh = mesh;
            ObjectColor = objectColor;
            Shininess = shininess;
        }

        public override void Draw(Matrix<float> projection, Matrix<float> mvMatrix)
        {
            RenderManager.Shader.ObjectColor = ObjectColor;
            RenderManager.Shader.Shininess = Shininess;

            Matrix<float> myMvMatrix = mvMatrix * Transform.GetModelMatrix();

            RenderManager.Shader.MvpMatrix = projection * myMvMatrix;
            RenderManager.Shader.MvMatrix = myMvMatrix;
            RenderManager.Shader.NormalMatrix = myMvMatrix.Inverse().Transpose();

            _mesh.Draw();
        }
    }
}