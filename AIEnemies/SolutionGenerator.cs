using AIEnemies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIEnemies
{
    class SolutionGenerator
    {
        private GameParameters gameParameters;

        public SolutionGenerator(GameParameters gameParameters)
        {
            this.gameParameters = gameParameters;
        }

        private bool IsSolutionIsInsideGame(Solution solution)
        {
            bool IsFeeldIsInsideGame(FieldCoordinates f) =>
                0 <= f.X && f.X < gameParameters.SizeX &&
                0 <= f.Y && f.Y < gameParameters.SizeY &&
                0 <= f.Z && f.Z < gameParameters.SizeZ;

            return IsFeeldIsInsideGame(solution.Coordinates.First()) &&
                IsFeeldIsInsideGame(solution.Coordinates.Last());
        }

        private bool?[] dimensions = new bool?[3];
        private int directionCombinationCount = 0;
        private FieldCoordinates currentPoint;
        private HashSet<Solution> solutions = new HashSet<Solution>();

        public HashSet<Solution> GetSolutions()
        {
            solutions.Clear();
            GenerateAllPosibleSolution();
            return solutions;
        }

        private void GenerateAllPosibleSolution()
        {
            var cartesianProduct = 
                from x in Enumerable.Range(0, gameParameters.SizeX)
                from y in Enumerable.Range(0, gameParameters.SizeY)
                from z in Enumerable.Range(0, gameParameters.SizeZ)
                select new { x, y, z };

            foreach(var coords in cartesianProduct)
            {
                directionCombinationCount = 0;
                currentPoint = new FieldCoordinates(coords.x, coords.y, coords.z);
                SetDirectionAndGenerateSolutions(0);
            }
        }

        private void SetDirectionAndGenerateSolutions(int dimensionNum)
        {
            if(dimensionNum < 3)
            {
                foreach(var state in new bool?[] { null, false, true })
                {
                    dimensions[dimensionNum] = state;
                    SetDirectionAndGenerateSolutions(dimensionNum + 1);
                }
            }
            else
            {
                if(directionCombinationCount != 0)
                {
                    AddNewSolution();
                }

                directionCombinationCount++;
            }
        }

        private void AddNewSolution()
        {
            var solution = GetSolution(GetNextFeeldFunc());
            if (IsSolutionIsInsideGame(solution))
            {
                solutions.Add(solution);
            }
        }

        private Func<FieldCoordinates, FieldCoordinates> GetNextFeeldFunc()
        {
            return prevField =>
            {
                int[] coords = new int[3];
                for (int x = 0; x < 3; x++)
                {
                    int tmp = prevField[x];
                    switch (dimensions[x])
                    {
                        case true:
                            tmp++;
                            break;
                        case false:
                            tmp--;
                            break;
                    }
                    coords[x] = tmp;
                }

                return new FieldCoordinates(coords);
            };
        }

        private Solution GetSolution(Func<FieldCoordinates, FieldCoordinates> nextFieldFunc)
        {
            var solution = new FieldCoordinates[4];
            var iterator = currentPoint;
            for(int x = 0; x < 4; x++)
            {
                solution[x] = iterator;
                iterator = nextFieldFunc(iterator);
            }

            return new Solution(solution);
        }
    }
}
