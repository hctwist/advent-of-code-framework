using System.Reflection;
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
        runner.Solve(0);
    }
    
    [TestMethod]
    public void TestBenchmark()
    {
        SolutionRunner runner = new();
        runner.Benchmark();
    }
}