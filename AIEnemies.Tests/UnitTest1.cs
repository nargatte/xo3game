using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AIEnemies;
using AIEnemies.Enemies;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

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
            for(int x = 0; x < 1; x++)
            {
                GameOrchestrator gameOrchestrator = new GameOrchestrator(new MCTSRave(r, Math.Sqrt(2), 500, 0.6), new MCTSAlgorithm(r, Math.Sqrt(2), 500), new AIEnemies.GameParameters(9));
                var s = gameOrchestrator.StartGame(() => { });

                l.Add(s);
            }

            var q = l.GroupBy(k => k, (k, el) => new { k, count = el.Count() }).Select(e => e.ToString()).ToArray();
            TestContext.WriteLine(String.Join(",", q));
        }

        Random random = new Random(1234);

        private (int, int, int) GetResultParrarel(Func<Random, GameOrchestrator> constructor, int nrOfTests = 100)
        {
            Task<GameResolution>[] tasks = new Task<GameResolution>[nrOfTests];
            for(int x = 0;x < nrOfTests; x++)
            {
                var r = new Random(random.Next());
                tasks[x] = new Task<GameResolution>(() =>
                {
                    var o = constructor(r);
                    return o.StartGame(() => { });
                });
                tasks[x].Start();
            }

            Task.WaitAll(tasks);
            var q = tasks.Select(t => t.Result).GroupBy(k => k, (k, l) => new { k, count = l.Count() });
            return (q.FirstOrDefault(a => a.k == GameResolution.Win)?.count ?? 0, q.FirstOrDefault(a => a.k == GameResolution.Loss)?.count ?? 0, q.FirstOrDefault(a => a.k == GameResolution.Draw)?.count ?? 0);

        }

        private Dictionary<PlayerType, Func<RunParameters, Random, IPlayer>> playerConstructors = new Dictionary<PlayerType, Func<RunParameters, Random, IPlayer>>
        {
            {PlayerType.Heurystyka, (rp, r) => new Heuristics() },
            {PlayerType.PodstawowyMCTS, (rp, r) => new MCTSAlgorithm(r, rp.cParametr, rp.IterationCount) },
            {PlayerType.Przycinanie, (rp, r) => new MCTSReduction(r, rp.cParametr, rp.IterationCount, rp.IterationReduction, rp.TreeHeightReduction) },
            {PlayerType.Rave, (rp, r) => new MCTSRave(r, rp.cParametr, rp.IterationCount, rp.bParametr) }
        };


        private IEnumerable<TestResult> GetTestResults(string hipotesis, PlayerType a, PlayerType b, RunParameters rp, IEnumerable<double> parametr, Action<RunParameters, double> setParametr)
        {

            foreach(double p in parametr)
            {
                setParametr(rp, p);
                var (w, l, d) = GetResultParrarel((r) => new GameOrchestrator(playerConstructors[a](rp, r), playerConstructors[b](rp, r), rp.GameParameters), 100);
                yield return new TestResult { RunParameters = rp, DrawCount = d, Hipotesis = hipotesis, Opponent1 = a, Opponent1WinsCount = w, Opponent2 = b, Opponent2WinsCount = l, TestingParametr = p };
            }
        }

        private void SaveResults(string baseName, IEnumerable<TestResult> testResults)
        {
            var tr = testResults.ToArray();
            var s = JsonConvert.SerializeObject(tr);
            File.WriteAllText($"{baseName}.txt", s);
        }

        [TestMethod]
        public void RaveParameter()
        {
            List<double> parametrs = new List<double>();
            for(int q = 0; q < 10; q++)
            {
                parametrs.Add(Math.Pow(10, q));
            }
            var n = "Rive parament b";
            SaveResults(n, GetTestResults(n, PlayerType.Rave, PlayerType.PodstawowyMCTS, new RunParameters { bParametr = 0, cParametr = Math.Sqrt(2), GameParameters = new GameParameters(4), IterationCount = 500 }, parametrs, (rp, p) => rp.bParametr = p));
        }

        [TestMethod]
        public void HeightParameter()
        {
            List<double> parametrs = new List<double>();
            for (int q = 0; q < 6; q++)
            {
                parametrs.Add(q);
            }
            var n = "Wysokość drzewa redukcji";
            SaveResults(n, GetTestResults(n, PlayerType.Przycinanie, PlayerType.PodstawowyMCTS, new RunParameters { bParametr = 0, cParametr = Math.Sqrt(2), GameParameters = new GameParameters(4), IterationCount = 500, IterationReduction = 100, TreeHeightReduction = 0 }, parametrs, (rp, p) => rp.TreeHeightReduction = (int)p));
        }

        [TestMethod]
        public void ReductionParameter()
        {
            List<double> parametrs = new List<double>();
            for (int q = 0; q < 10; q++)
            {
                parametrs.Add(1 + q*10);
            }
            var n = "Częstotliwość redukcji";
            SaveResults(n, GetTestResults(n, PlayerType.Przycinanie, PlayerType.PodstawowyMCTS, new RunParameters { bParametr = 0, cParametr = Math.Sqrt(2), GameParameters = new GameParameters(4), IterationCount = 500, TreeHeightReduction = 4}, parametrs, (rp, p) => rp.IterationReduction = (int)p));
        }

        RunParameters runParameters = new RunParameters { bParametr = 10, cParametr = Math.Sqrt(2), GameParameters = new GameParameters(4), IterationCount = 500, TreeHeightReduction = 3, IterationReduction = 100 };

        [TestMethod]
        public void H1()
        {
            var x = Math.Sqrt(2) - 0.05 * 15;
            List<double> parametrs = new List<double>();
            for (int q = 0; q < 31; q++)
            {
                parametrs.Add(x + q * 0.05);
            }

            BasicTest(parametrs, (rp, p) => rp.cParametr = p, "H1");
        }

        [TestMethod]
        public void H2()
        {
            List<double> parametrs = new List<double>();
            for (int q = 0; q < 13; q++)
            {
                parametrs.Add(Math.Pow(1.5, q) * 20);
            }

            BasicTest(parametrs, (rp, p) => rp.IterationCount = (int)p, "H2");
        }

        [TestMethod]
        public void H3()
        {
            List<double> parametrs = new List<double>();
            for (int q = 4; q <= 9; q++)
            {
                parametrs.Add(q);
            }

            BasicTest(parametrs, (rp, p) => rp.GameParameters = new GameParameters((int)p), "H3");
        }


        private void BasicTest(List<double> parametrs, Action<RunParameters, double> setParametr, string name)
        {
            var r1 = GetTestResults(name, PlayerType.PodstawowyMCTS, PlayerType.Heurystyka, runParameters, parametrs, setParametr);
            var r2 = GetTestResults(name, PlayerType.Przycinanie, PlayerType.PodstawowyMCTS, runParameters, parametrs, setParametr);
            var r3 = GetTestResults(name, PlayerType.Rave, PlayerType.PodstawowyMCTS, runParameters, parametrs, setParametr);
            var r4 = GetTestResults(name, PlayerType.Rave, PlayerType.Przycinanie, runParameters, parametrs, setParametr);
            var all = r1.Concat(r2).Concat(r3).Concat(r4);
            SaveResults(name, all);
        }

        [TestMethod]
        public void ProcessResults()
        {
            var z = int.MaxValue;
            var s = File.ReadAllText("H2.txt");
            var json = JsonConvert.DeserializeObject<TestResult[]>(s);
            var res = json.GroupBy(q => (q.Opponent1, q.Opponent2)).Select(j => (j.Key, j.OrderBy(a => a.TestingParametr).Select(a => (a.TestingParametr, a.Opponent1WinsCount, a.DrawCount)).ToArray() )).ToArray();
            TestContext.WriteLine(string.Join("\n\n", res.Select(a => $"{a.Key} ===> {string.Join("\n", a.Item2)}")));
        }
    }
}
