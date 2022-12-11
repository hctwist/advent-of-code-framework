namespace AdventOfCode.Framework.Runner.ConsoleHelpers;

internal class TableBuilder
{
    private const string VerticalSeparator = " | ";
    private const char HorizontalSeparator = '-';

    private readonly string[] headings;

    private readonly List<List<CellContents>> rows;

    private List<CellContents> currentRow;

    public TableBuilder(params string[] headings)
    {
        this.headings = headings;

        currentRow = new List<CellContents>();
        rows = new List<List<CellContents>>() { currentRow };
    }

    public TableBuilder NewRow()
    {
        if (currentRow.Count > 0)
        {
            currentRow = new List<CellContents>();
            rows.Add(currentRow);
        }
        return this;
    }
    
    public TableBuilder Cell(string text, ConsoleColor? color = null)
    {
        currentRow.Add(new CellContents(text.SplitLines(), color));
        return this;
    }

    public void WriteToConsole()
    {
        List<int> columnWidths = headings.Select(h => h.Length).ToList();

        foreach (List<CellContents> row in rows)
        {
            for (int i = 0; i < row.Count; i++)
            {
                int width = row[i].Lines.Max(l => l.Length);
                
                if (i < columnWidths.Count)
                {
                    columnWidths[i] = Math.Max(columnWidths[i], width);
                }
                else
                {
                    columnWidths.Add(width);
                }
            }
        }
        
        for (int i = 0; i < headings.Length; i++)
        {
            Console.Write(headings[i].PadRight(columnWidths[i]));
            if (i < columnWidths.Count - 1)
            {
                Console.Write(VerticalSeparator);
            }
        }

        if (headings.Length > 0)
        {
            Console.WriteLine();
            for (int i = 0; i < columnWidths.Count; i++)
            {
                Console.Write(new string(HorizontalSeparator, columnWidths[i]));
                if (i < columnWidths.Count - 1)
                {
                    Console.Write(VerticalSeparator);
                }
            }
            Console.WriteLine();
        }

        for (int r = 0; r < rows.Count; r++)
        {
            int rowLines = rows[r].Count == 0 ? 0 : rows[r].Max(row => row.Lines.Length);
            
            for (int l = 0; l < rowLines; l++)
            {
                for (int c = 0; c < rows[r].Count; c++)
                {
                    CellContents cell = rows[r][c];

                    if (l < cell.Lines.Length)
                    {
                        string cellText = cell.Lines[l].PadRight(columnWidths[c]);
                        if (cell.Color is ConsoleColor color)
                        {
                            ColoredConsole.Write(cellText, color);
                        }
                        else
                        {
                            Console.Write(cellText);
                        }
                    }
                    else
                    {
                        Console.Write(new string(' ', columnWidths[c]));
                    }
                    
                    if (c < rows[r].Count - 1)
                    {
                        Console.Write(VerticalSeparator);
                    }
                }

                if (l < rowLines - 1)
                {
                    Console.WriteLine();
                }
            }

            if (r < rows.Count - 1)
            {
                Console.WriteLine();
            }
        }
    }

    private record CellContents(string[] Lines, ConsoleColor? Color);
}