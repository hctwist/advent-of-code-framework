using AdventOfCode.Framework.Solutions;
using Spectre.Console;

namespace AdventOfCode.Framework.Console;

internal class NoOpSolutionLogger : ISolutionLogger
{
    /// <inheritdoc />
    public void Log(string message, Color? color = null)
    {
    }
}