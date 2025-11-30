using System.Reflection;
using AdventOfCode.Framework.Console;
using Spectre.Console;

namespace AdventOfCode.Framework.Solutions;

internal static class SolutionFinder
{
    internal static IReadOnlyList<SolutionEntry> Entries { get; }

    static SolutionFinder()
    {
        var entries = new List<SolutionEntry>();

        foreach (var solutionType in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()))
        {
            if (solutionType.IsInterface || solutionType.IsAbstract)
            {
                continue;
            }

            var solutionAttribute = solutionType.GetCustomAttribute<SolutionAttribute>();

            var implementsInterface = solutionType.IsAssignableTo(typeof(ISolution));

            if (!implementsInterface && solutionAttribute is null)
            {
                continue;
            }

            var hasParameterlessConstructor = solutionType.GetConstructor(Type.EmptyTypes) is not null;

            if (!implementsInterface)
            {
                AnsiConsole.WriteWarningLine($"Found solution {solutionType} that doesn't implement {nameof(ISolution)}. This solution will be ignored");
                continue;
            }

            if (solutionAttribute is null)
            {
                AnsiConsole.WriteWarningLine($"Found solution {solutionType} that isn't attributed with {nameof(SolutionAttribute)}. This solution will be ignored");
                continue;
            }

            if (!hasParameterlessConstructor)
            {
                AnsiConsole.WriteWarningLine($"Found solution {solutionType} that doesn't have a parameterless constructor. This solution will be ignored");
                continue;
            }

            if (!solutionAttribute.Enabled)
            {
                continue;
            }

            entries.Add(new SolutionEntry(solutionAttribute.Day, solutionAttribute.Solved, solutionType));
        }

        Entries = entries;
    }
}