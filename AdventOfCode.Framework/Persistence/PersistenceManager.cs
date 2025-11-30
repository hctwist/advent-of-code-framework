using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Humanizer;

namespace AdventOfCode.Framework.Persistence;

internal static class PersistenceManager
{
    private static readonly string FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Advent of Code");
    private static readonly string LastRunPath = Path.Combine(FolderPath, "Last Run.json");

    public static string ProblemDataDirectory { get; } = Path.Combine(FolderPath, "Problem Data");

    internal static void WriteLastRunFile(LastRun lastRun)
    {
        File.WriteAllText(LastRunPath, JsonSerializer.Serialize(lastRun));
    }

    internal static bool TryReadLastRunFile([NotNullWhen(true)] out LastRun? lastRun)
    {
        if (!File.Exists(LastRunPath))
        {
            lastRun = default;
            return false;
        }

        lastRun = JsonSerializer.Deserialize<LastRun>(File.ReadAllText(LastRunPath))!;
        return true;
    }

    internal static bool TryReadProblemFile(
        int day,
        Problem problem,
        ProblemFile file,
        [NotNullWhen(true)]
        out string? contents)
    {
        var path = GetProblemFilePath(day, problem, file);

        if (!File.Exists(path))
        {
            contents = default;
            return false;
        }

        contents = File.ReadAllText(path);
        return true;
    }

    internal static bool TryReadOrPromptForProblemFile(
        int day,
        Problem problem,
        ProblemFile file,
        [NotNullWhen(true)]
        out string? contents)
    {
        var path = GetProblemFilePath(day, problem, file);

        if (File.Exists(path))
        {
            contents = File.ReadAllText(path);
            return true;
        }

        var temporaryPath = GetTemporaryProblemFilePath(file);

        var startingContents =
            $"""
             Place the <{file.Humanize(LetterCasing.LowerCase)}> to <day {day}> <{problem.Humanize(LetterCasing.LowerCase)}> here and close the window when finished.

             This will be saved for next time the problem is run.
             """;

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

    internal static void WriteProblemFile(
        int day,
        Problem problem,
        ProblemFile file,
        string contents)
    {
        File.WriteAllText(GetProblemFilePath(day, problem, file), contents);
    }

    private static string GetTemporaryProblemFilePath(ProblemFile file)
    {
        return Path.ChangeExtension(Path.Combine(Path.GetTempPath(), $"{file.Humanize(LetterCasing.Title)} {Guid.NewGuid()}"), "txt");
    }

    private static string GetProblemFilePath(int day, Problem problem, ProblemFile file)
    {
        return Path.ChangeExtension(Path.Combine(ProblemDataDirectory, $"Day {day}", problem.Humanize(), file.Humanize(LetterCasing.Title)), "txt");
    }
}