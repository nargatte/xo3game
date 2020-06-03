using AIEnemies.Enemies;
using System;
using System.Collections.Generic;
using System.Text;
using C5;
using System.Linq;

namespace AIEnemies
{
    public class MCTSAlgorithm : IPlayer
    {
        public MCTSAlgorithm(Random r, double cParametr)
        {
            this.random = r;
            this.cParametr = cParametr;
            heuristics = new SimpleHeuristics(r);
        }

        private SimpleHeuristics heuristics;
        private Random random;
        private readonly double cParametr;
        private TreeNodeBase head;
        private bool myColor;

        public void ConfigureGame(bool yourColor, IReadOnlyGameState gameState)
        {
            head = new TreeNodeBase(gameState.GetCopy(), null);
            myColor = yourColor;
        }

        public void OpponentMove(Move move)
        {
            PerformMove(move);
        }

        public Move YourMove()
        {
            for(int x = 0; x < 1000; x++)
            {
                var bestNode = FindBestNode();
                if (bestNode == null)
                {
                    break;
                }

                if (bestNode.IsLeaf)
                {
                    var resolution = bestNode.State.GetResolution(myColor).Value;
                    UptadeNodesToHead(bestNode, resolution);
                }
                else
                {
                    AddNewNode(bestNode);
                }
            }
            var move = head.GetBestMove();
            PerformMove(move);
            return move;
        }

        private TreeNodeBase FindBestNode()
        {
            var nodes = AllNodes(head);
            if (!nodes.Any())
                return null;
            return AllNodes(head).Select(n => (n, n.GetScore(cParametr))).Aggregate((max, i) => max.Item2 < i.Item2 ? i : max).n;
        }

        private IEnumerable<TreeNodeBase> AllNodes(TreeNodeBase root)
        {
            if (root.PossibleMoves.Any())
            {
                yield return root;
            }
            else
            {
                foreach(var n in root.Childrens.Values.SelectMany(AllNodes))
                {
                    yield return n;
                }
            }

            if (root.IsLeaf)
            {
                yield return root;
            }
        }

        private void PerformMove(Move move)
        {
            foreach(var p in head.Childrens)
            {
                if(p.Key != move)
                {
                    p.Value.Delete();
                }
            }

            if(head.Childrens.TryGetValue(move, out var newHead))
            {
                head = newHead;
            }
            else
            {
                var state = head.State;
                state.PerformMove(move);
                head = new TreeNodeBase(state, null);
            }
        }

        private void AddNewNode(TreeNodeBase bestNode)
        {
            var move = GetRandomMove(bestNode);
            var state = bestNode.State.GetCopy();
            state.PerformMove(move);
            var resolution = PerformSimulation(state);
            UptadeNodesToHead(bestNode, resolution);
            bestNode.AddChild(move, state);
            UptadeNode(bestNode.Childrens[move], resolution);
        }

        private void UptadeNodesToHead(TreeNodeBase node, GameResolution resolution)
        {
            while(true)
            {
                UptadeNode(node, resolution);
                if (node == head)
                {
                    break;
                }
                node = node.Parent;
            }
        }

        private void UptadeNode(TreeNodeBase node, GameResolution resolution)
        {
            ///node.Update(resolution);
            if (node.State.NexMoveColor == myColor)
            {
                node.Update(resolution);
            }
            else
            {
                node.Update(resolution.InvertResolution());
            }
        }

        private Move GetRandomMove(TreeNodeBase node)
        {
            var moves = node.PossibleMoves;
            return moves[random.Next(moves.Count)];
        }

        private GameResolution PerformSimulation(GameState state) =>
            new GameOrchestrator(heuristics, heuristics, state).StartGame(() => { }, myColor);
    }
}
