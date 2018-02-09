using System;
using System.Collections.Generic;
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
            while (z != 4 && GameState[x, y, z] != null)
            {
                z++;
            }
            if (z == 4)
                return null;
            return z;
        }

        public int[][] IsEnd()
        {
            int[][] roz;

            roz = TwoVariable((x, y) => GenerateSystem(x, y, 0, 0, 0, 1));
            if (roz != null) return roz;

            roz = TwoVariable((x, z) => GenerateSystem(x, 0, z, 0, 1, 0));
            if (roz != null) return roz;

            roz = TwoVariable((y, z) => GenerateSystem(0, y, z, 1, 0, 0));
            if (roz != null) return roz;

            roz = OneVariable((z) => GenerateSystem(0, 0, z, 1, 1, 0));
            if (roz != null) return roz;

            roz = OneVariable((z) => GenerateSystem(3, 0, z, -1, 1, 0));
            if (roz != null) return roz;

            roz = OneVariable((x) => GenerateSystem(x, 0, 0, 0, 1, 1));
            if (roz != null) return roz;

            roz = OneVariable((y) => GenerateSystem(0, y, 0, 1, 0, 1));
            if (roz != null) return roz;

            roz = OneVariable((x) => GenerateSystem(x, 3, 0, 0, -1, 1));
            if (roz != null) return roz;

            roz = OneVariable((y) => GenerateSystem(3, y, 0, -1, 0, 1));
            if (roz != null) return roz;

            roz = GenerateSystem(0, 0, 0, 1, 1, 1);
            if (IsSystemWin(roz)) return roz;

            roz = GenerateSystem(3, 0, 0, -1, 1, 1);
            if (IsSystemWin(roz)) return roz;

            roz = GenerateSystem(0, 3, 0, 1, -1, 1);
            if (IsSystemWin(roz)) return roz;

            roz = GenerateSystem(3, 3, 0, -1, -1, 1);
            if (IsSystemWin(roz)) return roz;
            return null;
        }

        private int[][] TwoVariable(Func<int, int, int[][]> func)
        {
            for (int a = 0; a < 4; a++)
            {
                for (int b = 0; b < 4; b++)
                {
                    if (IsSystemWin(func(a, b)))
                        return func(a, b);
                }
            }
            return null;
        }

        private int[][] OneVariable(Func<int, int[][]> func)
        {
            for (int a = 0; a < 4; a++)
            {
                if (IsSystemWin(func(a)))
                    return func(a);
            }
            return null;
        }

        private int[][] GenerateSystem(int x, int y, int z, int addX, int addY, int addZ)
        {
            List<int[]> list = new List<int[]>();
            for (int i = 0; i < 4; i++)
            {
                list.Add(new []{x, y, z});
                x += addX;
                y += addY;
                z += addZ;
            }
            return list.ToArray();
        }

        private bool IsSystemWin(int[][] system)
        {
            bool? kolor = GameState[system[0][0], system[0][1], system[0][2]];
            if (kolor == null)
                return false;

            foreach (int[] i in system)
            {
                bool? tmp = GameState[i[0], i[1], i[2]];
                if (tmp != kolor)
                    return false;
            }
            return true;
        }
    }
}