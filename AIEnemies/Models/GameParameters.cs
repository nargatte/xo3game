using System;
using System.Collections.Generic;
using System.Text;

namespace AIEnemies
{
    public class GameParameters
    {
        public GameParameters()
        {
        }
        public GameParameters(int size)
        {
            SizeX = size;
            SizeY = size;
            SizeZ = size;
        }
        public GameParameters(int sizeX, int sizeY, int sizeZ)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
        }

        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public int SizeZ { get; set; }
    }
}
