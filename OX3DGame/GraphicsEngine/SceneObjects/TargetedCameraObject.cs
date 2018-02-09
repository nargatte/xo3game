using MathNet.Numerics.LinearAlgebra;

namespace OX3DGame.GraphicsEngine
{
    public class TargetedCameraObject : PointObject, ICamera
    {
        public PointObject Target { get; set; }

        public Matrix<float> GetViewMatrix()
        {
            return Math3D.Matrix3dHelper.LookAt(
                GetPosition(Matrix<float>.Build.DenseIdentity(4, 4)),
                Target.GetPosition(Matrix<float>.Build.DenseIdentity(4, 4)),
                Vector<float>.Build.Dense(new[] {0f, 1f, 0f}));
        }
    }
}