using AIEnemies.Enemies;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIEnemies
{
    public class GameOrchestrator
    {
        private readonly IPlayer whitePlayer;
        private readonly IPlayer blackPlayer;
        private readonly GameState gameState;

        public event Action<Move> moved;

        public GameOrchestrator(IPlayer whitePlayer, IPlayer blackPlayer, GameParameters gameParameters)
        {
            this.whitePlayer = whitePlayer;
            this.blackPlayer = blackPlayer;
            this.gameState = new GameState(gameParameters);

            whitePlayer.ConfigureGame(false, gameState);
            blackPlayer.ConfigureGame(true, gameState);
        }

        public GameOrchestrator(IPlayer whitePlayer, IPlayer blackPlayer, GameState gameState)
        {
            this.whitePlayer = whitePlayer;
            this.blackPlayer = blackPlayer;
            this.gameState = gameState.GetCopy();

            whitePlayer.ConfigureGame(false, this.gameState);
            blackPlayer.ConfigureGame(true, this.gameState);
        }

        public GameResolution StartGame(Action waiter, bool myColor = false)
        {
            Move move;
            GameResolution? gameResolution;

            gameResolution = gameState.GetResolution(myColor);
            if (gameResolution.HasValue)
                return gameResolution.Value;

            while (true)
            {
                waiter();
                if (!gameState.NexMoveColor)
                {
                    move = whitePlayer.YourMove();
                    gameState.PerformMove(move);
                    moved?.Invoke(move);
                    gameResolution = gameState.GetResolution(myColor);
                    if (gameResolution.HasValue)
                        return gameResolution.Value;
                    blackPlayer.OpponentMove(move);
                }
                else
                {
                    move = blackPlayer.YourMove();
                    gameState.PerformMove(move);
                    moved?.Invoke(move);
                    gameResolution = gameState.GetResolution(myColor);
                    if (gameResolution.HasValue)
                        return gameResolution.Value;
                    whitePlayer.OpponentMove(move);
                }
            }
        }
    }
}
