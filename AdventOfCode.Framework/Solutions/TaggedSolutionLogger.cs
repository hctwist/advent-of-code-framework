using Spectre.Console;

namespace AdventOfCode.Framework.Solutions;

internal class TaggedSolutionLogger : ISolutionLogger
{
    private readonly string tagMarkup;

    internal TaggedSolutionLogger(string tagMarkup)
    {
        this.tagMarkup = $"[grey]{tagMarkup}[/] ";
    }

    /// <inheritdoc />
    public void Log(string message)
    {
        AnsiConsole.MarkupLine($"{tagMarkup}{Markup.Escape(message)}");
    }

    /// <inheritdoc />
    public void LogError(string message)
    {
        AnsiConsole.MarkupLine($"{tagMarkup}[red]{Markup.Escape(message)}[/red]");
    }
}