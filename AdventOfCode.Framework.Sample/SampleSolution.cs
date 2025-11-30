using AdventOfCode.Framework.Solutions;
using Spectre.Console;

namespace AdventOfCode.Framework.Sample;

[Solution(Day = 1)]
internal class SampleSolution : ISolution
{
    /// <inheritdoc />
    public string SolveProblem1(ProblemInput input, ISolutionLogger logger)
    {
        logger.Log("Starting problem");
        Thread.Sleep(2_000);
        var matrix = input.Matrix;
        logger.Log("Finished problem", Color.Green);
        return matrix.Count.ToString();
    }

    /// <inheritdoc />
    public string SolveProblem2(ProblemInput input, ISolutionLogger logger)
    {
        throw new NotImplementedException();
    }
}