namespace PassRegulaParser.Core.Utils;

public static class StringUtils
{
    public static string ReplaceSpecialChars(string inputString)
    {
        if (string.IsNullOrEmpty(inputString))
            return inputString;

        var result = inputString.Replace("\n", " ").Replace("\t", " ");

        while (result.Contains("  "))
        {
            result = result.Replace("  ", " ");
        }

        return result.Trim();
    }
}
