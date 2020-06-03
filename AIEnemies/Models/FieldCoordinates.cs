using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AIEnemies
{
    public class FieldCoordinates : IComparable<FieldCoordinates>
    {
        public FieldCoordinates(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public FieldCoordinates(int[] coordinates)
        {
            X = coordinates[0];
            Y = coordinates[1];
            Z = coordinates[2];
        }

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public override bool Equals(object obj)
        {
            return obj is FieldCoordinates coordinates &&
                   X == coordinates.X &&
                   Y == coordinates.Y &&
                   Z == coordinates.Z;
        }

        public override int GetHashCode()
        {
            int hashCode = -307843816;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }



        public Move ToMove() => new Move(X, Z);

        public int CompareTo(FieldCoordinates other)
        {
            if (X.CompareTo(other.X) != 0)
                return X.CompareTo(other.X);
            if (Y.CompareTo(other.Y) != 0)
                return Y.CompareTo(other.Y);
            if (Z.CompareTo(other.Z) != 0)
                return Z.CompareTo(other.Z);
            return 0;
        }
    }
}
