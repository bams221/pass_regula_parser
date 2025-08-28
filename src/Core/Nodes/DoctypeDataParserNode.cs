using PassRegulaParser.Core.Interfaces;
using PassRegulaParser.Core.Utils;
using PassRegulaParser.Models;

namespace PassRegulaParser.Core.Nodes;

public class DoctypeDataParserNode(string doctypeDataJsonFilepath) : INodeElement
{
    private readonly JsonFileParser _jsonParser = new(doctypeDataJsonFilepath);

    public PassportData Process(PassportData passportData)
    {
        PassportData newPassportData = passportData.Clone();
        string documentName = _jsonParser.GetPropertyString("OneCandidate.DocumentName");
        newPassportData.DocumentType = documentName;
        return newPassportData;
    }
}
