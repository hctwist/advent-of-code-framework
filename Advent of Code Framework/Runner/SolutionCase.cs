namespace AdventOfCode.Framework.Runner;

internal record SolutionCase(
    int Day,
    SingleProblem Problem,
    Type Type,
    SolutionFactory Factory,
    string InputPath,
    bool Benchmark,
    string? ProblemSolution)
{
    public string ResolveInputPath(string? relativeTo)
    {
        return string.IsNullOrEmpty(relativeTo) ? InputPath : Path.Combine(relativeTo, InputPath);
    }
}