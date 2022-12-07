namespace AdventOfCode.Framework.Runner;

public class Options
{
    public string? InputBaseDirectory { internal get; init; } = null;

    public int MaxBenchmarkIterations { internal get; init; } = 10_000;

    public TimeSpan MaxBenchmarkTimePerProblem { internal get; init; } = TimeSpan.FromMinutes(5);
}