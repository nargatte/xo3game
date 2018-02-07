using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace OX3DGame.GraphicsEngine
{
    public class CompositeObject : PointObject
    {
        private readonly HashSet<PointObject> _compose = new HashSet<PointObject>();

        public override void Draw(Matrix<float> projection, Matrix<float> mvMatrix)
        {
            Matrix<float> matrix = mvMatrix * Transform.GetModelMatrix();
            foreach (PointObject sceneObject in _compose)
            {
                sceneObject.Draw(projection, matrix);
            }
        }

        public override int? PointOver(Matrix<float> mvpMatrix, float pointX, float pointY)
        {
            Matrix<float> matrix = mvpMatrix * Transform.GetModelMatrix();
            foreach (PointObject sceneObject in _compose)
            {
                int? result = sceneObject.PointOver(matrix, pointX, pointY);
                if (result.HasValue)
                return result;
            }
            return null;
        }

        public override void SetLight(Matrix<float> mvMatrix)
        {
            foreach (PointObject sceneObject in _compose)
            {
                sceneObject.SetLight(mvMatrix * Transform.GetModelMatrix());
            }
        }

        public void RemoveSceneObject(PointObject pointObject)
        {
            _compose.Remove(pointObject);
        }

        public void AddSceneObject(PointObject pointObject)
        {
            pointObject.Parent = this;
            _compose.Add(pointObject);
        }
    }
}