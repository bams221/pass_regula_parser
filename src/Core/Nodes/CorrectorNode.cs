using PassRegulaParser.Core.Interfaces;
using PassRegulaParser.Models;

namespace PassRegulaParser.Core.Nodes;

public class CorrectorNode() : INodeElement
{
    public PassportData Process(PassportData passportData)
    {
        PassportData newPassportData = passportData.Clone();
        newPassportData.Gender = CorrectGender(passportData.Gender);

        return newPassportData;
    }

    private static string CorrectGender(string? gender)
    {
        if (string.IsNullOrEmpty(gender))
            return string.Empty;

        return gender switch
        {
            "МУЖ" => "М",
            "МУЖ." => "М",
            "MУЖ" => "М",
            "M" => "М",
            "ЖЕН" => "Ж",
            "ЖЕН." => "Ж",
            "Ж" => "Ж",
            _ => string.Empty
        };
    }
}