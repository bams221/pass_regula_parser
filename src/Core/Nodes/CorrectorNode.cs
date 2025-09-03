using System.Text;
using System.Text.RegularExpressions;
using PassRegulaParser.Core.Interfaces;
using PassRegulaParser.Models;

namespace PassRegulaParser.Core.Nodes;

public class CorrectorNode() : INodeElement
{
    public PassportData Process(PassportData passportData)
    {
        PassportData newPassportData = passportData.Clone();
        newPassportData.Gender = CorrectGender(passportData.Gender);
        newPassportData.Authority = CorrectText(passportData.Authority);
        newPassportData.BirthCity = CorrectText(passportData.BirthCity);

        return newPassportData;
    }

    private static string CorrectGender(string? gender)
    {
        if (string.IsNullOrEmpty(gender))
            return string.Empty;

        return gender.First().ToString() switch
        {
            "М" => "М",
            "M" => "М",
            "Ж" => "Ж",
            "F" => "Ж",
            _ => string.Empty
        };
    }

    private static string CorrectText(string? text)
    {
        if (string.IsNullOrEmpty(text))
            return "";

        var allowedChars = new HashSet<char>(
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ" +
            ".,;:'\"-() "
        );

        var correctedText = new StringBuilder();

        foreach (char c in text)
        {
            if (allowedChars.Contains(c))
            {
                correctedText.Append(c);
            }
            else
            {
                correctedText.Append(' ');
            }
        }

        string result = Regex.Replace(correctedText.ToString(), @"\s+", " ");

        return result.Trim();
    }
}