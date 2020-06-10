using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEnemies.Tests
{
    class RunParameters
    {
        public int IterationCount { get; set; }
        public double cParametr { get; set; }
        public double bParametr { get; set; }
        public int IterationReduction { get; set; }
        public int TreeHeightReduction { get; set; }
        public GameParameters GameParameters { get; set; }
    }
}
