namespace AdventOfCode.Framework;

/// <summary>
/// A solution.
/// </summary>
public abstract class Solution
{
    /// <summary>
    /// Gets the solution input.
    /// </summary>
    protected Input Input { get; }

    /// <summary>
    /// Creates a new <see cref="Solution"/>.
    /// </summary>
    /// <param name="input">The solution input.</param>
    protected Solution(Input input)
    {
        Input = input;
    }

    /// <summary>
    /// Runs the solution to problem 1.
    /// </summary>
    /// <returns>The result. If this is null then this won't be considered as an attempt.</returns>
    protected internal abstract string? Problem1();
    
    /// <summary>
    /// Runs the solution to problem 2.
    /// </summary>
    /// <returns>The result. If this is null then this won't be considered as an attempt.</returns>
    protected internal abstract string? Problem2();
}