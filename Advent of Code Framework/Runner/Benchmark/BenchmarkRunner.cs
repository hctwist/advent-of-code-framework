using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Framework.Runner.Benchmark;

// TODO Debug warning
internal class BenchmarkRunner
{
    private readonly Options options;

    public BenchmarkRunner(Options options)
    {
        this.options = options;
    }

    public void Run(int day, Problem problem)
    {
        List<SolutionCase> cases = SolutionCaseLoader.GetSolutionCases()
            .Where(c => c.Day == day)
            .Where(c => c.Benchmark)
            .ToList();

        if (cases.Count == 0)
        {
            Console.WriteLine($"Could not find any solutions to benchmark for day {day}");
            return;
        }
        
        Run(cases, problem);
    }

    public void RunAll()
    {
        List<SolutionCase> cases = SolutionCaseLoader.GetSolutionCases()
            .Where(c => c.Benchmark)
            .ToList();
        
        if (cases.Count == 0)
        {
            Console.WriteLine($"Could not find any solutions to benchmark");
            return;
        }
        
        Run(cases, Problem.All);
    }

    private void Run(List<SolutionCase> solutionCases, Problem problem)
    {
        List<BenchmarkResult> results = new(solutionCases.Count * 2);
        List<SolutionCase> failedCases = new();

        foreach (SolutionCase solutionCase in solutionCases)
        {
            // TODO Progress indications
            if (problem.Includes(solutionCase.Problem))
            {
                if (TryBenchmark(solutionCase, out BenchmarkResult? result))
                {
                    results.Add(result);
                }
                else
                {
                    failedCases.Add(solutionCase);
                }
            }
        }

        List<BenchmarkResult> sortedResults = results
            .OrderBy(result => result.Case.Day)
            .ThenBy(result => result.Case.InputPath)
            .ThenBy(result => result.Case.Problem)
            .ThenBy(result => result.Case.Type.Name)
            .ThenBy(result => result.Ticks)
            .ToList();

        string[,] resultsTable = new string[sortedResults.Count + 1, 5];

        resultsTable[0, 0] = "Day";
        resultsTable[0, 1] = "Input";
        resultsTable[0, 2] = "Problem";
        resultsTable[0, 3] = "Solution";
        resultsTable[0, 4] = "Time (millis)";

        for (int i = 0; i < sortedResults.Count; i++)
        {
            BenchmarkResult result = sortedResults[i];
            resultsTable[i + 1, 0] = result.Case.Day.ToString();
            resultsTable[i + 1, 1] = result.Case.InputPath;
            resultsTable[i + 1, 2] = result.Case.Problem.ToDisplayString();
            resultsTable[i + 1, 3] = result.Case.Type.Name;
            resultsTable[i + 1, 4] = (result.Ticks / 10_000).ToString("N2");
        }

        Console.WriteLine();
        ConsoleHelper.WriteTable(resultsTable, true);

        // TODO Aggregate statistics

        if (failedCases.Count > 0)
        {
            Console.WriteLine();
            ConsoleHelper.WriteLine($"Failed to benchmark {failedCases.Count} solution cases:", ConsoleColor.Red);

            foreach (SolutionCase failedCase in failedCases)
            {
                ConsoleHelper.WriteLine($"{failedCase.Type.Name} ({failedCase.InputPath}, {failedCase.Problem.ToDisplayString()})", ConsoleColor.Red);
            }
        }
    }

    private bool TryBenchmark(SolutionCase solutionCase, [NotNullWhen(true)] out BenchmarkResult? result)
    {
        Input input = Input.FromFile(solutionCase.ResolveInputPath(options.InputBaseDirectory));

        Stopwatch stopwatch = new();
        stopwatch.Start();

        int warmupIterations = 0;
        for (int i = 0; i < options.MaxBenchmarkIterations; i++)
        {
            Solution solution = solutionCase.Factory.Create(input);

            switch (solutionCase.Problem)
            {
                case SingleProblem.Problem1:
                    solution.Problem1();
                    break;
                case SingleProblem.Problem2:
                    solution.Problem2();
                    break;
                default:
                    throw new ArgumentException();
            }
            
            if (stopwatch.ElapsedMilliseconds > options.MaxBenchmarkTimePerProblem.TotalMilliseconds)
            {
                break;
            }

            warmupIterations++;
        }

        if (warmupIterations == 0)
        {
            result = default;
            return false;
        }

        int iterations = 0;
        stopwatch.Restart();

        for (int i = 0; i < warmupIterations; i++)
        {
            Solution solution = solutionCase.Factory.Create(input);

            switch (solutionCase.Problem)
            {
                case SingleProblem.Problem1:
                    solution.Problem1();
                    break;
                case SingleProblem.Problem2:
                    solution.Problem2();
                    break;
                default:
                    throw new ArgumentException();
            }

            iterations++;
        }

        stopwatch.Stop();

        result = new BenchmarkResult(
            solutionCase,
            (double)stopwatch.ElapsedTicks / iterations);
        return true;
    }

    private record BenchmarkResult(SolutionCase Case, double Ticks);
}