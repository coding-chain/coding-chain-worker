using System;

namespace Domain.TestExecution.Helpers
{
    public static class StringExtensions
    {
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            var pos = text.IndexOf(search, StringComparison.Ordinal);
            if (pos < 0) return text;
            return text[..pos] + replace + text[(pos + search.Length)..];
        }
    }
}