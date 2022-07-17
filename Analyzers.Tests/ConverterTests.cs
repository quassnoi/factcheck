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

    [Theory(DisplayName = "BreakByCodeCharacterClass, when given text, should break by class")]
    [InlineData("latin", new[] { "latin" })]
    [InlineData("123", new[] { "123" })]
    [InlineData("one_two", new[] { "one", "two" })]
    [InlineData("one, two! three+four", new[] { "one", "two", "three", "four" })]
    [InlineData("abc123", new[] { "abc", "123" })]
    [InlineData("latinкириллица", new[] { "latin", "кириллица" })]
    public void BreakByCodeCharacterClassWhenGivenTextShouldBreakByClass(string input, IEnumerable<string> expectedOutput)
    {
        var actualOutput = Converters.BreakByCodeCharacterClass(input);
        actualOutput.Should().Equal(expectedOutput);
    }

}