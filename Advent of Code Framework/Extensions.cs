namespace AdventOfCode.Framework;

internal static class Extensions
{
    public static string[] SplitLines(this string str)
    {
        return str.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);
    }
}