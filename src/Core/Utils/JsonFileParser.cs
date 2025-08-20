using System.Text.Json;
using System.Text.Json.Nodes;
using PassRegulaParser.Core.Exceptions;

namespace PassRegulaParser.Core.Utils;

public class JsonFileParser : IDisposable
{
    private readonly JsonNode _rootNode;
    private FileStream? _fileStream;
    private readonly int _retryDelayMilliseconds;
    private const int MaxRetryAttempts = 3;

    public JsonFileParser(string jsonFilepath, int retryDelayMs = 2000)
    {
        _retryDelayMilliseconds = retryDelayMs;
        CheckJsonFilePath(jsonFilepath);
        _rootNode = TryReadFile(jsonFilepath);
    }

    public JsonNode RootNode => _rootNode;

    private JsonNode TryReadFile(string filePath)
    {
        int attempt = 0;
        while (true)
        {
            try
            {
                _fileStream = File.OpenRead(filePath);
                return JsonNode.Parse(_fileStream) ??
                       throw new ParsingException("JSON content is null");
            }
            catch (FileNotFoundException) when (attempt < MaxRetryAttempts - 1)
            {
                attempt++;
                Console.WriteLine($"File {filePath} not found. Attempt: {attempt}. Retrying...");
                Thread.Sleep(_retryDelayMilliseconds);
                continue;
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
    }

    private static void CheckJsonFilePath(string filepath)
    {
        if (string.IsNullOrWhiteSpace(filepath))
        {
            throw new ParsingException("File path cannot be null or empty");
        }
    }

    public string GetPropertyString(string propertyPath)
    {
        try
        {
            JsonNode? node = GetNodeByPath(propertyPath);
            return node?.GetValue<string>() ??
                   throw new ParsingException($"Property '{propertyPath}' is null");
        }
        catch (InvalidOperationException ex)
        {
            throw new ParsingException($"Invalid operation while accessing property '{propertyPath}'", ex);
        }
    }

    public JsonArray GetPropertyArray(string propertyPath)
    {
        try
        {
            return GetNodeByPath(propertyPath).AsArray();

        }
        catch (InvalidOperationException ex)
        {
            throw new ParsingException("Invalid operation: " + ex.Message);
        }
    }

    private JsonNode GetNodeByPath(string propertyPath)
    {
        JsonNode currentNode = _rootNode;
        foreach (var property in propertyPath.Split('.'))
        {
            currentNode = currentNode[property]
                ?? throw new ParsingException($"Property is not found at path: {propertyPath}");
        }
        return currentNode;
    }

    public void Dispose()
    {
        _fileStream?.Dispose();
        GC.SuppressFinalize(this);
    }
}