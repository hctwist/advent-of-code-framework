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
                Solve(creator.Factory, inputFilePath, problem);
                Console.WriteLine();
            }
        }
    }

    private static void Solve(Func<Input, Solution> factory, string inputFilePath, int? problem)
    {
        Input input = Input.FromFile(inputFilePath);

        void RunProblem(int n, Func<Solution, string?> problemSelector)
        {
            try
            {
                Console.WriteLine($"* Problem {n} *");

                Solution solution = factory(input);

                Stopwatch stopwatch = new();
                stopwatch.Start();

                string? result = problemSelector(solution);

                if (result is null)
                {
                    Console.WriteLine("No attempt");
                    return;
                }

                stopwatch.Stop();

                Console.WriteLine(result);

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} milliseconds");
                Console.ResetColor();
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("No attempt");
            }
        }

        if (problem is null or 1)
        {
            RunProblem(1, s => s.Problem1());
        }

        if (problem is null)
        {
            Console.WriteLine();
        }

        if (problem is null or 2)
        {
            RunProblem(2, s => s.Problem2());
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

        return creators.Count > 0 ? creators : throw new Exception($"Could not find any solution classes for day {day}");
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