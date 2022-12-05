namespace AdventOfCode.Framework.Tests.Mocks;

[Solution(0)]
[SolutionInput("Mocks/MockInput.txt", Benchmark = true)]
public class MockSolution2 : Solution
{
    /// <inheritdoc />
    public MockSolution2(Input input) : base(input)
    {
    }

    /// <inheritdoc />
    protected override string? Problem1()
    {
        return "Problem 1 result";
    }

    /// <inheritdoc />
    protected override string? Problem2()
    {
        return "Problem 2 result";
    }
}