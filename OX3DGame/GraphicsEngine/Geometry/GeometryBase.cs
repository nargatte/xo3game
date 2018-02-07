using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace OX3DGame.GraphicsEngine
{
    public class GeometryBase
    {
        public List<float[]> Vertexes = new List<float[]>();
        public List<int[]> Tringles = new List<int[]>();

        protected void ConstructSolidOfRevolution(List<float[]> gaugeCords, int vertexInCircle, bool close)
        {
            for (int i = 0; i < gaugeCords.Count; i++)
            {
                float cord_x = gaugeCords[i][0];
                float cord_y = gaugeCords[i][1];
                if (i == 0 && cord_x == 0) //start by Circuit - put center
                {
                    Vertexes.Add(new [] {0.0f, cord_y, 0.0f});
                }
                else if (i == 1 && gaugeCords[0][0] == 0) //start by Circuit - make Circuit
                {
                    AddVertexCircuit(Vector<float>.Build.Dense(new [] { 0.0f, cord_y, 0.0f}), cord_x, vertexInCircle);
                    AddTriangleCircle(1, vertexInCircle, 0);
                }
                else if (i == 0 && cord_x != 0) //start by loop
                {
                    AddVertexCircuit(Vector<float>.Build.Dense(new[] {0.0f, cord_y, 0.0f}), cord_x, vertexInCircle);
                }
                else if (i == gaugeCords.Count - 1 && cord_x == 0) //end by Circuit
                {
                    Vertexes.Add(new[] { 0.0f, cord_y, 0.0f });
                    AddTriangleCircle(Vertexes.Count-vertexInCircle-1, vertexInCircle, Vertexes.Count-1);
                    ReversTriangles(Tringles.Count-vertexInCircle, vertexInCircle);
                }
                else
                {
                    AddVertexCircuit(Vector<float>.Build.Dense(new[] { 0.0f, cord_y, 0.0f }), cord_x, vertexInCircle);
                    AddTriangleSprit(Vertexes.Count - 2*vertexInCircle, Vertexes.Count - vertexInCircle, vertexInCircle);
                }
            }
            if (close)
            {
                AddTriangleSprit(Vertexes.Count - vertexInCircle, 0, vertexInCircle);
            }
        }

        protected void AddVertexCircuit(Vector<float> center, float radio, int vertrexNumber)
        {
            for (int i = 0; i < vertrexNumber; i++)
            {
                float angle = (float) ((float)i / vertrexNumber * 2f * Math.PI);
                Vertexes.Add(new[] {(float) (center[0] + Math.Sin(angle)*radio), center[1], (float) (center[2] + Math.Cos(angle)*radio)});
            }
        }

        protected void AddTriangleCircle(int vertexStart, int vertexNumber, int indexCenter)
        {
            for (int i = 0; i < vertexNumber; i++)
            {
                int v1 = vertexStart + i;
                int v2 = vertexStart + (i + 1) % vertexNumber;
                Tringles.Add(new[] {indexCenter, v1, v2});
            }
        }

        protected void AddTriangleSprit(int vertexStart1, int vertexStart2, int vertexNumber)
        {
            for (int i = 0; i < vertexNumber; i++)
            {
                int v1 = vertexStart1 + i;
                int v2 = vertexStart1 + (i + 1) % vertexNumber;

                int v3 = vertexStart2 + i;
                int v4 = vertexStart2 + (i + 1) % vertexNumber;

                Tringles.Add(new[] { v1, v3, v4 });
                Tringles.Add(new[] { v1, v4, v2 });
            }
        }

        protected void ReversTriangles(int firstIndex, int number)
        {
            for (int i = 0; i < number; i++)
            {
                int[] tmp = Tringles[firstIndex + i];
                int swap = tmp[1];
                tmp[1] = tmp[2];
                tmp[2] = swap;
            }
        }

        private float[] TringleNormal(int x)
        {
            Vector<float> p1 = Vector<float>.Build.Dense(Vertexes[Tringles[x][0]]);
            Vector<float> p2 = Vector<float>.Build.Dense(Vertexes[Tringles[x][1]]);
            Vector<float> p3 = Vector<float>.Build.Dense(Vertexes[Tringles[x][2]]);

            Vector<float> v = p2 - p1;
            Vector<float> w = p3 - p1;

            Vector<float> n = Vector<float>.Build.Dense(new[]
            {
                v[1] * w[2] - v[2] * w[1],
                v[2] * w[0] - v[0] * w[2],
                v[0] * w[1] - v[1] * w[0]

            });

            return n.Normalize(2).ToArray();
        }

        public List<float[]> Normals()
        {
            List<List<int>> VertexTringles = new List<List<int>>(Vertexes.Count);
            for (int i = 0; i < Vertexes.Count; i++)
                VertexTringles.Add(new List<int>());
            for (int t = 0; t < Tringles.Count; t++)
            {
                int[] tringle = Tringles[t];
                foreach (int i in tringle)
                {
                    VertexTringles[i].Add(t);
                }
            }

            List<float[]> Normals = new List<float[]>(Vertexes.Count);

            for (int i = 0; i < Vertexes.Count; i++)
            {
                Vector<float> sum = Vector<float>.Build.Dense(3);
                foreach (int vertexTringle in VertexTringles[i])
                {
                    sum += Vector<float>.Build.Dense(TringleNormal(vertexTringle));
                }

                Normals.Add((sum / Vector<float>.Build.Dense(3, VertexTringles[i].Count)).Normalize(2).ToArray());
            }
            return Normals;
        }

    }
}