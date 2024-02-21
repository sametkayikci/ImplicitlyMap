namespace ImplicitlyMap.Extensions;

internal static class StringExtensions
{
    public static string RemoveSuffixes(this string text, params string[] suffixes)
    {
        foreach (var suffix in suffixes)
        {
            if (!text.EndsWith(suffix, StringComparison.Ordinal)) continue;
            if (suffix.Length > 0 && suffix.Length <= text.Length) return text[..^suffix.Length];
        }

        return text;
    }
}