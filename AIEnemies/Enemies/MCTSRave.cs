using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIEnemies.Models;

namespace AIEnemies.Enemies
{
    public class MCTSRave : MCTSAlgorithm
    {
        private readonly double _b;

        public MCTSRave(Random r, double cParametr, int iterationCount, double b) : base(r, cParametr, iterationCount)
        {
            _b = b;
        }

        protected override void SetRoot()
        {
            root = new NodeRave(gameState, null, _b);
        }

        protected override void BackPropagation(Node node, bool isMyMove, GameResolution resolution, GameState state)
        {
            while (true)
            {
                isMyMove = !isMyMove;
                node.SymulationStatistics.Update(isMyMove ? resolution : resolution.InvertResolution());

                foreach (var onBoard in state.GetCounterOnBoards())
                {
                    if (((NodeRave) node).RaveStatistics.TryGetValue(onBoard, out var statistics))
                    {
                        statistics.Update(isMyMove ? resolution : resolution.InvertResolution());
                    }
                }

                if (node == root)
                {
                    break;
                }

                node = node.Parent;
            }
        }

        protected override Move GetRandomMove(Node node) => ((NodeRave) node).GetBestRaveMove();
    }

}
