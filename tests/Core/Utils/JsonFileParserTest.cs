using PassRegulaParser.Core.Exceptions;
using PassRegulaParser.Core.Utils;

namespace PassRegulaParser.Tests.Core.Utils;

public class JsonFileParserTests : IDisposable
{
    private const string TestJson = @"
        {
            ""user"": {
                ""name"": ""John Doe"",
                ""age"": 30,
                ""address"": {
                    ""street"": ""123 Main St"",
                    ""city"": ""Anytown""
                },
                ""isActive"": true,
                ""tags"": [""tag1"", ""tag2""]
            },
            ""emptyValue"": null
        }";

    private readonly string _tempFilePath;

    public JsonFileParserTests()
    {
        _tempFilePath = Path.GetTempFileName();
        File.WriteAllText(_tempFilePath, TestJson);
    }

    public void Dispose()
    {
        if (File.Exists(_tempFilePath))
        {
            File.Delete(_tempFilePath);
        }
    }

    [Fact]
    public void Constructor_WithValidFilePath_InitializesSuccessfully()
    {
        var exception = Record.Exception(() => new JsonFileParser(_tempFilePath));
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WithEmptyFilePath_ThrowsParsingException()
    {
        Assert.Throws<ParsingException>(() => new JsonFileParser(""));
    }

    [Fact]
    public void Constructor_WithNonExistentFile_ThrowsParsingException()
    {
        var nonExistentPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Assert.Throws<ParsingException>(() => new JsonFileParser(nonExistentPath, 1));
    }

    [Fact]
    public void Constructor_WithInvalidJson_ThrowsParsingException()
    {
        var invalidJsonPath = Path.GetTempFileName();
        File.WriteAllText(invalidJsonPath, "{ invalid json }");

        try
        {
            Assert.Throws<ParsingException>(() => new JsonFileParser(invalidJsonPath));
        }
        finally
        {
            File.Delete(invalidJsonPath);
        }
    }


    [Fact]
    public void Constructor_WithFileThatAppearsAfterRetry_Succeeds()
    {
        var delayedFilePath = Path.Combine(Path.GetTempPath(), $"delayed_{Guid.NewGuid()}.json");

        try
        {
            Task.Run(async () =>
            {
                await Task.Delay(200);
                File.WriteAllText(delayedFilePath, TestJson);
            });

            var exception = Record.Exception(() => new JsonFileParser(delayedFilePath));
            Assert.Null(exception);
        }
        finally
        {
            if (File.Exists(delayedFilePath))
            {
                File.Delete(delayedFilePath);
            }
        }
    }

    [Fact]
    public void Constructor_WithFileThatNeverAppears_ThrowsParsingException()
    {
        var nonExistentPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        var ex = Assert.Throws<ParsingException>(() => new JsonFileParser(nonExistentPath, 50));
    }


    [Fact]
    public void GetStringProperty_WithValidPath_ReturnsCorrectValue()
    {
        var parser = new JsonFileParser(_tempFilePath);
        var result = parser.GetStringProperty("user.name");
        Assert.Equal("John Doe", result);
    }

    [Fact]
    public void GetStringProperty_WithNestedPath_ReturnsCorrectValue()
    {
        var parser = new JsonFileParser(_tempFilePath);
        var result = parser.GetStringProperty("user.address.street");
        Assert.Equal("123 Main St", result);
    }

    [Fact]
    public void GetStringProperty_WithNonExistentPath_ThrowsParsingException()
    {
        var parser = new JsonFileParser(_tempFilePath);
        Assert.Throws<ParsingException>(() => parser.GetStringProperty("user.nonexistent.property"));
    }

    [Fact]
    public void GetStringProperty_WithNullValue_ThrowsParsingException()
    {
        var parser = new JsonFileParser(_tempFilePath);
        Assert.Throws<ParsingException>(() => parser.GetStringProperty("emptyValue"));
    }

    [Fact]
    public void GetStringProperty_WithNonStringValue_ThrowsParsingException()
    {
        var parser = new JsonFileParser(_tempFilePath);
        Assert.Throws<ParsingException>(() => parser.GetStringProperty("user.age"));
    }

    [Fact]
    public void GetNodeByPath_WithValidPath_ReturnsCorrectNode()
    {
        var parser = new JsonFileParser(_tempFilePath);
        var node = parser.GetNodeByPath("user.address");
        Assert.NotNull(node);
        Assert.Equal("123 Main St", node["street"]?.GetValue<string>());
    }

    [Fact]
    public void GetNodeByPath_WithNonExistentPath_ReturnsNull()
    {
        var parser = new JsonFileParser(_tempFilePath);
        Assert.Throws<ParsingException>(() => parser.GetNodeByPath("user.nonexistent.property"));
    }

    [Fact]
    public void Dispose_CanBeCalledMultipleTimes()
    {
        var parser = new JsonFileParser(_tempFilePath);
        parser.Dispose();
        var exception = Record.Exception(() => parser.Dispose());
        Assert.Null(exception);
    }
}