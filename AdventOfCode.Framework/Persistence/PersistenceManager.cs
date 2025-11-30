using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Humanizer;

namespace AdventOfCode.Framework.Persistence;

internal static class PersistenceManager
{
    private static readonly string FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Advent of Code");
    private static readonly string RunStatePath = Path.Combine(FolderPath, "Run State.json");

    internal static void WriteRunState(RunState state)
    {
        var stateString = JsonSerializer.Serialize(state);
        File.WriteAllText(RunStatePath, stateString);
    }

    internal static bool TryReadRunState([NotNullWhen(true)] out RunState? state)
    {
        if (!File.Exists(RunStatePath))
        {
            state = default;
            return false;
        }

        var stateString = File.ReadAllText(RunStatePath);

        state = JsonSerializer.Deserialize<RunState>(stateString)!;
        return true;
    }

    internal static bool TryReadProblemFile(
        int day,
        Problem problem,
        ProblemFile file,
        [NotNullWhen(true)] out string? contents)
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

    internal static string ReadOrPromptForProblemFile(int day, Problem problem, ProblemFile file)
    {
        var path = GetProblemFilePath(day, problem, file);

        if (File.Exists(path))
        {
            var contents = File.ReadAllText(path);

            if (!string.IsNullOrWhiteSpace(contents))
            {
                return contents;
            }
        }
        else
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.Create(path).Dispose();
        }

        var process = Process.Start(
            new ProcessStartInfo
            {
                FileName = "notepad.exe",
                Arguments = $"\"{path}\""
            });

        process!.WaitForExit();

        return File.ReadAllText(path);
    }

    internal static void WriteProblemFile(
        int day,
        Problem problem,
        ProblemFile file,
        string contents)
    {
        File.WriteAllText(GetProblemFilePath(day, problem, file), contents);
    }

    private static string GetProblemFilePath(int day, Problem problem, ProblemFile file)
    {
        return Path.ChangeExtension(Path.Combine(FolderPath, "Problem Data", $"Day {day}", problem.Humanize(), file.Humanize()), "txt");
    }
}