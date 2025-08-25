using System.Globalization;

namespace PassRegulaParser.Core.Utils;

public static class FieldValidators
{
    public static bool Any(string text) => true;
    public static bool NotEmpty(string text) => !string.IsNullOrWhiteSpace(text);

    public static bool IsDate(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;
        return DateTime.TryParseExact(
            text,
            "dd.MM.yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _
        );
    }

    public static bool OnlyDigits(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;
        return text.All(char.IsDigit);
    }

    public static bool OnlyWords(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;
        return text.All(char.IsLetter);
    }
    
    public static bool OnlyWordsAndSpaces(string text)
    {
        return OnlyWords(text.Replace(" ", ""));
    }
}