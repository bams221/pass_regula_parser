namespace PassRegulaParser.Core.Utils;

public class DateFormatter
{

    public static string FormatDateText(string text)
    {
        string textDigits = text.Replace(".", "");
        string formattedText = "";

        if (textDigits.Length > 0)
            formattedText += textDigits[..Math.Min(2, textDigits.Length)];
        if (textDigits.Length > 2)
            formattedText += string.Concat(".", textDigits.AsSpan(2, Math.Min(2, textDigits.Length - 2)));
        if (textDigits.Length > 4)
            formattedText += string.Concat(".", textDigits.AsSpan(4, Math.Min(4, textDigits.Length - 4)));

        return formattedText;
    }
}
