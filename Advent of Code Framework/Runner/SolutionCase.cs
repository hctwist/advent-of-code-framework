namespace AdventOfCode.Framework.Runner;

public record SolutionCase(SolutionDefinition Definition, InputInformation Input);

public record SolutionDefinition(int Day, Type Type, SolutionFactory Factory);

public record InputInformation(string Path, bool Benchmark)
{
    public static string ResolvePath(string path, string? relativeTo)
    {
        return string.IsNullOrEmpty(relativeTo) ? path : System.IO.Path.Combine(relativeTo, path);
    }
}