using System;
using SharpGL;

namespace OX3DGame.GraphicsEngine
{
    public class Mesh
    {
        private readonly int _offSet;
        private readonly int _count;
        private readonly OpenGL gl;

        public Mesh(int offSet, int count, OpenGL gl)
        {
            _offSet = offSet;
            _count = count;
            this.gl = gl;
        }

        public void Draw()
        {
            gl.DrawElements(OpenGL.GL_TRIANGLES, _count*3, OpenGL.GL_UNSIGNED_SHORT, new IntPtr(sizeof(ushort)*_offSet));
        }
    }
}