using System;
using System.Collections.Generic;
using System.Text;

namespace AIEnemies
{
    public class Move
    {
        public int X { get; }
        public int Z { get; }

        public Move(int x, int z)
        {
            X = x;
            Z = z;
        }

        public FieldCoordinates ToField(int y) => new FieldCoordinates(X, y, Z);
    }
}
