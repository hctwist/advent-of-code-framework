namespace AdventOfCode.Framework;

/// <summary>
/// A solution.
/// </summary>
public abstract class Solution
{
    /// <summary>
    /// Gets the solution input.
    /// </summary>
    protected Input Input { get; private set; } = null!;

    internal void Initialise(string inputFilePath)
    {
        if (!File.Exists(inputFilePath))
        {
            throw new Exception($"Could not find input file {inputFilePath}");
        }

        Input = Input.FromFile(inputFilePath);
    }
    
    /// <summary>
    /// Runs the solution to problem 1.
    /// </summary>
    /// <returns>The result.</returns>
    protected internal abstract string Problem1();
    
    /// <summary>
    /// Runs the solution to problem 2.
    /// </summary>
    /// <returns>The result.</returns>
    protected internal abstract string Problem2();
}