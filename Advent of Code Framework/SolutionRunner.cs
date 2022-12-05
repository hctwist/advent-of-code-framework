using AdventOfCode.Framework.Runner;

namespace AdventOfCode.Framework;

/// <summary>
/// A solution bootstrapper.
/// </summary>
public class SolutionRunner
{
    private readonly AOCSolveRunner solveRunner;

    private readonly AOCBenchmarkRunner benchmarkRunner;
    
    /// <summary>
    /// Creates a new <see cref="SolutionRunner"/>.
    /// </summary>
    /// <param name="inputDirectoryPath">The path of the input directory.</param>
    public SolutionRunner(string? inputDirectoryPath)
    {
        if (inputDirectoryPath is not null && !Directory.Exists(inputDirectoryPath))
        {
            throw new Exception($"Could not find subdirectory {inputDirectoryPath}");
        }

        solveRunner = new AOCSolveRunner(inputDirectoryPath);
        benchmarkRunner = new AOCBenchmarkRunner(inputDirectoryPath);
    }

    /// <summary>
    /// Creates a new <see cref="SolutionRunner"/>.
    /// </summary>
    public SolutionRunner() : this(null)
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
            if (int.TryParse(args[1], out int day))
            {
                switch (args.Length)
                {
                    case 2:
                        Solve(day);
                        return;
                    case 3 when int.TryParse(args[2], out int problem):
                        Solve(day, problem);
                        return;
                }
            }
        }
        else if (args[0].Equals("benchmark", StringComparison.OrdinalIgnoreCase))
        {
            if (args.Length == 1)
            {
                Benchmark();
            }
        }

        PrintUsage();
    }

    /// <summary>
    /// Runs solutions in solve mode.
    /// </summary>
    /// <param name="day">The day to solve.</param>
    /// <param name="problem">The problem to solve. If this is null then both problems will be solved.</param>
    public void Solve(int day, int? problem = null)
    {
        Solve(day, ProblemHelpers.Parse(problem));
    }

    /// <summary>
    /// Runs solutions in solve mode.
    /// </summary>
    /// <param name="day">The day to solve.</param>
    /// <param name="problem">The problem to solve.</param>
    public void Solve(int day, Problem problem)
    {
        PrintIntro();
        solveRunner.Run(day, problem);
    }

    /// <summary>
    /// Runs solutions in benchmark mode.
    /// </summary>
    /// <param name="day">The day to benchmark. If this is null, then all days will be benchmarked.</param>
    /// <param name="problem">The problem to benchmark. If this is null, then both problems will be benchmarked.</param>
    public void Benchmark()
    {
        PrintIntro();
        benchmarkRunner.Run();
    }

    private static void PrintIntro()
    {
        Console.WriteLine(
            @"
   .-.                                                   \ /
  ( (                                |                  - * -
   '-`                              -+-                  / \
            \            o          _|_          \
            ))          }^{        /___\         ))
          .-#-----.     /|\     .---'-'---.    .-#-----.
     ___ /_________\   //|\\   /___________\  /_________\  
    /___\ |[] _ []|    //|\\    | A /^\ A |    |[] _ []| _.O,_
....|'#'|.|  |*|  |...///|\\\...|   |'|   |....|  |*|  |..(^)....");
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