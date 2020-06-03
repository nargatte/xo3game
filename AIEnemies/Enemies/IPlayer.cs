using System;
using System.Collections.Generic;
using System.Text;

namespace AIEnemies.Enemies
{
    public interface IPlayer
    {
        void ConfigureGame(bool yourColor, IReadOnlyGameState gameState);

        void OpponentMove(Move move);

        Move YourMove();
    }
}
