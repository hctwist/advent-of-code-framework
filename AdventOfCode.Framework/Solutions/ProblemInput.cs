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
    private readonly Lazy<IReadOnlyMatrix<char>> matrix;

    internal ProblemInput(string raw)
    {
        Raw = raw;
        lines = new Lazy<IReadOnlyList<string>>(() => raw.Split(["\n", "\r\n"], StringSplitOptions.None));
        matrix = new Lazy<IReadOnlyMatrix<char>>(() => BuildMatrix(Lines));
    }

    /// <summary>
    /// Gets the input as lines.
    /// </summary>
    public IReadOnlyList<string> Lines => lines.Value;

    /// <summary>
    /// Gets the input as a matrix.
    /// </summary>
    public IReadOnlyMatrix<char> Matrix => matrix.Value;

    private static Matrix<char> BuildMatrix(IReadOnlyList<string> lines)
    {
        var rowCount = lines.Count;
        var columnCount = lines[0].Length;

        var matrix = new Matrix<char>(rowCount, columnCount, default);

        for (var r = 0; r < lines.Count; r++)
        {
            var line = lines[r];

            if (line.Length != columnCount)
            {
                throw new InvalidOperationException("Input data does not have a fixed row length so a matrix cannot be built");
            }

            for (var c = 0; c < line.Length; c++)
            {
                matrix.Set(r, c, line[c]);
            }
        }

        return matrix;
    }
}