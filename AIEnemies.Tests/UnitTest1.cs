using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AIEnemies;
using AIEnemies.Enemies;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace AIEnemies.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private TestContext testContextInstance;

        /// <summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        public void TestMethod1()
        {
            Random r = new Random(0);
            var l = new List<GameResolution>();
            for(int x = 0; x < 10; x++)
            {
                GameOrchestrator gameOrchestrator = new GameOrchestrator(new MCTSAlgorithm(r, Math.Sqrt(2)), new Heuristics(), new AIEnemies.GameParameters(4, 4, 4));
                var s = gameOrchestrator.StartGame(() => { });

                l.Add(s);
            }

            var q = l.GroupBy(k => k, (k, el) => new { k, count = el.Count() }).Select(e => e.ToString()).ToArray();
            TestContext.WriteLine(String.Join(",", q));
        }
    }
}
