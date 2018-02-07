using System;
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;
using SharpGL.SceneGraph;

namespace OX3DGame.Math3D
{
    public static class Matrix3dHelper
    {
        public static Matrix<float> CreateMatrix() => Matrix<float>.Build.Dense(4, 4);
        public static Vector<float> CreateVector() => Vector<float>.Build.Dense(4);

        public static Matrix<float> Transform(float x, float y, float z)
        {
            Matrix<float> matrix = CreateMatrix();
            matrix[0, 0] = 1;
            matrix[1, 1] = 1;
            matrix[2, 2] = 1;
            matrix[3, 3] = 1;
            matrix[0, 3] = x;
            matrix[1, 3] = y;
            matrix[2, 3] = z;
            return matrix;
        }

        public static Matrix<float> RotateX(float angle)
        {
            Matrix<float> matrix = CreateMatrix();
            matrix[0, 0] = 1;
            matrix[3, 3] = 1;
            matrix[1, 1] = (float) Math.Cos(angle);
            matrix[2, 2] = (float) Math.Cos(angle);
            matrix[2, 1] = (float) Math.Sin(angle);
            matrix[1, 2] = (float) -Math.Sin(angle);
            return matrix;
        }

        public static Matrix<float> RotateY(float angle)
        {
            Matrix<float> matrix = CreateMatrix();
            matrix[1, 1] = 1;
            matrix[3, 3] = 1;
            matrix[0, 0] = (float)Math.Cos(angle);
            matrix[2, 2] = (float)Math.Cos(angle);
            matrix[0, 2] = (float)Math.Sin(angle);
            matrix[2, 0] = (float)-Math.Sin(angle);
            return matrix;
        }

        public static Matrix<float> RotateZ(float angle)
        {
            Matrix<float> matrix = CreateMatrix();
            matrix[2, 2] = 1;
            matrix[3, 3] = 1;
            matrix[1, 1] = (float)Math.Cos(angle);
            matrix[0, 0] = (float)Math.Cos(angle);
            matrix[0, 1] = (float)Math.Sin(angle);
            matrix[1, 0] = (float)-Math.Sin(angle);
            return matrix;
        }

        public static Matrix<float> Perspective(float near, float far, float aspectRatio, float fov)
        {
            float e = (float) (1 / Math.Tan(fov / 2));
            Matrix<float> matrix = CreateMatrix();
            matrix[0, 0] = e;
            matrix[1, 1] = (e / aspectRatio);
            matrix[2, 2] = -(far + near) / (far - near);
            matrix[3, 2] = -2 * far * near / (far - near);
            matrix[2, 3] = -1;
            return matrix;
        }

        public static Matrix<float> Scale(float x, float y, float z)
        {
            Matrix<float> matrix = CreateMatrix();
            matrix[0, 0] = x;
            matrix[1, 1] = y;
            matrix[2, 2] = z;
            matrix[3, 3] = 1;
            return matrix;
        }

        public static Matrix<float> LookAt(Vector<float> eye, Vector<float> target, Vector<float> up)
        {
            Vector<float> zaxis = (eye - target).Normalize(2);
            Vector<float> xaxis = Cross(up, zaxis).Normalize(2);
            Vector<float> yaxis = Cross(zaxis, xaxis);

            Matrix<float> matrix = CreateMatrix();
            matrix[0, 0] = xaxis[0];
            matrix[1, 0] = xaxis[1];
            matrix[2, 0] = xaxis[2];
            matrix[3, 0] = -xaxis.DotProduct(eye);

            matrix[0, 1] = yaxis[0];
            matrix[1, 1] = yaxis[1];
            matrix[2, 1] = yaxis[2];
            matrix[3, 1] = -yaxis.DotProduct(eye);

            matrix[0, 2] = zaxis[0];
            matrix[1, 2] = zaxis[1];
            matrix[2, 2] = zaxis[2];
            matrix[3, 2] = -zaxis.DotProduct(eye);

            matrix[3, 3] = 1;

            return matrix.Transpose();
        }

        public static Vector<float> Cross(Vector<float> left, Vector<float> right)
        {
            Vector<float> result = Vector<float>.Build.Dense(3);
            result[0] = left[1] * right[2] - left[2] * right[1];
            result[1] = -left[0] * right[2] + left[2] * right[0];
            result[2] = left[0] * right[1] - left[1] * right[0];

            return result;
        }

        public static Vector<float> Scale(Vector<float> point)
        {
            if (point[3] != 0)
                return Vector<float>.Build.Dense(new[] {point[0] / point[3], point[1] / point[3], point[2] / point[3]});
            return Vector<float>.Build.Dense(new[] {point[0], point[1], point[2]});
        }
    }
}