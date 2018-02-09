using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using MathNet.Numerics.LinearAlgebra;
using SharpGL;

namespace OX3DGame.GraphicsEngine
{
    public abstract class ShaderProgram : IDisposable
    {
        private readonly OpenGL gl;

        private uint HandleProgram { get; }

        protected abstract Shader VertexShader { get; }
        protected abstract Shader FragmentShader { get; }
        protected abstract string[] BindNames { get; }

        protected ShaderProgram(OpenGL gl)
        {
            this.gl = gl;
            HandleProgram = gl.CreateProgram();
        }

        private void AttachShaders()
        {
            gl.AttachShader(HandleProgram, VertexShader.HandleShader);
            gl.AttachShader(HandleProgram, FragmentShader.HandleShader);
        }

        private void BindAttribute()
        {
            for(uint x = 0; x < BindNames.Length; x++)
                gl.BindAttribLocation(HandleProgram, x, BindNames[x]);
        }

        protected int GetUniformLocation(string name) => gl.GetUniformLocation(HandleProgram, name);
        protected void SetMatrix4(Matrix<float> matrix, int uniformId) => gl.UniformMatrix4(uniformId, 1, false, matrix.AsColumnMajorArray());
        protected void SetVector3(Vector<float> vector, int uniformId) => gl.Uniform3(uniformId, vector[0], vector[1], vector[2]);
        protected void SetValue(float value, int uniformId) => gl.Uniform1(uniformId, value);
        protected void SetVectors3(Vector<float>[] vectors, int uniformId)
        {
            Array.ForEach(vectors, v =>
            {
                if(v.Count != 3) throw new Exception("Numbers of dementions is not 3");
            } );
            List<float> list = new List<float>();
            Array.ForEach(vectors, v => list.AddRange(v.AsArray()));
            gl.Uniform3(uniformId, vectors.Length, list.ToArray());
        }

        private void Link()
        {
            gl.LinkProgram(HandleProgram);

            VertexShader.Dispose();
            FragmentShader.Dispose();

            CheckKonsolidationErrors();
        }

        private void CheckKonsolidationErrors()
        {
            int[] status = {0};
            gl.GetProgram(HandleProgram, OpenGL.GL_LINK_STATUS, status);
            if (status[0] == OpenGL.GL_FALSE)
            {
                StringBuilder stringBuilder = new StringBuilder(1024);
                gl.GetProgramInfoLog(HandleProgram, 1024, IntPtr.Zero, stringBuilder);
                Dispose();
                throw new Exception(stringBuilder.ToString());
            }
        }

        protected void Prepare()
        {
            AttachShaders();
            BindAttribute();
            Link();
            LookForUniforms();
        }

        protected abstract void LookForUniforms();

        public void UseProgram()
        {
            gl.UseProgram(HandleProgram);
        }

        public void Dispose()
        {
            gl.DeleteProgram(HandleProgram);
        }
    }
}