using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Humanizer;

namespace AdventOfCode.Framework.Persistence;

internal static class PersistenceManager
{
#if DEBUG
    private static readonly string FolderName = "Advent of Code Framework";
#else
    private static readonly string FolderName = "Advent of Code";
#endif

    private static readonly string FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), FolderName);
    private static readonly string LastRunPath = Path.Combine(FolderPath, "Last Run.json");

    public static string ProblemDataDirectory { get; } = Path.Combine(FolderPath, "Problem Data");

    internal static void WriteLastRunFile(LastRun lastRun)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(LastRunPath)!);
        File.WriteAllText(LastRunPath, JsonSerializer.Serialize(lastRun));
    }

    internal static bool TryReadLastRunFile([NotNullWhen(true)] out LastRun? lastRun)
    {
        if (!TryReadAllText(LastRunPath, out var lastRunContents))
        {
            lastRun = default;
            return false;
        }

        lastRun = JsonSerializer.Deserialize<LastRun>(lastRunContents)!;
        return true;
    }

    internal static bool TryReadProblemInputFile(
        int day,
        ProblemDataFlavour flavour,
        [NotNullWhen(true)]
        out string? contents)
    {
        var path = GetProblemInputFilePath(day, flavour);
        return TryReadAllText(path, out contents);
    }

    internal static bool TryReadProblemAnswerFile(
        int day,
        Problem problem,
        ProblemDataFlavour dataFlavour,
        [NotNullWhen(true)]
        out string? contents)
    {
        var path = GetProblemAnswerFilePath(day, problem, dataFlavour);
        return TryReadAllText(path, out contents);
    }

    internal static bool TryReadOrPromptForProblemInputFile(
        int day,
        ProblemDataFlavour flavour,
        [NotNullWhen(true)]
        out string? contents)
    {
        var path = GetProblemInputFilePath(day, flavour);

        if (TryReadAllText(path, out contents))
        {
            return true;
        }

        var temporaryPath = GetTemporaryProblemFilePath($"{flavour.Humanize(LetterCasing.Title)} Input");

        var prompt =
            $"""
             Place the <{flavour.Humanize(LetterCasing.LowerCase)}> <input> to <day {day}> here and close the window when finished.

             This will be saved for next time the problem is run.
             """;

        return TryPromptForProblemFile(path, temporaryPath, prompt, out contents);
    }

    internal static bool TryReadOrPromptForProblemAnswerFile(
        int day,
        Problem problem,
        ProblemDataFlavour flavour,
        [NotNullWhen(true)]
        out string? contents)
    {
        var path = GetProblemAnswerFilePath(day, problem, flavour);

        if (TryReadAllText(path, out contents))
        {
            return true;
        }

        var temporaryPath = GetTemporaryProblemFilePath($"{flavour.Humanize(LetterCasing.Title)} Answer");

        var prompt =
            $"""
             Place the <{flavour.Humanize(LetterCasing.LowerCase)}> <answer> to <day {day}> <{problem.Humanize(LetterCasing.LowerCase)}> here and close the window when finished.

             This will be saved for next time the problem is run.
             """;

        return TryPromptForProblemFile(path, temporaryPath, prompt, out contents);
    }

    internal static void WriteProblemInputFile(
        int day,
        ProblemDataFlavour flavour,
        string contents)
    {
        File.WriteAllText(GetProblemInputFilePath(day, flavour), contents);
    }

    internal static void WriteProblemAnswerFile(
        int day,
        Problem problem,
        ProblemDataFlavour flavour,
        string contents)
    {
        File.WriteAllText(GetProblemAnswerFilePath(day, problem, flavour), contents);
    }

    private static bool TryPromptForProblemFile(
        string path,
        string temporaryPath,
        string startingContents,
        [NotNullWhen(true)]
        out string? contents)
    {
        File.WriteAllText(temporaryPath, startingContents);

        var process = Process.Start(
            new ProcessStartInfo
            {
                FileName = "notepad.exe",
                Arguments = $"\"{temporaryPath}\""
            });

        process!.WaitForExit();

        contents = File.ReadAllText(temporaryPath);

        if (contents == startingContents)
        {
            File.Delete(temporaryPath);
            return false;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.Move(temporaryPath, path);
        return true;
    }

    private static string GetTemporaryProblemFilePath(string name)
    {
        return Path.ChangeExtension(Path.Combine(Path.GetTempPath(), $"{name} {Guid.NewGuid()}"), "txt");
    }

    private static string GetProblemInputFilePath(int day, ProblemDataFlavour dataFlavour)
    {
        return Path.ChangeExtension(Path.Combine(ProblemDataDirectory, $"Day {day}", $"{dataFlavour.Humanize(LetterCasing.Title)} Input"), "txt");
    }

    private static string GetProblemAnswerFilePath(int day, Problem problem, ProblemDataFlavour flavour)
    {
        return Path.ChangeExtension(Path.Combine(ProblemDataDirectory, $"Day {day}", $"{problem.Humanize()} {flavour.Humanize(LetterCasing.Title)} Answer"), "txt");
    }

    private static bool TryReadAllText(string path, [NotNullWhen(true)] out string? contents)
    {
        if (File.Exists(path))
        {
            contents = File.ReadAllText(path);
            return true;
        }

        contents = default;
        return false;
    }
}