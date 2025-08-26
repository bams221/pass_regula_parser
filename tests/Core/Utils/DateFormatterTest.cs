using PassRegulaParser.Core.Utils;

namespace PassRegulaParser.Tests.Core.Utils;

public class DateFormatterTests
{
    [Theory]
    [InlineData("1", "1")]
    [InlineData("12", "12")]
    [InlineData("123", "12.3")]
    [InlineData("1234", "12.34")]
    [InlineData("12345", "12.34.5")]
    [InlineData("123456", "12.34.56")]
    [InlineData("1234567", "12.34.567")]
    [InlineData("12345678", "12.34.5678")]
    [InlineData("", "")]
    [InlineData(".", "")]
    [InlineData("..", "")]
    [InlineData("1.", "1")]
    [InlineData("1.2", "12")]
    [InlineData("1.23", "12.3")]
    [InlineData("1.234", "12.34")]
    [InlineData("1.2345", "12.34.5")]
    [InlineData("1.2.3.4", "12.34")]
    public void FormatDateText_ShouldFormatCorrectly(string input, string expected)
    {
        var result = DateFormatter.FormatDateText(input);
        Assert.Equal(expected, result);
    }
}
