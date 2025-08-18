using PassRegulaParser.Core.Interfaces;
using PassRegulaParser.Core.Utils;
using PassRegulaParser.Model;

namespace PassRegulaParser.Core.Nodes;

class RussianPassportSecondPageParserNode : INodeElement
{
    private const string fieldListPath = "DocVisualExtendedInfo.pArrayFields";
    private const string visualDataFilename = "Visual_OCR_Data.json";
    readonly string _secondPageFolderPath;
    readonly bool _isFolderExists;

    public RussianPassportSecondPageParserNode(string secondPageFolderPath)
    {
        _secondPageFolderPath = secondPageFolderPath;
        _isFolderExists = FolderChecker.IsFolderExists(_secondPageFolderPath);
    }

    public PassportData Process(PassportData passportData)
    {
        if (!_isFolderExists)
        {
            Console.WriteLine($"Folder {_secondPageFolderPath} does not exists. Skipping");
            return passportData;
        }

        string visualDataFilepath = Path.Combine(_secondPageFolderPath, visualDataFilename);

        JsonFileParser _jsonParser = new(visualDataFilepath);
        FieldList fieldList = new(
            fieldArray: _jsonParser.GetNodeByPath(fieldListPath).AsArray(),
            valuePropertyName: "Buf_Text",
            namePropertyName: "FieldName"
            );

        passportData.IssueDate = fieldList.GetValue("Date of Issue");
        passportData.Authority = fieldList.GetValue("Authority");
        passportData.AuthorityCode = fieldList.GetValue("Authority Code");

        return passportData;
    }

}