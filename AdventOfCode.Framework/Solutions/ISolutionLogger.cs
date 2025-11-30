using Spectre.Console;

namespace AdventOfCode.Framework.Solutions;

/// <summary>
/// Logs progress whilst solving.
/// </summary>
public interface ISolutionLogger
{
    /// <summary>
    /// Logs a message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="color">The log colour.</param>
    void Log(string message, Color? color = null);
}