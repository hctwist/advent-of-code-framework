using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace AdventOfCode.Framework.Runner;

internal class AOCBenchmarkRunner
{
    private readonly string? inputDirectoryPath;

    public AOCBenchmarkRunner()
    {
        
    }
    
    public AOCBenchmarkRunner(string? inputDirectoryPath)
    {
        this.inputDirectoryPath = inputDirectoryPath;
    }

    public void Run()
    {
        
    }
}