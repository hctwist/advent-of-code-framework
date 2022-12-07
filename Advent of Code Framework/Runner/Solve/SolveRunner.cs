using System.Diagnostics;
using AdventOfCode.Framework.Runner.Benchmark;

namespace AdventOfCode.Framework.Runner.Solve;

internal class SolveRunner
{
    private readonly Options options;

    public SolveRunner(Options options)
    {
        this.options = options;
    }

    public void Run(int day, Problem problem)
    {
        List<SolutionCase> solutionCases = SolutionCaseLoader.GetSolutionCases()
            .Where(c => c.Day == day)
            .Where(c => problem.Includes(c.Problem))
            .ToList();

        if (solutionCases.Count == 0)
        {
            Console.WriteLine($"Could not find any solutions for day {day}");
            return;
        }
        
        Run(solutionCases);
    }

    public void RunAll()
    {
        List<SolutionCase> solutionCases = SolutionCaseLoader.GetSolutionCases();
        
        if (solutionCases.Count == 0)
        {
            Console.WriteLine("Could not find any solutions");
            return;
        }

        Run(solutionCases);
    }

    public void RunLatest()
    {
        List<SolutionCase> solutionCases = SolutionCaseLoader.GetSolutionCases();
        
        if (solutionCases.Count == 0)
        {
            Console.WriteLine("Could not find any solutions");
            return;
        }

        int latestDay = solutionCases.Max(c => c.Day);
        List<SolutionCase> latestCases = solutionCases
            .Where(c => c.Day == latestDay)
            .ToList();

        Run(latestCases);
    }

    private void Run(List<SolutionCase> solutionCases)
    {
        foreach (IGrouping<int, SolutionCase> solutionCasesByDay in solutionCases.GroupBy(c => c.Day).OrderBy(c => c.Key))
        {
            Console.WriteLine($"~ Day {solutionCasesByDay.Key} ~");
            Console.WriteLine();
            foreach (IGrouping<string, SolutionCase> solutionCaseByInputPath in solutionCasesByDay.GroupBy(c => c.InputPath))
            {
                Console.WriteLine($"{solutionCaseByInputPath.Key}");
                Console.WriteLine("------");

                foreach (IGrouping<SingleProblem, SolutionCase> solutionCasesByProblem in solutionCaseByInputPath.GroupBy(c => c.Problem))
                {
                    Run(solutionCasesByProblem.ToArray(), solutionCasesByProblem.Key);
                }
            }
        }
    }

    private void Run(SolutionCase[] solutionCases, SingleProblem problem)
    {
        Console.WriteLine($"* {problem.ToDisplayString()} *");

        SolveResult[] results = solutionCases.Select(Solve).ToArray();

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

    private SolveResult Solve(SolutionCase solutionCase)
    {
        Input input = Input.FromFile(solutionCase.ResolveInputPath(options.InputBaseDirectory));

        Stopwatch stopwatch = new();
        stopwatch.Start();

        Solution solution = solutionCase.Factory.Create(input);
        string? result = solutionCase.Problem switch
        {
            SingleProblem.Problem1 => solution.Problem1(),
            SingleProblem.Problem2 => solution.Problem2(),
            _ => throw new ArgumentException()
        };

        stopwatch.Stop();

        return new SolveResult(solutionCase.Type.Name, result, stopwatch.ElapsedMilliseconds);
    }

    private record SolveResult(string Name, string? Result, long Time);
}