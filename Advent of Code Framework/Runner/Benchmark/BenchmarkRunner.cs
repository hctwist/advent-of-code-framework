using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using AdventOfCode.Framework.Runner.ConsoleHelpers;

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

        TableBuilder resultsBuilder = new("Day", "Input", "Problem", "Solution", "Time (millis)");

        foreach (BenchmarkResult result in sortedResults)
        {
            resultsBuilder.Cell(result.Case.Day.ToString())
                .Cell(result.Case.InputPath)
                .Cell(result.Case.Problem.ToDisplayString())
                .Cell(result.Case.Type.Name)
                .Cell((result.Ticks / 10_000).ToString("N2"))
                .NewRow();
        }

        resultsBuilder.WriteToConsole();

        // TODO Aggregate statistics

        if (failedCases.Count > 0)
        {
            Console.WriteLine();
            ColoredConsole.WriteLine($"Failed to benchmark {failedCases.Count} solution cases:", ConsoleColor.Red);

            foreach (SolutionCase failedCase in failedCases)
            {
                ColoredConsole.WriteLine($"{failedCase.Type.Name} ({failedCase.InputPath}, {failedCase.Problem.ToDisplayString()})", ConsoleColor.Red);
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