using MathNet.Numerics.LinearAlgebra;

namespace OX3DGame.GraphicsEngine
{
    public class LightObject : PointObject
    {
        public Vector<float> LightColorDiff { get; set; } = Vector<float>.Build.Dense(new[] { 1f, 1f, 1f });
        public Vector<float> LightColorSpec { get; set; } = Vector<float>.Build.Dense(new[] { 1f, 0f, 0f });
        private readonly int _number;

        public LightObject(int number)
        {
            _number = number;
        }

        public override void SetLight(Matrix<float> mvMatrix)
        {
            RenderManager.Shader.LightPositions[_number] = Math3D.Matrix3dHelper.Scale(mvMatrix * Transform.GetModelMatrix() * Vector<float>.Build.Dense(new []{0f, 0f, 0f, 1f}));
            RenderManager.Shader.LightColorsDiff[_number] = LightColorDiff;
            RenderManager.Shader.LightColorsSpec[_number] = LightColorSpec;
        }
    }
}