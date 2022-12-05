using System.Diagnostics;
using System.Reflection;

namespace AdventOfCode.Framework;

/// <summary>
/// A solution bootstrapper.
/// </summary>
public class SolutionRunner
{
    private readonly string? inputDirectoryPath;
    
    /// <summary>
    /// Creates a new <see cref="SolutionRunner"/>.
    /// </summary>
    /// <param name="inputDirectoryPath">The path of the input directory.</param>
    public SolutionRunner(string inputDirectoryPath)
    {
        if (!Directory.Exists(inputDirectoryPath))
        {
            throw new Exception($"Could not find subdirectory {inputDirectoryPath}");
        }
        
        this.inputDirectoryPath = inputDirectoryPath;
    }

    /// <summary>
    /// Creates a new <see cref="SolutionRunner"/>.
    /// </summary>
    public SolutionRunner()
    {
        inputDirectoryPath = null;
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
            switch (args.Length)
            {
                case 1:
                    Benchmark();
                    return;
                case 2 when int.TryParse(args[1], out int day):
                    Benchmark(day);
                    return;
                case 3 when int.TryParse(args[1], out int day) && int.TryParse(args[2], out int problem):
                    Benchmark(day, problem);
                    return;
            }
        }

        PrintUsage();
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

    /// <summary>
    /// Runs solutions in solve mode.
    /// </summary>
    /// <param name="day">The day to solve.</param>
    /// <param name="problem">The problem to solve. If this is null then both problems will be solved.</param>
    public void Solve(int day, int? problem = null)
    {
        PrintIntro();

        SolveSingle(day, problem);
    }

    public void SolveAll()
    {
        foreach (var day in Enumerable.Range(1, 25))
        {
            if (TryGetSolutionCreators(day, out var solutionCreators))
            {
                SolveSingle(day);
            }
            else
            {
                Console.WriteLine($"~ Day {day} ~");
                Console.WriteLine();
                Console.WriteLine($"No solutions found for day {day}");
                Console.WriteLine();
            }
        }
    }

    private void SolveSingle(int day, int? problem = null)
    {
        if (problem is not null && problem != 1 && problem != 2)
        {
            throw new ArgumentException("Problem needs to be either '1' or '2'", nameof(problem));
        }

        List<SolutionCreator> solutionCreators = GetSolutionCreators(day);

        Console.WriteLine($"~ Day {day} ~");
        Console.WriteLine();
        foreach (SolutionCreator creator in solutionCreators)
        {
            Console.WriteLine($"[{creator.Name}]");
            Console.WriteLine();

            foreach (string inputFilePath in creator.InputFilePaths)
            {
                string simplifiedPath = string.IsNullOrEmpty(inputDirectoryPath) ?
                    inputFilePath :
                    Path.GetRelativePath(inputDirectoryPath, inputFilePath);
                Console.WriteLine($"- {simplifiedPath}");
                Console.WriteLine();
                var results = Solve(creator.Factory, inputFilePath, problem);
                foreach (var result in results)
                {
                    Output(result);
                    Console.WriteLine();
                }
            }
        }
    }

    private void Output(ProblemResult problemResult)
    {
        Console.WriteLine($"* Problem {problemResult.ProblemNumber} *");

        if (problemResult.Result is null)
        {
            Console.WriteLine("No attempt");
        }

        Console.WriteLine(problemResult.Result);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"Time taken: {problemResult.ElapsedMilliseconds} milliseconds");
        Console.ResetColor();
    }

    private record ProblemResult
    {
        public int ProblemNumber { get; init; }
        public string? Result { get; init; }
        public long ElapsedMilliseconds { get; init; } 
    }

    private static IEnumerable<ProblemResult> Solve(Func<Input, Solution> factory, string inputFilePath, int? problem)
    {
        Input input = Input.FromFile(inputFilePath);

        ProblemResult RunProblem(int n, Func<Solution, string?> problemSelector)
        {
            Stopwatch stopwatch = new();
            string? result = null;
            try
            {
                Solution solution = factory(input);

                stopwatch.Start();

                result = problemSelector(solution);
            }
            catch (NotImplementedException)
            {
                //No special handling needed here.
            }
            return new ProblemResult()
            {
                ProblemNumber = n,
                Result = result,
                ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
            };
        }

        if (problem is null or 1)
        {
            yield return RunProblem(1, s => s.Problem1());
        }

        if (problem is null or 2)
        {
            yield return RunProblem(2, s => s.Problem2());
        }
    }

    /// <summary>
    /// Runs solutions in benchmark mode.
    /// </summary>
    /// <param name="day">The day to benchmark. If this is null, then all days will be benchmarked.</param>
    /// <param name="problem">The problem to benchmark. If this is null, then both problems will be benchmarked.</param>
    public static void Benchmark(int? day = null, int? problem = null)
    {
        PrintIntro();
        throw new NotImplementedException();
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

    private List<SolutionCreator> GetSolutionCreators(int day)
    {
        if (!TryGetSolutionCreators(day, out var creators))
        {
            throw new Exception($"Could not find any solution classes for day {day}");
        }
        return creators;
    }

    private bool TryGetSolutionCreators(int day, out List<SolutionCreator>? solutionCreators)
    {
        IEnumerable<Type> solutions = Assembly.GetEntryAssembly()!
            .GetTypes()
            .Where(type => type.IsSubclassOf(typeof(Solution)));

        List<SolutionCreator> creators = new();

        foreach (Type solutionType in solutions)
        {
            SolutionMetadata solutionMetadata = GetSolutionMetadata(solutionType);
            if (solutionMetadata.SolutionAttribute.Day == day &&
                solutionMetadata.SolutionAttribute.Enabled &&
                solutionMetadata.SolutionInputAttributes.Any(inputAttribute => inputAttribute.Enabled))
            {
                Func<Input, Solution> factory = GetSolutionFactory(solutionType);
                creators.Add(
                    new SolutionCreator(
                        solutionType.Name,
                        factory,
                        solutionMetadata.SolutionInputAttributes
                            .Where(inputAttribute => inputAttribute.Enabled)
                            .Select(inputAttribute => inputAttribute.Path)
                            .Select(path => string.IsNullOrEmpty(inputDirectoryPath) ? path : Path.Combine(inputDirectoryPath, path))
                            .ToArray()));
            }
        }

        if (creators.Count > 0)
        {
            solutionCreators = creators;
            return true;
        }
        solutionCreators = null;
        return false;
    }

    private static SolutionMetadata GetSolutionMetadata(Type type)
    {
        SolutionAttribute solutionAttribute = type.GetCustomAttribute<SolutionAttribute>() ?? throw new Exception($"Could not find solution attribute on {type.Name}");

        SolutionInputAttribute[] solutionInputAttributes = type.GetCustomAttributes<SolutionInputAttribute>().ToArray();
        if (solutionInputAttributes.Length == 0)
        {
            throw new Exception($"Could not find a solution input attribute on {type.Name}");
        }

        return new SolutionMetadata(solutionAttribute, solutionInputAttributes);
    }

    private static Func<Input, Solution> GetSolutionFactory(Type type)
    {
        ConstructorInfo? constructor = type.GetConstructor(new[] { typeof(Input) });

        if (constructor == null)
        {
            throw new Exception($"Could not resolve a valid constructor for {type.Name} (required a single parameter of type {nameof(Input)}");
        }

        return input => (Solution)constructor.Invoke(new object[] { input });
    }

    private record SolutionCreator(string Name, Func<Input, Solution> Factory, string[] InputFilePaths);

    private record SolutionMetadata(SolutionAttribute SolutionAttribute, SolutionInputAttribute[] SolutionInputAttributes);
}