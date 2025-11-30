namespace AdventOfCode.Framework.Solvers;

internal static class AnswerVerification
{
    internal static bool Check(string actual, string expected)
    {
        return actual.Trim(Environment.NewLine.ToCharArray()) == expected.Trim(Environment.NewLine.ToCharArray());
    }
}