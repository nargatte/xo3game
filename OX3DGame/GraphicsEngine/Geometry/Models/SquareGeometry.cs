namespace OX3DGame.GraphicsEngine
{
    public class SquareGeometry : GeometryBase
    {
        public SquareGeometry()
        {
            Vertexes.Add(new []{-1f, 0f, -1f});
            Vertexes.Add(new []{1f, 0f, -1f});
            Vertexes.Add(new []{1f, 0f, 1f});
            Vertexes.Add(new []{-1f, 0f, 1f});

            Tringles.Add(new []{2, 1, 0});
            Tringles.Add(new []{0, 3, 2});
        }
    }
}