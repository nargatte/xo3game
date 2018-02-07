using OX3DGame.GraphicsEngine.ShadersSource;
using SharpGL;

namespace OX3DGame.GraphicsEngine
{
    public class GouraudPhongShader : UniformShader
    {
        public GouraudPhongShader(OpenGL gl) : base(gl)
        {
            VertexShader = new Shader(gl, OpenGL.GL_VERTEX_SHADER, Properties.Resources.Gouraud_Phong_vert);
            FragmentShader = new Shader(gl, OpenGL.GL_FRAGMENT_SHADER, Properties.Resources.Simple_frag);

            Prepare();
        }

        protected override Shader VertexShader { get; }
        protected override Shader FragmentShader { get; }
    }
}