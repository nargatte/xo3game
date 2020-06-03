using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace AIEnemies.Models
{
    public class SolutionsContainer
    {
        private IReadOnlyCollection<Solution>[,,] solutionsByPosition;
        public IReadOnlyCollection<Solution> Solutions { get; }

        public SolutionsContainer(GameParameters gameParameters, HashSet<Solution> solutions)
        {
            var container = new List<Solution>[gameParameters.SizeX, gameParameters.SizeY, gameParameters.SizeZ];

            var cartesianProduct =
            from x in Enumerable.Range(0, gameParameters.SizeX)
            from y in Enumerable.Range(0, gameParameters.SizeY)
            from z in Enumerable.Range(0, gameParameters.SizeZ)
            select new { x, y, z };

            foreach (var coords in cartesianProduct)
            {
                container[coords.x, coords.y, coords.z] = new List<Solution>();
            }

            foreach (var solution in solutions)
            {
                foreach (var coords in solution.Coordinates)
                {
                    container[coords.X, coords.Y, coords.Z].Add(solution);
                }
            }

            solutionsByPosition = container;
            Solutions = solutions;
        }

        public IEnumerable<Solution> GetSolutionsByField(FieldCoordinates field) => solutionsByPosition[field.X, field.Y, field.Z];
    }
}
