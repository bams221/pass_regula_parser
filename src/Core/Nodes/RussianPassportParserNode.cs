using PassRegulaParser.Core.Interfaces;
using PassRegulaParser.Core.Utils;
using PassRegulaParser.Models;

namespace PassRegulaParser.Core.Nodes;

class RussianPassportParserNode(string doctypeDataJsonFilepath) : INodeElement
{
    private const string FieldListPath = "Text.fieldList";
    private readonly JsonFileParser _jsonParser = new(doctypeDataJsonFilepath);

    public PassportData Process(PassportData passportData)
    {
        FieldList fieldList = new(
            fieldArray: _jsonParser.GetPropertyArray(FieldListPath),
            valuePropertyName: "value",
            namePropertyName: "fieldName",
            languagePropertyName: "lcidName",
            language: "Russian"
        );

        passportData.SerialNumber = fieldList.GetValue("Document Number");
        passportData.BirthDate = fieldList.GetValue("Date of Birth");
        passportData.FullName = fieldList.GetValueRussian("Surname And Given Names");
        passportData.Gender = fieldList.GetValueRussian("Sex");
        passportData.BirthCity = fieldList.GetValueRussian("Place of Birth");

        passportData.Authority = fieldList.GetValueRussian("Authority");
        passportData.AuthorityCode = fieldList.GetValue("Authority Code");
        passportData.IssueDate = fieldList.GetValue("Date of Issue");
        return passportData;
    }
}
