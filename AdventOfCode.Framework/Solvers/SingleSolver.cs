using System.Diagnostics;
using AdventOfCode.Framework.Console;
using AdventOfCode.Framework.Persistence;
using AdventOfCode.Framework.Solutions;
using Humanizer;
using Spectre.Console;

namespace AdventOfCode.Framework.Solvers;

internal static class SingleSolver
{
    internal static void Solve(int day, Problem problem)
    {
        var table = new Table();
        table.AddColumns($"Day {day}", problem.Humanize());

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();

        var solutionEntries = SolutionFinder.FindAll()
            .Where(s => s.Day == day)
            .ToList();

        switch (solutionEntries.Count)
        {
            case 0:
                AnsiConsole.WriteErrorLine($"Could not find solution for day {day}");
                System.Console.ReadKey();
                return;
            case > 1:
                AnsiConsole.WriteErrorLine($"Found more than one solution type for day {day}:");
                AnsiConsole.WriteLine($"{string.Join(Environment.NewLine, solutionEntries.Select(e => e.Type))}");
                return;
        }

        var solutionEntry = solutionEntries.Single();

        var sampleInput = PersistenceManager.ReadOrPromptForProblemFile(day, problem, ProblemFile.SampleInput);
        var sampleOutput = PersistenceManager.ReadOrPromptForProblemFile(day, problem, ProblemFile.SampleOutput);
        var mainInput = PersistenceManager.ReadOrPromptForProblemFile(day, problem, ProblemFile.MainInput);
        var mainOutput = PersistenceManager.TryReadProblemFile(day, problem, ProblemFile.MainOutput, out var o) ? o : null;

        string? newMainOutput = null;

        AnsiConsole.Status()
            .Start(
                "-",
                c =>
                {
                    SolveProblem(solutionEntry, problem, "sample", c, sampleInput, sampleOutput);
                    newMainOutput = SolveProblem(solutionEntry, problem, "main", c, mainInput, mainOutput);
                });

        var choices = new List<ExitChoice>();

        if (newMainOutput is not null)
        {
            choices.Add(ExitChoice.SaveSolution);
        }

        choices.Add(ExitChoice.Exit);

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<ExitChoice>()
                .AddChoices(choices)
                .UseHumanizerConverter());

        switch (choice)
        {
            case ExitChoice.SaveSolution:
                PersistenceManager.WriteProblemFile(day, problem, ProblemFile.MainOutput, newMainOutput!);
                break;
            case ExitChoice.Exit:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static string? SolveProblem(
        SolutionEntry entry,
        Problem problem,
        string tag,
        StatusContext context,
        string input,
        string? expectedOutput)
    {
        context.Status($"Solving {tag} problem");

        var logger = new TaggedSolutionLogger(tag);
        var stopwatch = Stopwatch.StartNew();
        var output = entry.Solve(problem, new ProblemInput(input), logger);

        if (output is null)
        {
            logger.Log("No solution");
            return null;
        }

        var elapsedTime = stopwatch.Elapsed;
        logger.Log($"Finished in {elapsedTime}");

        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine(output);
        AnsiConsole.WriteLine();

        if (expectedOutput is not null)
        {
            var correct = Verification.VerifyOutput(output, expectedOutput);

            AnsiConsole.MarkupLine(correct ? "[green]Solution is correct[/]" : "[red]Solution is incorrect[/]");
            AnsiConsole.WriteLine();
        }

        return output;
    }

    private enum ExitChoice
    {
        SaveSolution,
        Exit
    }
}