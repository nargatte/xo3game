using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIEnemies.Models;

namespace AIEnemies
{
    class NodeRave : Node
    {
        private readonly double _b;

        private readonly Dictionary<CounterOnBoard, SymulationStatistics> _raveStatisticsByBoard = new Dictionary<CounterOnBoard, SymulationStatistics>();
        private readonly Dictionary<Move, SymulationStatistics> _raveStatisticsByMove = new Dictionary<Move, SymulationStatistics>();

        public IReadOnlyDictionary<CounterOnBoard, SymulationStatistics> RaveStatistics => _raveStatisticsByBoard;

        public NodeRave(IReadOnlyGameState state, Node parent, double b) : base(state, parent)
        {
            _b = b;

            foreach (var move in unvisitedMoves)
            {
                var s = new SymulationStatistics();
                _raveStatisticsByBoard.Add(state.GetCounterOnBoard(move), s);
                _raveStatisticsByMove.Add(move, s);
            }
        }

        private double BetaFactor(int x, int y) => y / ((double) x + y + 4 * x * y * _b * _b);
        protected override double GetScore(KeyValuePair<Move, Node> pair, double cParametr)
        {
            var rave = _raveStatisticsByMove[pair.Key];
            var betha = BetaFactor(pair.Value.SymulationStatistics.SimultaionCount, rave.SimultaionCount);
            return (1 - betha) * pair.Value.SymulationStatistics.GetExploatatin() + betha * rave.GetExploatatin() +
                   pair.Value.SymulationStatistics.GetExploration(cParametr, SymulationStatistics.SimultaionCount);
        }

        public override void AddChild(Move move, GameState state)
        {
            var node = new NodeRave(state, this, _b);
            childrens.Add(move, node);
            unvisitedMoves.Remove(move);
        }

        public Move GetBestRaveMove() => unvisitedMoves
            .Select(m => new {m, score = _raveStatisticsByMove[m].GetExploatatin()})
            .Aggregate((max, it) => max.score < it.score ? it : max).m;
    }
}  
