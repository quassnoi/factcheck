using System.Collections.Immutable;
using FactCheck.Analyzers.Tests.Helpers;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace FactCheck.Analyzers.Tests;

public class XunitDisplayNameMissingCodeFixTests
{
    private readonly string _xunitMockProjectFixedPath = Path.Combine("MockProjects", "Xunit.MockProject.Fixed");
    private readonly string _xunitMockProjectPath = Path.Combine("MockProjects", "Xunit.MockProject");


    private static CSharpCodeFixTest<XunitDisplayNameMissingAnalyzer, XunitDisplayNameMissingCodeFix, XUnitVerifier> TestFactory(
        string testCode,
        string fixedCode,
        params DiagnosticResult[] diagnosticResults)
    {
        var cSharpCodeFixTest = new CSharpCodeFixTest<XunitDisplayNameMissingAnalyzer, XunitDisplayNameMissingCodeFix, XUnitVerifier>
        {
            ReferenceAssemblies = ReferenceAssemblies.Default.AddPackages(
                ImmutableArray.Create(
                    new PackageIdentity("xunit", "2.4.1")
                )),
            TestCode = testCode,
            FixedCode = fixedCode
        };

        cSharpCodeFixTest.ExpectedDiagnostics.AddRange(diagnosticResults);
        return cSharpCodeFixTest;
    }

    [Fact(DisplayName = "XunitDisplayNameMissingCodeFix, when DisplayName attribute is missing on Fact, will create it")]
    public async Task XunitDisplayNameMissingCodeFixWhenDisplayNameAttributeIsMissingOnFactWillCreateIt()
    {
        var (testCode, fixedCode) = await (
            FileHelper.LoadModule(_xunitMockProjectPath, nameof(XunitDisplayNameMissingCodeFixWhenDisplayNameAttributeIsMissingOnFactWillCreateIt)),
            FileHelper.LoadModule(_xunitMockProjectFixedPath, nameof(XunitDisplayNameMissingCodeFixWhenDisplayNameAttributeIsMissingOnFactWillCreateIt))
        );
        var codeFixTest = TestFactory(testCode, fixedCode);
        await codeFixTest.RunAsync();
    }

    [Fact(DisplayName = "XunitDisplayNameMissingCodeFix, when DisplayName attribute is missing on Theory, will create it")]
    public async Task XunitDisplayNameMissingCodeFixWhenDisplayNameAttributeIsMissingOnTheoryWillCreateIt()
    {
        var (testCode, fixedCode) = await (
            FileHelper.LoadModule(_xunitMockProjectPath, nameof(XunitDisplayNameMissingCodeFixWhenDisplayNameAttributeIsMissingOnTheoryWillCreateIt)),
            FileHelper.LoadModule(_xunitMockProjectFixedPath, nameof(XunitDisplayNameMissingCodeFixWhenDisplayNameAttributeIsMissingOnTheoryWillCreateIt))
        );
        var codeFixTest = TestFactory(testCode, fixedCode);
        await codeFixTest.RunAsync();
    }

    [Theory(DisplayName = "XunitDisplayNameMissingCodeFix, when method name has case, will split words")]
    [InlineData("Initcase", "Initcase")]
    [InlineData("UPPERCASE", "UPPERCASE")]
    [InlineData("lowercase", "Lowercase")]
    [InlineData("camelCase", "Camel case")]
    [InlineData("PascalCase", "Pascal case")]
    [InlineData("snake_case", "Snake case")]
    [InlineData("camelPascal_snakeMixed", "Camel pascal snake mixed")]
    [InlineData("ALLCapsPreserved", "ALL caps preserved")]
    public async Task XunitDisplayNameMissingCodeFixWhenMethodNameHasCaseWillSplitWords(string testMethodName, string displayNameValue)
    {
        var (rawTestCode, rawFixedCode) = await (
            FileHelper.LoadModule(_xunitMockProjectPath, nameof(XunitDisplayNameMissingCodeFixWhenMethodNameHasCaseWillSplitWords)),
            FileHelper.LoadModule(_xunitMockProjectFixedPath, nameof(XunitDisplayNameMissingCodeFixWhenMethodNameHasCaseWillSplitWords))
        );

        var testCode = rawTestCode.Replace("TestMethodName", testMethodName);
        var fixedCode = rawFixedCode
            .Replace("TestMethodName", testMethodName)
            .Replace("DisplayNameValue", displayNameValue);

        var codeFixTest = TestFactory(testCode, fixedCode);
        await codeFixTest.RunAsync();
    }
}