using System.Text.Json;
using PassRegulaParser.Core.Exceptions;

namespace PassRegulaParser.Core.Utils;

public class JsonFileParser
{
    private readonly JsonDocument _jsonDoc;

    public JsonFileParser(string jsonFilepath)
    {
        CheckJsonFilePath(jsonFilepath);
        try
        {
            _jsonDoc = ParseJson(jsonFilepath);
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

    public JsonDocument JsonDocument => _jsonDoc;

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
        return JsonDocument.Parse(jsonContent);
    }

    public string GetStringProperty(string propertyPath)
    {
        try
        {
            JsonElement currentElement = _jsonDoc.RootElement;

            foreach (var property in propertyPath.Split('.'))
            {
                currentElement = currentElement.GetProperty(property);
            }

            return currentElement.GetString() ??
                   throw new ParsingException($"Property '{propertyPath}' is null");
        }
        catch (KeyNotFoundException)
        {
            throw new ParsingException($"Property path '{propertyPath}' not found in JSON");
        }
        catch (InvalidOperationException ex)
        {
            throw new ParsingException($"Invalid operation while accessing property '{propertyPath}'", ex);
        }
    }

    public void Dispose()
    {
        _jsonDoc?.Dispose();
    }
}