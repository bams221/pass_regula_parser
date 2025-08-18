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
            var fieldList = (_jsonParser.GetNodeByPath(FieldListPath)?.AsArray()) ??
                throw new ParsingException($"Field list not found at path: {FieldListPath}");
            passportData.SerialNumber = JsonUtils.FindFieldValueByFieldName(fieldList, "Document Number");
            passportData.BirthDate = JsonUtils.FindFieldValueByFieldName(fieldList, "Date of Birth");
            passportData.FullName = JsonUtils.FindFieldValueByFieldName(fieldList, "Surname And Given Names", "Russian");
            passportData.Gender = JsonUtils.FindFieldValueByFieldName(fieldList, "Sex", "Russian");
            passportData.BirthCity = JsonUtils.FindFieldValueByFieldName(fieldList, "Place of Birth", "Russian");

            return passportData;
        }
        catch (Exception ex)
        {
            throw new ParsingException("Error processing Russian passport data", ex);
        }
    }
}