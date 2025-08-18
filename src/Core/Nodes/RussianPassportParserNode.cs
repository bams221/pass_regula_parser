using System.Text.Json.Nodes;
using PassRegulaParser.Core.Exceptions;
using PassRegulaParser.Core.Interfaces;
using PassRegulaParser.Core.Utils;
using PassRegulaParser.Model;

namespace PassRegulaParser.Core.Nodes;

class RussianPassportParserNode(string doctypeDataJsonFilepath) : INodeElement
{
    private const string FieldListPath = "Text.fieldList";
    private readonly JsonFileParser _jsonParser = new(doctypeDataJsonFilepath);

    public PassportData Process(PassportData passportData)
    {
        try
        {
            JsonArray fieldList = (_jsonParser.GetNodeByPath(FieldListPath)?.AsArray()) ??
                throw new ParsingException($"Field list not found at path: {FieldListPath}");
            passportData.SerialNumber = GetValueFromJson(fieldList, "Document Number");
            passportData.BirthDate = GetValueFromJson(fieldList, "Date of Birth");
            passportData.FullName = GetValueFromJsonWithLang(fieldList, "Surname And Given Names");
            passportData.Gender = GetValueFromJsonWithLang(fieldList, "Sex");
            passportData.BirthCity = GetValueFromJsonWithLang(fieldList, "Place of Birth");

            return passportData;
        }
        catch (Exception ex)
        {
            throw new ParsingException("Error processing Russian passport data", ex);
        }
    }

    private static string GetValueFromJson(JsonArray fieldList, string fieldName)
    {
        return JsonUtils.FindValueByFieldName(
            fieldList: fieldList,
            fieldName: fieldName,
            valuePropertyName: "value",
            namePropertyName: "fieldName"
            );
    }
    private static string GetValueFromJsonWithLang(JsonArray fieldList, string fieldName)
    {
        return JsonUtils.FindValueByFieldName(
            fieldList: fieldList,
            fieldName: fieldName,
            valuePropertyName: "value",
            namePropertyName: "fieldName",
            languagePropertyName: "lcidName",
            language: "Russian"
            );
    }
}
