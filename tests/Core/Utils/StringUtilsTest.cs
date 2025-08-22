using PassRegulaParser.Core.Utils;

namespace PassRegulaParser.Tests.Core.Utils
{
    public class StringUtilsTests
    {
        [Theory]
        [InlineData("", "")]
        [InlineData(" ", "")]
        [InlineData("  ", "")]
        [InlineData("a", "a")]
        [InlineData("a b", "a b")]
        [InlineData("a  b", "a b")]
        [InlineData("a\nb", "a b")]
        [InlineData("a\tb", "a b")]
        [InlineData("a\n\nb", "a b")]
        [InlineData("a\t\tb", "a b")]
        [InlineData("a\n\tb", "a b")]
        [InlineData("  a  b  ", "a b")]
        [InlineData("a\nb\nc", "a b c")]
        [InlineData("a\tb\tc", "a b c")]
        public void ReplaceSpecialChars_ShouldReturnCorrectResult(string input, string expected)
        {
            var result = StringUtils.ReplaceSpecialChars(input);

            Assert.Equal(expected, result);
        }
    }
}
