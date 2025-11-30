namespace AdventOfCode.Framework.Console;

internal class ClearableRegion
{
    private readonly int startingLeft;
    private readonly int startingTop;

    private ClearableRegion()
    {
        (startingLeft, startingTop) = System.Console.GetCursorPosition();
    }

    internal static ClearableRegion Start()
    {
        return new ClearableRegion();
    }

    internal void Clear()
    {
        // Clear all text up to the starting line
        for (var t = System.Console.CursorTop; t > startingTop; t--)
        {
            System.Console.SetCursorPosition(0, t);
            System.Console.Write(new string(' ', System.Console.WindowWidth));
        }

        // Clear the starting line
        System.Console.SetCursorPosition(startingLeft, startingTop);
        System.Console.Write(new string(' ', System.Console.WindowWidth - startingLeft));
        System.Console.SetCursorPosition(startingLeft, startingTop);
    }
}