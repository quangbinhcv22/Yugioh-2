using System;
using System.Text.RegularExpressions;

namespace QBPlugins.TextCase
{
    public static class TextCaseConvert
    {
        public static string ToSnake(Enum @enum)
        {
            var input = @enum.ToString();
            if (string.IsNullOrEmpty(input)) return input;

            const string pattern = "([a-z0-9])([A-Z])";
            const string replacement = "$1_$2";

            return Regex.Replace(input, pattern, replacement, RegexOptions.Compiled).ToLower();
        }
    }
}