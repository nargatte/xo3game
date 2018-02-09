using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;

namespace OX3DGame.GraphicsEngine
{
    public class SpotLightObject : PointObject
    {
        public float Focus = 30;
        public Vector<float> LightColorDiff = Vector<float>.Build.Dense(new[] { 0.2f, 1f, 1f });
        public Vector<float> LightColorSpec = Vector<float>.Build.Dense(new[] { 0f, 0f, 1f });

        public override void SetLight(Matrix<float> mvMatrix)
        {
            Matrix<float> matrix = mvMatrix * Transform.GetModelMatrix();

            Vector<float> lightVector = Vector<float>.Build.Dense(new[] {0.0f, -1f, 0f, 0.0f});
            lightVector = Math3D.Matrix3dHelper.Scale(Math3D.Matrix3dHelper.NormalMatrix(matrix) * lightVector)
                .Normalize(2);
            if (lightVector[1] < 0)
                lightVector = -lightVector;

            Vector<float> lightPopsition =
                Math3D.Matrix3dHelper.Scale(matrix * Vector<float>.Build.Dense(new[] {0.0f, 0f, 0f, 1.0f}));

            RenderManager.Shader.SpotLightFocus = Focus;
            RenderManager.Shader.SpotLightColor = LightColorDiff;
            RenderManager.Shader.SpotLightColorSpec = LightColorSpec;
            RenderManager.Shader.SpotLightVector = lightVector;
            RenderManager.Shader.SpotLightPosition = lightPopsition;
        }
    }
}