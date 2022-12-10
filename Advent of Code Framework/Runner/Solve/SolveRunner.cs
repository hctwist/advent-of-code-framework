using System.Diagnostics;
using AdventOfCode.Framework.Runner.ConsoleHelpers;

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
        List<SolveResult> results = solutionCases
            .Select(Solve)
            .OrderBy(result => result.Case.Day)
            .ThenBy(result => result.Case.InputPath)
            .ThenBy(result => result.Case.Problem)
            .ThenBy(result => result.Case.Type.Name)
            .ToList();

        HashSet<SolveResult> inconsistentResults = new();
        foreach (IGrouping<(int Day, string InputPath, SingleProblem Problem), SolveResult> grouping in results.GroupBy(result => (result.Case.Day, result.Case.InputPath, result.Case.Problem)))
        {
            if (grouping.Where(result => result.Result is not null).Distinct().Count() > 1)
            {
                foreach (SolveResult result in grouping)
                {
                    inconsistentResults.Add(result);
                }
            }
        }

        TableBuilder resultsBuilder = new("Day", "Input", "Problem", "Solution", "Time (approx.)", "Result");

        foreach (SolveResult result in results)
        {
            resultsBuilder.Cell(result.Case.Day.ToString())
                .Cell(result.Case.InputPath)
                .Cell(result.Case.Problem.ToDisplayString())
                .Cell(result.Case.Type.Name)
                .Cell($"{result.Time}ms")
                .Cell(result.Result ?? "-", GetColorForResult(result.Result, result.Case.ProblemSolution, inconsistentResults.Contains(result)))
                .NewRow();
        }

        resultsBuilder.WriteToConsole();
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

        return new SolveResult(solutionCase, result, stopwatch.ElapsedMilliseconds);
    }

    private static ConsoleColor? GetColorForResult(string? result, string? solution, bool inconsistent)
    {
        if (result is null)
        {
            return null;
        }
        
        if (solution is null)
        {
            return inconsistent ? ConsoleColor.Yellow : null;
        }

        return result == solution ? ConsoleColor.Green : ConsoleColor.Red;
    }

    private record SolveResult(SolutionCase Case, string? Result, long Time);
}