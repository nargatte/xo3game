using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AIEnemies.Enemies
{
    public class SimpleHeuristics : IPlayer
    {
        private bool myColor;
        private IReadOnlyGameState gameState;
        private Random random;

        public SimpleHeuristics(Random random)
        {
            this.random = random;
        }
        public void ConfigureGame(bool yourColor, IReadOnlyGameState gameState)
        {
            this.myColor = yourColor;
            this.gameState = gameState;
        }

        public void OpponentMove(Move move)
        {
        }

        public Move GetBestMove(int n, bool color)
        {
            var (ms, mm) = gameState.SolutionTracker.GetSolutionsWithMaxCounters(color);
            if (mm == n)
            {
                foreach (var s in ms)
                {
                    foreach (var c in s.Coordinates)
                    {
                        if (!gameState.GetField(c).HasValue && gameState.GetHeight(c.ToMove()) == c.Y)
                            return c.ToMove();
                    }
                }
            }
            return null;
        }

        public Move YourMove()
        {
            Move m = GetBestMove(3, myColor);
            if (m != null)
                return m;
            m = GetBestMove(3, !myColor);
            if (m != null)
                return m;
            var moves = gameState.GetAllPossibleMoves().ToArray();
            return moves[random.Next(moves.Length)];
        }
    }
}
