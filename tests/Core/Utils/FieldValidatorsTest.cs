using PassRegulaParser.Core.Utils;

namespace PassRegulaParser.Tests.Core.Utils;

public class FieldValidatorsTests
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Any_AlwaysReturnsTrue(string text)
    {
        Assert.True(FieldValidators.Any(text));
    }

    [Theory]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("text", true)]
    public void NotEmpty_ReturnsExpectedResult(string text, bool expected)
    {
        Assert.Equal(expected, FieldValidators.NotEmpty(text));
    }

    [Theory]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("01.01.2020", true)]
    [InlineData("31.12.2024", true)]
    [InlineData("32.01.2020", false)]
    [InlineData("01.13.2020", false)]
    [InlineData("01.01.20", false)]
    [InlineData("not a date", false)]
    public void IsDate_ReturnsExpectedResult(string text, bool expected)
    {
        Assert.Equal(expected, FieldValidators.IsDate(text));
    }

    [Theory]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("123", true)]
    [InlineData("123a", false)]
    [InlineData("123 ", false)]
    public void OnlyDigits_ReturnsExpectedResult(string text, bool expected)
    {
        Assert.Equal(expected, FieldValidators.OnlyDigits(text));
    }

    [Theory]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("abc", true)]
    [InlineData("abc1", false)]
    [InlineData("abc ", false)]
    public void OnlyWords_ReturnsExpectedResult(string text, bool expected)
    {
        Assert.Equal(expected, FieldValidators.OnlyWords(text));
    }

    [Theory]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("abc def", true)]
    [InlineData("abc def 1", false)]
    [InlineData("abc def ", true)]
    public void OnlyWordsAndSpaces_ReturnsExpectedResult(string text, bool expected)
    {
        Assert.Equal(expected, FieldValidators.OnlyWordsAndSpaces(text));
    }
}
