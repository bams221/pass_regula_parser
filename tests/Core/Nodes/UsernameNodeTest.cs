using PassRegulaParser.Core.Nodes;
using PassRegulaParser.Models;

namespace PassRegulaParser.Tests.Core.Nodes;

public class UsernameNodeTests
{
    private readonly UsernameNode _usernameNode;

    public UsernameNodeTests()
    {
        _usernameNode = new UsernameNode();
    }

    [Fact]
    public void Process_ShouldReturnNewInstance()
    {
        var original = new PassportData {};

        var result = _usernameNode.Process(original);

        Assert.NotSame(original, result);
    }

    [Fact]
    public void Process_ShouldReturnUsername()
    {
        var passport = new PassportData {};

        var result = _usernameNode.Process(passport);

        Assert.Equal(Environment.UserName, result.Username);
    }

    [Fact]
    public void Process_ShouldPreserveOtherProperties()
    {
        var original = new PassportData
        {
            FullName = "Иванов Иван Иванович",
            Serial = "45 01",
            Number = "123456",
            BirthPlace = "Москва",
            Gender = "МУЖ"
        };

        var result = _usernameNode.Process(original);

        Assert.Equal(original.FullName, result.FullName);
        Assert.Equal(original.Serial, result.Serial);
        Assert.Equal(original.Number, result.Number);
        Assert.Equal(original.BirthPlace, result.BirthPlace);

        Assert.Equal(Environment.UserName, result.Username);
    }

    [Fact]
    public void Process_ShouldHandleAllNullProperties()
    {
        var passport = new PassportData
        {
            FullName = null,
            Serial = null,
            Number = null,
            BirthPlace = null,
            BirthDate = null,
            Gender = null,
            IssueDate = null,
            Authority = null,
            AuthorityCode = null,
            PhotoBase64 = null
        };

        var result = _usernameNode.Process(passport);

        Assert.Null(result.FullName);
        Assert.Null(result.Serial);
        Assert.Null(result.Number);
        Assert.Null(result.BirthPlace);
        Assert.Null(result.BirthDate);
        Assert.Null(result.Gender);
        Assert.Null(result.IssueDate);
        Assert.Null(result.Authority);
        Assert.Null(result.AuthorityCode);
        Assert.Null(result.PhotoBase64);
    }
}