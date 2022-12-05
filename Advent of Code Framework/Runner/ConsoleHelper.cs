namespace AdventOfCode.Framework.Runner;

internal static class ConsoleHelper
{
    public static void WriteLine(string line, ConsoleColor color)
    {
        ConsoleColor oldColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(line);
        Console.ForegroundColor = oldColor;
    }
}