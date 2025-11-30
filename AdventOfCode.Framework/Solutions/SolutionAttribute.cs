namespace AdventOfCode.Framework.Solutions;

/// <summary>
/// Marks a class as an Advent of Code puzzle solution. Solutions attributed with this should implement <see cref="ISolution"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class SolutionAttribute : Attribute
{
    /// <summary>
    /// The day the solution is for.
    /// </summary>
    public required int Day { get; set; }

    /// <summary>
    /// Whether the solution should be detected.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Whether the puzzle can be considered solved with this solution.
    /// </summary>
    public bool Solved { get; set; } = false;
}