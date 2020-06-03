using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIEnemies
{
    public class Solution
    {
        public Solution(IEnumerable<FieldCoordinates> coordinates)
        {
            Coordinates = new SortedSet<FieldCoordinates>(coordinates);
        }

        public IReadOnlyCollection<FieldCoordinates> Coordinates { get; }

        public override bool Equals(object obj)
        {
            return obj is Solution solution &&
                   ((Solution)obj).Coordinates.Zip(Coordinates, (a, b) => a.Equals(b)).All(t => t);
        }

        public override int GetHashCode()
        {
            return Coordinates.Select(c => c.GetHashCode()).Aggregate((s, i) => unchecked(s + i));
        }
    }
}
