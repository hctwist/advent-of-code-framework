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
        Solution solution = GetSolution(solutionNumber);

        try
        {
            Console.WriteLine("-- Problem 1 --");
            Console.WriteLine(solution.Problem1());
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("No attempt");
        }

        Console.WriteLine();

        try
        {
            Console.WriteLine("-- Problem 2 --");
            Console.WriteLine(solution.Problem2());
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("No attempt");
        }
    }

    private static void Benchmark(int? solutionNumber)
    {
        throw new NotImplementedException();
    }

    private static Solution GetSolution(int solutionNumber)
    {
        IEnumerable<Type> solutions = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.IsSubclassOf(typeof(Solution)));

        foreach (Type solutionType in solutions)
        {
            SolutionAttribute solutionMetadata = GetSolutionMetadata(solutionType);
            if (solutionMetadata.Enabled && solutionMetadata.Number == solutionNumber)
            {
                Solution solution = CreateSolution(solutionType);
                solution.Initialise(solutionMetadata.InputFilePath);
            }
        }

        throw new Exception($"Could not find solution class for solution {solutionNumber}");
    }

    private static SolutionAttribute GetSolutionMetadata(Type type)
    {
        return type.GetCustomAttribute<SolutionAttribute>() ?? throw new Exception($"Could not find solution attribute on {type.Name}");
    }

    private static Solution CreateSolution(Type type)
    {
        ConstructorInfo? constructor = type.GetConstructor(Array.Empty<Type>());

        if (constructor == null)
        {
            throw new Exception($"Could not resolve a valid empty constructor for {type.Name}");
        }

        return (Solution)constructor.Invoke(Array.Empty<object>());
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  aoc run <solution number>");
        Console.WriteLine("  aoc benchmark [solution number]");
    }
}