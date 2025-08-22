using PassRegulaParser.Models;
using PassRegulaParser.Core.Nodes;
using PassRegulaParser.Core.Exceptions;

namespace PassRegulaParser.Tests.Core.Nodes;

public class RussianPassportParserNodeTests
{
    private readonly string textDataFilepath = Path.Combine("C:\\Users\\idenisovav\\Data\\Projects\\PassRegulaParser\\tests\\testingData\\TextData.json");
    [Fact]
    public void Process_ShouldSetDocumentTypeFromJson()
    {
        var passportData = new PassportData();
        var node = new RussianPassportParserNode(textDataFilepath);

        var result = node.Process(passportData);

        Assert.Equal("ИВАНОВ ИВАН ИВАНОВИЧ", result.FullName);
        Assert.Equal("2043467632", result.SerialNumber);
        Assert.Equal("ГОР. ЕКАТЕРИНБУРГ", result.BirthCity);
        Assert.Equal("04.06.1993", result.BirthDate);
        Assert.Equal("МУЖ", result.Gender);
        Assert.Equal("27.04.2013", result.IssueDate);
        Assert.Equal("МВД РФ", result.Authority);
        Assert.Equal("660123", result.AuthorityCode);
    }

    [Fact]
    public void Process_ShouldReturnSamePassportDataInstance()
    {
        var passportData = new PassportData();
        var node = new RussianPassportParserNode(textDataFilepath);

        var result = node.Process(passportData);

        Assert.Same(passportData, result);
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
