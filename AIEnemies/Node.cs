using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AIEnemies
{
    [DebuggerDisplay("{SymulationStatistics.SimultaionCount} {SymulationStatistics.WinCount} {childrens.Count}")]
    public class Node : IComparable<Node>
    {
        public Node Parent { get; }
        protected Dictionary<Move, Node> childrens = new Dictionary<Move, Node>();
        protected List<Move> unvisitedMoves = new List<Move>();

        public SymulationStatistics SymulationStatistics { get; } = new SymulationStatistics();

        public IReadOnlyList<Move> UnvisitedMoves => unvisitedMoves;

        public IReadOnlyDictionary<Move, Node> Childrens => childrens;

        public Node(IReadOnlyGameState state, Node parent)
        {
            this.Parent = parent;
            foreach (var move in state.GetAllPossibleMoves())
            {
                unvisitedMoves.Add(move);
            }
            
        }

        public virtual void AddChild(Move move, GameState state)
        {
            var node = new Node(state, this);
            childrens.Add(move, node);
            unvisitedMoves.Remove(move);
        }

        public int CompareTo(Node other) => GetHashCode();

        public bool IsFullyUncover => !unvisitedMoves.Any();
        public bool IsLeaf => !unvisitedMoves.Any() && !childrens.Any();

        public void RemoveChildrensExcept(Move m)
        {
            var newChildrens = new Dictionary<Move, Node>();
            newChildrens.Add(m, childrens[m]);
            childrens = newChildrens;
        }

        public void RemoveChildrens()
        {
            var newChildrens = new Dictionary<Move, Node>();
            childrens = newChildrens;
        }

        public Move GetBestMove() => childrens.Aggregate((max, it) =>
                max.Value.SymulationStatistics.SimultaionCount < it.Value.SymulationStatistics.SimultaionCount
                    ? it
                    : max)
            .Key;

        public KeyValuePair<Move, Node> GetChildrenWithBestScore(double cParametr)
        {
            return childrens.Select(p => new {p, score = GetScore(p, cParametr)})
                .Aggregate((max, it) => max.score < it.score ? it : max).p;
        }

        protected virtual double GetScore(KeyValuePair<Move, Node> pair, double cParametr) =>
            pair.Value.SymulationStatistics.GetExploration(cParametr, SymulationStatistics.SimultaionCount) +
            pair.Value.SymulationStatistics.GetExploatatin();  
    }
}
