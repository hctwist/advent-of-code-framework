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
        currentRow.Add(new CellContents(text, color));
        return this;
    }

    public void WriteToConsole()
    {
        List<int> columnWidths = headings.Select(h => h.Length).ToList();

        foreach (List<CellContents> row in rows)
        {
            for (int i = 0; i < row.Count; i++)
            {
                if (i < columnWidths.Count)
                {
                    columnWidths[i] = Math.Max(columnWidths[i], row[i].Text.Length);
                }
                else
                {
                    columnWidths.Add(row[i].Text.Length);
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

        for (int i = 0; i < rows.Count; i++)
        {
            for (int j = 0; j < rows[i].Count; j++)
            {
                CellContents cell = rows[i][j];
                string cellText = cell.Text.PadRight(columnWidths[j]);
                if (cell.Color is ConsoleColor c)
                {
                    ColoredConsole.Write(cell.Text, c);
                }
                else
                {
                    Console.Write(cellText);
                }

                if (j < rows[i].Count - 1)
                {
                    Console.Write(VerticalSeparator);
                }
            }

            if (i < rows.Count)
            {
                Console.WriteLine();
            }
        }
    }

    private record CellContents(string Text, ConsoleColor? Color);
}