using System;
using System.Collections.Generic;
using System.Text;

namespace AIEnemies
{
    static class GameResolutionHelper
    {
        public static GameResolution InvertResolution(this GameResolution gameResolution)
        {
            switch (gameResolution)
            {
                case GameResolution.Loss:
                    return GameResolution.Win;
                case GameResolution.Win:
                    return GameResolution.Loss;
                default:
                    return GameResolution.Draw;
            }
        }
    }
}
