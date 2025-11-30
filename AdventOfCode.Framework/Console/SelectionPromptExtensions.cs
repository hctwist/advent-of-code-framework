using Humanizer;
using Spectre.Console;

namespace AdventOfCode.Framework.Console;

internal static class SelectionPromptExtensions
{
    extension<T>(SelectionPrompt<T> prompt) where T : struct, Enum
    {
        internal SelectionPrompt<T> UseHumanizerConverter()
        {
            return prompt.UseConverter(c => c.Humanize());
        }
    }
}