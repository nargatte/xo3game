using AIEnemies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIEnemies.Enemies
{
    public class Heuristics : IPlayer
    {
        private bool myColor;
        private IReadOnlyGameState gameState;

        public void ConfigureGame(bool yourColor, IReadOnlyGameState gameState)
        {
            this.myColor = yourColor;
            this.gameState = gameState;
        }

        public void OpponentMove(Move move)
        {
        }

        public Move YourMove() => GetStatistics().Max().Move;

        private IEnumerable<MoveStatistics> GetStatistics()
        {
            foreach (var m in gameState.GetAllPossibleMoves())
            {
                var h = gameState.GetHeight(m);
                var f = m.ToField(h);
                yield return new MoveStatistics(f, gameState, myColor);
            }
        }

        private struct MoveStatistics : IComparable<MoveStatistics>
        {
            public Move Move { get; }
            private readonly int myMax;
            private readonly int opponentMax;
            private readonly int mySolutionCount;
            private readonly int opponentSolutionCount;

            public MoveStatistics(FieldCoordinates field, IReadOnlyGameState gs, bool color)
            {
                Move = field.ToMove();
                (myMax, mySolutionCount) = GetSolutionsWithMaxCounters(gs, color, field);
                (opponentMax, opponentSolutionCount) = GetSolutionsWithMaxCounters(gs, !color, field);
            }

            private static int CountCounters(IReadOnlyGameState gs, Solution s, bool counterType)
            {
                int c = 0;
                foreach (var field in s.Coordinates)
                {
                    var fieldValue = gs.GetField(field);
                    if (!fieldValue.HasValue)
                    {
                        continue;
                    }
                    else if (fieldValue == counterType)
                    {
                        c++;
                    }
                    else
                    {
                        return 0;
                    }
                        
                }

                return c;
            }

            private static (int, int) GetSolutionsWithMaxCounters(IReadOnlyGameState gs, bool color, FieldCoordinates field)
            {
                int setCount = 0;
                int max = 0;
                foreach (var s in gs.SolutionTracker.Solutions.GetSolutionsByField(field))
                {
                    var count = CountCounters(gs, s, color);
                    if (count == max)
                    {
                        setCount++;
                    }
                    else if (count > max)
                    {
                        max = count;
                        setCount = 1;
                    }
                }

                return (max, setCount);
            }

            public int CompareTo(MoveStatistics other)
            {
                int q;
                q = opponentMax.CompareTo(other.opponentMax);
                if (q != 0) return q;
                q = myMax.CompareTo(other.myMax);
                if (q != 0) return q;
                q = opponentSolutionCount.CompareTo(other.opponentSolutionCount);
                if (q != 0) return q;
                return mySolutionCount.CompareTo(other.mySolutionCount);
            }
        }
    }
}
