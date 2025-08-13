namespace PassRegulaParser.Core.ParserNodes;

using System.Text.Json;
using PassRegulaParser.Core.Dto;
using PassRegulaParser.Core.Exceptions;

class DoctypeDataParserNode : NodeElement
{
    private readonly JsonDocument _jsonDoc;

    public DoctypeDataParserNode(string doctypeDataJsonFilepath)
    {
        CheckJsonFilePath(doctypeDataJsonFilepath);
        try
        {
            _jsonDoc = ParseJson(doctypeDataJsonFilepath);
        }
        catch (JsonException ex)
        {
            throw new ParsingException("Invalid JSON format", ex);
        }
        catch (Exception ex) when (ex is not ParsingException)
        {
            throw new ParsingException("Error parsing JSON file", ex);
        }
    }

    private static void CheckJsonFilePath(string filepath)
    {
        if (string.IsNullOrWhiteSpace(filepath))
        {
            throw new ParsingException("File path cannot be null or empty");
        }

        if (!File.Exists(filepath))
        {
            throw new ParsingException($"File not found: {filepath}");
        }
    }

    private static JsonDocument ParseJson(string filepath)
    {
        string jsonContent = File.ReadAllText(filepath);
        JsonDocument jsonDoc = JsonDocument.Parse(jsonContent);
        return jsonDoc;
    }

    private static string ExtractDocumentNameFromJsonDocument(JsonDocument jsonDoc)
    {
        try
        {
            var oneCandidate = jsonDoc.RootElement.GetProperty("OneCandidate");
            var docName = oneCandidate.GetProperty("DocumentName").GetString() ?? throw new ParsingException("DocumentName is null");
            return docName;
        }
        catch (KeyNotFoundException)
        {
            throw new ParsingException("Key not found in json");
        }
        
    }

    public PassportData Process(PassportData passportData)
    {
        string documentName = ExtractDocumentNameFromJsonDocument(_jsonDoc);
        passportData.DocumentType = documentName;
        return passportData;
    }
}