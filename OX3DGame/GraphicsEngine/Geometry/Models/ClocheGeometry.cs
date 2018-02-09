using System.Collections.Generic;

namespace OX3DGame.GraphicsEngine
{
    public class ClocheGeometry : GeometryBase
    {
        public ClocheGeometry()
        {
            List<float[]> points = new List<float[]>
            {
                new []{0.0f, 0.0f},
                new []{1f, 0f},
                new []{2f, -3f},
                new []{-0.01f, 0.29f},
                new []{-0.01f, 0.0f}
            };

            ConstructSolidOfRevolution(points, 20, false);
        }
    }
}