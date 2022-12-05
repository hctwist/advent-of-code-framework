using System.Reflection;

namespace AdventOfCode.Framework.Runner;

public class SolutionFactory
{
    private readonly ConstructorInfo constructor;

    public SolutionFactory(Type type)
    {
        constructor = type.GetConstructor(new[] { typeof(Input) }) ??
            throw new Exception($"Could not resolve a valid constructor for {type.Name} (required a single parameter of type {nameof(Input)}");
    }

    public Solution Create(Input input)
    {
        return (Solution)constructor.Invoke(new object[] { input });
    }
}