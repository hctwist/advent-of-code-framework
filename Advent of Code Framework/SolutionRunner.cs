using AdventOfCode.Framework.Runner;
using AdventOfCode.Framework.Runner.Benchmark;
using AdventOfCode.Framework.Runner.Solve;

namespace AdventOfCode.Framework;

/// <summary>
/// A solution bootstrapper.
/// </summary>
public class SolutionRunner
{
    private readonly SolveRunner solveRunner;

    private readonly BenchmarkRunner benchmarkRunner;

    /// <summary>
    /// Creates a new <see cref="SolutionRunner"/>.
    /// </summary>
    /// <param name="options">The runner options.</param>
    public SolutionRunner(Options options)
    {
        solveRunner = new SolveRunner(options);
        benchmarkRunner = new BenchmarkRunner(options);
    }

    /// <summary>
    /// Creates a new <see cref="SolutionRunner"/>.
    /// </summary>
    public SolutionRunner() : this(new Options())
    {
    }

    /// <summary>
    /// Runs solutions.
    /// </summary>
    /// <param name="args">The input arguments.</param>
    public void Run(string[] args)
    {
        if (args.Length == 0)
        {
            PrintUsage();
            return;
        }

        if (args[0].Equals("solve", StringComparison.OrdinalIgnoreCase))
        {
            switch (args.Length)
            {
                case 1:
                    SolveAll();
                    return;
                case 2 when int.TryParse(args[1], out int day):
                    Solve(day);
                    return;
                case 3 when int.TryParse(args[1], out int day) && int.TryParse(args[2], out int problem):
                    Solve(day, ProblemHelpers.Parse(problem));
                    return;
            }
        }
        else if (args[0].Equals("benchmark", StringComparison.OrdinalIgnoreCase))
        {
            switch (args.Length)
            {
                case 1:
                    BenchmarkAll();
                    return;
                case 2 when int.TryParse(args[1], out int day):
                    Benchmark(day);
                    return;
                case 3 when int.TryParse(args[1], out int day) && int.TryParse(args[2], out int problem):
                    Benchmark(day, ProblemHelpers.Parse(problem));
                    return;
            }
        }

        PrintUsage();
    }

    /// <summary>
    /// Runs solutions in solve mode.
    /// </summary>
    /// <param name="day">The day to solve.</param>
    /// <param name="problem">The problem to solve.</param>
    public void Solve(int day, Problem problem = Problem.All)
    {
        PrintIntro();
        solveRunner.Run(day, problem);
    }

    /// <summary>
    /// Runs all solutions in solve mode..
    /// </summary>
    public void SolveAll()
    {
        PrintIntro();
        solveRunner.RunAll();
    }

    /// <summary>
    /// Runs solutions for the latest day in solve mode.
    /// </summary>
    public void SolveLatest()
    {
        PrintIntro();
        solveRunner.RunLatest();
    }

    /// <summary>
    /// Runs solutions in benchmark mode.
    /// </summary>
    /// <param name="day">The day to benchmark. If this is null, then all days will be benchmarked.</param>
    /// <param name="problem">The problem to benchmark. If this is null, then both problems will be benchmarked.</param>
    public void Benchmark(int day, Problem problem = Problem.All)
    {
        PrintIntro();
        benchmarkRunner.Run(day, problem);
    }

    /// <summary>
    /// Runs all solutions in benchmark mode.
    /// </summary>
    public void BenchmarkAll()
    {
        PrintIntro();
        benchmarkRunner.RunAll();
    }

    private static void PrintIntro()
    {
        string[] intros = Directory.GetFiles("Runner/Intros");
        string intro = File.ReadAllText(intros[new Random().Next(intros.Length)]);
        
        Console.WriteLine(intro);
        Console.WriteLine();
        Console.WriteLine();
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Invalid arguments. Expected arguments of the form:");
        Console.WriteLine("  solve <day> [problem]");
        Console.WriteLine("  benchmark [day] [problem]");
        Console.WriteLine("Examples:");
        Console.WriteLine("  solve 1");
        Console.WriteLine("  solve 25 2");
        Console.WriteLine("  benchmark");
        Console.WriteLine("  benchmark 12 1");
        Console.WriteLine("  benchmark 24 2");
    }
}