using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEnemies.Tests
{
    class TestResult
    {
        public string Hipotesis { get; set; }
        public PlayerType Opponent1 { get; set; }
        public PlayerType Opponent2 { get; set; }
        public int Opponent1WinsCount { get; set; }
        public int Opponent2WinsCount { get; set; }
        public int DrawCount { get; set; }
        public double TestingParametr { get; set; }
        public RunParameters RunParameters { get; set; }
    }
}
