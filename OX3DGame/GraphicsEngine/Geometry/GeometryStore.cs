using System;
using System.Collections.Generic;
using System.Linq;
using SharpGL;

namespace OX3DGame.GraphicsEngine
{
    public class GeometryStore : IDisposable
    { 
        public Mesh Torus { get; private set; }
        public Mesh Waltz { get; private set; }
        public Mesh Phlox { get; private set; }
        public Mesh Candlestick { get; private set; }
        public Mesh Square { get; private set; }
        public Mesh WaltzPoor { get; private set; }
        public Mesh Cloche { get; private set; }

        private OpenGL gl;

        private readonly List<float> _vectorData = new List<float>();
        private readonly List<float> _normalData = new List<float>();
        private readonly List<ushort> _indexesData = new List<ushort>();

        private uint[] _VertexAndNormalBufforHandle = new[] { 0u };
        private uint[] _IndexBufforHandle = new[] { 0u };
        private uint[] _VAOHandle = new[] { 0u };

        public GeometryStore(OpenGL gl)
        {
            this.gl = gl;

            CreateModels();

            gl.GenBuffers(1, _VertexAndNormalBufforHandle);
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, _VertexAndNormalBufforHandle[0]);
            gl.BufferData(OpenGL.GL_ARRAY_BUFFER, _vectorData.Concat(_normalData).ToArray(), OpenGL.GL_STATIC_DRAW);

            gl.GenBuffers(1, _IndexBufforHandle);
            gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, _IndexBufforHandle[0]);
            gl.BufferData(OpenGL.GL_ELEMENT_ARRAY_BUFFER, _indexesData.ToArray(), OpenGL.GL_STATIC_DRAW);

            gl.GenVertexArrays(1, _VAOHandle);
            gl.BindVertexArray(_VAOHandle[0]);

            gl.VertexAttribPointer(0, 3, OpenGL.GL_FLOAT, false, 0, IntPtr.Zero);
            gl.VertexAttribPointer(1, 3, OpenGL.GL_FLOAT, false, 0, new IntPtr(_vectorData.Count*sizeof(float)));

            gl.EnableVertexAttribArray(0);
            gl.EnableVertexAttribArray(1);
            gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, _IndexBufforHandle[0]);
        }

        private void CreateModels()
        {
            Torus = AddGeometry(new TorusGeometry(0.5f, 1f, 12, 20));
            Waltz = AddGeometry(new WaltzGeometry(1f, 1f, 32));
            WaltzPoor = AddGeometry(new WaltzGeometry(1f, 1f, 12));
            Phlox = AddGeometry(new PhloxGeometry());
            Candlestick = AddGeometry(new CandlestickGeometry(15));
            Square = AddGeometry(new SquareGeometry());
            Cloche = AddGeometry(new ClocheGeometry());
        }

        private Mesh AddGeometry(GeometryBase geometryBase)
        {
            Mesh m = new Mesh(_indexesData.Count, geometryBase.Tringles.Count, gl);
            int vertexCount = _vectorData.Count/3;
            foreach (float[] vertex in geometryBase.Vertexes)
                _vectorData.AddRange(vertex);
            foreach (float[] normal in geometryBase.Normals())
                _normalData.AddRange(normal);
            foreach (int[] indexes in geometryBase.Tringles)
                _indexesData.AddRange(indexes.Select(x => (ushort) (x + vertexCount)));
            return m;
        }

        public void Dispose()
        {
            gl.DeleteBuffers(1, _VertexAndNormalBufforHandle);
            gl.DeleteBuffers(1, _IndexBufforHandle);
            gl.DeleteVertexArrays(1, _VAOHandle);
        }
    }
}