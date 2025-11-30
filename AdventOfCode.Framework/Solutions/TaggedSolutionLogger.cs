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
    public void Log(string message, Color? color = null)
    {
        var colorMarkup = color?.ToMarkup();

        AnsiConsole.MarkupLine(
            colorMarkup is not null ?
                $"{tagMarkup}[{colorMarkup}]{Markup.Escape(message)}[/]" :
                $"{tagMarkup}{Markup.Escape(message)}");
    }
}