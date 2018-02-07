using MathNet.Numerics.LinearAlgebra;

namespace OX3DGame.GraphicsEngine
{
    public class LightObject : PointObject
    {
        public Vector<float> LightColor { get; set; }
        private int _number;

        public LightObject(Vector<float> lightColor, int number)
        {
            LightColor = lightColor;
            _number = number;
        }

        public override void SetLight(Matrix<float> mvMatrix)
        {
            RenderManager.Shader.LightPosition = Math3D.Matrix3dHelper.Scale(mvMatrix * Transform.GetModelMatrix() * Vector<float>.Build.Dense(new []{0f, 0f, 0f, 1f}));
        }
    }
}