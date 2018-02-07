using System;
using System.Text;
using SharpGL;

namespace OX3DGame.GraphicsEngine
{
    public class Shader : IDisposable
    {
        private readonly OpenGL gl;

        public uint HandleShader { get; }

        public Shader(OpenGL gl, uint type, string code)
        {
            this.gl = gl;
            HandleShader = gl.CreateShader(type);
            LoadShaderString(code);
        }

        private void LoadShaderString(string code)
        {
            gl.ShaderSource(HandleShader, code);
            gl.CompileShader(HandleShader);
            CheckCompileErrors();
        }

        private void CheckCompileErrors()
        {
            int[] status = { 0 };
            gl.GetShader(HandleShader, OpenGL.GL_COMPILE_STATUS, status);
            if (status[0] == OpenGL.GL_FALSE)
            {
                StringBuilder stringBuilder = new StringBuilder(1024);
                gl.GetShaderInfoLog(HandleShader, 1024, IntPtr.Zero, stringBuilder);
                Dispose();
                throw new Exception(stringBuilder.ToString());
            }
        }

        public void Dispose()
        {
            gl.DeleteShader(HandleShader);
        }
    }
}