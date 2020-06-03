using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using MathNet.Numerics.LinearAlgebra;
using OX3DGame.FluentValue;
using OX3DGame.GraphicsEngine.ShadersSource;
using SharpGL;

namespace OX3DGame.GraphicsEngine
{
    public class RenderManager : IDisposable 
    {
        private OpenGL gl;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public static Matrix<float> ProjectionMatrix;
        public static double MsPerFrame = 0;
        public static long FrameCount = 0;
        public static UniformShader Shader { get; private set; }
        public static GeometryStore GeometryStore { get; private set; }
        public static float AmbientStrength = 0.2f;
        public static AnimationType AnimationType = AnimationType.None;

        public Scene Scene { get; private set; }

        public void ChangeShader(UniformShader shader)
        {
            Shader = shader;
            Shader.UseProgram();
            Shader.AmbientStrength = AmbientStrength;
        }

        public void ResetGame()
        {
            Scene = new Scene();
        }

        public void SetLineMode(bool lineMode)
        {
            if(lineMode)
                gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_LINE);
            else
                gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
        }

        public RenderManager(OpenGL gl)
        {
            this.gl = gl;
        }

        public void ClickOn(float x, float y)
        {
            Scene.ClickOn(x, y);
        }

        public void MouseMove(float x, float y)
        {
            Scene.MouseMove(x, y);
        }

        public void Initialize()
        {
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.CullFace(OpenGL.GL_BACK);
            gl.Enable(OpenGL.GL_CULL_FACE);
            gl.ClearColor(0.3f, 0.3f, 0.3f, 0);

            Shader = new PhongPhongShader(gl);
            Shader.UseProgram();
            Shader.AmbientStrength = AmbientStrength;
            GeometryStore = new GeometryStore(gl);

            Scene = new Scene();

        }

        public void Resize(int width, int height)
        {
            ProjectionMatrix = Math3D.Matrix3dHelper.Perspective(1, 100, (float)height / width, (float) (45f / 180f * Math.PI));
        }

        public void Draw()
        {
            FrameCount++;
            MsPerFrame = Math.Min(_stopwatch.Elapsed.TotalMilliseconds, 100d);
            _stopwatch.Restart();

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            Scene.Draw();

            OpenGlErrorsChecker();
        }

        private void OpenGlErrorsChecker()
        {
            while (true)
            {
                uint errorType = gl.GetError();
                if (errorType == OpenGL.GL_NO_ERROR) break;
                string errorString = gl.GetErrorDescription(errorType);
                string title = null;
                switch (errorType)
                {
                    case OpenGL.GL_INVALID_ENUM:
                        title = "Argument wyliczeniowy wykracza poza dopuszczalną wartość.";
                        break;
                    case OpenGL.GL_INVALID_VALUE:
                        title = "Argument numeryczny wykracza poza dopuszczalną wartość.";
                        break;
                    case OpenGL.GL_INVALID_OPERATION:
                        title = "Operacja nie może być wykonana w aktualnym stanie";
                        break;
                    case OpenGL.GLU_OUT_OF_MEMORY:
                        title = "Brakuje pamięci potrzebnej do wykonania polecenia";
                        break;
                }

                throw new Exception(title + "\n" + errorString);
            }
        }

        public void Dispose()
        {
            Shader?.Dispose();
            GeometryStore?.Dispose();
        }
    }
}