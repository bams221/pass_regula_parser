using PassRegulaParser.Core.Interfaces;
using PassRegulaParser.Core.Utils;
using PassRegulaParser.Models;

namespace PassRegulaParser.Core.Nodes;

public class RussianPassportParserNode(string doctypeDataJsonFilepath) : INodeElement
{
    private const string FieldListPath = "Text.fieldList";
    private readonly JsonFileParser _jsonParser = new(doctypeDataJsonFilepath);

    public PassportData Process(PassportData passportData)
    {
        PassportData newPassportData = passportData.Clone();
        FieldList fieldList = new(
            fieldArray: _jsonParser.GetPropertyArray(FieldListPath),
            valuePropertyName: "value",
            namePropertyName: "fieldName",
            languagePropertyName: "lcidName",
            language: "Russian"
        );

        newPassportData.Serial = fieldList.GetValue("Document Series");
        newPassportData.Number = fieldList.GetValue("Booklet Number");
        newPassportData.BirthDate = fieldList.GetValue("Date of Birth");
        newPassportData.FullName = fieldList.GetValueRussian("Surname And Given Names");
        newPassportData.Gender = fieldList.GetValueRussian("Sex");
        string birthCity = fieldList.GetValueRussian("Place of Birth");
        newPassportData.BirthCity = StringUtils.ReplaceSpecialChars(birthCity);

        string authority = fieldList.GetValueRussian("Authority");
        newPassportData.Authority = StringUtils.ReplaceSpecialChars(authority);
        newPassportData.AuthorityCode = fieldList.GetValue("Authority Code");
        newPassportData.IssueDate = fieldList.GetValue("Date of Issue");
        return newPassportData;
    }
}
