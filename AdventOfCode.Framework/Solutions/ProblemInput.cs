using AdventOfCode.Framework.Tools;

namespace AdventOfCode.Framework.Solutions;

/// <summary>
/// Input for a particular Advent of Code problem.
/// </summary>
public class ProblemInput
{
    /// <summary>
    /// Gets the raw string input.
    /// </summary>
    public string Raw { get; }

    private readonly Lazy<IReadOnlyList<string>> lines;
    private readonly Lazy<ImmutableMatrix<char>> matrix;

    internal ProblemInput(string raw)
    {
        Raw = raw;
        lines = new Lazy<IReadOnlyList<string>>(() => raw.Split(["\n", "\r\n"], StringSplitOptions.None));
        matrix = new Lazy<ImmutableMatrix<char>>(BuildMatrix);
    }

    /// <summary>
    /// Gets the input as lines.
    /// </summary>
    public IReadOnlyList<string> Lines => lines.Value;

    /// <summary>
    /// Gets the input as a matrix.
    /// </summary>
    public ImmutableMatrix<char> Matrix => matrix.Value;

    private ImmutableMatrix<char> BuildMatrix()
    {
        if (Lines.Count == 0)
        {
            return new ImmutableMatrix<char>(new char[0, 0]);
        }

        var rowCount = Lines.Count;
        var columnCount = Lines[0].Length;

        var data = new char[rowCount, columnCount];

        for (var r = 0; r < Lines.Count; r++)
        {
            var line = Lines[r];

            if (line.Length != columnCount)
            {
                throw new InvalidOperationException("Input data does not have a fixed row length so a matrix cannot be built");
            }

            for (var c = 0; c < line.Length; c++)
            {
                data[r, c] = line[c];
            }
        }

        return new ImmutableMatrix<char>(data);
    }
}