using Spectre.Console;

namespace AdventOfCode.Framework.Console;

internal static class AnsiConsoleExtensions
{
    extension(AnsiConsole)
    {
        internal static void WriteErrorLine(string message)
        {
            AnsiConsole.WriteLine(new Markup(message, new Style(foreground: Color.Red)));
        }

        internal static void WriteWarningLine(string message)
        {
            AnsiConsole.WriteLine(new Markup(message, new Style(foreground: Color.Yellow)));
        }

        internal static void WriteLine(Markup markup)
        {
            AnsiConsole.Write(markup);
            AnsiConsole.WriteLine();
        }
    }
}