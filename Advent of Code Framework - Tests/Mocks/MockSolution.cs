namespace AdventOfCode.Framework.Tests.Mocks;

[Solution(0)]
[SolutionInput("Mocks/MockInput.txt", Benchmark = true, Problem1Solution = "53455636")]
public class MockSolution : Solution
{
    /// <inheritdoc />
    public MockSolution(Input input) : base(input)
    {
    }

    /// <inheritdoc />
    protected override string? Problem1()
    {
        return "53455636";
    }

    /// <inheritdoc />
    protected override string? Problem2()
    {
        return null;
    }
}