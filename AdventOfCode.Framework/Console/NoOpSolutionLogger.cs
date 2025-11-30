using AdventOfCode.Framework.Solutions;

namespace AdventOfCode.Framework.Console;

internal class NoOpSolutionLogger : ISolutionLogger
{
    /// <inheritdoc />
    public void Log(string message)
    {
    }

    /// <inheritdoc />
    public void LogError(string message)
    {
    }
}