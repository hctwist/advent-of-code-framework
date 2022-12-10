using System.Reflection;

namespace AdventOfCode.Framework.Runner;

internal static class SolutionCaseLoader
{
    public static List<SolutionCase> GetSolutionCases()
    {
        IEnumerable<Type> solutionTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(Solution)));

        List<SolutionCase> cases = new();

        foreach (Type solutionType in solutionTypes)
        {
            SolutionMetadata solutionMetadata = GetSolutionMetadata(solutionType);

            if (!solutionMetadata.SolutionAttribute.Enabled)
            {
                continue;
            }

            SolutionFactory factory = new(solutionType);

            foreach (SolutionInputAttribute inputAttribute in solutionMetadata.SolutionInputAttributes)
            {
                if (!inputAttribute.Enabled)
                {
                    continue;
                }
                
                if (inputAttribute.Problem.Includes(SingleProblem.Problem1))
                {
                    cases.Add(new SolutionCase(
                        solutionMetadata.SolutionAttribute.Day,
                        SingleProblem.Problem1,
                        solutionType,
                        factory,
                        inputAttribute.Path,
                        inputAttribute.Benchmark,
                        inputAttribute.Problem1Solution));
                }
                
                if (inputAttribute.Problem.Includes(SingleProblem.Problem2))
                {
                    cases.Add(new SolutionCase(
                        solutionMetadata.SolutionAttribute.Day,
                        SingleProblem.Problem2,
                        solutionType,
                        factory,
                        inputAttribute.Path,
                        inputAttribute.Benchmark,
                        inputAttribute.Problem2Solution));
                }
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