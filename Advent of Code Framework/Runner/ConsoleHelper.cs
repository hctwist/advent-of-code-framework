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

    public static void WriteTable(string[,] table, bool header)
    {
        const string VerticalSeparator = " | ";

        List<int> columnWidths = new();

        for (int col = 0; col < table.GetLength(1); col++)
        {
            int maxWidth = 0;
            for (int row = 0; row < table.GetLength(0); row++)
            {
                maxWidth = Math.Max(maxWidth, table[row, col].Length);
            }
            columnWidths.Add(maxWidth);
        }

        for (int row = 0; row < table.GetLength(0); row++)
        {
            for (int col = 0; col < table.GetLength(1); col++)
            {
                Console.Write(table[row, col].PadRight(columnWidths[col]));
                if (col < table.GetLength(1) - 1)
                {
                    Console.Write(VerticalSeparator);
                }
            }

            if (header && row == 0)
            {
                Console.WriteLine();
                for (int col = 0; col < table.GetLength(1); col++)
                {
                    Console.Write(new string('-', columnWidths[col]));
                    if (col < table.GetLength(1) - 1)
                    {
                        Console.Write(VerticalSeparator);
                    }
                }
            }

            if (row < table.GetLength(0) - 1)
            {
                Console.WriteLine();
            }
        }
    }
}