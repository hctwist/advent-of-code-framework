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
        if (!File.Exists(filePath))
        {
            throw new Exception($"Input file {filePath} not found");
        }

        string raw = File.ReadAllText(filePath);
        return new Input(raw, raw.SplitLines());
    }
}