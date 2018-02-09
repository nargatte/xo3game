using System.Windows.Input;
using MathNet.Numerics.LinearAlgebra;
using OX3DGame.FluentValue;

namespace OX3DGame.GraphicsEngine
{
    public class RotaryCameraObject : CompositeObject, ICamera
    { 
        public TargetedCameraObject Camera { get; } = new TargetedCameraObject();

        public RotaryCameraObject(float start, float maxZoom, float maxWay)
        {
            Camera.Target = this;
            Camera.Transform.PositionZ = PhysicalValue.GetDefaultWithKeys(
                () => Keyboard.IsKeyDown(Key.F), () => Keyboard.IsKeyDown(Key.R), 1.2f, start, maxZoom, maxWay);
            AddSceneObject(Camera);

            Transform.RotationY = PhysicalValue.GetDefaultWithKeys(
                () => Keyboard.IsKeyDown(Key.D), () => Keyboard.IsKeyDown(Key.A), 0.1f);

            Transform.RotationX = PhysicalValue.GetDefaultWithKeys(
                () => Keyboard.IsKeyDown(Key.S), () => Keyboard.IsKeyDown(Key.W), 0.1f, -4f, -13f, 0.5f);
        }

        public Matrix<float> GetViewMatrix() => Camera.GetViewMatrix();
    }
}