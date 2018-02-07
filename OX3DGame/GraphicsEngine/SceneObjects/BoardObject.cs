namespace OX3DGame.GraphicsEngine
{
    public class BoardObject : CompositeObject
    {
        CompositeObject[,] Stakes = new CompositeObject[4,4]; 
        public BoardObject()
        {
            float offset = 6f;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    Stakes[x, y] = new CompositeObject();
                    Stakes[x, y].AddSceneObject(new StakeObject());
                    Stakes[x, y].Transform.PositionX = -3 * offset / 2 + offset * x;
                    Stakes[x, y].Transform.PositionZ = -3 * offset / 2 + offset * y;

                    SquareButtonObject squareButtonObject = new SquareButtonObject(x + 4*y);
                    squareButtonObject.Transform.ScaleX = offset;
                    squareButtonObject.Transform.ScaleZ = offset;
                    Stakes[x, y].AddSceneObject(squareButtonObject);

                    AddSceneObject(Stakes[x, y]);
                }
            }
            AddSceneObject(new StandObject());
        }
    }
}