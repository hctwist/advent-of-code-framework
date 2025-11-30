using System.Reflection;

namespace AdventOfCode.Framework.Solutions;

internal static class SolutionFinder
{
    internal static IEnumerable<SolutionEntry> FindAll()
    {
        foreach (var solutionType in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()))
        {
            if (!solutionType.IsAssignableTo(typeof(ISolution)))
            {
                continue;
            }

            var solutionAttribute = solutionType.GetCustomAttribute<SolutionAttribute>();

            if (solutionAttribute is null)
            {
                continue;
            }

            if (!solutionAttribute.Enabled)
            {
                continue;
            }

            yield return new SolutionEntry(solutionAttribute.Day, solutionAttribute.Solved, solutionType);
        }
    }
}