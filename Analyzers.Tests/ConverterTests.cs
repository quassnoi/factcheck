using FactCheck.Common;
using FluentAssertions;

namespace FactCheck.Analyzers.Tests;

public class ConverterTests
{
    [Theory(DisplayName = "SplitCode, when given code, splits")]
    [InlineData("lowercase", new[] { "lowercase" })]
    [InlineData("Initcap", new[] { "Initcap" })]
    [InlineData("UPPERCASE", new[] { "UPPERCASE" })]
    public void SplitCodeWhenGivenCodeSplits(string input, IEnumerable<string> expectedOutput)
    {
        var actualOutput = Converters.SplitCode(input).ToList();
        actualOutput.Should().Equal(expectedOutput);
    }
}