using System.Reflection;
using AdventOfCode.Framework.Runner.Benchmark;
using AdventOfCode.Framework.Runner.Solve;
using AdventOfCode.Framework.Tests.Mocks;
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
            MaxTimePerCase = TimeSpan.FromSeconds(5)
        });
        runner.BenchmarkAll();
    }
}