namespace AdventOfCode.Framework.Solutions;

/// <summary>
/// A solution to a day's Advent of Code puzzle. Solutions implementing this should also be attributed with <see cref="SolutionAttribute"/>.
/// </summary>
public interface ISolution
{
    /// <summary>
    /// Solves the first problem of the day.
    /// </summary>
    /// <param name="input">The problem input.</param>
    /// <param name="logger">A logger to log solution progress. This should be used in preference to <see cref="Console"/> as it supports the interactive style that the solutions are run within.</param>
    /// <returns>The problem solution, or null if no solution is to be provided.</returns>
    string? SolveProblem1(ProblemInput input, ISolutionLogger logger);

    /// <summary>
    /// Solves the second problem of the day.
    /// </summary>
    /// <param name="input">The problem input.</param>
    /// <param name="logger">A logger to log solution progress. This should be used in preference to <see cref="Console"/> as it supports the interactive style that the solutions are run within.</param>
    /// <returns>The problem solution, or null if no solution is to be provided.</returns>
    string? SolveProblem2(ProblemInput input, ISolutionLogger logger);
}