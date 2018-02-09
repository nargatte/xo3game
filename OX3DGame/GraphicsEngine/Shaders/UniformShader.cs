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
        private int _ambientStrengthId;
        private int _lightPositionsId;
        private int _lightColorsDiffId;
        private int _lightColorsSpecId;
        private int _spotLightPositionId;
        private int _spotLightColorDiffId;
        private int _spotLightColorSpecId;
        private int _spotLightVectorId;
        private int _spotLightFocusId;

        private readonly Vector<float>[] _lightPositins = new Vector<float>[Scene.NumberOfCandles];
        private readonly Vector<float>[] _lightColorsDiff = new Vector<float>[Scene.NumberOfCandles];
        private readonly Vector<float>[] _lightColorsSpec = new Vector<float>[Scene.NumberOfCandles];

        public UniformShader(OpenGL gl) : base(gl)
        {
            for (int i = 0; i < _lightPositins.Length; i++)
            {
                _lightPositins[i] = Vector<float>.Build.Dense(3);
            }
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

        public Vector<float>[] LightPositions => _lightPositins;

        public Vector<float>[] LightColorsDiff => _lightColorsDiff;

        public Vector<float>[] LightColorsSpec => _lightColorsSpec;

        public Vector<float> SpotLightPosition
        {
            set => SetVector3(value, _spotLightPositionId);
        }

        public Vector<float> SpotLightColor
        {
            set => SetVector3(value, _spotLightColorDiffId);
        }

        public Vector<float> SpotLightColorSpec
        {
            set => SetVector3(value, _spotLightColorSpecId);
        }

        public Vector<float> SpotLightVector
        {
            set => SetVector3(value, _spotLightVectorId);
        }

        public float SpotLightFocus
        {
            set => SetValue(value, _spotLightFocusId);
        }

        public void SetLights()
        {
            SetVectors3(_lightPositins, _lightPositionsId);
            SetVectors3(_lightColorsDiff, _lightColorsDiffId);
            SetVectors3(_lightColorsSpec, _lightColorsSpecId);
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
            _ambientStrengthId = GetUniformLocation("ambientStrength");
            _lightPositionsId = GetUniformLocation("lightPositions");
            _lightColorsDiffId = GetUniformLocation("lightColorsDiff");
            _lightColorsSpecId = GetUniformLocation("lightColorsSpec");
            _spotLightPositionId = GetUniformLocation("spotLightPosition");
            _spotLightColorDiffId = GetUniformLocation("spotLightColorDiff");
            _spotLightColorSpecId = GetUniformLocation("spotLightColorSpec");
            _spotLightVectorId = GetUniformLocation("spotLightVector");
            _spotLightFocusId = GetUniformLocation("spotLightFocus");
        }
    }
}