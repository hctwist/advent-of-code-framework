namespace AdventOfCode.Framework.Tests.Mocks;

[Solution(0)]
[SolutionInput("Mocks/MockInput.txt", Benchmark = true)]
public class MockSolution : Solution
{
    /// <inheritdoc />
    public MockSolution(Input input) : base(input)
    {
    }

    /// <inheritdoc />
    protected override string? Problem1()
    {
        return "null";
    }

    /// <inheritdoc />
    protected override string? Problem2()
    {
        return null;
    }
}