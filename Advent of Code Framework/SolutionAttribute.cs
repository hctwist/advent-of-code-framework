namespace AdventOfCode.Framework;

/// <summary>
/// Metadata for solutions.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class SolutionAttribute : Attribute
{
    /// <summary>
    /// Gets or sets whether the solution is enabled.
    /// </summary>
    public bool Enabled { get; set; }
    
    internal int Day { get; }

    /// <summary>
    /// Creates a new <see cref="SolutionAttribute"/>.
    /// </summary>
    /// <param name="day">The day .</param>
    public SolutionAttribute(int day)
    {
        Day = day;
        Enabled = true;
    }
}