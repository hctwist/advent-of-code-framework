namespace AdventOfCode.Framework;

/// <summary>
/// Solution input.
/// </summary>
/// <param name="Raw">The raw input.</param>
/// <param name="Lines">The input split into lines.</param>
public record Input(string Raw, string[] Lines)
{
    internal static Input FromFile(string filePath)
    {
        string raw = File.ReadAllText(filePath);
        string[] lines = raw.Split(Environment.NewLine);

        return new Input(raw, lines);
    }
}