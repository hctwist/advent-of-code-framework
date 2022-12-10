namespace AdventOfCode.Framework.Runner.ConsoleHelpers;

internal static class ColoredConsole
{
    public static void Write(string line, ConsoleColor color)
    {
        ConsoleColor oldColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(line);
        Console.ForegroundColor = oldColor;
    }
    
    public static void WriteLine(string line, ConsoleColor color)
    {
        Write(line, color);
        Console.WriteLine();
    }
}