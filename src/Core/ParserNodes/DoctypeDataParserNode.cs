using PassRegulaParser.Core.Dto;
using PassRegulaParser.Core.Utils;

namespace PassRegulaParser.Core.ParserNodes;
class DoctypeDataParserNode : NodeElement
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
