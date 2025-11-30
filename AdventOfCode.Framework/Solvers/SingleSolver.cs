using System.Diagnostics;
using AdventOfCode.Framework.Console;
using AdventOfCode.Framework.Persistence;
using AdventOfCode.Framework.Solutions;
using Humanizer;
using Spectre.Console;

namespace AdventOfCode.Framework.Solvers;

internal static class SingleSolver
{
    internal static void Solve()
    {
        var days = SolutionFinder.Entries.OrderBy(e => e.Solved).ThenBy(e => e.Day).Select(e => e.Day);
        var day = AnsiConsole.Prompt(new SelectionPrompt<int>().AddChoices(days).EnableSearch().UseConverter(c => $"Day {c}"));

        var problem = AnsiConsole.Prompt(
            new SelectionPrompt<Problem>()
                .UseHumanizerConverter()
                .AddChoices(Enum.GetValues<Problem>()));

        PersistenceManager.WriteLastRunFile(new LastRun(day, problem));

        Solve(day, problem);
    }

    internal static void Solve(int day, Problem problem)
    {
        var table = new Table();
        table.AddColumns($"Day {day}", problem.Humanize());

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();

        var solutionEntries = SolutionFinder.Entries
            .Where(s => s.Day == day)
            .ToList();

        switch (solutionEntries.Count)
        {
            case 0:
                AnsiConsole.WriteErrorLine($"Could not find solution for day {day}");
                return;
            case > 1:
                AnsiConsole.WriteErrorLine($"Found more than one solution type for day {day}:");
                AnsiConsole.WriteLine($"{string.Join(Environment.NewLine, solutionEntries.Select(e => e.Type))}");
                return;
        }

        var solutionEntry = solutionEntries.Single();

        if (!PersistenceManager.TryReadOrPromptForProblemFile(day, problem, ProblemFile.ExampleInput, out var exampleInput))
        {
            AnsiConsole.WriteErrorLine("No example input was written to the file");
            return;
        }

        if (!PersistenceManager.TryReadOrPromptForProblemFile(day, problem, ProblemFile.ExampleAnswer, out var exampleAnswer))
        {
            AnsiConsole.WriteErrorLine("No example answer was written to the file");
            return;
        }

        if (!PersistenceManager.TryReadOrPromptForProblemFile(day, problem, ProblemFile.PuzzleInput, out var puzzleInput))
        {
            AnsiConsole.WriteErrorLine("No puzzle input was written to the file");
            return;
        }

        var puzzleAnswer = PersistenceManager.TryReadProblemFile(day, problem, ProblemFile.PuzzleAnswer, out var o) ? o : null;

        string? newPuzzleAnswer = null;

        AnsiConsole.Status()
            .Start(
                "-",
                c =>
                {
                    SolveProblem(solutionEntry, problem, "example", c, exampleInput, exampleAnswer);
                    newPuzzleAnswer = SolveProblem(solutionEntry, problem, "puzzle", c, puzzleInput, puzzleAnswer);
                });

        var choices = new List<ExitChoice>();

        if (newPuzzleAnswer is not null)
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
                PersistenceManager.WriteProblemFile(day, problem, ProblemFile.PuzzleAnswer, newPuzzleAnswer!);
                break;
            case ExitChoice.Exit:
                Environment.Exit(0);
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
        string? expectedAnswer)
    {
        context.Status($"Solving {tag} problem");

        var logger = new TaggedSolutionLogger(tag);
        var stopwatch = Stopwatch.StartNew();
        var answer = entry.Solve(problem, new ProblemInput(input), logger);

        if (answer is null)
        {
            logger.Log("No solution");
            return null;
        }

        var elapsedTime = stopwatch.Elapsed;
        logger.Log($"Finished in {elapsedTime}");

        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine(answer);
        AnsiConsole.WriteLine();

        if (expectedAnswer is not null)
        {
            var correct = AnswerVerification.Check(answer, expectedAnswer);

            if (correct)
            {
                logger.Log("Answer is correct", Color.Green);
            }
            else
            {
                logger.Log("Answer is incorrect", Color.Red);
            }

            AnsiConsole.WriteLine();
        }

        return answer;
    }

    private enum ExitChoice
    {
        SaveSolution,
        Exit
    }
}