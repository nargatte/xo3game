using OX3DGame.GraphicsEngine.ShadersSource;
using SharpGL;

namespace OX3DGame.GraphicsEngine
{
    public class GouraudBlinnShader : UniformShader
    {
        public GouraudBlinnShader(OpenGL gl) : base(gl)
        {
            VertexShader = new Shader(gl, OpenGL.GL_VERTEX_SHADER, Properties.Resources.Gouraud_Blinn_vert);
            FragmentShader = new Shader(gl, OpenGL.GL_FRAGMENT_SHADER, Properties.Resources.Simple_frag);

            Prepare();
        }

        protected override Shader VertexShader { get; }
        protected override Shader FragmentShader { get; }
    }
}