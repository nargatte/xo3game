using MathNet.Numerics.LinearAlgebra;

namespace OX3DGame.GraphicsEngine
{
    public class SquareButtonObject : PointObject
    {
        private int _id;

        public SquareButtonObject(int id)
        {
            _id = id;
        }

        public override int? PointOver(Matrix<float> mvpMatrix, float pointX, float pointY)
        {
            Matrix<float> matrix = mvpMatrix * Transform.GetModelMatrix();

            Vector<float> point = Vector<float>.Build.Dense(new[] {pointX, pointY});

            Vector<float> v1 = matrix * Vector<float>.Build.Dense(new[] {-0.5f, 0.0f, -0.5f, 1.0f });
            Vector<float> v2 = matrix * Vector<float>.Build.Dense(new[] {-0.5f, 0.0f, 0.5f, 1.0f });
            Vector<float> v3 = matrix * Vector<float>.Build.Dense(new[] {0.5f, 0.0f, 0.5f, 1.0f });
            Vector<float> v4 = matrix * Vector<float>.Build.Dense(new[] {0.5f, 0.0f, -0.5f, 1.0f });

            v1 = Math3D.Matrix3dHelper.Scale(v1);
            v2 = Math3D.Matrix3dHelper.Scale(v2);
            v3 = Math3D.Matrix3dHelper.Scale(v3);
            v4 = Math3D.Matrix3dHelper.Scale(v4);

            if (PointInTriangle(point, v1, v2, v3))
                return _id;

            if (PointInTriangle(point, v3, v4, v1))
                return _id;

            return null;
        }

        private float sign(Vector<float> p1, Vector<float> p2, Vector<float> p3)
        {
            return (p1[0] - p3[0]) * (p2[1] - p3[1]) - (p2[0] - p3[0]) * (p1[1] - p3[1]);
        }

        private bool PointInTriangle(Vector<float> pt, Vector<float> v1, Vector<float> v2, Vector<float> v3)
        {
            bool b1, b2, b3;

            b1 = sign(pt, v1, v2) < 0.0f;
            b2 = sign(pt, v2, v3) < 0.0f;
            b3 = sign(pt, v3, v1) < 0.0f;

            return ((b1 == b2) && (b2 == b3));
        }
    }
}