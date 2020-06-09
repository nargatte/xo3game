using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using AIEnemies.Models;

namespace AIEnemies
{
    public class GameState : IReadOnlyGameState
    {
        private GameState()
        {

        }

        public GameState(GameParameters gameParameters)
        {
            feelds = new List<bool>[gameParameters.SizeX, gameParameters.SizeZ];
            for (int x = 0; x < gameParameters.SizeX; x++)
                for (int z = 0; z < gameParameters.SizeZ; z++)
                    feelds[x, z] = new List<bool>();
            this.gameParameters = gameParameters;
            solutionTracker = new SolutionTracker(new Models.SolutionsContainer(gameParameters, new SolutionGenerator(gameParameters).GetSolutions()));
        }

        public GameState GetCopy()
        {
            var game = new GameState();
            game.gameParameters = this.gameParameters;
            game.feelds = new List<bool>[gameParameters.SizeX, gameParameters.SizeZ];
            for (int x = 0; x < gameParameters.SizeX; x++)
                for (int z = 0; z < gameParameters.SizeZ; z++)
                    game.feelds[x, z] = this.feelds[x, z].ToList();
            game.solutionTracker = new SolutionTracker(this.solutionTracker);
            game.NexMoveColor = this.NexMoveColor;
            return game;
        }

        public IEnumerable<CounterOnBoard> GetCounterOnBoards()
        {
            for (int x = 0; x < gameParameters.SizeX; x++)
                for (int z = 0; z < gameParameters.SizeZ; z++)
                    for(int y = 0; y < feelds[x, z].Count; y++)
                    {
                        yield return new CounterOnBoard(feelds[x, z][y], new FieldCoordinates(x, y, z));
                    }

        }

        private List<bool>[,] feelds;

        private GameParameters gameParameters;

        public bool NexMoveColor { get; private set; } = false;

        public IReadOnlySolutionTracker SolutionTracker => solutionTracker;

        private SolutionTracker solutionTracker;

        public int GetHeight(Move move) => feelds[move.X, move.Z].Count;
        public bool? GetField(FieldCoordinates field) => feelds[field.X, field.Z].Count > field.Y ? (bool?)feelds[field.X, field.Z][field.Y] : null;
        private bool CanPerformMove(Move move) => feelds[move.X, move.Z].Count < gameParameters.SizeY;

        public IEnumerable<Move> GetAllPossibleMoves()
        {
            if (solutionTracker.GetSolutionsWithMaxCounters(false).Item2 == 4 ||
                solutionTracker.GetSolutionsWithMaxCounters(true).Item2 == 4)
            {
                return Enumerable.Empty<Move>();
            }

            var moves =
            from x in Enumerable.Range(0, gameParameters.SizeX)
            from z in Enumerable.Range(0, gameParameters.SizeZ)
            select new Move(x, z);
            return moves.Where(CanPerformMove);
        }

        public CounterOnBoard GetCounterOnBoard(Move move)
        {
            var h = GetHeight(move);
            var c = move.ToField(h);
            return new CounterOnBoard(NexMoveColor, c);
        }

        public void PerformMove(Move move)
        {
            if (!CanPerformMove(move))
                throw new Exception("Cant perform move");

            var y = feelds[move.X, move.Z].Count;
            solutionTracker?.AddField(move.ToField(y), NexMoveColor);

            feelds[move.X, move.Z].Add(NexMoveColor);
            NexMoveColor = !NexMoveColor;
        }

        public GameResolution? GetResolution(bool player)
        {
            if (solutionTracker.GetSolutionsWithMaxCounters(player).Item2 == 4)
                return GameResolution.Win;
            else if (solutionTracker.GetSolutionsWithMaxCounters(!player).Item2 == 4)
                return GameResolution.Loss;
            else if (!GetAllPossibleMoves().Any())
                return GameResolution.Draw;
            return null;
        }
    }
}
