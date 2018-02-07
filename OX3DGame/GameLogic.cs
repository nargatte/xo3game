using System.Runtime.CompilerServices;

namespace OX3DGame
{
    public class GameLogic
    {
        public bool IsMoveBlack { get; private set; }

        public bool?[,,] GameState { get; private set; } = new bool?[4, 4, 4];

        public void PerformMove(int x, int y)
        {
            GameState[x, y, GetFreePosition(x, y).Value] = IsMoveBlack;
            IsMoveBlack = !IsMoveBlack;
        }

        public int? GetFreePosition(int x, int y)
        {
            int z = 0;
            while (GameState[x, y, z] == null && z != 4)
            {
                z++;
            }
            if (z == 4)
                return null;
            return z;
        }

        public int[][] IsEnd()
        {
            bool won = true;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    won = true;
                    for (int z = 0; z < 4; z++)
                    {
                        if (GameState[x, y, z] == null || GameState[x, y, 0] != GameState[x, y, z])
                        {
                            won = false;
                            break;
                        }
                    }
                    if (won) return new[]
                     {
                        new[] {x, y, 0},
                        new[] {x, y, 1},
                        new[] {x, y, 2},
                        new[] {x, y, 3}
                    };
                }
            }

            for (int x = 0; x < 4; x++)
            {
                for (int z = 0; z < 4; z++)
                {
                    won = true;
                    for (int y = 0; y < 4; y++)
                    {
                        if (GameState[x, y, z] == null || GameState[x, y, 0] != GameState[x, y, z])
                        {
                            won = false;
                            break;
                        }
                    }
                    if (won) return new[]
                    {
                        new[] {x, 0, z},
                        new[] {x, 1, z},
                        new[] {x, 2, z},
                        new[] {x, 3, z}
                    };
                }
            }

            for (int y = 0; y < 4; y++)
            {
                for (int z = 0; z < 4; z++)
                {
                    won = true;
                    for (int x = 0; x < 4; x++)
                    {
                        if (GameState[x, y, z] == null || GameState[x, y, 0] != GameState[x, y, z])
                        {
                            won = false;
                            break;
                        }
                    }
                    if (won) return new[]
                    {
                        new[] {0, y, z},
                        new[] {1, y, z},
                        new[] {2, y, z},
                        new[] {3, y, z}
                    };
                }
            }

            for (int x = 0; x < 4; x++)
            {
                won = true;
                for (int p = 0; p < 4; p++)
                {
                    if (GameState[x, p, p] == null || GameState[x, p, 0] != GameState[x, p, p])
                    {
                        won = false;
                        break;
                    }
                }
                if (won) return new[]
                {
                    new[] {x, 0, 0},
                    new[] {x, 1, 1},
                    new[] {x, 2, 2},
                    new[] {x, 3, 3}
                };
            }

            for (int x = 0; x < 4; x++)
            {
                won = true;
                for (int p = 0; p < 4; p++)
                {
                    if (GameState[x, 3 - p, p] == null || GameState[x, 3 - p, 0] != GameState[x, 3 - p, p])
                    {
                        won = false;
                        break;
                    }
                }
                if (won) return new[]
                {
                    new[] {x, 0, 3},
                    new[] {x, 1, 2},
                    new[] {x, 2, 1},
                    new[] {x, 3, 0}
                };
            }

            for (int y = 0; y < 4; y++)
            {
                won = true;
                for (int p = 0; p < 4; p++)
                {
                    if (GameState[p, y, p] == null || GameState[p, y, 0] != GameState[p, y, p])
                    {
                        won = false;
                        break;
                    }
                }
                if (won) return new[]
                {
                    new[] {0, y, 0},
                    new[] {1, y, 1},
                    new[] {2, y, 2},
                    new[] {3, y, 3}
                };
            }

            for (int y = 0; y < 4; y++)
            {
                won = true;
                for (int p = 0; p < 4; p++)
                {
                    if (GameState[y, 3 - p, p] == null || GameState[y, 3 - p, 0] != GameState[y, 3 - p, p])
                    {
                        won = false;
                        break;
                    }
                }
                if (won) return new[]
                {
                    new[] {0, y, 3},
                    new[] {1, y, 2},
                    new[] {2, y, 1},
                    new[] {3, y, 0}
                };
            }

            won = true;
            for (int p = 0; p < 4; p++)
            {
                if (GameState[p, p, p] == null || GameState[p, p, 0] != GameState[p, p, p])
                {
                    won = false;
                    break;
                }
            }
            if (won) return new[]
            {
                new[] {0, 0, 0},
                new[] {1, 1, 1},
                new[] {2, 2, 2},
                new[] {3, 3, 3}
            };

            won = true;
            for (int p = 0; p < 4; p++)
            {
                if (GameState[3 - p, p, p] == null || GameState[3 - p, p, 0] != GameState[3 - p, p, p])
                {
                    won = false;
                    break;
                }
            }
            if (won) return new[]
            {
                new[] {3, 0, 0},
                new[] {2, 1, 1},
                new[] {1, 2, 2},
                new[] {0, 3, 3}
            };

            won = true;
            for (int p = 0; p < 4; p++)
            {
                if (GameState[p, 3 - p, p] == null || GameState[p, 3 - p, 0] != GameState[p, 3 - p, p])
                {
                    won = false;
                    break;
                }
            }
            if (won) return new[]
            {
                new[] {0, 3, 0},
                new[] {1, 2, 1},
                new[] {2, 1, 2},
                new[] {3, 0, 3}
            };

            won = true;
            for (int p = 0; p < 4; p++)
            {
                if (GameState[3 - p, 3 - p, p] == null || GameState[3 - p, 3 - p, 0] != GameState[3 - p, 3 - p, p])
                {
                    won = false;
                    break;
                }
            }
            if (won) return new[]
            {
                new[] {3, 3, 0},
                new[] {2, 2, 1},
                new[] {1, 1, 2},
                new[] {0, 0, 3}
            };

            return null;
        }
    }
}