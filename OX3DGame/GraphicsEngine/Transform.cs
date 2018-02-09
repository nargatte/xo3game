using MathNet.Numerics.LinearAlgebra;
using OX3DGame.FluentValue;

namespace OX3DGame.GraphicsEngine
{
    public class Transform
    {
        public ValueBase PositionX { get; set; } 
        public ValueBase PositionY { get; set; } 
        public ValueBase PositionZ { get; set; }
        
        public ValueBase RotationX { get; set; } 
        public ValueBase RotationY { get; set; } 
        public ValueBase RotationZ { get; set; } 

        public ValueBase ScaleX { get; set; } 
        public ValueBase ScaleY { get; set; } 
        public ValueBase ScaleZ { get; set; }

        public Transform(float positionX, float positionY, float positionZ)
        {
            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;

            RotationX = 0f;
            RotationY = 0f;
            RotationZ = 0f;

            ScaleX = 1f;
            ScaleY = 1f;
            ScaleZ = 1f;
        }

        public Matrix<float> GetModelMatrix()
        {
            return Math3D.Matrix3dHelper.Transform(PositionX, PositionY, PositionZ) *
                   Math3D.Matrix3dHelper.RotateY(RotationY) *
                   Math3D.Matrix3dHelper.RotateX(RotationX) *
                   Math3D.Matrix3dHelper.RotateZ(RotationZ) *
                   Math3D.Matrix3dHelper.Scale(ScaleX, ScaleY, ScaleZ);
        }

        public Matrix<float> GetViewMatrix(Transform lookAt)
        {
            return Math3D.Matrix3dHelper.LookAt(Vector<float>.Build.Dense(new[] {PositionX.GetValue(), PositionY.GetValue(), PositionZ.GetValue()}),
                Vector<float>.Build.Dense(new[] {lookAt.PositionX.GetValue(), lookAt.PositionY.GetValue(), lookAt.PositionZ.GetValue()}),
                Vector<float>.Build.Dense(new[] {0f, 1f, 0f}));
        }
    }
}