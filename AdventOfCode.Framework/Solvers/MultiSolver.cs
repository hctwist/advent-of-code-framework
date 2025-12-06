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

        AnsiConsole.Progress()
            .Columns(
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new ElapsedTimeColumn(),
                new SpinnerColumn())
            .Start(c =>
            {
                var tasks = new List<SolutionTask>();

                foreach (var entry in SolutionFinder.Entries.OrderBy(e => e.Day))
                {
                    var task = c.AddTask($"Day {entry.Day}", false).MaxValue(2);
                    tasks.Add(new SolutionTask(task, entry));
                }

                foreach (var task in tasks)
                {
                    task.ProgressTask.StartTask();

                    var problem1Result = SolveProblem(task.SolutionEntry, Problem.Problem1);
                    task.ProgressTask.Value(1);

                    if (problem1Result is not null)
                    {
                        results.Add(new TaggedProblemResult(task.SolutionEntry.Day, Problem.Problem1, problem1Result));
                    }

                    var problem2Result = SolveProblem(task.SolutionEntry, Problem.Problem2);
                    task.ProgressTask.Value(2);

                    if (problem2Result is not null)
                    {
                        results.Add(new TaggedProblemResult(task.SolutionEntry.Day, Problem.Problem2, problem2Result));
                    }

                    task.ProgressTask.StopTask();
                }
            });

        if (results.Count == 0)
        {
            AnsiConsole.WriteErrorLine("No solutions could be run. Before a solution can be included here it has to be run solo via 'Solve' mode");
            return;
        }

        var table = new Table().AddColumns("Day", "Problem", "Result", "Elapsed time");

        foreach (var result in results)
        {
            table.AddRow(
                new Text(result.Day.ToString()),
                new Text(result.Problem.Humanize()),
                CreateResultRenderable(result.Result.Correct),
                new Text(result.Result.ElapsedTime.ToString()));
        }

        AnsiConsole.Write(table);
    }

    private static ProblemResult? SolveProblem(SolutionEntry entry, Problem problem)
    {
        if (!PersistenceManager.TryReadProblemInputFile(entry.Day, ProblemDataFlavour.Puzzle, out var input))
        {
            return null;
        }

        var stopwatch = Stopwatch.StartNew();
        var answer = entry.Solve(problem, new ProblemInput(input), new NoOpSolutionLogger());
        var elapsedTime = stopwatch.Elapsed;

        if (answer is null)
        {
            return null;
        }

        bool? correct;

        if (!PersistenceManager.TryReadProblemAnswerFile(entry.Day, problem, ProblemDataFlavour.Puzzle, out var expectedAnswer))
        {
            correct = null;
        }
        else
        {
            correct = AnswerVerification.Check(answer, expectedAnswer);
        }

        return new ProblemResult(elapsedTime, correct);
    }

    private static IRenderable CreateResultRenderable(bool? result)
    {
        return result switch
        {
            true => new Text("  Correct  ", new Style(background: Color.Green)),
            false => new Text(" Incorrect ", new Style(background: Color.Red)),
            null => new Text("  Unknown  ", new Style(background: Color.Grey))
        };
    }

    private record SolutionTask(ProgressTask ProgressTask, SolutionEntry SolutionEntry);

    private record ProblemResult(TimeSpan ElapsedTime, bool? Correct);

    private record TaggedProblemResult(int Day, Problem Problem, ProblemResult Result);
}