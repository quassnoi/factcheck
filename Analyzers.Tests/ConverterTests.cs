using FactCheck.Common;
using FluentAssertions;

namespace FactCheck.Analyzers.Tests;

public class ConverterTests
{
    [Theory(DisplayName = "SplitCode, when given code, should split")]
    [InlineData("lowercase", new[] { "lowercase" })]
    [InlineData("Initcap", new[] { "Initcap" })]
    [InlineData("UPPERCASE", new[] { "UPPERCASE" })]
    [InlineData("", new string[] { })]
    public void SplitCodeWhenGivenCodeSplits(string input, IEnumerable<string> expectedOutput)
    {
        var actualOutput = Converters.SplitCode(input).ToList();
        actualOutput.Should().Equal(expectedOutput);
    }

    [Theory(DisplayName = "SplitText, when given text, should split")]
    [InlineData("latin", new[] { "latin" })]
    [InlineData("123", new[] { "123" })]
    [InlineData("one_two", new[] { "one", "two" })]
    [InlineData("one, two! three+four", new[] { "one", "two", "three", "four" })]
    [InlineData("abc123", new[] { "abc", "123" })]
    [InlineData("latin кириллица", new[] { "latin", "кириллица" })]
    [InlineData("_", new string[] { })]
    public void SplitTextWhenGivenTextShouldSplit(string input, IEnumerable<string> expectedOutput)
    {
        var actualOutput = Converters.SplitText(input);
        actualOutput.Should().Equal(expectedOutput);
    }
}