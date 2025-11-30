using System.Diagnostics;
using AdventOfCode.Framework.Console;
using AdventOfCode.Framework.Persistence;
using AdventOfCode.Framework.Solutions;
using Humanizer;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace AdventOfCode.Framework.Solvers;

internal static class MultiSolver
{
    internal static void SolveAll()
    {
        var results = new List<TaggedProblemResult>();

        var clearableRegion = ClearableRegion.Start();

        AnsiConsole.Progress()
            .Columns(
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new ElapsedTimeColumn(),
                new SpinnerColumn())
            .Start(c =>
            {
                var tasks = new List<SolutionTask>();

                foreach (var entry in SolutionFinder.FindAll().OrderBy(e => e.Day))
                {
                    var task = c.AddTask($"Day {entry.Day}").MaxValue(2);
                    tasks.Add(new SolutionTask(task, entry));
                }

                foreach (var task in tasks)
                {
                    var mainProblem1Result = SolveProblem(task.SolutionEntry, Problem.Problem1, ProblemFile.MainInput, ProblemFile.MainOutput);
                    task.Progress.Value(1);

                    if (mainProblem1Result is not null)
                    {
                        results.Add(new TaggedProblemResult(task.SolutionEntry.Day, Problem.Problem1, mainProblem1Result));
                    }

                    var mainProblem2Result = SolveProblem(task.SolutionEntry, Problem.Problem2, ProblemFile.MainInput, ProblemFile.MainOutput);
                    task.Progress.Value(2);

                    if (mainProblem2Result is not null)
                    {
                        results.Add(new TaggedProblemResult(task.SolutionEntry.Day, Problem.Problem2, mainProblem2Result));
                    }
                }
            });

        clearableRegion.Clear();

        var correctColumnName = "Correct";
        var table = new Table().AddColumns("Day", "Problem", correctColumnName, "Elapsed time");

        foreach (var result in results)
        {
            AnsiConsole.WriteLine();
            table.AddRow(
                new Text(result.Day.ToString()),
                new Text(result.Problem.Humanize()),
                CreateResult(result.Result.Correct, correctColumnName.Length),
                new Text(result.Result.ElapsedTime.ToString()));
        }

        AnsiConsole.Write(table);

        System.Console.ReadKey();
    }

    private static ProblemResult? SolveProblem(
        SolutionEntry entry,
        Problem problem,
        ProblemFile inputFile,
        ProblemFile outputFile)
    {
        if (!PersistenceManager.TryReadProblemFile(entry.Day, problem, inputFile, out var input))
        {
            return null;
        }

        var stopwatch = Stopwatch.StartNew();
        var output = entry.Solve(problem, new ProblemInput(input), new NoOpSolutionLogger());
        var elapsedTime = stopwatch.Elapsed;

        if (output is null)
        {
            return null;
        }

        bool? correct;

        if (!PersistenceManager.TryReadProblemFile(entry.Day, problem, outputFile, out var expectedOutput))
        {
            correct = null;
        }
        else
        {
            correct = Verification.VerifyOutput(output, expectedOutput);
        }

        return new ProblemResult(elapsedTime, correct);
    }

    private static IRenderable CreateResult(bool? result, int width)
    {
        var canvas = new Canvas(width, 1);
        var color = result switch
        {
            true => Color.Green,
            false => Color.Red,
            null => Color.Grey
        };

        for (var i = 0; i < width; i++)
        {
            canvas.SetPixel(i, 0, color);
        }

        return canvas;
    }

    private record SolutionTask(ProgressTask Progress, SolutionEntry SolutionEntry);

    private record ProblemResult(TimeSpan ElapsedTime, bool? Correct);

    private record TaggedProblemResult(int Day, Problem Problem, ProblemResult Result);
}