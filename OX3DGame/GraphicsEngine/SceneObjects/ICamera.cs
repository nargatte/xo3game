using MathNet.Numerics.LinearAlgebra;

namespace OX3DGame.GraphicsEngine
{
    public interface ICamera
    {
        Matrix<float> GetViewMatrix();
    }
}