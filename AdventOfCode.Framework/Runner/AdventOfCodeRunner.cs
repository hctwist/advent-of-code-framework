using System.Diagnostics;
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
        if (PersistenceManager.TryReadLastRunFile(out var lastRun))
        {
            runnerOptions.Add(RunnerOption.Rerun);
        }

        if (SolutionFinder.Entries.Count == 0)
        {
            AnsiConsole.MarkupLine($"No solutions found. Each solution must implement [fuchsia]{nameof(ISolution)}[/], be attributed with [fuchsia]{nameof(SolutionAttribute)}[/] and have a parameterless constructor");
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
            return;
        }

        runnerOptions.Add(RunnerOption.Solve);
        runnerOptions.Add(RunnerOption.SolveAll);
        runnerOptions.Add(RunnerOption.OpenProblemData);

        var runnerOption = AnsiConsole.Prompt(
            new SelectionPrompt<RunnerOption>()
                .UseConverter(o => o switch
                {
                    RunnerOption.Rerun => $"{o.Humanize()} (day {lastRun!.Day}, {lastRun.Problem.Humanize(LetterCasing.LowerCase)})",
                    _ => o.Humanize()
                })
                .AddChoices(runnerOptions));

        switch (runnerOption)
        {
            case RunnerOption.Rerun:
                Rerun();
                break;
            case RunnerOption.Solve:
                SingleSolver.Solve();
                System.Console.ReadKey();
                break;
            case RunnerOption.SolveAll:
                SolveAll();
                break;
            case RunnerOption.OpenProblemData:
                OpenProblemData();
                break;
        }
    }

    /// <summary>
    /// Reruns the last solution solved.
    /// </summary>
    public static void Rerun()
    {
        if (PersistenceManager.TryReadLastRunFile(out var lastRun))
        {
            Solve(lastRun.Day, lastRun.Problem);
        }
        else
        {
            AnsiConsole.WriteErrorLine("Could not find a previous run");
            System.Console.ReadKey();
        }
    }

    /// <summary>
    /// Solves a particular problem.
    /// </summary>
    /// <param name="day">The day to target.</param>
    /// <param name="problem">The problem to solve.</param>
    public static void Solve(int day, Problem problem)
    {
        SingleSolver.Solve(day, problem);
        System.Console.ReadKey();
    }

    /// <summary>
    /// Runs solutions for all available problems.
    /// </summary>
    public static void SolveAll()
    {
        MultiSolver.SolveAll();
        System.Console.ReadKey();
    }

    private static void OpenProblemData()
    {
        Directory.CreateDirectory(PersistenceManager.ProblemDataDirectory);
        Process.Start(
            new ProcessStartInfo()
            {
                FileName = PersistenceManager.ProblemDataDirectory,
                UseShellExecute = true
            });
    }
}