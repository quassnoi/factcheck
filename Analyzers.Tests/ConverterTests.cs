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
    public void SplitCodeWhenGivenCodeShouldSplit(string input, IEnumerable<string> expectedOutput)
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

    [Theory(DisplayName = "CodeToText, when given text, should convert to code")]
    [InlineData("", null)]
    [InlineData("_?^&*", null)]
    [InlineData("Two Words", "TwoWords")]
    [InlineData("lowercase words", "LowercaseWords")]
    [InlineData("PascalCase preserves case", "PascalCasePreservesCase")]
    [InlineData("snake_case_preserves_case", "SnakeCasePreservesCase")]
    [InlineData("UPPERCASE preserves case", "UPPERCASEPreservesCase")]
    public void CodeToTextWhenGivenTextShouldConvertToCode(string text, string? expectedCode)
    {
        var actualCode = Converters.TextToCode(text);
        Assert.Equal(expectedCode, actualCode);
    }
}