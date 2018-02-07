using System.Collections.Generic;

namespace OX3DGame.GraphicsEngine
{
    public class CandlestickGeometry : GeometryBase
    {
        public CandlestickGeometry(int nVertex)
        {
            List<float[]> points = new List<float[]>
            {
                new []{0.0f, 3.0f},
                new []{1.9f, 3.0f},
                new []{1.9f, 2.8f},
                new []{1.7f, 2.8f},
                new []{1.6f, 0.7f},
                new []{0.2f, 0.7f},
                new []{0.2f, 0.2f},
                new []{2.9f, 0.2f},
                new []{2.9f, 0.0f},
                new []{0.0f, 0.0f}
            };
            ConstructSolidOfRevolution(points, nVertex, false);
        }
    }
}