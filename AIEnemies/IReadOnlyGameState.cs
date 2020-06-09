using System;
using System.Collections.Generic;
using AIEnemies.Models;

namespace AIEnemies
{
    public interface IReadOnlyGameState
    {
        bool NexMoveColor { get; }
        IReadOnlySolutionTracker SolutionTracker { get; }

        IEnumerable<Move> GetAllPossibleMoves();
        bool? GetField(FieldCoordinates field);
        int GetHeight(Move move);
        GameResolution? GetResolution(bool player);
        GameState GetCopy();
        CounterOnBoard GetCounterOnBoard(Move move);
    }
}