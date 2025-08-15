using PassRegulaParser.Core.Exceptions;
using PassRegulaParser.Core.Interfaces;
using PassRegulaParser.Core.Utils;
using PassRegulaParser.Model;
using System.Text.Json.Nodes;

namespace PassRegulaParser.Core.Nodes;

class RussianPassportParserNode(string doctypeDataJsonFilepath) : INodeElement
{
    private const string FieldListPath = "Text.fieldList";
    private readonly JsonFileParser _jsonParser = new(doctypeDataJsonFilepath);

    public PassportData Process(PassportData passportData)
    {
        try
        {
            var fieldList = (_jsonParser.GetNodeByPath(FieldListPath)?.AsArray()) ??
                throw new ParsingException($"Field list not found at path: {FieldListPath}");
            passportData.SerialNumber = FindFieldValue(fieldList, "Document Number");
            passportData.BirthDate = FindFieldValue(fieldList, "Date of Birth");
            passportData.FullName = FindFieldValue(fieldList, "Surname And Given Names", "Russian");
            passportData.Gender = FindFieldValue(fieldList, "Sex", "Russian");
            passportData.BirthCity = FindFieldValue(fieldList, "Place of Birth", "Russian");

            return passportData;
        }
        catch (Exception ex)
        {
            throw new ParsingException("Error processing Russian passport data", ex);
        }
    }

    private static string FindFieldValue(JsonArray fieldList, string fieldName, string? language = null)
    {
        foreach (var field in fieldList)
        {
            if (field == null) continue;

            var fieldNode = field.AsObject();
            if (fieldNode["fieldName"]?.GetValue<string>() != fieldName)
                continue;

            if (language != null && fieldNode["lcidName"]?.GetValue<string>() != language)
                continue;

            return fieldNode["value"]?.GetValue<string>() ?? string.Empty;
        }
        return string.Empty;
    }
}