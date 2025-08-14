using System.Text.Json;
using System.Text.Json.Nodes;
using PassRegulaParser.Core.Exceptions;

namespace PassRegulaParser.Core.Utils;

public class JsonFileParser : IDisposable
{
    private readonly JsonNode _rootNode;
    private readonly FileStream? _fileStream;

    public JsonFileParser(string jsonFilepath)
    {
        CheckJsonFilePath(jsonFilepath);
        try
        {
            _fileStream = File.OpenRead(jsonFilepath);
            _rootNode = JsonNode.Parse(_fileStream) ??
                        throw new ParsingException("JSON content is null");
        }
        catch (JsonException ex)
        {
            throw new ParsingException("Invalid JSON format", ex);
        }
        catch (Exception ex) when (ex is not ParsingException)
        {
            throw new ParsingException("Error parsing JSON file", ex);
        }
        finally
        {
            _fileStream?.Dispose();
        }
    }

    public JsonNode RootNode => _rootNode;

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

    public string GetStringProperty(string propertyPath)
    {
        try
        {
            JsonNode? node = GetNodeByPath(propertyPath);
            return node?.GetValue<string>() ??
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
    public JsonNode? GetNodeByPath(string propertyPath)
    {
        JsonNode? currentNode = _rootNode;
        foreach (var property in propertyPath.Split('.'))
        {
            currentNode = currentNode?[property];
            if (currentNode == null) return null;
        }
        return currentNode;
    }

    public void Dispose()
    {
        _fileStream?.Dispose();
        GC.SuppressFinalize(this);
    }
}