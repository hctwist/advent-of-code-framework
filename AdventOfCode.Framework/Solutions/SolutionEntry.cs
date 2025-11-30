namespace AdventOfCode.Framework.Solutions;

internal class SolutionEntry
{
    internal int Day { get; }

    internal bool Solved { get; }

    internal Type Type { get; }

    private readonly Lazy<ISolution> solution;

    internal SolutionEntry(int day, bool solved, Type type)
    {
        Day = day;
        Solved = solved;
        Type = type;

        solution = new Lazy<ISolution>(() => (ISolution)Activator.CreateInstance(Type)!);
    }

    internal string? Solve(Problem problem, ProblemInput input, ISolutionLogger logger)
    {
        return problem switch
        {
            Problem.Problem1 => solution.Value.SolveProblem1(input, logger),
            Problem.Problem2 => solution.Value.SolveProblem2(input, logger),
            _ => throw new ArgumentOutOfRangeException(nameof(problem), problem, null)
        };
    }
}