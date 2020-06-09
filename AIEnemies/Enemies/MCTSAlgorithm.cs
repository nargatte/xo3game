using AIEnemies.Enemies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace AIEnemies
{
    public class MCTSAlgorithm : IPlayer
    {
        public MCTSAlgorithm(Random r, double cParametr, int iterationCount)
        {
            this.random = r;
            this.cParametr = cParametr;
            _iterationCount = iterationCount;
            heuristics = new SimpleHeuristics(r);
        }

        protected SimpleHeuristics heuristics;
        protected Random random;
        protected readonly double cParametr;
        protected readonly int _iterationCount;
        protected Node root;
        protected bool myColor;
        protected IReadOnlyGameState gameState;

        public void ConfigureGame(bool yourColor, IReadOnlyGameState gameState)
        {
            myColor = yourColor;
            this.gameState = gameState;
            SetRoot();
        }

        public void OpponentMove(Move move)
        {
            UpdateTreeRoot(move);
        }

        public virtual Move YourMove()
        {
            for (int i = 0; i < _iterationCount; i++)
            {
                DoAlgorithmIteration();
            }

            var move = root.GetBestMove();
            UpdateTreeRoot(move);
            return move;
        }

        protected virtual void SetRoot()
        {
            root = new Node(gameState, null);
        }

        protected Node Selection(GameState state)
        {
            var node = root;
            while (node.IsFullyUncover && !node.IsLeaf)
            {
                var pair = node.GetChildrenWithBestScore(cParametr);
                state.PerformMove(pair.Key);
                node = pair.Value;
            }

            return node;
        }

        protected Node Expantion(Node parent, GameState state, Move move)
        {
            state.PerformMove(move);
            parent.AddChild(move, state);
            return parent.Childrens[move];
        }

        protected virtual void BackPropagation(Node node, bool isMyMove, GameResolution resolution, GameState state)
        {
            while (true)
            { 
                isMyMove = !isMyMove;
                node.SymulationStatistics.Update(isMyMove ? resolution : resolution.InvertResolution());

                if (node == root)
                {
                    break;
                }

                node = node.Parent;
            }
        }

        protected void DoAlgorithmIteration()
        {
            var state = gameState.GetCopy();
            var node = Selection(state);
            if (!node.IsLeaf)
            {
                var move = GetRandomMove(node);
                node = Expantion(node, state, move);
            }

            var isMyMove = state.NexMoveColor == myColor;
            var resolution = PerformSimulation(state);
            BackPropagation(node, isMyMove, resolution, state);
        }

        protected void UpdateTreeRoot(Move move)
        {
            if (root.Childrens.TryGetValue(move, out var child))
            {
                root = child;
            }
            else
            {
                SetRoot();
            }
        }
    
        protected virtual Move GetRandomMove(Node node)
        {
            var moves = node.UnvisitedMoves;
            return moves[random.Next(moves.Count)];
        }

        protected GameResolution PerformSimulation(GameState state) =>
            new GameOrchestrator(heuristics, heuristics, state).StartGame(() => { }, myColor);
    }
}
 