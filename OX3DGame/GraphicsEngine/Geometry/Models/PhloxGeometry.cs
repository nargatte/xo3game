using System.Collections.Generic;

namespace OX3DGame.GraphicsEngine
{
    public class PhloxGeometry : GeometryBase
    {
        public PhloxGeometry()
        {
            List<float[]> points = new List<float[]>
            {
                new []{0.0f, 1.0f},
                new []{0.5f, 0.3f},
                new []{0f, 0f}
            };

            ConstructSolidOfRevolution(points, 5, false);
        }
    }
}