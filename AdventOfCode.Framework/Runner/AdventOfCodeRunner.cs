using AdventOfCode.Framework.Console;
using AdventOfCode.Framework.Persistence;
using AdventOfCode.Framework.Solutions;
using AdventOfCode.Framework.Solvers;
using Humanizer;
using Spectre.Console;

namespace AdventOfCode.Framework.Runner;

/// <summary>
/// Entry point for running Advent of Code solutions.
/// </summary>
public static class AdventOfCodeRunner
{
    /// <summary>
    /// Starts the runner interactively, with no predefined options.
    /// </summary>
    public static void Run()
    {
        AnsiConsole.WriteLine(Art.Random());
        AnsiConsole.WriteLine();

        var runnerOptions = new List<RunnerOption>();

        // Check if we have a previous run
        if (PersistenceManager.TryReadRunState(out var runState))
        {
            runnerOptions.Add(RunnerOption.Rerun);
        }

        var solutionEntries = SolutionFinder.FindAll().ToList();

        if (solutionEntries.Count == 0)
        {
            AnsiConsole.MarkupLine($"No solutions found. Each solution must implement [fuchsia]{nameof(ISolution)}[/] and be attributed with [fuchsia]{nameof(SolutionAttribute)}[/]");
            AnsiConsole.WriteLine();

            var panel = new Panel(
                    (
                        """
                        [[[fuchsia]Solution[/]([aqua]Day[/] = [aqua]1[/])]]
                        [blue]internal class[/] [fuchsia]ExampleSolution[/] : [fuchsia]ISolution[/]
                        {
                            ...
                        }
                        """))
                .Header("Example")
                .Padding(3, 1);
            AnsiConsole.Write(panel);

            System.Console.ReadKey();
            return;
        }

        runnerOptions.Add(RunnerOption.Solve);
        runnerOptions.Add(RunnerOption.SolveAll);

        var runnerOption = AnsiConsole.Prompt(
            new SelectionPrompt<RunnerOption>()
                .UseConverter(o => o switch
                {
                    RunnerOption.Rerun => $"{o.Humanize()} (day {runState!.Day}, {runState.Problem.Humanize(LetterCasing.LowerCase)})",
                    _ => o.Humanize()
                })
                .AddChoices(runnerOptions));

        if (runnerOption == RunnerOption.Rerun)
        {
            Rerun();
        }
        else if (runnerOption == RunnerOption.Solve)
        {
            var days = solutionEntries.OrderBy(e => e.Solved).ThenBy(e => e.Day).Select(e => e.Day);
            var day = AnsiConsole.Prompt(new SelectionPrompt<int>().AddChoices(days).EnableSearch().UseConverter(c => $"Day {c}"));

            var problem = AnsiConsole.Prompt(
                new SelectionPrompt<Problem>()
                    .UseHumanizerConverter()
                    .AddChoices(Enum.GetValues<Problem>()));

            PersistenceManager.WriteRunState(new RunState(day, problem));

            Solve(day, problem);
        }
        else if (runnerOption == RunnerOption.SolveAll)
        {
            SolveAll();
        }
    }

    /// <summary>
    /// Reruns the last solution solved.
    /// </summary>
    public static void Rerun()
    {
        if (!PersistenceManager.TryReadRunState(out var state))
        {
            AnsiConsole.WriteErrorLine("Could not find a previous run");
            return;
        }

        Solve(state.Day, state.Problem);
    }

    /// <summary>
    /// Solves a particular problem.
    /// </summary>
    /// <param name="day">The day to target.</param>
    /// <param name="problem">The problem to solve.</param>
    public static void Solve(int day, Problem problem)
    {
        SingleSolver.Solve(day, problem);
    }

    /// <summary>
    /// Runs solutions for all available problems.
    /// </summary>
    public static void SolveAll()
    {
        MultiSolver.SolveAll();
    }
}