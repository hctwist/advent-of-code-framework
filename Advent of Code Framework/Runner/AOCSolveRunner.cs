using System.Diagnostics;

namespace AdventOfCode.Framework.Runner;

internal class AOCSolveRunner
{
    private readonly string? inputDirectoryPath;

    public AOCSolveRunner(string? inputDirectoryPath)
    {
        this.inputDirectoryPath = inputDirectoryPath;
    }

    public void Run(int day, Problem problem)
    {
        List<SolutionCase> solutionCases = SolutionCaseLoader.GetSolutionCases(day);

        Console.WriteLine($"~ Day {day} ~");
        Console.WriteLine();
        foreach (IGrouping<string, SolutionCase> solutionCaseByInputPath in solutionCases.GroupBy(c => c.Input.Path))
        {
            Console.WriteLine($"{solutionCaseByInputPath.Key}");
            Console.WriteLine("------");

            SolutionDefinition[] solutionDefinitions = solutionCaseByInputPath.Select(c => c.Definition).ToArray();

            if (problem is Problem.Problem1 or Problem.All)
            {
                Run(solutionDefinitions, solutionCaseByInputPath.Key, SingleProblem.Problem1);
            }

            if (problem == Problem.All)
            {
                Console.WriteLine();
            }

            if (problem is Problem.Problem2 or Problem.All)
            {
                Run(solutionDefinitions, solutionCaseByInputPath.Key, SingleProblem.Problem2);
            }
        }
    }

    private void Run(SolutionDefinition[] solutionDefinitions, string inputPath, SingleProblem problem)
    {
        Console.WriteLine($"* {problem.ToDisplayString()} *");

        SolveResult[] results = solutionDefinitions
            .Select(d => Solve(d, inputPath, problem))
            .ToArray();

        bool consistentResults = results.Where(r => r.Result is not null).Distinct().Count() <= 1;

        foreach (SolveResult result in results)
        {
            Console.WriteLine($"[{result.Name}]");

            if (result.Result is null)
            {
                Console.WriteLine("No attempt");
            }
            else
            {
                if (consistentResults)
                {
                    Console.WriteLine(result.Result);
                }
                else
                {
                    ConsoleHelper.WriteLine(result.Result, ConsoleColor.Red);
                }
                ConsoleHelper.WriteLine($"Time taken (approx.): {result.Time} milliseconds", ConsoleColor.DarkGray);
            }

            Console.WriteLine();
        }
    }

    private SolveResult Solve(SolutionDefinition solutionDefinition, string inputPath, SingleProblem problem)
    {
        Input input = Input.FromFile(InputInformation.ResolvePath(inputPath, inputDirectoryPath));

        Stopwatch stopwatch = new();
        stopwatch.Start();

        Solution solution = solutionDefinition.Factory.Create(input);
        string? result = problem switch
        {
            SingleProblem.Problem1 => solution.Problem1(),
            SingleProblem.Problem2 => solution.Problem2(),
            _ => throw new ArgumentException(nameof(problem))
        };

        stopwatch.Stop();

        return new SolveResult(solutionDefinition.Type.Name, result, stopwatch.ElapsedMilliseconds);
    }

    private record SolveResult(string Name, string? Result, long Time);
}