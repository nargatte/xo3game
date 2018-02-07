using MathNet.Numerics.LinearAlgebra;
using SharpGL;

namespace OX3DGame.GraphicsEngine.ShadersSource
{
    public abstract class UniformShader : ShaderProgram
    {
        private int _mvpMatrixId;
        private int _mvMatrixId;
        private int _normalMatrixId;
        private int _objectColorId;
        private int _specularColor;
        private int _shininessId;
        private int _lpId;
        private int _ambientStrengthId;

        public UniformShader(OpenGL gl) : base(gl)
        {
        }

        public Matrix<float> MvpMatrix
        {
            set => SetMatrix4(value, _mvpMatrixId);
        }

        public Matrix<float> MvMatrix
        {
            set => SetMatrix4(value, _mvMatrixId);
        }

        public Matrix<float> NormalMatrix
        {
            set => SetMatrix4(value, _normalMatrixId);
        }

        public Vector<float> ObjectColor
        {
            set => SetVector3(value, _objectColorId);
        }

        public Vector<float> SpecularColor
        {
            set => SetVector3(value, _specularColor);
        }

        public float Shininess
        {
            set => SetValue(value, _shininessId);
        }

        public Vector<float> LightPosition
        {
            set => SetVector3(value, _lpId);
        }

        public float AmbientStrength
        {
            set => SetValue(value, _ambientStrengthId);
        }

        protected override string[] BindNames => new[] {"inPosition", "inNormal"};

        protected override void LookForUniforms()
        {
            _mvpMatrixId = GetUniformLocation("mvpMatrix");
            _mvMatrixId = GetUniformLocation("mvMatrix");
            _normalMatrixId = GetUniformLocation("normalMatrix");
            _objectColorId = GetUniformLocation("objectColor");
            _specularColor = GetUniformLocation("specularColor");
            _shininessId = GetUniformLocation("shininess");
            _lpId = GetUniformLocation("LightPos");
            _ambientStrengthId = GetUniformLocation("ambientStrength");
        }
    }
}