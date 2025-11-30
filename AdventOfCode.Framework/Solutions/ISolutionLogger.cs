namespace AdventOfCode.Framework.Solutions;

/// <summary>
/// Logs progress whilst solving.
/// </summary>
public interface ISolutionLogger
{
    /// <summary>
    /// Logs an info message.
    /// </summary>
    /// <param name="message">The message.</param>
    void Log(string message);

    /// <summary>
    /// Logs an error message.
    /// </summary>
    /// <param name="message">The message.</param>
    void LogError(string message);
}