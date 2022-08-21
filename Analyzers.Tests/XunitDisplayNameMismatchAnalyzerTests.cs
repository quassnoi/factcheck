using System.Collections.Immutable;
using FactCheck.Analyzers.Tests.Helpers;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Verify = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<FactCheck.XunitDisplayNameMismatchAnalyzer>;

namespace FactCheck.Analyzers.Tests;

public class XunitDisplayNameMismatchAnalyzerTests
{
    private readonly string _xunitMockProjectPath = Path.Combine("MockProjects", "Xunit.MockProject");

    private static CSharpAnalyzerTest<XunitDisplayNameMismatchAnalyzer, XUnitVerifier> TestFactory(string code, params DiagnosticResult[] diagnosticResults)
    {
        var cSharpAnalyzerTest = new CSharpAnalyzerTest<XunitDisplayNameMismatchAnalyzer, XUnitVerifier>
        {
            ReferenceAssemblies = ReferenceAssemblies.Default.AddPackages(
                ImmutableArray.Create(
                    new PackageIdentity("xunit", "2.4.1")
                )),
            TestState =
            {
                Sources =
                {
                    code
                }
            }
        };

        cSharpAnalyzerTest.ExpectedDiagnostics.AddRange(diagnosticResults);
        return cSharpAnalyzerTest;
    }

    [Theory(DisplayName = "XunitDisplayNameMismatchAnalyzer, when DisplayName matches method name, should not issue diagnostic")]
    [InlineData("System, when condition, should behave", "SystemWhenConditionShouldBehave")]
    [InlineData("PascalCase, when condition, should behave", "PascalCaseWhenConditionShouldBehave")]
    [InlineData("System, when camelCaseCondition, should behave", "SystemWhenCamelCaseConditionShouldBehave")]
    [InlineData("System, when snake_case_condition, should behave", "SystemWhenSnakeCaseConditionShouldBehave")]
    public async Task XunitDisplayNameMismatchAnalyzerWhenDisplayNameMatchesMethodNameShouldNotIssueDiagnostic(string displayName, string methodName)
    {
        var template = await FileHelper.LoadModule(_xunitMockProjectPath, nameof(XunitDisplayNameMismatchAnalyzerWhenDisplayNameMatchesMethodNameShouldNotIssueDiagnostic));
        var code = template
            .Replace("DisplayNameValue", displayName)
            .Replace("TestMethodName", methodName);

        await TestFactory(code).RunAsync();
    }

    [Theory]
    [InlineData("System, when condition, should behave", "systemwhenconditionshouldbehave")]
    [InlineData("System, when condition, should behave", "_system_when_condition_should_behave")]
    public async Task XunitDisplayNameMismatchAnalyzerWhenDisplayNameOnlyDiffersInCaseAndSeparatorsShouldNotIssueDiagnostic(string displayName, string methodName)
    {
        var template = await FileHelper.LoadModule(_xunitMockProjectPath,
            nameof(XunitDisplayNameMismatchAnalyzerWhenDisplayNameOnlyDiffersInCaseAndSeparatorsShouldNotIssueDiagnostic));
        var code = template
            .Replace("DisplayNameValue", displayName)
            .Replace("TestMethodName", methodName);

        await TestFactory(code).RunAsync();
    }

    [Fact]
    public async Task XunitDisplayNameMismatchAnalyzerWhenDisplayNameDoesNotMatchMethodNameShouldIssueDiagnostic()
    {
        var code = await FileHelper.LoadModule(_xunitMockProjectPath, nameof(XunitDisplayNameMismatchAnalyzerWhenDisplayNameDoesNotMatchMethodNameShouldIssueDiagnostic));

        await TestFactory(
                code,
                Verify.Diagnostic(Diagnostics.FactCheck0002XunitDisplayNameMismatch)
                    .WithLocation(6, 17)
                    .WithArguments("System, when condition, should behave", "Test")
            )
            .RunAsync();
    }
}