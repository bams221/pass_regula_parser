using PassRegulaParser.Core.Interfaces;
using PassRegulaParser.Core.Utils;
using PassRegulaParser.Model;

namespace PassRegulaParser.Core.Nodes;

class DoctypeDataParserNode : INodeElement
{
    private readonly JsonFileParser _jsonParser;

    public DoctypeDataParserNode(string doctypeDataJsonFilepath)
    {
        _jsonParser = new JsonFileParser(doctypeDataJsonFilepath);
    }

    public PassportData Process(PassportData passportData)
    {
        string documentName = _jsonParser.GetStringProperty("OneCandidate.DocumentName");
        passportData.DocumentType = documentName;
        return passportData;
    }
}
