using PassRegulaParser.Core.Nodes;
using PassRegulaParser.Models;

namespace PassRegulaParser.Tests.Core.Nodes;

public class CorrectorNodeTests
{
    private readonly CorrectorNode _correctorNode;

    public CorrectorNodeTests()
    {
        _correctorNode = new CorrectorNode();
    }

    [Fact]
    public void Process_ShouldReturnNewInstance()
    {
        var original = new PassportData { Gender = "МУЖ" };

        var result = _correctorNode.Process(original);

        Assert.NotSame(original, result);
    }

    [Theory]
    [InlineData("МУЖ", "М")]
    [InlineData("МУЖ.", "М")]
    [InlineData("MУЖ", "М")]
    [InlineData("M", "М")]
    [InlineData("ЖЕН", "Ж")]
    [InlineData("ЖЕН.", "Ж")]
    [InlineData("Ж", "Ж")]
    [InlineData("", "")]
    [InlineData("UNKNOWN", "")]
    public void Process_ShouldCorrectGender(string inputGender, string expectedGender)
    {
        var passport = new PassportData { Gender = inputGender };

        var result = _correctorNode.Process(passport);

        Assert.Equal(expectedGender, result.Gender);
    }

    [Fact]
    public void Process_ShouldPreserveOtherProperties()
    {
        var original = new PassportData
        {
            FullName = "Иванов Иван Иванович",
            Serial = "45 01",
            Number = "123456",
            BirthCity = "Москва",
            Gender = "МУЖ"
        };

        var result = _correctorNode.Process(original);

        Assert.Equal(original.FullName, result.FullName);
        Assert.Equal(original.Serial, result.Serial);
        Assert.Equal(original.Number, result.Number);
        Assert.Equal(original.BirthCity, result.BirthCity);

        Assert.Equal("М", result.Gender);
    }

    [Fact]
    public void Process_ShouldHandleAllNullProperties()
    {
        var passport = new PassportData
        {
            FullName = null,
            Serial = null,
            Number = null,
            BirthCity = null,
            BirthDate = null,
            Gender = null,
            IssueDate = null,
            Authority = null,
            AuthorityCode = null,
            PhotoBase64 = null
        };

        var result = _correctorNode.Process(passport);

        Assert.Null(result.FullName);
        Assert.Null(result.Serial);
        Assert.Null(result.Number);
        Assert.Null(result.BirthCity);
        Assert.Null(result.BirthDate);
        Assert.Equal("", result.Gender);
        Assert.Null(result.IssueDate);
        Assert.Null(result.Authority);
        Assert.Null(result.AuthorityCode);
        Assert.Null(result.PhotoBase64);
    }
}