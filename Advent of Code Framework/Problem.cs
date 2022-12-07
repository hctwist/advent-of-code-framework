namespace AdventOfCode.Framework;

/// <summary>
/// The AOC problems.
/// </summary>
public enum Problem
{
    All, Problem1, Problem2
}

internal enum SingleProblem
{
    Problem1, Problem2
}

internal static class ProblemHelpers
{
    public static Problem Parse(int? problem)
    {
        return problem switch
        {
            1 => Problem.Problem1,
            2 => Problem.Problem2,
            _ => Problem.All
        };
    }

    public static bool Includes(this Problem problem, SingleProblem singleProblem)
    {
        return problem switch
        {
            Problem.All => true,
            Problem.Problem1 => singleProblem == SingleProblem.Problem1,
            Problem.Problem2 => singleProblem == SingleProblem.Problem2,
            _ => throw new ArgumentException(nameof(problem))
        };
    }

    public static string ToDisplayString(this SingleProblem problem)
    {
        return problem switch
        {
            SingleProblem.Problem1 => "Problem 1",
            SingleProblem.Problem2 => "Problem 2",
            _ => throw new ArgumentException(nameof(problem))
        };
    }
}