using AIEnemies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIEnemies
{
    class SolutionTracker : IReadOnlySolutionTracker
    {
        private readonly Dictionary<Solution, int[]> SolutionCounterCount = new Dictionary<Solution, int[]>();
        private int[] maxCount = new int[] { 0, 0 };
        private HashSet<Solution>[] bestSolutions = new HashSet<Solution>[] { new HashSet<Solution>(), new HashSet<Solution>() };
        public SolutionsContainer Solutions { get; }
        public SolutionTracker(SolutionsContainer solutions)
        {
            this.Solutions = solutions;
            foreach (var s in solutions.Solutions)
            {
                SolutionCounterCount.Add(s, new int[] { 0, 0 });
            }
        }

        public SolutionTracker(SolutionTracker solutionTracker)
        {
            this.Solutions = solutionTracker.Solutions;
            foreach (var p in solutionTracker.SolutionCounterCount)
            {
                SolutionCounterCount.Add(p.Key, p.Value.ToArray());
            }
            maxCount = solutionTracker.maxCount.ToArray();
            bestSolutions[0] = new HashSet<Solution>(solutionTracker.bestSolutions[0]);
            bestSolutions[1] = new HashSet<Solution>(solutionTracker.bestSolutions[1]);
        }

        public void AddField(FieldCoordinates fieldCoordinates, bool player)
        {
            var pi = ToPlayerIndex(player);
            foreach (var s in Solutions.GetSolutionsByField(fieldCoordinates))
            {
                var ct = SolutionCounterCount[s];
                ct[pi]++;
                if (maxCount[pi] == ct[pi])
                {
                    bestSolutions[pi].Add(s);
                }
                else if (maxCount[pi] < ct[pi])
                {
                    bestSolutions[pi].Clear();
                    bestSolutions[pi].Add(s);
                    maxCount[pi] = ct[pi];
                }
            }
        }

        public (IReadOnlyCollection<Solution>, int) GetSolutionsWithMaxCounters(bool player)
        {
            var pi = ToPlayerIndex(player);
            return (bestSolutions[pi], maxCount[pi]);
        }

        private int ToPlayerIndex(bool color) => color ? 1 : 0;
    }
}
