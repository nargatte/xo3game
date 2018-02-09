using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace OX3DGame.GraphicsEngine
{
    public class WaltzGeometry : GeometryBase
    {
        public WaltzGeometry(float radius, float height, int nCircle)
        {
            //Vertexes.Add(new []{0f, height, 0.0f});
            //AddVertexCircuit(Vector<float>.Build.Dense(new []{0.0f, height, 0.0f}), radius, nCircle);
            //AddTriangleCircle(1, nCircle, 0);
            //AddVertexCircuit(Vector<float>.Build.Dense(new[] { 0.0f, height, 0.0f}), radius, nCircle);
            //AddVertexCircuit(Vector<float>.Build.Dense(new[] { 0.0f, 0.0f, 0.0f }), radius, nCircle);
            //AddTriangleSprit(Vertexes.Count - 2*nCircle, Vertexes.Count - nCircle, nCircle);

            List<float[]> points = new List<float[]>
            {
                new []{0.0f, height},
                new []{radius, height},
                new []{radius, 0f}
            };

            ConstructSolidOfRevolution(points, nCircle, false);

        }
    }
}