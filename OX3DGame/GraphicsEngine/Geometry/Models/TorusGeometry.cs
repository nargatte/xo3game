using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace OX3DGame.GraphicsEngine
{
    public class TorusGeometry : GeometryBase
    {
        public TorusGeometry(float rLittleCircle, float rBigCircle, int nLittleCircle, int nBigCircle)
        {

            List<float[]> points = new List<float[]>();
            for (int x = 0; x < nLittleCircle; x++)
            {
                double angle = 2d * Math.PI * x / nLittleCircle;
                points.Add(new []{(float)Math.Sin(angle)*rLittleCircle + rBigCircle, (float)Math.Cos(angle)*rLittleCircle});
            }

            ConstructSolidOfRevolution(points, nBigCircle, true);
        }
    }
}