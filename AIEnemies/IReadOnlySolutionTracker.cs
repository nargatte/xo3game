using AIEnemies.Models;
using System.Collections.Generic;

namespace AIEnemies
{
    public interface IReadOnlySolutionTracker
    {
        (IReadOnlyCollection<Solution>, int) GetSolutionsWithMaxCounters(bool player);
        SolutionsContainer Solutions { get; }
    }
}