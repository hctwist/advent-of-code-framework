namespace AdventOfCode.Framework.Solutions;

/// <summary>
/// Input for a particular Advent of Code problem.
/// </summary>
public class ProblemInput
{
    /// <summary>
    /// The raw string input.
    /// </summary>
    public string Raw { get; }

    /// <summary>
    /// The input lines.
    /// </summary>
    public IReadOnlyList<string> Lines { get; }

    internal ProblemInput(string raw)
    {
        Raw = raw;
        Lines = raw.Split(["\n", "\r\n"], StringSplitOptions.None);
    }
}