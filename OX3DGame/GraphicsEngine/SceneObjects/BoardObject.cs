using System.Security.Cryptography.X509Certificates;

namespace OX3DGame.GraphicsEngine
{
    public class BoardObject : CompositeObject
    {
        public CompositeObject[,] Stakes { get; } = new CompositeObject[4,4]; 
        public BoardObject()
        {
            float offset = 4.5f;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    Stakes[x, y] = new CompositeObject();
                    Stakes[x, y].AddSceneObject(new StakeObject());
                    Stakes[x, y].Transform.PositionX = -3 * offset / 2 + offset * x;
                    Stakes[x, y].Transform.PositionZ = -3 * offset / 2 + offset * y;
                    Stakes[x, y].Transform.PositionY = 2;

                    SquareButtonObject squareButtonObject = new SquareButtonObject(x + 4*y);
                    squareButtonObject.Transform.ScaleX = offset*0.75f;
                    squareButtonObject.Transform.ScaleZ = offset*0.75f;
                    squareButtonObject.Transform.PositionY = 4;
                    Stakes[x, y].AddSceneObject(squareButtonObject);

                    //var q = new TagObject(true);
                    //var q2 = new TagObject(false);
                    //var q3 = new TagObject(true);
                    //var q4 = new TagObject(false);
                    //q.Transform.PositionY = 0.5f;
                    //q2.Transform.PositionY = 1.5f;
                    //q3.Transform.PositionY = 2.5f;
                    //q4.Transform.PositionY = 3.5f;
                    //Stakes[x, y].AddSceneObject(q);
                    //Stakes[x, y].AddSceneObject(q2);
                    //Stakes[x, y].AddSceneObject(q3);
                    //Stakes[x, y].AddSceneObject(q4);

                    AddSceneObject(Stakes[x, y]);
                }
            }
            
            AddSceneObject(new StandObject());
        }
    }
}