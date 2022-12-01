using System.Reflection;

namespace AdventOfCode.Framework;

/// <summary>
/// A solution bootstrapper.
/// </summary>
public class SolutionRunner
{
    /// <summary>
    /// Runs the solution.
    /// </summary>
    /// <param name="args">The input arguments.</param>
    public static void Run(string[] args)
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

        if (args.Length == 0)
        {
            PrintUsage();
            return;
        }

        if (args[0].Equals("run", StringComparison.OrdinalIgnoreCase))
        {
            if (args.Length == 2 && int.TryParse(args[1], out int solutionNumber))
            {
                Run(solutionNumber);
                return;
            }
        }
        else if (args[0].Equals("benchmark", StringComparison.OrdinalIgnoreCase))
        {
            switch (args.Length)
            {
                case 1:
                    Benchmark(null);
                    return;
                case 2 when int.TryParse(args[1], out int solutionNumber):
                    Benchmark(solutionNumber);
                    return;
            }
        }

        PrintUsage();
    }

    private static void Run(int solutionNumber)
    {
        SolutionCreator solutionCreator = GetSolutionCreator(solutionNumber);
        Input input = Input.FromFile(solutionCreator.InputFilePath);

        void RunProblem(int problemNumber, Func<Solution, string> problemSelector)
        {
            try
            {
                Console.WriteLine($"-- Problem {problemNumber} --");
                Solution solution = solutionCreator.Factory(input);
                Console.WriteLine(problemSelector(solution));
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("No attempt");
            }
        }

        RunProblem(1, s => s.Problem1());
        Console.WriteLine();
        RunProblem(2, s => s.Problem2());
    }

    private static void Benchmark(int? solutionNumber)
    {
        throw new NotImplementedException();
    }

    private static SolutionCreator GetSolutionCreator(int solutionNumber)
    {
        IEnumerable<Type> solutions = Assembly.GetEntryAssembly()!
            .GetTypes()
            .Where(type => type.IsSubclassOf(typeof(Solution)));

        foreach (Type solutionType in solutions)
        {
            SolutionAttribute solutionMetadata = GetSolutionMetadata(solutionType);
            if (solutionMetadata.Enabled && solutionMetadata.Number == solutionNumber)
            {
                Func<Input, Solution> factory = GetSolutionFactory(solutionType);
                return new SolutionCreator(factory, solutionMetadata.InputFilePath);
            }
        }

        throw new Exception($"Could not find solution class for solution {solutionNumber}");
    }

    private static SolutionAttribute GetSolutionMetadata(Type type)
    {
        return type.GetCustomAttribute<SolutionAttribute>() ?? throw new Exception($"Could not find solution attribute on {type.Name}");
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

    private static void PrintUsage()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  aoc run <solution number>");
        Console.WriteLine("  aoc benchmark [solution number]");
    }

    private record SolutionCreator(Func<Input, Solution> Factory, string InputFilePath);
}