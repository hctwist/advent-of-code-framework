namespace AdventOfCode.Framework.Solvers;

internal static class Verification
{
    internal static bool VerifyOutput(string actual, string expected)
    {
        return actual.Trim(Environment.NewLine.ToCharArray()) == expected.Trim(Environment.NewLine.ToCharArray());
    }
}