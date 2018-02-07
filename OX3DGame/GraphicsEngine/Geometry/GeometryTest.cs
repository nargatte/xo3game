using System.Collections.Generic;

namespace OX3DGame.GraphicsEngine
{
    public class GeometryTest : GeometryBase
    {
        public GeometryTest()
        {
            //Vertexes.Add(new []{0.0f, 0.0f, 0.0f});
            //AddVertexCircuit(Vector<float>.Build.Dense(3), 0.3f, 6);
            //AddTriangleCircle(1, 6, 0);
            //AddVertexCircuit(Vector<float>.Build.Dense(3), 0.4f, 6);
            //AddTriangleSprit(1, 7, 6);
            //AddVertexCircuit(Vector<float>.Build.Dense(3), 0.5f, 6);
            //AddTriangleSprit(7, 13, 6);
            //AddVertexCircuit(Vector<float>.Build.Dense(3), 0.6f, 6);
            //AddTriangleSprit(13, 19, 6);
            //ReversTriangles(6, 12);
            List<float[]> l = new List<float[]>
            {
                new [] {0.0f, 0.3f },
                new [] {0.3f, 0.1f },
                new [] {0.5f, -0.1f },
                new [] {0.7f, -0.3f }
            };
            ConstructSolidOfRevolution(l, 100, false);
        }
    }
}