namespace AdventOfCode.Framework.Runner;

/// <summary>
/// Solution runner options.
/// </summary>
public class Options
{
    /// <summary>
    /// A base directory to resolve inputs. If this is not set, then inputs will be resolved relative to the build root.
    /// </summary>
    public string? InputBaseDirectory { internal get; init; } = null;

    /// <summary>
    /// Benchmarking options.
    /// </summary>
    public BenchmarkOptions Benchmarks { internal get; init; } = new();
}

public class BenchmarkOptions
{
    /// <summary>
    /// Whether to allow benchmarking in debug mode.
    /// </summary>
    public bool AllowDebugMode { internal get; init; } = false;

    /// <summary>
    /// Whether to abort all benchmarks if a solution throws an exception.
    /// </summary>
    public bool AbortWithExceptions { internal get; init; } = false;

    /// <summary>
    /// The maximum number of iterations per problem.
    /// </summary>
    public int MaxIterations { internal get; init; } = 10_000;

    /// <summary>
    /// The maximum time to run each problem.
    /// </summary>
    public TimeSpan MaxTime { internal get; init; } = TimeSpan.FromMinutes(5);
}