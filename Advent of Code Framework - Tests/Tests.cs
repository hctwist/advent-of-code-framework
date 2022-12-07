using AdventOfCode.Framework.Runner;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Framework.Tests;

[TestClass]
public class Tests
{
    [TestMethod]
    public void TestSolve()
    {
        SolutionRunner runner = new();
        runner.SolveAll();
    }
    
    [TestMethod]
    public void TestBenchmark()
    {
        SolutionRunner runner = new(new Options()
        {
            MaxBenchmarkTimePerProblem = TimeSpan.FromSeconds(5)
        });
        runner.BenchmarkAll();
    }
}