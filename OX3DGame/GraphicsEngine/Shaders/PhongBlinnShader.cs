using OX3DGame.GraphicsEngine.ShadersSource;
using SharpGL;
using SharpGL.SceneGraph.Shaders;

namespace OX3DGame.GraphicsEngine
{
    public class PhongBlinnShader : UniformShader
    {
        public PhongBlinnShader(OpenGL gl) : base(gl)
        {
            VertexShader = new Shader(gl, OpenGL.GL_VERTEX_SHADER, Properties.Resources.Simple_vert);
            FragmentShader = new Shader(gl, OpenGL.GL_FRAGMENT_SHADER, Properties.Resources.Phong_Blinn_frag);

            Prepare();
        }

    protected override Shader VertexShader { get; }
    protected override Shader FragmentShader { get; }
    }
}