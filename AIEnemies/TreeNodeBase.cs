using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AIEnemies
{
    [DebuggerDisplay("{childrens.Count}")]
    class TreeNodeBase : IComparable<TreeNodeBase>
    {
        public GameState State { get; }
        public TreeNodeBase Parent { get; }
        private int winCount;
        private int simulationCount;
        private Dictionary<Move, TreeNodeBase> childrens = new Dictionary<Move, TreeNodeBase>();
        private List<Move> possibleMoves = new List<Move>();
        public bool Deleted { get; private set; } = false;

        public IReadOnlyList<Move> PossibleMoves => possibleMoves;

        public IReadOnlyDictionary<Move, TreeNodeBase> Childrens => childrens;

        public bool IsLeaf => State.GetResolution(false).HasValue;

        public TreeNodeBase(GameState state, TreeNodeBase parent)
        {
            this.State = state;
            this.Parent = parent;
            if (!IsLeaf)
            {
                foreach (var move in state.GetAllPossibleMoves())
                {
                    possibleMoves.Add(move);
                }
            }
        }

        public void AddChild(Move move, GameState state)
        {
            var node = new TreeNodeBase(state, this);
            childrens.Add(move, node);
            possibleMoves.Remove(move);
        }

        public double GetScore(double c)
        {
            if(Parent == null)
            {
                return 0;
            }

            return GetExploatation() + GetExploration(c);
        }

        public double GetExploatation() => (double)winCount / simulationCount;

        public double GetExploration(double c) => c * Math.Sqrt(Math.Log(Parent.simulationCount) / simulationCount);

        public void Update(GameResolution gameResolution)
        {
            simulationCount++;
            if(gameResolution == GameResolution.Win)
            {
                winCount++;
            }
        }

        public void Delete()
        {
            Deleted = true;
            foreach(var node in childrens.Values)
            {
                node.Delete();
            }
        }

        public Move GetBestMove() => 
            childrens.Aggregate((max, i) => max.Value.simulationCount < i.Value.simulationCount ? i : max).Key;

        public int CompareTo(TreeNodeBase other)
        {
            return GetHashCode();
        }
    }
}
