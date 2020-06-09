using System;

namespace AIEnemies
{
    public class SymulationStatistics
    {
        public int SimultaionCount { get; private set; } = 0;
        public int WinCount { get; private set; } = 0;

        public void Update(GameResolution gameResolution)
        {
            SimultaionCount++;
            if (gameResolution == GameResolution.Win)
            {
                WinCount++;
            }
        }

        public double GetExploatatin() => (double)WinCount / SimultaionCount;

        public double GetExploration(double c, int parentSimulationCount) => c * Math.Sqrt(Math.Log(parentSimulationCount) / SimultaionCount);
    }
}