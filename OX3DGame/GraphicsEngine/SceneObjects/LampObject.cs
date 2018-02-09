using System;
using System.Windows.Input;
using MathNet.Numerics.LinearAlgebra;
using OX3DGame.FluentValue;

namespace OX3DGame.GraphicsEngine
{
    public class LampObject : CompositeObject
    {
        public SpotLightObject Light { get; }

        public LampObject()
        {
            ValueBase length = PhysicalValue.GetDefaultWithKeys(
                () => Keyboard.IsKeyDown(Key.G),
                () => Keyboard.IsKeyDown(Key.T),
                1 ,20, 10, 30);

            float swerve = 0.5f;
            Transform.RotationZ = new BasicOperation1Arg(new MotionIntervalValue(0, (float) (2*Math.PI), 4000, t => t.Restart()), f => (float) ((float) Math.Sin(f)*swerve/Math.PI));
            Transform.RotationX = new BasicOperation1Arg(new MotionIntervalValue(0, (float)(2 * Math.PI), 3000, t => t.Restart()), f => (float)((float)Math.Sin(f) * swerve / Math.PI));

            float lineR = 0.2f;

            MeshObject line = new MeshObject(RenderManager.GeometryStore.WaltzPoor, 
                Vector<float>.Build.Dense(new []{0.8f, 0.8f, 0.8f}), 1000);

            line.Transform.ScaleX = lineR;
            line.Transform.ScaleY = length;
            line.Transform.ScaleZ = lineR;
            line.Transform.PositionY = new NegativeValue(length);

            MeshObject cloche = new MeshObject(RenderManager.GeometryStore.Cloche,
                Vector<float>.Build.Dense(new[] { 0.8f, 0.8f, 0.8f }), 1000);

            cloche.Transform.PositionY = new NegativeValue(length);

            Light = new SpotLightObject();

            Light.Transform.PositionY = new NegativeValue(length);

            AddSceneObject(line);
            AddSceneObject(cloche);
            AddSceneObject(Light);

        }
    }
}