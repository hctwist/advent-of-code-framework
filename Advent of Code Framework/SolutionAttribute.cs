namespace AdventOfCode.Framework;

/// <summary>
/// Metadata for solutions.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class SolutionAttribute : Attribute
{
    internal int Number { get; }
    
    internal string InputFilePath { get; }

    internal bool Enabled { get; }
    
    /// <summary>
    /// Creates a new <see cref="SolutionAttribute"/>.
    /// </summary>
    /// <param name="number">The solution number.</param>
    /// <param name="inputFilePath">The file path to the input.</param>
    /// <param name="enabled">Whether this solution is enabled.</param>
    public SolutionAttribute(int number, string inputFilePath, bool enabled = true)
    {
        Number = number;
        InputFilePath = inputFilePath;
        Enabled = enabled;
    }
}