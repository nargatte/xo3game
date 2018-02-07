using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;
using SharpGL.Enumerations;

namespace OX3DGame.GraphicsEngine
{
    public class PointObject
    {
        public PointObject Parent { get; set; }
        public Transform Transform { get; set; } = new Transform(0.0f, 0.0f, 0.0f);

        public virtual void Draw(Matrix<float> projection, Matrix<float> mvMatrix)
        {
        }

        public virtual int? PointOver(Matrix<float> mvpMatrix, float pointX, float pointY)
        {
            return null;
        }

        public virtual void SetLight(Matrix<float> mvMatrix)
        {
            
        }

        public Vector<float> GetPosition(Matrix<float> view)
        {
            Matrix<float> matrix = Transform.GetModelMatrix();
            PointObject p = Parent;
            while(p != null)
            {
                matrix = p.Transform.GetModelMatrix() * matrix;
            }

            Vector<float> pos = view * matrix * Vector<float>.Build.Dense(new []{0f, 0f, 0f, 1f});
            return Math3D.Matrix3dHelper.Scale(pos);
        }
    }
}