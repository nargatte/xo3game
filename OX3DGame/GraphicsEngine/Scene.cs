using System;
using System.Diagnostics;
using System.Windows.Input;
using MathNet.Numerics.LinearAlgebra;
using OX3DGame.FluentValue;

namespace OX3DGame.GraphicsEngine
{
    public class Scene
    {
        enum SceneState
        {
            Turn,
            Animation,
            EndGame
        }

        public const int NumberOfCandles = 5;
        private const float CandleRadius = 15f;
        private const float CandleOffSetAngle = 0f;
        private const float CandleMaxHeight = 7f;
        private const float CandleMinHeight = 1f;

        private GameLogic _gameLogic = new GameLogic();
        private SceneState _sceneState = SceneState.Turn;

        private CompositeObject _all = new CompositeObject();

        private MeshObject _floor = new MeshObject(RenderManager.GeometryStore.Square, Vector<float>.Build.Dense(new[] { 0.3f, 0.8f, 1f }), 1000);
        private BoardObject _board = new BoardObject();
        private CandleObject[] _condles = new CandleObject[NumberOfCandles];
        private CompositeObject _allCondles = new CompositeObject();
        private LampObject _lamp = new LampObject();

        private TagObject _acctualTag = null;
        private float _hoverX = 0;
        private float _hoverZ = 0;
        private float _acctualTagY = 0;

        private TagObject[,,] _archiverTags = new TagObject[4,4,4];

        private RotaryCameraObject _camera;

        public Scene()
        {
            Random random = new Random();
            for (int i = 0; i < NumberOfCandles; i++)
            {
                _condles[i] = new CandleObject((float) (random.NextDouble()*(CandleMaxHeight - CandleMinHeight) + CandleMinHeight), i);
                _condles[i].Transform.PositionX = (float)Math.Sin(2 * Math.PI * i / NumberOfCandles) * CandleRadius;
                _condles[i].Transform.PositionZ = (float)Math.Cos(2 * Math.PI * i / NumberOfCandles) * CandleRadius;
                _allCondles.AddSceneObject(_condles[i]);
            }
            _allCondles.Transform.RotationY = CandleOffSetAngle;

            _all.AddSceneObject(_floor);
            _all.AddSceneObject(_board);
            _all.AddSceneObject(_allCondles);
            _all.AddSceneObject(_lamp);

            _floor.Transform.ScaleX = 1000;
            _floor.Transform.ScaleZ = 1000;
            _lamp.Transform.PositionY = 45;

            _allCondles.Transform.RotationY =
                PhysicalValue.GetDefaultWithKeys(() => Keyboard.IsKeyDown(Key.E), () => Keyboard.IsKeyDown(Key.Q), 0.1f);

            NewTurn();
        }

        public void Draw()
        {
            Matrix<float> vmMatrix = _camera.GetViewMatrix();
            _all.SetLight(vmMatrix);
            RenderManager.Shader.SetLights();
            _all.Draw(RenderManager.ProjectionMatrix, vmMatrix);
        }

        private void NewTurn()
        {
            _sceneState = SceneState.Turn;

            if (RenderManager.AnimationType != AnimationType.None || _camera == null)
            {
                _camera = new RotaryCameraObject(30, 15, 50);

                _camera.Transform.PositionY = 5f;
            }

            if (EndGameCheck())
                return;

            _acctualTag = new TagObject(_gameLogic.IsMoveBlack);
            _acctualTag.MakeMeSpin(true, 10000);
            _acctualTag.Transform.PositionY = 8;
            _acctualTag.Transform.PositionX = new FollowValue(0, new FunctionProviderValue(() => _hoverX), 0.5f);
            _acctualTag.Transform.PositionZ = new FollowValue(0, new FunctionProviderValue(() => _hoverZ), 0.5f);
            _all.AddSceneObject(_acctualTag);
        }

        public void MouseMove(float x, float y)
        {
            int? position = _all.PointOver(RenderManager.ProjectionMatrix * _camera.GetViewMatrix(), x, y);
            if(position == null) return;
            Vector<float> vectorPosition = _board.Stakes[position.Value % 4, position.Value / 4].GetPosition(Matrix<float>.Build.DenseIdentity(4,4));
            _hoverX = vectorPosition[0];
            _hoverZ = vectorPosition[2];
        }

        public void ClickOn(float x, float y)
        {
            if (_sceneState != SceneState.Turn)
                return;

            int? position = _all.PointOver(RenderManager.ProjectionMatrix * _camera.GetViewMatrix(), x, y);

            if(position == null)
                return;

            int? freePosition = _gameLogic.GetFreePosition(position.Value % 4, position.Value / 4);

            if (freePosition == null) return;

            _all.RemoveSceneObject(_acctualTag);
            _board.Stakes[position.Value % 4, position.Value / 4].AddSceneObject(_acctualTag);
            _acctualTag.StopSpin();
            _acctualTag.Transform.PositionX = 0;
            _acctualTag.Transform.PositionZ = 0;

            _gameLogic.PerformMove(position.Value % 4, position.Value / 4);
            _acctualTagY = freePosition.Value * 1 + 0.5f;
            _archiverTags[position.Value % 4, position.Value / 4, freePosition.Value] = _acctualTag;

            ChooseAnimation();
        }

        public void PerformeMove(int x, int y)
        {
            if (_sceneState != SceneState.Turn)
                return;

            int? freePosition = _gameLogic.GetFreePosition(x, y);

            if (freePosition == null) return;

            _all.RemoveSceneObject(_acctualTag);
            _board.Stakes[x, y].AddSceneObject(_acctualTag);
            _acctualTag.StopSpin();
            _acctualTag.Transform.PositionX = 0;
            _acctualTag.Transform.PositionZ = 0;

            _gameLogic.PerformMove(x, y);
            _acctualTagY = freePosition.Value * 1 + 0.5f;
            _archiverTags[x, y, freePosition.Value] = _acctualTag;

            ChooseAnimation();
        }

        private void ChooseAnimation()
        {
            if (RenderManager.AnimationType == AnimationType.None)
            {
                _acctualTag.Transform.PositionY = _acctualTagY;
                NewTurn();
            }   
            else if (RenderManager.AnimationType == AnimationType.Observing)
            {
                _sceneState = SceneState.Animation;
                _acctualTag.Transform.PositionY = new MotionIntervalValue(15, _acctualTagY, 5000, value => NewTurn());
                _acctualTag.MakeMeSpin(false, 3000+_acctualTagY*350);
                _camera.Camera.Target = _acctualTag;
            }
            else if (RenderManager.AnimationType == AnimationType.Tracking)
            {
                _sceneState = SceneState.Animation;
                _acctualTag.Transform.PositionY = new MotionIntervalValue(15, _acctualTagY, 5000, value => NewTurn());
                _acctualTag.MakeMeSpin(false, 3000 + _acctualTagY * 350);
                _camera = new RotaryCameraObject(10, 5, 15);
                _camera.Transform.PositionX = new FunctionProviderValue(() =>_acctualTag.GetPosition(Matrix<float>.Build.DenseIdentity(4,4))[0]);
                _camera.Transform.PositionY = new FunctionProviderValue(() =>_acctualTag.GetPosition(Matrix<float>.Build.DenseIdentity(4,4))[1]);
                _camera.Transform.PositionZ = new FunctionProviderValue(() =>_acctualTag.GetPosition(Matrix<float>.Build.DenseIdentity(4,4))[2]);
            }
        }

        private bool EndGameCheck()
        {
            int[][] EndState = _gameLogic.IsEnd();
            if (EndState == null)
                return false;
            foreach (int[] ints in EndState)
            {
                _archiverTags[ints[0], ints[1], ints[2]].Blink();
            }
            _sceneState = SceneState.EndGame;
            return true;
        }
    }
}