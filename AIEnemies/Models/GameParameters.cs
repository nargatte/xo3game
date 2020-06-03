using System;
using System.Collections.Generic;
using System.Text;

namespace AIEnemies
{
    public class GameParameters
    {
        public GameParameters(int sizeX, int sizeY, int sizeZ)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
        }

        public int SizeX { get; }
        public int SizeY { get; }
        public int SizeZ { get; }
    }
}
