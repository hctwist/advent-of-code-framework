using System.Reflection;

namespace AdventOfCode.Framework.Runner;

internal static class SolutionCaseLoader
{
    public static List<SolutionCase> GetSolutionCases(int? day)
    {
        IEnumerable<Type> solutionTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(Solution)));

        List<SolutionCase> cases = new();

        foreach (Type solutionType in solutionTypes)
        {
            SolutionMetadata solutionMetadata = GetSolutionMetadata(solutionType);

            if ((day is not null && solutionMetadata.SolutionAttribute.Day != day) ||
                !solutionMetadata.SolutionAttribute.Enabled)
            {
                continue;
            }

            SolutionFactory factory = new(solutionType);

            foreach (SolutionInputAttribute inputAttribute in solutionMetadata.SolutionInputAttributes)
            {
                InputInformation inputInformation = new(inputAttribute.Path, inputAttribute.Benchmark);
                cases.Add(
                    new SolutionCase(
                        new SolutionDefinition(solutionMetadata.SolutionAttribute.Day, solutionType, factory),
                        inputInformation));
            }
        }

        return cases;
    }

    private static SolutionMetadata GetSolutionMetadata(Type type)
    {
        SolutionAttribute solutionAttribute = type.GetCustomAttribute<SolutionAttribute>() ?? throw new Exception($"Could not find solution attribute on {type.Name}");

        SolutionInputAttribute[] solutionInputAttributes = type.GetCustomAttributes<SolutionInputAttribute>().ToArray();
        if (solutionInputAttributes.Length == 0)
        {
            throw new Exception($"Could not find a solution input attribute on {type.Name}");
        }

        return new SolutionMetadata(solutionAttribute, solutionInputAttributes);
    }

    private record SolutionMetadata(SolutionAttribute SolutionAttribute, SolutionInputAttribute[] SolutionInputAttributes);
}