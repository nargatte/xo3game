using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;

namespace AIEnemies.Enemies
{
    public class MCTSReduction : MCTSAlgorithm
    {
        private readonly int _iterationReduction;
        private readonly int _treeHeightReduction;

        public MCTSReduction(Random r, double cParametr, int iterationCount, int IterationReduction, int treeHeightReduction) : base(r, cParametr, iterationCount)
        {
            _iterationReduction = IterationReduction;
            _treeHeightReduction = treeHeightReduction;
        }

        public override Move YourMove()
        {
            for (int i = 0; i < _iterationCount; i++)
            {
                DoAlgorithmIteration();

                if ((i + 1) % _iterationReduction == 0)
                {
                    ReductNodesRec(root);
                }
            }

            var move = root.GetBestMove();
            UpdateTreeRoot(move);
            return move;
        }

        private void ReductNodesRec(Node node, int height = 0, bool isBest = true)
        {
            if (!node.Childrens.Any())
            {
                return;
            }

            var best = node.Childrens.Aggregate((max, it) =>
                max.Value.SymulationStatistics.GetExploatatin()< it.Value.SymulationStatistics.GetExploatatin() ? it : max);

            if (height < _treeHeightReduction)
            {
                foreach (var nc in node.Childrens.Values)
                {
                    ReductNodesRec(nc, height+1, isBest && nc == best.Value);
                }
            }
            else
            {
                if (isBest)
                {
                    node.RemoveChildrensExcept(best.Key);
                    ReductNodesRec(node.Childrens[best.Key], height+1, true);
                }
                else
                {
                    node.RemoveChildrens();
                }
            }
        }
    }
}
