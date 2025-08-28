using PassRegulaParser.Models;
using PassRegulaParser.Core.Nodes;
using PassRegulaParser.Core.Exceptions;

namespace PassRegulaParser.Tests.Core.Nodes;

public class DoctypeDataParserNodeTests
{
    private readonly string jsonFilePath = "C:\\Users\\idenisovav\\Data\\Projects\\PassRegulaParser\\tests\\testingData\\ChoosenDoctype_Data.json";
    [Fact]
    public void Process_ShouldSetDocumentTypeFromJson()
    {
        var passportData = new PassportData();
        var node = new DoctypeDataParserNode(jsonFilePath);

        var result = node.Process(passportData);

        Assert.Equal("Russian Federation - Passport (2018)", result.DocumentType);
    }

    [Fact]
    public void Process_ShouldReturnNewPassportDataInstance()
    {
        var passportData = new PassportData();
        var node = new DoctypeDataParserNode(jsonFilePath);

        var result = node.Process(passportData);

        Assert.NotSame(passportData, result);
    }

    [Fact]
    public void Process_ShouldNotThrowIfJsonFileNotFound()
    {
        var notExistingJsonFilePath = "not_existing.json";
        var passportData = new PassportData();

        Assert.Throws<ParsingException>(() => new DoctypeDataParserNode(notExistingJsonFilePath));
        Assert.Null(passportData.DocumentType);
    }
}
