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

        private Matrix<float> _projectionMatrix;

        public static double MsPerFrame = 0;
        public static long FrameCount = 0;
        public static UniformShader Shader { get; private set; }
        public static GeometryStore GeometryStore { get; private set; }

        private CompositeObject so;
        private CameraObject c;

        public RenderManager(OpenGL gl)
        {
            this.gl = gl;
        }


        public void Initialize()
        {
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            //gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_LINE);
            gl.CullFace(OpenGL.GL_BACK);
            //gl.Enable(OpenGL.GL_CULL_FACE);
            gl.ClearColor(0, 0, 0, 0);

            Shader = new GouraudPhongShader(gl);
            Shader.UseProgram();
            Shader.AmbientStrength = 0.2f;
            Shader.SpecularColor = Vector<float>.Build.Dense(new[] {1f, 1f, 1f});
            GeometryStore = new GeometryStore(gl);

            so = new CompositeObject();
            c = new CameraObject();
            LightObject lo = new LightObject(Vector<float>.Build.Dense(new[] { 0.93f, 0.0f, 0.0f }), 0);
            lo.Transform.PositionY = 20f;
            lo.Transform.PositionX = 20f;
            so.Transform.PositionZ = -3f;
            so.Transform.RotationY = new MotionIntervalValue(0, (float) (2f*Math.PI), 10000, t => t.Restart());
            PointObject mo1 = new BoardObject();
            mo1.Transform.PositionX = 1f;
            so.AddSceneObject(mo1);
            so.AddSceneObject(lo);
            c.Transform.PositionY = 20f;
            c.Transform.PositionZ = 15f;
            c.Target = so;

        }

        public void Resize(int width, int height)
        {
            _projectionMatrix = Math3D.Matrix3dHelper.Perspective(1, 100, (float)height / width, (float) (45f / 180f * Math.PI));
        }

        public void Draw()
        {
            FrameCount++;
            MsPerFrame = _stopwatch.Elapsed.TotalMilliseconds;
            _stopwatch.Restart();

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            var vm = c.GetViewMatrix();
            so.SetLight(vm);
            so.Draw(_projectionMatrix, vm);

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